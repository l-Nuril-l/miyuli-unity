using UnityEngine;

public class PlayerAdmin : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    public FlyCamera flyCameraScript;
    public CameraController cameraControllerScript;
    public bool Noclip;

    private void Start()
    {
        //playerMovementScript = transform.GetComponent<PlayerMovement>();
        //flyCameraScript = transform.GetComponent<FlyCamera>();
    }

    private void Update()
    {
        // При нажатии клавиши F2 меняем состояние скриптов.
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SwitchNoclip();
        }
    }

    public void SwitchNoclip()
    {
        Noclip = !Noclip;
        playerMovementScript.enabled = !Noclip;
        flyCameraScript.enabled = Noclip;
        cameraControllerScript.NoclipSupport = Noclip;
    }

}
