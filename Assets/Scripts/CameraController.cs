using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject playerAgent;
    private Transform cameraHandle;
    bool switching = false;

    private void Start() {
        cameraHandle = playerAgent.transform.Find("CameraHandle");
    }

    private void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, cameraHandle.position, 0.1f);
        
        if (Vector3.Distance(transform.position, cameraHandle.position) < 0.2f)
            switching = false;
        if(!switching)
            transform.LookAt(playerAgent.transform.position);
    }

    public void SwitchPlayer(GameObject player) {
        switching = true;
        playerAgent = player;
        cameraHandle = playerAgent.transform.Find("CameraHandle");
    }
}
