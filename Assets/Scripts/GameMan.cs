using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public static GameMan Instance { get; private set; }

    public GameObject PlayerAgent;

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

}
