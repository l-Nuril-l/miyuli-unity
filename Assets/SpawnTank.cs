using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTank : NetworkBehaviour
{
    public GameObject tankPrefab;
    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            CmdSpawnTank();
        }
    }

    [Command]
    public void CmdSpawnTank()
    {
        GameObject tank = Instantiate(tankPrefab, transform.position + transform.forward, Quaternion.identity);
        NetworkServer.Spawn(tank);
        Destroy(tank, 100);
    }
}
