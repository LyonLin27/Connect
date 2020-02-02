using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    public float fireCD;
    public Vector3 projectileSpeed;
    

    public override void StartFire()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        while (true)
        {
            print(gameMan.projectileNomralIndex);
            gameMan.projectileNormal[gameMan.GetEnemyProjectile(0)].GetComponent<EnemyProjectile>().StartWork
                (transform.position, projectileSpeed);
            yield return new WaitForSeconds(fireCD);
        }
       
    }
}
