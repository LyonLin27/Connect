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
    void Awake()
    {
        gameMan = GameMan.Instance;
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
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

    IEnumerator DisableAfterTime(float time) {
        yield return new WaitForSeconds(time);
        FinishWork();
    }
}
