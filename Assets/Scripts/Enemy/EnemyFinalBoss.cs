﻿using System.Collections;
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
    public float stat1Weapon3CD = 0.3f;
    bool stat1Weapon3 = true;
    public List<GameObject> stat2Anchors;
    GameObject[] stat2Fields;
    int stat2Serious = 0;


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
        stat2Fields = new GameObject[20];
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
            yield return new WaitForSeconds(4f);
            StartCoroutine(Weapon1());
            yield return new WaitForSeconds(6f);
            StartCoroutine(SummonForceField());
            StartCoroutine(WeaponSummonEnemy());
            yield return new WaitForSeconds(12f);
            stat1Weapon1 = false;
            StartCoroutine(Weapon2());
            yield return new WaitForSeconds(12f);
            stat1Weapon2 = false;
            allowMovement = false;
            yield return new WaitForSeconds(5f);
            
            StartCoroutine(WeaponImminue());
            yield return new WaitForSeconds(7f);
            stat1BlueField1.GetComponent<ForceField>().DeactiveForceField();
            stat1BlueField2.GetComponent<ForceField>().DeactiveForceField();
            stat1RedField1.GetComponent<ForceField>().DeactiveForceField();
            stat1RedField2.GetComponent<ForceField>().DeactiveForceField();
            movement = true;
            allowMovement = true;

        }
        
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
            for (int i = 0; i < 3; i++)
            {
                GameObject proj = gameMan.GetEnemyProj(0);
                proj.transform.position = transform.position;
                proj.transform.rotation = transform.rotation;
                proj.GetComponent<EnemyProjectile>().StartWork();
                switch (i)
                {
                    case 0:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(-1f, 0f, -1f).normalized * 7f;
                        break;
                    case 1:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -1f).normalized * 7f;
                        break;
                    case 2:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(1f, 0f, -1f).normalized * 7f;
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
            for (int i = 0; i < 4; i++)
            {
                GameObject proj = gameMan.GetEnemyProj(0); 
                proj.transform.rotation = transform.rotation;
                proj.GetComponent<EnemyProjectile>().StartWork();
                switch (i)
                {
                    case 0:
                        proj.transform.position = transform.position + new Vector3(-3f,0f,0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(-0.5f, 0f, -1f).normalized * 5f;
                        break;
                    case 1:
                        proj.transform.position = transform.position + new Vector3(-3f, 0f, 0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(0.5f, 0f, -1f).normalized * 5f;
                        break;
                    case 2:
                        proj.transform.position = transform.position + new Vector3(3f, 0f, 0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(-0.5f, 0f, -1f).normalized * 5f;
                        break;
                    case 3:
                        proj.transform.position = transform.position + new Vector3(3f, 0f, 0f);
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(0.5f, 0f, -1f).normalized * 5f;
                        break;
                    
                }

            }
            yield return new WaitForSeconds(stat1Weapon2CD);

        }
    }

    public IEnumerator Weapon3()
    {
        while (stat1Weapon1)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject proj = gameMan.GetProjectileTracer();
                proj.transform.position = transform.position;
                proj.transform.rotation = transform.rotation;
                proj.GetComponent<EnemyProjectile>().StartWork();
                switch (i)
                {
                    case 0:
                        proj.transform.position = transform.position + new Vector3(-2.5f, 0f, 0f);
                        break;
                    case 1:
                        proj.transform.position = transform.position + new Vector3(2.5f, 0f, 0f);
                        break;
                    
                }

            }
            yield return new WaitForSeconds(stat1Weapon3CD);

        }
    }

    public IEnumerator WeaponSummonEnemy()
    {

        for (int i = 0; i < 2; i++)
        {
            GameObject proj = gameMan.GetEnemy2();
            proj.SetActive(true);
            proj.transform.position = transform.position + 
                new Vector3(Random.Range(-9f,9f),0f,Random.Range(-1.5f,-7f));
            proj.transform.rotation = transform.rotation;
           
        }
        yield return null;

    }
    public IEnumerator WeaponImminue()
    {
    
        for (int i = 0; i < 40; i++)
        {
            GameObject proj = gameMan.GetProjectileImmune();
            proj.transform.position = transform.position + new Vector3(-10f+0.5f*i, 0f, 0f);

            proj.transform.rotation = transform.rotation;
            proj.GetComponent<EnemyProjectile>().StartWork();
            proj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0f, -1f).normalized * 6f;
            

        }
        yield return null;

        
    }

    public void Stat2Begin()
    {
        for (int i = 0; i< stat2Anchors.Count; i++)
        {
            GameObject ffb = gameMan.GetForceFieldBlue();
            ffb.transform.position = stat2Anchors[i].transform.position;
            ffb.SetActive(true);
            ffb.GetComponent<ForceField>().ActiveForceField();
            stat2Fields[i] = ffb;
        }
    }
    public void Stat2IncreaseSerious()
    {

        if (stat2Serious == 0)
        {
            stat2Serious += 1;
            Stat2ChangeColor(new int[] { 0,4,15,19 });
        }
        else if (stat2Serious == 1)
        {
            stat2Serious += 1;
            Stat2ChangeColor(new int[] { 1, 3, 5, 9, 16, 18 });
        }

        else if (stat2Serious == 2)
        {
            stat2Serious += 1;
            Stat2ChangeColor(new int[] {2,10,14,17 });
        }
        else if (stat2Serious == 3)
        {
            stat2Serious += 1;
            Stat2ChangeColor(new int[] { 6, 8,11,13 });
        }
        else if (stat2Serious == 4)
        {
            stat2Serious += 1;
            Stat2ChangeColor(new int[] { 7,12 });
        }
    }
    void Stat2ChangeColor(int[] change)
    {
        for (int i = 0; i < change.Length; i++)
        {
            stat2Fields[i].GetComponent<ForceField>().DeactiveForceField();
            GameObject ffr = gameMan.GetForceFieldRed();
            ffr.transform.position = stat2Anchors[i].transform.position;
            ffr.SetActive(true);
            ffr.GetComponent<ForceField>().ActiveForceField();
        }

    }
}