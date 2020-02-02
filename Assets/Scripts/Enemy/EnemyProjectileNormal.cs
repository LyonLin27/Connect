using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Normal Projectile
/// </summary>
public class EnemyProjectileNormal : EnemyProjectile
{
   
    public override void StartWork(Vector3 startPosition, Vector3 InitialSpeed)
    {
        working = true;
        gameObject.SetActive(true);
        gameObject.transform.position = startPosition;
        rb.velocity = InitialSpeed;
    }
    public override void FinishWork()
    {
        working = false;
        gameObject.SetActive(false);
    }
}
