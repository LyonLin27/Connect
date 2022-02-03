using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    // status
    public float alertRange = 10f;
    private bool starting = false;
    private int phase = 0;

    // front cannons
    public List<Transform> frontCannons;
    public float frontCannonFireItv = 2f;
    public int frontCannonFireRepeat = 5;
    public float frontCannonFireRepeatItv = 0.2f;
    public float frontCannonProjSpd = 20f;
    private float frontCannonLastFireTime = 0f;
    private bool frontCannonFiring = true;

    // walls
    List<EnemyBossWall> wallList;
    private bool wallActive;
    public float wallStartTime = 5f;
    public int firingWalls = 3;
    public float wallsFireItv = 6f;
    public float wallsProjLife = 4f;
    public float wallsProjSpd = 15f;

    public Transform centerWall;
    private Vector3 centerWallStartPos;

	public override void Reset()
	{
        StopAllCoroutines();
        phase = 0;
        active = false;
        starting = false;
        hp = hp_max;

        starting = false;
        frontCannonFiring = true;
        wallActive = false;

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;

        foreach (EnemyBossWall wall in wallList)
        {
            wall.Reset();
        }

        centerWall.position = centerWallStartPos;
    }

	protected override void Start()
    {
        gameMan = GameMan.Instance;
        rb = gameObject.GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
        hp = hp_max;
        phase = 0;

        mat = GetComponent<MeshRenderer>().material;
        outroPtc = transform.Find("outroPtc").GetComponent<ParticleSystem>();
        centerWallStartPos = centerWall.position;

        //walls
        wallList = new List<EnemyBossWall>();
        foreach (Transform trans in transform.Find("BossWalls")) {
            foreach(Transform t in trans)
                wallList.Add(t.GetComponent<EnemyBossWall>());
        }
        Debug.Log(string.Format("Wall Collected: {0}", wallList.Count));

        active = false;
    }

    protected override void Update()
    {
        base.Update();

        if (!active)
        {
            if (!starting)
            {
                if (Distance2Player() < alertRange || hp < hp_max)
                {
                    StartCoroutine("Intro");
                    Debug.Log("Boss Starting..");
                }
            }
            return;
        }

        if (phase == 1)
        {
            HandlePhase1();
        }

        if (hp < hp_max * 0.8f && phase <= 1)
        {
            StartPhase2();
        }

        if (phase == 2)
        {
            HandlePhase2();
        }

        if (hp < hp_max * 0.4f && phase == 2)
        {
            StartPhase3();
        }
    }

    private void HandlePhase1()
    {
        if (frontCannonFiring && Time.time - frontCannonLastFireTime > frontCannonFireItv)
        {
            frontCannonLastFireTime = Time.time;
            StartCoroutine("FireFrontCannonsRepeat");
        }
    }

    private void StartPhase2() {
        phase = 2;
        wallActive = true;
        foreach (EnemyBossWall wall in wallList)
        {
            wall.Activate();
        }
        frontCannonFiring = false;

        StartCoroutine("FireWallCannonsRepeat");
    }

    private void HandlePhase2()
    {
        
    }

    private void StartPhase3()
    {
        phase = 3;
        wallActive = false;
        StopAllCoroutines();
        foreach (EnemyBossWall wall in wallList)
        {
            wall.ReturnToStartPos();
        }
        StartCoroutine("DropCenterWall");
        StartCoroutine("WaitForReturn");
    }

	protected override IEnumerator Intro()
	{
        starting = true;
        yield return new WaitForSeconds(2);

        active = true;
        hp = hp_max;
        phase = 1;
        Debug.Log("Boss Start Completed");
    }

    IEnumerator FireFrontCannonsRepeat()
    {
        for (int i = 0; i < frontCannonFireRepeat; i++)
        {
            FireFrontCannons();
            yield return new WaitForSeconds(frontCannonFireRepeatItv);
        }
    }

    IEnumerator FireWallCannonsRepeat()
    {
        yield return new WaitForSeconds(wallStartTime);
        while (wallActive)
        {
            int inc = (int)(hp_max - hp) / 2000;
            FireWallCannons(firingWalls + inc);
            yield return new WaitForSeconds(wallsFireItv);
        }
    }

    IEnumerator WaitForReturn()
    {
        yield return new WaitForSeconds(3f);
        foreach (EnemyBossWall wall in wallList)
        {
            wall.ReturnFinish();
        }
        wallActive = true;
        StartCoroutine("FireWallCannonsRepeat2");
    }

    IEnumerator FireWallCannonsRepeat2()
    {
        while (wallActive)
        {
            int inc = (int)(hp_max - hp) / 2000;
            FireWallCannons(firingWalls);
            yield return new WaitForSeconds(wallsFireItv * 0.75f);
        }
    }

    IEnumerator DropCenterWall()
    {
        while (centerWall.position.y > -2f) {
            yield return new WaitForEndOfFrame();
            centerWall.position -= Vector3.up * Time.deltaTime;
        }
    }

    private void FireFrontCannons()
    {
        foreach (Transform cannon in frontCannons)
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
            proj.GetComponent<Rigidbody>().velocity = frontCannonProjSpd * proj.transform.forward;
        }
    }

    private float Distance2Player()
    {
        return Vector3.Distance(GameMan.Instance.PlayerAgent.transform.position, transform.position);
    }

    private void FireWallCannons(int num)
    {
        int step = wallList.Count/num;

        for (int i = 0; i < wallList.Count; i += step)
        {
            int randIndex = i+ Random.Range(0, step);
            if (randIndex >= wallList.Count)
            {
                if(phase == 2)
                    wallList[wallList.Count-1].StartFire();
                else
                    wallList[wallList.Count - 1].StartPhase2Fire();
                break;
            }
            else {
                if (phase == 2)
                    wallList[randIndex].StartFire();
                else
                    wallList[randIndex].StartPhase2Fire();
            }
        }
    }

	protected override void Death()
	{
        active = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
        foreach (EnemyBossWall wall in wallList)
        {
            wall.revive = false;
            wall.Deactivate();
        }

        GameMan.Instance.ResetAllProjNicely();
	}
}
