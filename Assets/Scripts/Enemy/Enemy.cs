using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameMan gameMan;
    protected Rigidbody rb;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        gameMan = GameMan.Instance;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void StartFire()
    {

    }
}
