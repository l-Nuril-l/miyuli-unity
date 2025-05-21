using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urinate : NetworkBehaviour
{
    public KeyCode interactKey = KeyCode.G;
    public Transform cameraPos;
    public GameObject urineObject;
    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(interactKey))
        {
            CmdUrinate();
        }
    }

    [Command]
    public void CmdUrinate()
    {
        if (urineObject != null && cameraPos != null)
        {
            Urinates();
        }
    }

    private void Urinates()
    {
        var startTime = DateTime.Now;
        StartCoroutine(UrinatesCoroutine(startTime));
    }

    private IEnumerator UrinatesCoroutine(DateTime startTime)
    {
        while (DateTime.Now < startTime.AddSeconds(3))
        {
            // ������� ������ urine � ������������� ��� ��������
            GameObject urine = Instantiate(urineObject, cameraPos.transform.position + cameraPos.transform.forward + cameraPos.transform.up * -1f, Quaternion.identity);
            NetworkServer.Spawn(urine);
            Rigidbody peelRB = urine.GetComponent<Rigidbody>();
            peelRB.linearVelocity = new Vector3(2.5f * cameraPos.forward.x, 6f, 2.5f * cameraPos.forward.z);
            Destroy(urine, 3);

            // ������� 0.3 ������� ����� ��������� ���������
            yield return new WaitForSeconds(0.1f);
        }
    }
}
