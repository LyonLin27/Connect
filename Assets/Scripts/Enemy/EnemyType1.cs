using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    public float fireCD;
    public float projectileSpeed;
    public float rotSpd = 10f;
    private float lastFireTime = 0f;
    public int way = 1;

    protected override void Start() {
        base.Start();
        StartFire();
    }

    protected override void Update() {
        base.Update();
        if (active && Time.time - lastFireTime > fireCD) {
            lastFireTime = Time.time;
            GameObject proj = gameMan.GetEnemyProj(0);
            proj.transform.position = transform.position;
            proj.transform.rotation = transform.rotation;
            proj.GetComponent<EnemyProjectile>().StartWork();
            proj.GetComponent<Rigidbody>().velocity = projectileSpeed * proj.transform.forward;

            if (way >= 2) {
                proj = gameMan.GetEnemyProj(0);
                proj.transform.position = transform.position;
                proj.transform.rotation = transform.rotation;
                proj.GetComponent<EnemyProjectile>().StartWork();
                proj.GetComponent<Rigidbody>().velocity = -projectileSpeed * proj.transform.forward;
            }

            if (way >= 4) {
                proj = gameMan.GetEnemyProj(0);
                proj.transform.position = transform.position;
                proj.transform.rotation = transform.rotation;
                proj.GetComponent<EnemyProjectile>().StartWork();
                proj.GetComponent<Rigidbody>().velocity = projectileSpeed * proj.transform.right;

                proj = gameMan.GetEnemyProj(0);
                proj.transform.position = transform.position;
                proj.transform.rotation = transform.rotation;
                proj.GetComponent<EnemyProjectile>().StartWork();
                proj.GetComponent<Rigidbody>().velocity = -projectileSpeed * proj.transform.right;
            }
        }
        
    }

    protected void FixedUpdate() {
        if(active)
            rb.AddTorque(transform.up * rotSpd * Time.fixedDeltaTime, ForceMode.Force);
    }
}
