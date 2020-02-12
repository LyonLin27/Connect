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
    private Vector3 gateStartPos;
    private bool closeGate = false;

    private void Start() {
        currWave = startWave;
        enemyList = new List<Enemy>();
        waveTxt.CrossFadeAlpha(0f, 1f, true);
        gateStartPos = frontGate.position;
    }

    private void Update() {
        foreach (Enemy enemy in enemyList) {
            if (!enemy.gameObject.activeInHierarchy) {
                enemyList.Remove(enemy);
                Destroy(enemy.gameObject);
                break;
            }
        }
        if (currWave >= 10) {
            return;
        }
        if (waveEnd && enemyList.Count == 0) {
            waveEnd = false;
            currWave++;
            if (currWave >= 10) {
                return;
            }
            StartCoroutine("StartWaveAfterTime", currWave);
        }
        if (closeGate) {
            frontGate.position = Vector3.Lerp(frontGate.position, frontGateClose.position, Time.deltaTime*2f);
        }
        if (closeGate && Vector3.Distance(frontGate.position, frontGateClose.position) < 0.1f) {
            frontGate.position = frontGateClose.position;
            closeGate = false;
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

    public void OnRetry() {
        currWave = startWave;
        foreach (Enemy enemy in enemyList) {
            enemy.gameObject.SetActive(false);
        }
        waveEnd = false;
        this.GetComponent<Collider>().enabled = true;
        frontGate.position = gateStartPos;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent")
            && other.GetComponentInParent<AgentController>().isPlayer) {
            waveEnd = true;
            closeGate = true;
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
