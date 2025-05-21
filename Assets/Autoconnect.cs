using Mirror;
#if UNITY_EDITOR
using ParrelSync;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoconnect : MonoBehaviour
{

    void Start()
    {
#if UNITY_EDITOR
        if (ClonesManager.IsClone())
        {
            // ���� ��� ����-��������, ������������� ������������ � ���������� �������
            // ��� ����� ���� ������� ��� ������� � ���������������� ���������
            NetworkManager.singleton.networkAddress = "localhost";
            NetworkManager.singleton.StartClient();
        }
        else
        {
            // ���� ��� ������������ ��������, ������������� ��������� ������
            // ��� ����� ���� ������� ��� ������� ��������� ����� ����� ����
            NetworkManager.singleton.StartHost();
        }
#endif
    }
}
