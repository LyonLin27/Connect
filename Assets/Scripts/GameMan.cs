using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public static GameMan Instance { get; private set; }

    public GameObject PlayerAgent;
    [HideInInspector]
    public List<GameObject> Agents;

    [HideInInspector]
    public GameObject[] playerProjArr;
    public GameObject playerProjParent;
    public GameObject playerProjPrefab;
    private int playerProjIndex = 0;
    private int ppaLen = 200;

    public GameObject enemyProjParent;
    
    [HideInInspector]
    public GameObject[] enemyProjArr_Normal;
    public GameObject enemyProjPrefab_Normal;
    private int enemyProjIndex_Normal = 0;
    private int epaLen_Normal = 300;

    [HideInInspector]
    public GameObject[] projPtcWtArr;
    public GameObject projPtcParent;
    public GameObject projPtcWtPrefab;
    private int projPtcWtIndex = 0;
    private int ppwaLen = 20;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        Agents = new List<GameObject>();
        Agents.Add(PlayerAgent);

        playerProjArr = new GameObject[ppaLen];
        for (int i = 0; i < ppaLen; i++) {
            playerProjArr[i] = Instantiate(playerProjPrefab, playerProjParent.transform);
            playerProjArr[i].SetActive(false);
        }

        enemyProjArr_Normal = new GameObject[epaLen_Normal];
        for (int i = 0; i < epaLen_Normal; i++)
        {
            enemyProjArr_Normal[i] = Instantiate(enemyProjPrefab_Normal, enemyProjParent.transform);
            enemyProjArr_Normal[i].SetActive(false);
        }

        projPtcWtArr = new GameObject[ppwaLen];
        for (int i = 0; i < ppwaLen; i++) {
            projPtcWtArr[i] = Instantiate(projPtcWtPrefab, projPtcParent.transform);
        }
    }

    public void SwitchPlayer() {
        if (Agents.Count == 0) {
            print("Game Over");
            return;
        }
        PlayerAgent = Agents[0];
        PlayerAgent.GetComponent<AgentController>().isPlayer = true;

        foreach (GameObject agent in Agents) {
            agent.GetComponent<AgentController>().UpdateFollowTarget();
        }
        Camera.main.GetComponent<CameraController>().SwitchPlayer(PlayerAgent);
    }

    public GameObject GetPlayerProj() {
        playerProjIndex++;
        if (playerProjIndex >= ppaLen) {
            playerProjIndex = 0;
        }
        return playerProjArr[playerProjIndex];
    }

    public GameObject GetEnemyProj(int projectileType)
    {
        switch (projectileType)
        {
            case 0: //Normal Projectile
                enemyProjIndex_Normal++;
                if (enemyProjIndex_Normal >= epaLen_Normal) {
                    enemyProjIndex_Normal = 0;
                }
                return enemyProjArr_Normal[enemyProjIndex_Normal];
            default:
                return null;
        }
    }

    public GameObject GetProjPtcWt() {
        projPtcWtIndex++;
        if (projPtcWtIndex >= ppwaLen) {
            projPtcWtIndex = 0;
        }
        return projPtcWtArr[projPtcWtIndex];
    }

}
