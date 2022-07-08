using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothFactor;

    private void FixedUpdate()
    {
        cameraFollow();
    }

    void cameraFollow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, target.position, smoothFactor * Time.fixedDeltaTime);
        transform.position = target.position + offset;
    }
}
