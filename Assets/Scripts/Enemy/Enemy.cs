using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameMan gameMan;
    // Start is called before the first frame update
    void Start()
    {
        gameMan = GameMan.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void StartFire()
    {

    }
}
