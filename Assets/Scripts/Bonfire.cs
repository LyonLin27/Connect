using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour {
    private ParticleSystem startP;
    private ParticleSystem fireP;
    private ParticleSystem smokeP;

    public bool active;
    // Start is called before the first frame update
    void Start() {
        GameMan.Instance.Bonfires.Add(this);
        startP = transform.Find("StartParticle").GetComponent<ParticleSystem>();
        fireP = transform.Find("FireParticle").GetComponent<ParticleSystem>();
        smokeP = transform.Find("SmokeParticle").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        if (!active && Vector3.Distance(transform.position, GameMan.Instance.PlayerAgent.transform.position) < 3f) {
            GameMan.Instance.ActivateBonfire(this);
        }
    }

    public void Activate() {
        active = true;
        startP.Play();
        fireP.Play();
        smokeP.Play();
    }

    public void Deactivate() {
        active = false;
        startP.Stop();
        fireP.Stop();
        smokeP.Stop();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent.GetComponent<AgentController>() && other.transform.parent.GetComponent<AgentController>().isPlayer) {
            GameMan.Instance.LevelUpUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.parent.GetComponent<AgentController>() && other.transform.parent.GetComponent<AgentController>().isPlayer) {
            GameMan.Instance.LevelUpUI.SetActive(false);
        }
    }
}
