using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMan : MonoBehaviour
{
    public int startWave = 0;
    public int currWave = 0;
    private List<Enemy> enemyList;
    private bool waveEnd;

    private void Start() {
        currWave = startWave;
    }
    private void Update() {
        if (waveEnd && enemyList.Count == 0) {
            waveEnd = false;
            currWave++;
            StartCoroutine("Wave" + currWave);
        }
    }

    IEnumerator Wave1() {
        yield return new WaitForSeconds(2f);


    }
}
