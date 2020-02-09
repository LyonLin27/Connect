using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameMan gameMan;
    protected Rigidbody rb;
    protected bool active = false;
    protected Vector3 startPos;
    protected Quaternion startRot;

    public int hp_max = 5;
    private int hp;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        gameMan = GameMan.Instance;
        rb = gameObject.GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
        hp = hp_max;
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
        active = false;
        gameObject.SetActive(false);
    }

    public void Activate() {
        active = true;
    }

    public void Deactivate() {
        active = false;
    }

    public void Reset() {
        active = false;
        gameObject.SetActive(true);
        transform.position = startPos;
        transform.rotation = startRot;
        hp = hp_max;
    }
}
