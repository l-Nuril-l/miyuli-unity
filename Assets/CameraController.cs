using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public float sensX = 250;
    public float sensY = 250;
    public float mobileSens = 10;

    float xRotation;
    float yRotation;

    private Transform orientation;

    public bool IsCursorMode;
    public bool NoclipSupport = false;

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        orientation = transform.Find("Orientation");
        IsCursorMode = PlatformChecker.IsMobilePlatform();
        if (!IsCursorMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    int? cameraFingerId = null;

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SwitchCursorMode();
        }

        if (PlatformChecker.IsMobilePlatform())
        {
            // Перебираем все активные касания
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // Если палец только что коснулся и он справа — запоминаем его fingerId
                if (touch.phase == TouchPhase.Began && touch.position.x > Screen.width / 2)
                {
                    cameraFingerId = touch.fingerId;
                }

                // Если текущий тач — тот, что контролирует камеру, и он движется — вращаем камеру
                if (cameraFingerId != null && touch.fingerId == cameraFingerId && touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.deltaPosition;

                    yRotation += delta.x * Time.deltaTime * mobileSens;
                    xRotation -= delta.y * Time.deltaTime * mobileSens;
                    xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                }

                // Если палец отпущен — сбрасываем
                if (touch.phase == TouchPhase.Ended && touch.fingerId == cameraFingerId)
                {
                    cameraFingerId = null;
                }
            }

        }

        else
        {
            if (IsCursorMode) return;
            // get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
        }
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        CmdRotateCamAndOther(xRotation, yRotation);
    }

    public void SwitchCursorMode()
    {
        IsCursorMode = !IsCursorMode;
        if (!IsCursorMode)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //[Command]
    private void CmdRotateCamAndOther(float xRotation, float yRotation)
    {
        Camera.main.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(NoclipSupport ? xRotation : 0, yRotation, 0);
    }
}
