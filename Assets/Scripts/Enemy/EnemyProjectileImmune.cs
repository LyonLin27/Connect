using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileImmune : EnemyProjectile
{
    protected override void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Agent"))
        {

            GameObject ptc = GameMan.Instance.GetProjPtcRd();
            ptc.transform.position = transform.position;
            ptc.GetComponent<ParticleSystem>().Play();
            FinishWork();
        }
    }
}
