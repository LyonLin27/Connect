using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinalBoss : Enemy
{
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
    public void ActiveForceFieldRed(Vector3 position)
    {
        GameObject ffr =gameMan.GetForceFieldRed();
        ffr.transform.position = position;
        ffr.SetActive(true);
        ffr.GetComponent<ForceField>().StartCoroutine(ffr.GetComponent<ForceField>().ActiveForceField());

    }
    public void ActiveForceFieldBlue(Vector3 position)
    {
        GameObject ffb = gameMan.GetForceFieldBlue();
        ffb.transform.position = position;
        ffb.SetActive(true);
        ffb.GetComponent<ForceField>().StartCoroutine(ffb.GetComponent<ForceField>().ActiveForceField());

    }

    public IEnumerator Fight()
    {
        yield return new WaitForSeconds(1f);
        print("here");
        ActiveForceFieldBlue(transform.position);
        ActiveForceFieldRed(gameMan.PlayerAgent.transform.position);
        yield return null;
    }
}
