using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGate : Enemy
{

    public float hp_regen = 2f;
    public ParticleSystem deathParticle;

    public delegate void BreakAction();
    public event BreakAction OnBreak;

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
        if (OnBreak != null)
            OnBreak();
    }

    protected override IEnumerator Outro() {
        yield return new WaitForSeconds(0.2f);
        Death();
    }

    public void Restore()
    {
        hp = hp_max;
        Activate();
        gameObject.SetActive(true);
    }
}
