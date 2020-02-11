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

    public Transform[] WaveSpawnPoints;
    public GameObject enemyPrefab1;

    private void Start() {
        currWave = startWave;
        enemyList = new List<Enemy>();
        waveTxt.CrossFadeAlpha(0f, 1f, true);
    }
    private void Update() {
        foreach (Enemy enemy in enemyList) {
            if (!enemy.gameObject.activeInHierarchy) {
                enemyList.Remove(enemy);
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
        StartCoroutine("Wave" + currWave);
    }

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

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent")) {
            waveEnd = true;
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
