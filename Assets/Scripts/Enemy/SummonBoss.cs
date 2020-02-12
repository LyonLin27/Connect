using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonBoss : MonoBehaviour
{
    public GameObject bossObj;
    public GameObject bossWall;
    public Vector3 bossWallPosition;
    bool wallMoving;
    // Start is called before the first frame update
    void Start()
    {
        bossWallPosition = bossWall.transform.position;
       
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (wallMoving)
        {
            
            if (Mathf.Abs(bossWallPosition.z- bossWall.transform.position.z) <= 2f)
            {
                bossWall.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

                wallMoving = false;
            }
            else
            {
                bossWall.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 1f);
            }
        }
        else
        {
            bossWall.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Agent") && bossObj.activeInHierarchy== false)
        {
            bossObj.SetActive(true);
            bossObj.GetComponent<EnemyFinalBoss>().StartBoss();
            float minZ = Mathf.Infinity;
            for (int i = 0; i< GameMan.Instance.Agents.Count; i++)
            {
                if (GameMan.Instance.Agents[i].transform.position.z < minZ)
                {
                    minZ = transform.position.z;
                }
            }
            bossWall.SetActive(true);
            if (minZ < bossWallPosition.z)
            {
                bossWall.transform.position = new Vector3(bossWall.transform.position.x,
                bossWall.transform.position.y, minZ - 1f);
                wallMoving = true;
            }
            else
            {
                bossWall.transform.position = bossWallPosition - new Vector3(0, 0, 2f);
                wallMoving = true;
            }
          
        }
    }
}
