using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject playerAgent;
    private Transform cameraHandle;

    private void Start() {
        cameraHandle = playerAgent.transform.Find("CameraHandle");
    }

    private void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, cameraHandle.position, 0.1f);
        transform.LookAt(playerAgent.transform.position);
    }

    public void SwitchPlayer(GameObject player) {
        playerAgent = player;
        cameraHandle = playerAgent.transform.Find("CameraHandle");
    }
}
