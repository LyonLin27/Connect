using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameMan gameMan;
    protected Rigidbody rb;
    protected ParticleSystem introPtc;
    protected ParticleSystem outroPtc;
    protected bool active = true;
    protected Vector3 startPos;
    protected Quaternion startRot;

    public float hp_max = 5f;
    [SerializeField]
    protected float hp;
    



    protected Material mat;
    protected Color startColor;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        gameMan = GameMan.Instance;
        rb = gameObject.GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
        hp = hp_max;



        mat = GetComponent<MeshRenderer>().material;

        // for one room level
        introPtc = transform.Find("introPtc").GetComponent<ParticleSystem>();
        outroPtc = transform.Find("outroPtc").GetComponent<ParticleSystem>();
        active = false;
        StartCoroutine("Intro");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Color currentColor = new Color(1f * (1-hp/hp_max), startColor.g, startColor.b);
        mat.SetColor("_BaseColor", currentColor);
    }

    public virtual void StartFire()
    {

    }

    protected virtual void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.layer == LayerMask.NameToLayer("PlayerProj")) {
            hp -= coll.gameObject.GetComponent<PlayerProj>().dmg;
        }
        if (hp <= 0) {
            StartCoroutine("Outro");
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

    protected virtual IEnumerator Intro() {

        GetComponent<MeshRenderer>().enabled = false;

        introPtc.Play();

        yield return new WaitForSeconds(2);



        active = true;

        hp = hp_max;

        GetComponent<MeshRenderer>().enabled = true;
    }

    protected virtual IEnumerator Outro() {

        active = false;

        GetComponent<MeshRenderer>().enabled = false;
        outroPtc.Play();

        yield return new WaitForSeconds(2);

        Death();

    }
}
