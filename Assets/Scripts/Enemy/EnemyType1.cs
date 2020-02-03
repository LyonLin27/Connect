using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    public float fireCD;
    public float projectileSpeed;
    public float rotSpd = 10f;

    protected override void Start() {
        base.Start();
        StartFire();
    }

    protected void FixedUpdate() {
        rb.AddTorque(transform.up * rotSpd * Time.fixedDeltaTime, ForceMode.Force);
    }

    public override void StartFire()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireCD);
            GameObject proj = gameMan.GetEnemyProj(0);
            proj.transform.position = transform.position;
            proj.transform.rotation = transform.rotation;
            proj.GetComponent<EnemyProjectile>().StartWork();
            proj.GetComponent<Rigidbody>().velocity = projectileSpeed * proj.transform.forward;
        }
       
    }
}
