using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeAim : Enemy
{
    public float fireCD;
    public float projectileSpeed = 14f;
    public int ammo = 5;
    public float interval = 0.1f;
    private float lastFireTime = 0f;

    protected override void Start() {
        base.Start();
        StartFire();
    }

    protected override void Update() {
        base.Update();
        if (!gameMan.PlayerAgent.GetComponent<AgentController>().connected) {
            return;
        }

        transform.forward = gameMan.PlayerAgent.transform.position - this.transform.position;
        if (active && Time.time - lastFireTime > fireCD) {
            lastFireTime = Time.time;
            StartCoroutine("ShootNumberProj", ammo);
        }
    }

    private IEnumerator ShootNumberProj(int num) {
        int i = num;
        while (i > 0) {
            if (!active) break;

            i--;
            GameObject proj = gameMan.GetEnemyProj(0);
            proj.transform.position = transform.position;
            proj.transform.rotation = transform.rotation;
            proj.GetComponent<EnemyProjectile>().StartWork();
            proj.GetComponent<Rigidbody>().velocity = projectileSpeed * proj.transform.forward;
            yield return new WaitForSeconds(interval);
        }
    }

}
