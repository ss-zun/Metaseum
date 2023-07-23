using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RotateNickName : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = virtualCamera.transform.forward;
    }
}
