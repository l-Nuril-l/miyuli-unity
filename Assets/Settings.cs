using UnityEngine;

public class Settings : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 143;
    }

}
