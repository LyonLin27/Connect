using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileTracer : EnemyProjectile
{
    public float tracerSpeed;
    GameObject target;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (target == null)
        {
            StopAllCoroutines();
            FinishWork();
        }
        rb.velocity = (target.transform.position - transform.position).normalized * tracerSpeed;
    }

    public override void StartWork()
    {
        target = GameMan.Instance.Agents[Random.Range(0, GameMan.Instance.Agents.Count)];
        base.StartWork();

    }
}
