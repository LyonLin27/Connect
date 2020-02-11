﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameMan gameMan;
    protected Rigidbody rb;
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
