using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    /// <summary>
    /// 0: increase speed. 1: decrease speed
    /// </summary>
    public int projectileType;
    public float power = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (projectileType == 0)
        {
            if (other.gameObject.tag == "Projectile")
            {
                other.gameObject.GetComponent<Rigidbody>().velocity *= (1f + power);
            }
            else if (other.gameObject.tag == "ProjectileTracer")
            {
                other.gameObject.GetComponent<EnemyProjectileTracer>().tracerSpeed *= (1f + power);
            }
            
        }
        else
        {
            if (other.gameObject.tag == "Projectile")
            {
                other.gameObject.GetComponent<Rigidbody>().velocity *= (1f - power);
            }
            else if (other.gameObject.tag == "ProjectileTracer")
            {
                other.gameObject.GetComponent<EnemyProjectileTracer>().tracerSpeed *= (1f - power);
            }

        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (projectileType == 0)
        {
            if (other.gameObject.tag == "Projectile")
            {
                other.gameObject.GetComponent<Rigidbody>().velocity /= (1f + power);
            }
            else if (other.gameObject.tag == "ProjectileTracer")
            {
                other.gameObject.GetComponent<EnemyProjectileTracer>().tracerSpeed /= (1f + power);
            }

        }
        else
        {
            if (other.gameObject.tag == "Projectile")
            {
                other.gameObject.GetComponent<Rigidbody>().velocity /= (1f - power);
            }
            else if (other.gameObject.tag == "ProjectileTracer")
            {
                other.gameObject.GetComponent<EnemyProjectileTracer>().tracerSpeed /= (1f - power);
            }

        }
    }
   
}
