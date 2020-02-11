using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    /// <summary>
    /// 0: increase speed. 1: decrease speed
    /// </summary>
    public int projectileType;
    public float size = 9f;
    public float power = 0.5f;
    bool startActive = false;
    bool startDeactive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (startDeactive == true)
        {
            startActive = false;
            transform.localScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime*1.5f;
            if (transform.localScale.x <= 0.1f)
            {
                startDeactive = false;
                gameObject.SetActive(false);

            }
        }
        else if (startActive == true)
        {
            transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime*1.5f;
            if (transform.localScale.x >= size)
            {
                startActive = false;

            }
        }
    }
  
    public void ActiveForceField()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        startActive = true;
    }
    public void DeactiveForceField()
    {
        startDeactive = true;
    }

  
    private void OnTriggerEnter(Collider other)
    {
        if (projectileType == 0)
        {
            if (other.gameObject.tag == "Projectile"|| other.gameObject.tag == "ProjectileImmune")
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
            if (other.gameObject.tag == "Projectile" || other.gameObject.tag == "ProjectileImmune")
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
            if (other.gameObject.tag == "Projectile" || other.gameObject.tag == "ProjectileImmune")
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
            if (other.gameObject.tag == "Projectile" || other.gameObject.tag == "ProjectileImmune")
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
