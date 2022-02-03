using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveMan : MonoBehaviour
{
    public int startWave = 0;
    public int currWave = 0;
    public TextMeshProUGUI waveTxt;
    private List<Enemy> enemyList;
    private bool waveEnd = false;

    public Transform WaveSpawnPoints;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefabAim;
    public Transform frontGate;
    public Transform frontGateClose;
    public Transform secondGate;
    public Transform secondGateClose;
    public BreakableGate bossGate;
    private Vector3 frontGateStartPos;
    private Vector3 secondGateStartPos;
    private GateTrigger frontGateTrigger;
    private GateTrigger secondGateTrigger;
    private bool closeFrontGate = false;
    private bool closeSecondGate = false;

    private bool paused = false;

	private void Awake()
	{
        currWave = startWave;
        enemyList = new List<Enemy>();
        frontGateStartPos = frontGate.position;
        frontGateTrigger = frontGate.GetComponentInChildren<GateTrigger>();
        secondGateStartPos = secondGate.position;
        secondGateTrigger = secondGate.GetComponentInChildren<GateTrigger>();
        bossGate.OnBreak += OnBossGateBreak;
	}

	private void Start() {
        waveTxt.CrossFadeAlpha(0f, 1f, true);
    }

    private void Update() {
        foreach (Enemy enemy in enemyList) {
            if (!enemy.gameObject.activeInHierarchy) {
                enemyList.Remove(enemy);
                Destroy(enemy.gameObject);
                break;
            }
        }
        if (currWave >= 10 || paused) {
            return;
        }
        if (waveEnd && enemyList.Count == 0) {
            waveEnd = false;
            currWave++;
            if (currWave >= 10) {
                return;
            }

            GameMan.Instance.ResetAllProjNicely();

            StartCoroutine("StartWaveAfterTime", currWave);
        }

        if (closeFrontGate) {
            frontGate.position = Vector3.Lerp(frontGate.position, frontGateClose.position, Time.deltaTime*2f);
        }
        if (closeFrontGate && Vector3.Distance(frontGate.position, frontGateClose.position) < 0.1f) {
            frontGate.position = frontGateClose.position;
            closeFrontGate = false;
        }

        if (closeSecondGate)
        {
            secondGate.position = Vector3.Lerp(secondGate.position, secondGateClose.position, Time.deltaTime * 2f);
        }
        if (closeSecondGate && Vector3.Distance(secondGate.position, secondGateClose.position) < 0.1f)
        {
            secondGate.position = secondGateClose.position;
            closeSecondGate = false;
        }
    }

    IEnumerator StartWaveAfterTime(int wave) {
        waveTxt.text = "Wave " + wave;
        waveTxt.CrossFadeAlpha(1f, 1f, true);
        yield return new WaitForSeconds(3f);
        waveTxt.CrossFadeAlpha(0f, 1f, true);
        StartCoroutine("WaveN", currWave);
    }

    IEnumerator WaveN(int n) {
        yield return new WaitForSeconds(2f);
        string wavePointName = "Wave" + n;
        Transform[] spawnPoints = WaveSpawnPoints.Find(wavePointName).GetComponentsInChildren<Transform>();
        foreach (Transform point in spawnPoints) {
            if (point.name == wavePointName) continue;
            yield return new WaitForSeconds(0.5f);
            //GameObject enemy = Instantiate(enemyPrefab1, point.position, point.rotation);
            //enemyList.Add(enemy.GetComponent<Enemy>());
            SpawnEnemyWaveN(n, point.position, point.name);
        }
        waveEnd = true;
    }

    private void SpawnEnemyWaveN(int waveNum, Vector3 pos, string name) {
        GameObject enemy;
        switch (waveNum) {
            case 1:
                enemy = Instantiate(enemyPrefab1);
                break;
            case 2:
                enemy = Instantiate(enemyPrefab1);
                break;
            case 3:
                enemy = Instantiate(enemyPrefab1);
                enemy.GetComponent<EnemyType1>().hp_max = 20;
                enemy.GetComponent<EnemyType1>().fireCD = 0.2f;
                break;
            case 4:
                if (name.Contains("Aim")) {
                    enemy = Instantiate(enemyPrefabAim);
                    enemy.GetComponent<EnemyTypeAim>().hp_max = 20;
                    enemy.GetComponent<EnemyTypeAim>().fireCD = 5f;
                    enemy.GetComponent<EnemyTypeAim>().interval = 0.1f;
                }
                else {
                    enemy = Instantiate(enemyPrefab1);
                    enemy.GetComponent<EnemyType1>().hp_max = 20;
                    enemy.GetComponent<EnemyType1>().fireCD = 0.4f;
                    enemy.GetComponent<EnemyType1>().way = 2;
                }
                break;
            case 5:
                enemy = Instantiate(enemyPrefabAim);
                enemy.GetComponent<EnemyTypeAim>().hp_max = 20;
                enemy.GetComponent<EnemyTypeAim>().fireCD = 4f;
                enemy.GetComponent<EnemyTypeAim>().interval = 0.1f;
                break;
            case 6:
                enemy = Instantiate(enemyPrefab1);
                enemy.GetComponent<EnemyType1>().hp_max = 50;
                enemy.GetComponent<EnemyType1>().fireCD = 0.15f;
                enemy.GetComponent<EnemyType1>().rotSpd = 30f;
                break;
            case 7:
                if (name.Contains("Aim")) {
                    enemy = Instantiate(enemyPrefabAim);
                    enemy.GetComponent<EnemyTypeAim>().hp_max = 30;
                    enemy.GetComponent<EnemyTypeAim>().fireCD = 3f;
                    enemy.GetComponent<EnemyTypeAim>().interval = 0.1f;
                }
                else {
                    enemy = Instantiate(enemyPrefab1);
                    enemy.GetComponent<EnemyType1>().hp_max = 60;
                    enemy.GetComponent<EnemyType1>().fireCD = 0.15f;
                    enemy.GetComponent<EnemyType1>().way = 4;
                    enemy.GetComponent<EnemyType1>().projLife = 5f;
                }
                break;
            case 8:
                enemy = Instantiate(enemyPrefab1);
                enemy.GetComponent<EnemyType1>().hp_max = 60;
                enemy.GetComponent<EnemyType1>().fireCD = 0.1f;
                enemy.GetComponent<EnemyType1>().way = 4;
                enemy.GetComponent<EnemyType1>().projLife = 5f;
                break;
            case 9:
                enemy = Instantiate(enemyPrefabAim);
                enemy.GetComponent<EnemyTypeAim>().hp_max = 70;
                enemy.GetComponent<EnemyTypeAim>().fireCD = 3f;
                enemy.GetComponent<EnemyTypeAim>().ammo = 7;
                enemy.GetComponent<EnemyTypeAim>().interval = 0.1f;
                enemy.GetComponent<EnemyTypeAim>().projLife = 6f;
                break;
            default:
                enemy = Instantiate(enemyPrefab1);
                break;
        }
        enemy.transform.position = pos;
        enemy.GetComponent<Enemy>().value = waveNum * 3;
        enemyList.Add(enemy.GetComponent<Enemy>());
    }

    public void OnRetry(Bonfire bonfire) {
        OnBonfireEnter(bonfire);
    }

    // case1: player breaks boss gate
    //      pause current waves, open gate 1, keep gate 1 trigger off
    //      open gate 3..?
    // case2: player touches bonfire 1
    //      open gate 1, turn on trigger, reset waves, restore boss gate
    // case3: player touches bonfire 2
    //      restore boss gate, open gate 3...?
    // Retry Bonfire 1: reset waves, open gate 1, restore boss gate
    // Retry Bonfire 2: reset waves, close boss gate, open gate 3

    public void OnGateTriggered(int id)
    {
        paused = false;
        if (id == 1)
            OnGate1Triggered();
        else if (id == 2)
            OnGate2Triggered();
    }

    public void OnBossGateBreak()
    {
        PauseWaves();
        OpenGate1(false);
        OpenGate2(true);
    }

    public void OnBonfireEnter(Bonfire bonfire)
    {
        if (bonfire.ID == 1)
        {
            OpenGate1(true);
            ResetWaves();
            RestoreBossGate();
        }
        else if (bonfire.ID == 2)
        {
            OpenGate2(true);
            ResetWaves();
            RestoreBossGate();
        }
    }

    private void ResetWaves()
    {
        if (waveEnd == false && currWave == startWave)
            return;

        paused = false;
        currWave = startWave;
        foreach (Enemy enemy in enemyList)
        {
            enemy.gameObject.SetActive(false);
        }
        waveEnd = false;
    }

    private void PauseWaves()
    {
        paused = true;
        GameMan.Instance.ResetAllProj();
    }

    private void OpenGate1(bool triggerOn)
    {
        frontGate.position = frontGateStartPos;
        if(frontGateTrigger != null)
            frontGateTrigger.SwitchTrigger(triggerOn);
    }

    private void OpenGate2(bool triggerOn)
    {
        secondGate.position = secondGateStartPos;
        if (secondGateTrigger != null)
            secondGateTrigger.SwitchTrigger(triggerOn);
    }

    private void RestoreBossGate()
    {
        bossGate.Restore();
    }

    private void OnGate1Triggered()
    {
        waveEnd = true;
        closeFrontGate = true;
        frontGateTrigger.SwitchTrigger(false);
    }

    private void OnGate2Triggered()
    {
        closeSecondGate = true;
        secondGateTrigger.SwitchTrigger(false);
    }
}
