using Mirror;
using UnityEngine;

public class NetworkTest : NetworkBehaviour
{
    //Юнити скрипт при нажатии на Y в аудиосоурс воспроизводится звук

    public AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //audioSource.clip = Resources.Load<AudioClip>("Sounds\\actors\\male\\moe\\aggressive" + UnityEngine.Random.Range(1, 5));
        //audioSource.Play();
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log(isServer + "Local");
            Cmd();
            CmdSetPlayerAuthority(GameObject.Find("Box").GetComponent<NetworkIdentity>());
        }
    }

    [Command]
    private void Cmd()
    {
        Debug.Log(isServer + "Command");
        Rpc();
    }

    [Command]
    private void CmdSetPlayerAuthority(NetworkIdentity playerIdentity)
    {
        if (playerIdentity != null)
        {
            playerIdentity.AssignClientAuthority(connectionToClient);
        }
    }

    [ClientRpc]
    private void Rpc()
    {
        Debug.Log(isServer + "RPC" + netId);
    }
}
