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
            // Если это клон-редактор, автоматически подключаемся к локальному серверу
            // Это может быть полезно для отладки и предварительного просмотра
            NetworkManager.singleton.networkAddress = "localhost";
            NetworkManager.singleton.StartClient();
        }
        else
        {
            // Если это оригинальный редактор, автоматически запускаем сервер
            // Это может быть полезно для запуска серверной части вашей игры
            NetworkManager.singleton.StartHost();
        }
#endif
    }
}
