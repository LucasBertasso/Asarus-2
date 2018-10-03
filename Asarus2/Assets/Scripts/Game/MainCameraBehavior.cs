using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehavior : MonoBehaviour {
    [Header("Camera Cofigurations")]

    [Space(5)]
    [Tooltip("Diz em que posição a câmera vai ficar em relação ao personagem")]
    public Vector3 offset;
    [Tooltip("Quem a câmera deve seguir")]
    public Transform inicialTarget;
    [Tooltip("Velocida em que a camera irá se ajustar")]
    public float smoothSpeed = 0.125f;
    [Tooltip("Diz se a camera deve parar na beira")]
    public bool followPlayer = true,EdgeEnable = true;
    [Tooltip("Posição máxima e minima da camera")]
    public Vector3 MaxCam, MinCam;

    // variaveis para camera shake
    public float shakeTime, shakeAmount;

    private void FixedUpdate()
    {
            Vector3 desiredPosition = inicialTarget.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

        if (shakeTime > 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;

            transform.position = new Vector3(transform.position.x + ShakePos.x,
                                             transform.position.y + ShakePos.y,
                                             transform.position.z);
            shakeTime -= Time.deltaTime;
        }

        if (EdgeEnable)
        {
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, MinCam.x, MaxCam.x),
            Mathf.Clamp(transform.position.y, MinCam.y, MaxCam.y),
            Mathf.Clamp(transform.position.z, MinCam.z, MaxCam.z));
        }
        offset = new Vector3(inicialTarget.transform.localScale.x,offset.y,offset.z);

    }
    void ShakeCamera(float shakeTime2)
    {
        shakeTime = 0;
        shakeTime = shakeTime2;
    }
    void ChangeCamera(Transform targetCamera)
    {
        if (followPlayer)
        {
            inicialTarget = targetCamera;
        }
    }
    void ChangeCameraPosition(Vector3 newPos)
    {
        offset = newPos;
    }

}
