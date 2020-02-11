using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinalBoss : Enemy
{
   
    GameObject stat1BlueField1;
    GameObject stat1BlueField2;
    GameObject stat1RedField1;
    GameObject stat1RedField2;

    bool movement = true;
    public bool allowMovement = true;
    public float bossLeftBoundary;
    public float bossRightBoundary;
    public Vector3 bossStartPoint;
    public float initialBossSpeed;
    float bossSpeed;

    public float stat1SummonFieldCD = 10f;
    public float stat1Weapon1CD = 0.2f;
    bool stat1Weapon1 = true;
    public float stat1Weapon2CD = 0.3f;
    bool stat1Weapon2 = true;


    // Start is called before the first frame update
    protected override void Start()
    {
        //base.Start();
        gameMan = GameMan.Instance;
        rb = gameObject.GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
        hp = hp_max;
        StartCoroutine(Fight());
        bossSpeed = initialBossSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        //base.Update();
        rb.velocity = new Vector3(bossSpeed, 0f, 0f);
        if (movement)
        {
            if (transform.position.x<= bossStartPoint.x+bossLeftBoundary)
            {
                bossSpeed = initialBossSpeed;
            }
            else if (transform.position.x >= bossStartPoint.x + bossRightBoundary)
            {
                bossSpeed = -initialBossSpeed;
            }
            else if (allowMovement == false)
            {
                if (Vector3.Distance(transform.position, bossStartPoint) <= 0.1f)
                {
                    bossSpeed = 0;
                    movement = false;
                }
            }
        }
    }
    public void StartMove()
    {
        bossSpeed = initialBossSpeed;
        movement = true;
        allowMovement = true;
    }
    public GameObject ActiveForceFieldRed(Vector3 position)
    {
        GameObject ffr =gameMan.GetForceFieldRed();
        ffr.transform.position = position;
        ffr.SetActive(true);
        ffr.GetComponent<ForceField>().ActiveForceField();
        return ffr;

    }
    public GameObject ActiveForceFieldBlue(Vector3 position)
    {
        GameObject ffb = gameMan.GetForceFieldBlue();
        ffb.transform.position = position;
        ffb.SetActive(true);
        ffb.GetComponent<ForceField>().ActiveForceField();
        return ffb;
    }
    public IEnumerator Fight()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(Weapon1());
            yield return new WaitForSeconds(6f);
            StartCoroutine(SummonForceField());
            yield return new WaitForSeconds(12f);
            stat1Weapon1 = false;
            StartCoroutine(Weapon2());
            yield return new WaitForSeconds(12f);
            stat1Weapon2 = false;
        }
        
        
        yield return null;
    }
    

    public IEnumerator SummonForceField()
    {
      
        //Blue First
        stat1BlueField1 = ActiveForceFieldBlue(gameMan.PlayerAgent.transform.position);
        yield return new WaitForSeconds(stat1SummonFieldCD);
        stat1BlueField2 = ActiveForceFieldBlue(gameMan.PlayerAgent.transform.position);
        yield return new WaitForSeconds(stat1SummonFieldCD);
        stat1RedField1 = ActiveForceFieldRed(gameMan.PlayerAgent.transform.position);
        yield return new WaitForSeconds(stat1SummonFieldCD);
        stat1RedField2 = ActiveForceFieldRed(gameMan.PlayerAgent.transform.position);
        yield return null;
    }
    public IEnumerator Weapon1()
    {
        while (stat1Weapon1)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject proj = gameMan.GetEnemyProj(0);
                proj.transform.position = transform.position;
                proj.transform.rotation = transform.rotation;
                proj.GetComponent<EnemyProjectile>().StartWork();
                switch (i)
                {
                    case 0:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(-1f, 0f, -1f).normalized * 5f;
                        break;
                    case 1:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -1f).normalized * 5f;
                        break;
                    case 2:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(1f, 0f, -1f).normalized * 5f;
                        break;
                    case 3:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(-3f, 0f, -1f).normalized * 5f;
                        break;
                    case 4:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(3f, 0f, -1f).normalized * 5f;
                        break;
                }

            }
            yield return new WaitForSeconds(stat1Weapon1CD);

        }
    }

    public IEnumerator Weapon2()
    {
        while (stat1Weapon2)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject proj = gameMan.GetEnemyProj(0); 
                proj.transform.rotation = transform.rotation;
                proj.GetComponent<EnemyProjectile>().StartWork();
                switch (i)
                {
                    case 0:
                        proj.transform.position = transform.position + new Vector3(-3f,0f,0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(-0.5f, 0f, -1f).normalized * 7f;
                        break;
                    case 1:
                        proj.transform.position = transform.position + new Vector3(-3f, 0f, 0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(0.5f, 0f, -1f).normalized * 7f;
                        break;
                    case 2:
                        proj.transform.position = transform.position + new Vector3(3f, 0f, 0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(-0.5f, 0f, -1f).normalized * 7f;
                        break;
                    case 3:
                        proj.transform.position = transform.position + new Vector3(3f, 0f, 0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(0.5f, 0f, -1f).normalized * 7f;
                        break;
                    case 4:
                        proj.transform.position = transform.position + new Vector3(-3f, 0f, 0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(-2f, 0f, -1f).normalized * 7f;
                        break;
                    case 5:
                        proj.transform.position = transform.position + new Vector3(3f, 0f, 0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(2f, 0f, -1f).normalized * 7f;
                        break;
                }

            }
            yield return new WaitForSeconds(stat1Weapon2CD);

        }
    }
}
