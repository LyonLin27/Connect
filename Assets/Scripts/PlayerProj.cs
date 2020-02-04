using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProj : MonoBehaviour {

    public float speed = 20;
    public int dmg = 1;
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        StartCoroutine(DisableAfterTime(3f));
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject ptc = GameMan.Instance.GetProjPtcWt();
        ptc.transform.position = transform.position;
        ptc.GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);
    }

    IEnumerator DisableAfterTime(float time) {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
