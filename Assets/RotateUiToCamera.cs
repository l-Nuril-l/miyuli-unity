using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUiToCamera : MonoBehaviour
{
    public RectTransform rectTransform;
    private void Update()
    {
        rectTransform.rotation = Camera.main.transform.rotation;
    }
}
