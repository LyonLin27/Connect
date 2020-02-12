using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    private int epaLen_Normal = 400;

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

    [HideInInspector]
    public GameObject[] forceFieldRedArr;
    public GameObject forceFieldRedPrefab;
    private int forceFieldRedIndex = 0;
    private int forceFieldRedLen = 20;

    [HideInInspector]
    public GameObject[] forceFieldBlueArr;
    public GameObject forceFieldBluePrefab;
    private int forceFieldBlueIndex = 0;
    private int forceFieldBlueLen = 20;

    [HideInInspector]
    public GameObject[] projTracerArr;
    public GameObject projTracerPrefab;
    private int projTracerIndex = 0;
    private int projTracerLen = 40;

    [HideInInspector]
    public GameObject[] projImmuneArr;
    public GameObject projImmunePrefab;
    private int projImmuneIndex = 0;
    private int projImmuneLen = 45;

    [HideInInspector]
    public GameObject[] enemy2Arr;
    public GameObject enemy2Prefab;
    private int enemy2Index = 0;
    private int enemy2Len = 30;

    public GameObject bossObj;
    public GameObject bossWall;


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

        forceFieldRedArr = new GameObject[forceFieldRedLen];
        for (int i = 0; i < forceFieldRedLen; i++)
        {
            forceFieldRedArr[i] = Instantiate(forceFieldRedPrefab, projPtcParent.transform);
            forceFieldRedArr[i].SetActive(false);
        }
        forceFieldBlueArr = new GameObject[forceFieldBlueLen];
        for (int i = 0; i < forceFieldBlueLen; i++)
        {
            forceFieldBlueArr[i] = Instantiate(forceFieldBluePrefab, projPtcParent.transform);
            forceFieldBlueArr[i].SetActive(false);
        }

        projTracerArr = new GameObject[projTracerLen];
        for (int i = 0; i < projTracerLen; i++)
        {
            projTracerArr[i] = Instantiate(projTracerPrefab, projPtcParent.transform);
            projTracerArr[i].SetActive(false);
        }

        projImmuneArr = new GameObject[projImmuneLen];
        for (int i = 0; i < projImmuneLen; i++)
        {
            projImmuneArr[i] = Instantiate(projImmunePrefab, projPtcParent.transform);
            projImmuneArr[i].SetActive(false);
        }

        enemy2Arr = new GameObject[enemy2Len];
        for (int i = 0; i < enemy2Len; i++)
        {
            enemy2Arr[i] = Instantiate(enemy2Prefab, projPtcParent.transform);
            enemy2Arr[i].SetActive(false);
        }

        moneyUI.text = money.ToString();
    }

    private void Update() {

        if(Input.GetKeyDown(KeyCode.Equals)){

            money += 100;

            moneyUI.text = money.ToString();

        }

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
        bossObj.GetComponentInChildren<EnemyFinalBoss>().StopAllCoroutines();
        bossObj.SetActive(false);
        bossWall.SetActive(false);
        Camera.main.GetComponent<CameraController>().enabled = true;
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
        foreach (GameObject obj in forceFieldBlueArr)
        {
            obj.GetComponent<ForceField>().DeactiveForceField();
        }
        foreach (GameObject obj in forceFieldRedArr)
        {
            obj.GetComponent<ForceField>().DeactiveForceField();
        }
        foreach (GameObject obj in enemy2Arr)
        {
            obj.SetActive(false);
        }
    }

    public void OnRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public GameObject GetForceFieldRed()
    {
        forceFieldRedIndex++;
        if (forceFieldRedIndex >= forceFieldRedLen)
        {
            forceFieldRedIndex = 0;
        }
        return forceFieldRedArr[forceFieldRedIndex];
    }

    public GameObject GetForceFieldBlue()
    {
        forceFieldBlueIndex++;
        if (forceFieldBlueIndex >= forceFieldBlueLen)
        {
            forceFieldBlueIndex = 0;
        }
        return forceFieldBlueArr[forceFieldBlueIndex];
    }

    public GameObject GetProjectileTracer()
    {
        projTracerIndex++;
        if (projTracerIndex >= projTracerLen)
        {
            projTracerIndex = 0;
        }
        return projTracerArr[projTracerIndex];
    }

    public GameObject GetProjectileImmune()
    {
        projImmuneIndex++;
        if (projImmuneIndex >= projImmuneLen)
        {
            projImmuneIndex = 0;
        }
        return projImmuneArr[projImmuneIndex];
    }

    public GameObject GetEnemy2()
    {
        enemy2Index++;
        if (enemy2Index >= enemy2Len)
        {
            enemy2Index = 0;
        }
        return enemy2Arr[enemy2Index];
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
