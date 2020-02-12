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

    private void Start() {
        currWave = startWave;
        enemyList = new List<Enemy>();
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
        if (waveEnd && enemyList.Count == 0) {
            waveEnd = false;
            currWave++;

            StartCoroutine("StartWaveAfterTime", currWave);
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
                    enemy.GetComponent<EnemyTypeAim>().fireCD = 3f;
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
                enemy = Instantiate(enemyPrefab1);
                enemy.GetComponent<EnemyType1>().hp_max = 25;
                enemy.GetComponent<EnemyType1>().fireCD = 0.4f;
                enemy.GetComponent<EnemyType1>().way = 2;
                break;
            default:
                enemy = Instantiate(enemyPrefab1);
                break;
        }
        enemy.transform.position = pos;
        enemyList.Add(enemy.GetComponent<Enemy>());

    }

    /**
    IEnumerator Wave1() {
        yield return new WaitForSeconds(2f);
        Transform[] spawnPoints = WaveSpawnPoints[0].GetComponentsInChildren<Transform>();
        foreach (Transform point in spawnPoints) {
            if (point == WaveSpawnPoints[0]) continue;
            yield return new WaitForSeconds(0.5f);
            GameObject enemy = Instantiate(enemyPrefab1, point.position, point.rotation);
            enemyList.Add(enemy.GetComponent<Enemy>());
        }
        waveEnd = true;
    }

    IEnumerator Wave2() {
        yield return new WaitForSeconds(2f);
        Transform[] spawnPoints = WaveSpawnPoints[1].GetComponentsInChildren<Transform>();
        foreach (Transform point in spawnPoints) {
            if (point == WaveSpawnPoints[1]) continue;
            yield return new WaitForSeconds(0.5f);
            GameObject enemy = Instantiate(enemyPrefab1, point.position, point.rotation);
            enemyList.Add(enemy.GetComponent<Enemy>());
        }
        waveEnd = true;
    }

    IEnumerator Wave3() {
        yield return new WaitForSeconds(2f);
        Transform[] spawnPoints = WaveSpawnPoints[2].GetComponentsInChildren<Transform>();
        foreach (Transform point in spawnPoints) {
            if (point == WaveSpawnPoints[2]) continue;
            yield return new WaitForSeconds(0.5f);
            GameObject enemy = Instantiate(enemyPrefab1, point.position, point.rotation);
            enemy.GetComponent<EnemyType1>().hp_max = 20;
            enemy.GetComponent<EnemyType1>().fireCD = 0.2f;
            enemyList.Add(enemy.GetComponent<Enemy>());
        }
        waveEnd = true;
    }**/

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent")) {
            waveEnd = true;
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
