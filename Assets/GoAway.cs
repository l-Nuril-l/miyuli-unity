using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAway : NetworkBehaviour
{
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.H))
        {
            CMDSayGoAway();
        }
    }

    [Command]
    public void CMDSayGoAway()
    {
        SayGoAway();
    }

    [ClientRpc]
    private void SayGoAway()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sounds\\actors\\male\\moe\\aggressive" + UnityEngine.Random.Range(1, 5));
        audioSource.Play();
    }
}
