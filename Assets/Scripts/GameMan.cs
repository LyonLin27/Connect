using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMan : MonoBehaviour
{
    public static GameMan Instance { get; private set; }

    public GameObject AgentPrefab;
    public GameObject PlayerAgent;
    [HideInInspector]
    public List<GameObject> Agents;

    public Bonfire LastBonfire;
    [HideInInspector]
    public List<Bonfire> Bonfires;
    [HideInInspector]
    public List<EnemyRoom> EnemyRooms;
    public GameObject GameOverUI;
    public GameObject LevelUpUI;
    public TextMeshProUGUI moneyUI;
    public int money = 0;
    public WaveMan waveMan;

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

    [HideInInspector]
    public GameObject[] projPtcRdArr;
    public GameObject projPtcRdPrefab;
    private int projPtcRdIndex = 0;
    private int ppraLen = 20;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Bonfires = new List<Bonfire>();
        EnemyRooms = new List<EnemyRoom>();
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

        projPtcRdArr = new GameObject[ppraLen];
        for (int i = 0; i < ppraLen; i++) {
            projPtcRdArr[i] = Instantiate(projPtcRdPrefab, projPtcParent.transform);
        }

        moneyUI.text = money.ToString();
    }

    public void SwitchPlayer() {
        if (Agents.Count == 0) {
            GameOverUI.SetActive(true);
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

    public void ActivateBonfire(Bonfire bonfire) {
        LastBonfire = bonfire;
        foreach(Bonfire bf in Bonfires) {
            bf.Deactivate();
        }
        bonfire.Activate();

        foreach (EnemyRoom er in EnemyRooms) {
            er.ResetEnemy();
        }
    }

    public void OnRetry() {
        Vector3 bonfirePos = LastBonfire.transform.position;
        float playerY = PlayerAgent.transform.position.y;
        PlayerAgent.GetComponent<AgentController>().isPlayer = false;
        PlayerAgent = Instantiate(AgentPrefab);
        PlayerAgent.transform.position = new Vector3(bonfirePos.x, playerY, bonfirePos.z - 3f);
        PlayerAgent.GetComponent<AgentController>().ReviveAsPlayer();
        GameOverUI.SetActive(false);
        waveMan.OnRetry();

        foreach (EnemyRoom er in EnemyRooms) {
            er.ResetEnemy();
        }

        foreach (GameObject proj in enemyProjArr_Normal) {
            proj.SetActive(false);
        }
    }

    public GameObject GetPlayerProj() {
        playerProjIndex++;
        if (playerProjIndex >= ppaLen) {
            playerProjIndex = 0;
        }
        return playerProjArr[playerProjIndex];
    }

    public GameObject GetEnemyProj(int projectileType, float lifeTime = 8f)
    {
        switch (projectileType)
        {
            case 0: //Normal Projectile
                enemyProjIndex_Normal++;
                if (enemyProjIndex_Normal >= epaLen_Normal) {
                    enemyProjIndex_Normal = 0;
                }
                enemyProjArr_Normal[enemyProjIndex_Normal].GetComponent<EnemyProjectile>().lifeTime = lifeTime;
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

    public GameObject GetProjPtcRd() {
        projPtcRdIndex++;
        if (projPtcRdIndex >= ppraLen) {
            projPtcRdIndex = 0;
        }
        return projPtcRdArr[projPtcRdIndex];
    }

    public void AddMoney(int value) {
        money += value;
        moneyUI.text = money.ToString();
    }

    public void SpendMoney(int value) {
        money -= value;
        moneyUI.text = money.ToString();
    }
}
