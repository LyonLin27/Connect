using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    protected GameMan gameMan;
    protected Rigidbody rb;
   
    public bool working = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameMan = GameMan.Instance;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.x < gameMan.minX || transform.position.x > gameMan.maxX)
        {
            FinishWork();
        }
        if (transform.position.z < gameMan.minZ || transform.position.z > gameMan.maxZ)
        {
            FinishWork();
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public virtual void DealDamage()
    {

    }
    public virtual void StartWork(Vector3 startPosition, Vector3 InitialSpeed)
    {
        
    }
    public virtual void StartWork(Vector3 param1, Vector3 param2, float param3, float param4)
    {

    }
    public virtual void FinishWork()
    {

    }
}
