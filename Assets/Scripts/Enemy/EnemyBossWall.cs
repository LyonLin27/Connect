using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossWall : Enemy
{
    public float moveForce = 100f;
    public float keepDistFromPlayer = 10f;
    public float keepDistFromWall = 20f;
    public float projSpd = 15f;
    public float reviveCD = 5f;

    public float returnSpd = 10f;

    public List<Transform> frontCannons;
    public List<Transform> backCannons;
    public ParticleSystem firePtc;

    private float forceMoveBackTime;
    public bool revive = true;

    private bool returning = false;

    protected override void Start()
    {
        gameMan = GameMan.Instance;
        rb = gameObject.GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
        hp = hp_max;

        mat = GetComponent<MeshRenderer>().material;

        active = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

	public override void Reset()
	{
		base.Reset();

        StopAllCoroutines();

        active = false;
        revive = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

	public override void Activate()
    {
        base.Reset();
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        introPtc = transform.Find("introPtc").GetComponent<ParticleSystem>();
        outroPtc = transform.Find("outroPtc").GetComponent<ParticleSystem>();
        active = false;
        revive = true;
        StartCoroutine("Intro");
    }

    public void ReturnToStartPos()
    {
        forceMoveBackTime = 0f;
        returning = true;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    public void ReturnFinish()
    {
        rb.isKinematic = false;
        returning = false;
        GetComponent<Collider>().enabled = true;
    }

	protected override void Death()
	{
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        if(revive)
            StartCoroutine("Revive");
	}

	public override void Deactivate()
	{
        revive = false;
		base.Deactivate();
        StopAllCoroutines();
        StartCoroutine("Outro");
	}

	public override void StartFire()
    {
        if (!GetComponent<MeshRenderer>().enabled || returning)
            return;

        Vector3 playerDir = GameMan.Instance.PlayerAgent.transform.position - transform.position;
        if (Vector3.Dot(playerDir, transform.forward) > 0)
        {
            StartCoroutine("FireStraightCannons", frontCannons[0]);
        }
        else {
            StartCoroutine("FireStraightCannons", backCannons[0]);
        }
	}

    public void StartPhase2Fire()
    {
        if (!gameObject.activeInHierarchy)
            return;

        Vector3 playerDir = GameMan.Instance.PlayerAgent.transform.position - transform.position;
        if (Vector3.Dot(playerDir, transform.forward) > 0)
        {
            StartCoroutine("FireBarCannons", frontCannons);
        }
        else
        {
            StartCoroutine("FireBarCannons", backCannons);
        }
    }

    private void FireCannons(List<Transform> cannons)
    {
        foreach (Transform cannon in cannons)
        {
            Vector3 shootDir = GameMan.Instance.PlayerAgent.transform.position - transform.position;
            shootDir.y = 0;

            GameObject proj = gameMan.GetEnemyProj(0);
            proj.transform.position = cannon.position;
            proj.transform.forward = shootDir;
            if (projLife > 0)
            {
                proj.GetComponent<EnemyProjectile>().lifeTime = projLife;
            }
            proj.GetComponent<EnemyProjectile>().StartWork();
            proj.GetComponent<Rigidbody>().velocity = projSpd * proj.transform.forward;
        }
    }

    IEnumerator Revive()
    {
        yield return new WaitForSeconds(reviveCD);
        Activate();
    }

    IEnumerator FireStraightCannons(Transform cannon)
    {
        firePtc.Play();
        yield return new WaitForSeconds(1.8f);
        firePtc.Stop();

        for (int i = 0; i < 5; i++)
        {
            Vector3 shootDir = GameMan.Instance.PlayerAgent.transform.position - transform.position;
            shootDir.y = 0;

            GameObject proj = gameMan.GetEnemyProj(0);
            proj.transform.position = cannon.position;
            proj.transform.forward = shootDir;
            if (projLife > 0)
            {
                proj.GetComponent<EnemyProjectile>().lifeTime = projLife;
            }
            proj.GetComponent<EnemyProjectile>().StartWork();
            proj.GetComponent<Rigidbody>().velocity = projSpd * proj.transform.forward;

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FireBarCannons(List<Transform> cannons)
    {
        firePtc.Play();
        yield return new WaitForSeconds(1.8f);
        firePtc.Stop();

        foreach(Transform cannon in cannons)
        {
            Vector3 shootDir = GameMan.Instance.PlayerAgent.transform.position - transform.position;
            shootDir.y = 0;

            GameObject proj = gameMan.GetEnemyProj(0);
            proj.transform.position = cannon.position;
            proj.transform.forward = shootDir;
            if (projLife > 0)
            {
                proj.GetComponent<EnemyProjectile>().lifeTime = projLife;
            }
            proj.GetComponent<EnemyProjectile>().StartWork();
            proj.GetComponent<Rigidbody>().velocity = projSpd * proj.transform.forward;
        }
    }

    private void FixedUpdate()
	{
        if (active)
        {
            HandleMovement();
        }
	}

    private void HandleMovement()
    {
        if (returning)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, returnSpd * Time.fixedDeltaTime);

            return;
        }

        if (forceMoveBackTime > 0f)
        {
            forceMoveBackTime -= Time.fixedDeltaTime;
            rb.AddForce(-1f * transform.forward * moveForce * Time.fixedDeltaTime);
            return;
        }

        Vector3 playerDir = GameMan.Instance.PlayerAgent.transform.position - transform.position;
        bool closeToWall = CloseToWall();
        bool closeToPlayer = playerDir.magnitude < keepDistFromPlayer;

        if (closeToWall)
        {
            forceMoveBackTime += Random.Range(1f,5f);
        }

        if (Vector3.Dot(playerDir, transform.forward) > 0)
        {
            if(!closeToPlayer)
                rb.AddForce(transform.forward * moveForce * Time.fixedDeltaTime);
        }
        else
        {
            rb.AddForce(-1f * transform.forward * moveForce * Time.fixedDeltaTime);
        }
    }

    private bool CloseToWall()
    {
        RaycastHit hitWall, hitEnemy;

        Physics.Raycast(transform.position, transform.forward, out hitWall, 20f, LayerMask.GetMask("Wall"), QueryTriggerInteraction.Ignore);
        if (hitWall.collider && hitWall.distance < keepDistFromWall)
        {
            Debug.DrawLine(transform.position, hitWall.point, Color.green);
            return true;
        }

        Physics.Raycast(transform.position, transform.forward, out hitEnemy, 20f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore);

        if (hitEnemy.collider && hitEnemy.distance < keepDistFromWall)
        {
            GameObject obj = hitEnemy.collider.gameObject;

            if (obj.GetComponent<EnemyBossWall>() && Vector3.Dot(obj.transform.forward, transform.forward) < -0.9f)
            {
                Debug.DrawLine(transform.position, hitEnemy.point, Color.blue);
                return true;
            }
            else if (obj.layer == LayerMask.GetMask("Wall"))
            {
                return true;
            }
        }

        return false;
    }

}
