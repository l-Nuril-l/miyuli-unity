using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : NetworkBehaviour
{

    public bool firstPerson = true;
    private Camera mainCamera;

    private void Start()
    {
        // Кешируем камеру один раз при старте
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found!");
        }
    }

    private void Update()
    {
        if (!isLocalPlayer || mainCamera == null) return;

        // Переключение камеры по кнопке R
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleCameraMode();
        }

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetPosition;

        if (firstPerson)
        {
            targetPosition = transform.position;
        }
        else
        {
            // Камера на 5 единиц позади игрока (по направлению назад)
            targetPosition = transform.position - transform.forward * 5f;
        }

        // Можно сделать плавное движение, раскомментировав следующую строку:
        // mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 10f);

        // Если плавность не нужна — сразу ставим позицию:
        mainCamera.transform.position = targetPosition;
    }

    public void ToggleCameraMode()
    {
        firstPerson = !firstPerson;
    }
}
