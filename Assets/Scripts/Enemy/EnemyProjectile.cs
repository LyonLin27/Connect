using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    protected GameMan gameMan;
    protected Rigidbody rb;
   
    public bool working = false;
    public float lifeTime = 5f;

    private Vector3 defaultScale;
    private bool deactivating = false;

    // Start is called before the first frame update
    protected void Awake()
    {
        gameMan = GameMan.Instance;
        rb = GetComponent<Rigidbody>();
        defaultScale = transform.localScale;
    }

    protected virtual void Update() {
        if(working)
            Work();
    }

    public virtual void StartWork()
    {
        StopAllCoroutines();
        deactivating = false;
        working = true;
        transform.localScale = defaultScale;
        gameObject.SetActive(true);
        StartCoroutine("DisableAfterTime", lifeTime);
        StartCoroutine("ShrinkAfterTime", lifeTime-0.5f);
    }

    protected virtual void Work() {
        if (deactivating) {
            transform.localScale = transform.localScale - defaultScale * Time.deltaTime * 2.0f;
        }
    }

    public virtual void FinishWorkNicely()
    {
        if (gameObject.activeInHierarchy && working && !deactivating)
        {
            StopAllCoroutines();
            StartCoroutine("DisableAfterTime", 0.5f);
            StartCoroutine("ShrinkAfterTime", 0f);
        }
    }

    protected virtual void FinishWork()
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

    protected IEnumerator ShrinkAfterTime(float time) {
        yield return new WaitForSeconds(time);
        deactivating = true;
        rb.velocity = Vector3.zero;
    }
}
