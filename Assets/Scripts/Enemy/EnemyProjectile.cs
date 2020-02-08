using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    protected GameMan gameMan;
    protected Rigidbody rb;
   
    public bool working = false;
    public float lifeTime = 5f;

    // Start is called before the first frame update
    protected void Awake()
    {
        gameMan = GameMan.Instance;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update() {
        if(working)
            Work();
    }

    public virtual void StartWork()
    {
        working = true;
        gameObject.SetActive(true);
        StartCoroutine("DisableAfterTime", lifeTime);
    }

    protected virtual void Work() {
    
    }

    public virtual void FinishWork()
    {
        working = false;
        gameObject.SetActive(false);
    }

    protected void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Agent") ||
            coll.gameObject.layer == LayerMask.NameToLayer("PlayerProj")) {
            
            GameObject ptc = GameMan.Instance.GetProjPtcRd();
            ptc.transform.position = transform.position;
            ptc.GetComponent<ParticleSystem>().Play();
            FinishWork();
        }
    }

    protected IEnumerator DisableAfterTime(float time) {
        yield return new WaitForSeconds(time);
        FinishWork();
    }
}
