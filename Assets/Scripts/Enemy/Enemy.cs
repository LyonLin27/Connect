using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameMan gameMan;
    protected Rigidbody rb;

    public int hp = 5;
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

    protected virtual void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.layer == LayerMask.NameToLayer("PlayerProj")) {
            hp -= coll.gameObject.GetComponent<PlayerProj>().dmg;
        }
        if (hp <= 0) {
            Death();
        }
    }

    protected virtual void Death() {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
