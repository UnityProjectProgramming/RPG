using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewChanger : MonoBehaviour
{
    public enum BlendType { BlendIn, BlendOut};

    [SerializeField] Camera mainCamera;
    [SerializeField] float smoothSpeed;
    [SerializeField] GameObject bleendingPosition;
    [SerializeField] BlendType blendType;

    Vector3 cameraOriginalPos;

    private void Start()
    {
        cameraOriginalPos = mainCamera.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if(blendType == BlendType.BlendIn && other.tag == "Player")
        {
            Debug.Log("Enterd");
            StartBlendingIn();
        }
        else if(blendType == BlendType.BlendOut && other.tag == "Player")
        {
            StartBlendingOut();
        }
    }

    void StartBlendingIn()
    {
        Vector3 smoothedPosition = Vector3.Lerp(bleendingPosition.transform.position, cameraOriginalPos, smoothSpeed * Time.deltaTime);
        mainCamera.transform.position = smoothedPosition;
    }

    void StartBlendingOut()
    {
        Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, bleendingPosition.transform.position, smoothSpeed * Time.deltaTime);
        mainCamera.transform.position = smoothedPosition;
    }
}
