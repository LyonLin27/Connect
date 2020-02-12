using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonBoss : MonoBehaviour
{
    public GameObject bossObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Agent") && bossObj.activeInHierarchy== false)
        {
            bossObj.SetActive(true);
            bossObj.GetComponent<EnemyFinalBoss>().StartBoss();
        }
    }
}
