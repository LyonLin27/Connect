using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    private Enemy[] EnemyChildren;

    // Start is called before the first frame update
    void Start()
    {
        GameMan.Instance.EnemyRooms.Add(this);
        EnemyChildren = transform.GetComponentsInChildren<Enemy>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent")) {
            foreach (Enemy enemy in EnemyChildren) {
                enemy.Activate();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent")) {
            foreach (Enemy enemy in EnemyChildren) {
                enemy.Deactivate();
            }
        }
    }

    public void ResetEnemy() {
        foreach (Enemy enemy in EnemyChildren) {
            enemy.Reset();
        }
    }
}
