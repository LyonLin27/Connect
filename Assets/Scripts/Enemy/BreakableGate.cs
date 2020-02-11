using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGate : Enemy
{

    public float hp_regen = 2f;
    public ParticleSystem deathParticle;

    protected override void Start() {
        //base.Start();
        gameMan = GameMan.Instance;
        rb = gameObject.GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
        hp = hp_max;

        mat = GetComponent<MeshRenderer>().material;
    }

    protected override void Update() {
        base.Update();
        if(hp < hp_max)
            hp += hp_regen * Time.deltaTime;
    }

    protected override void Death() {
        base.Death();
        deathParticle.Play();
    }
}
