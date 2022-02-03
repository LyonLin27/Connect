using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public int ID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent")
            && other.GetComponentInParent<AgentController>().isPlayer)
        {
            GameMan.Instance.OnGateTriggered(ID);
        }
    }

    public void SwitchTrigger(bool on)
    {
        gameObject.SetActive(on);
    }
}
