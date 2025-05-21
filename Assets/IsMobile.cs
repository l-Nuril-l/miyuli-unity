using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlatformChecker.IsMobilePlatform()) gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
