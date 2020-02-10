using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinalBoss : Enemy
{
    GameObject stat1BlueField1;
    GameObject stat1BlueField2;
    GameObject stat1RedField1;
    GameObject stat1RedField2;
    public float stat1SummonFieldCD = 10f;
    public float stat1Weapon1CD = 0.2f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Fight());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
        StartCoroutine(Weapon1());
        StartCoroutine(SummonForceField());
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
        while (true)
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
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(-1f, 0f, -1f).normalized * 3f;
                        break;
                    case 1:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -1f).normalized * 3f;
                        break;
                    case 2:
                        proj.GetComponent<Rigidbody>().velocity = new Vector3(1f, 0f, -1f).normalized * 3f;
                        break;
                }

            }
            yield return new WaitForSeconds(stat1Weapon1CD);

        }
    }
}
