using Mirror;
using Mirror.Examples.Pong;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowCrystal : NetworkBehaviour
{
    public GameObject crystalPrefab; // ?????? ?????????
    public Transform cameraPos; // ??????? ??????
    public float throwForce = 10f; // ???? ??????
    public float throwMaxDistance = 500f;
    public float animDistance = 1f;
    public float moveDuration = 1f; // ???????????? ??????????? ?????????
    public float destroyDelay = 3f; // ???????? ????? ???????????? ?????????
    public LayerMask obstacleLayer; // ???? ???????????

    private bool isThrowing = false;
    private Transform orientation;

    private void Start()
    {
        cameraPos = transform.Find("CameraPos");
        orientation = transform.Find("Orientation");
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.Q) && !isThrowing) // ?????? ????????????? ??????? Q ??? ?????? ????????? (?????? ???????? ?? ?????? ???????)
        {
            CmdThrow();
        }
    }

    [Command]
    public void CmdThrow()
    {
        if (!isServer)
        {
            return;
        }
        if (crystalPrefab != null && cameraPos != null)
        {
            GameObject crystal = Instantiate(crystalPrefab, cameraPos.position - cameraPos.forward, Quaternion.identity, transform);
            NetworkServer.Spawn(crystal);
            RpcThrow(crystal);
        }
    }

    //[ClientRpc]
    private void ChangeParent(GameObject crystal, bool state)
    {
        // crystal.transform.parent = state ? transform : null;
        // if (state == false) Debug.Log(DateTime.Now.Millisecond);
    }

    [ClientRpc]
    private void RpcThrow(GameObject crystal)
    {
        NetworkIdentity ballIdentity = crystal.GetComponent<NetworkIdentity>();
        crystal.transform.parent = transform;
        StartCoroutine(MoveCrystal(crystal));
        Destroy(crystal, moveDuration + destroyDelay);
    }

    private IEnumerator MoveCrystal(GameObject crystal)
    {
        Vector3 startPosition = crystal.transform.position;
        Vector3 endPosition = cameraPos.position + cameraPos.right * 1f;

        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition, endPosition);
        Rigidbody crystalRB = crystal.GetComponent<Rigidbody>();
        crystalRB.isKinematic = true;

        while (crystal != null && Time.time - startTime < moveDuration)
        {
            float distanceCovered = (Time.time - startTime) / moveDuration;
            float fractionOfJourney = distanceCovered / journeyLength;

            Vector3 offset = Quaternion.Euler(0, 90 * Time.deltaTime, 0f) * (crystal.transform.position - cameraPos.position);
            crystal.transform.position = cameraPos.position + offset;

            yield return null;
        }

        if (crystal != null)
        {

            crystalRB.isKinematic = false;

            RaycastHit hit;

            Vector3 throwDirection = orientation.forward * throwForce;
            if (Physics.Raycast(cameraPos.position, orientation.forward, out hit, throwMaxDistance))
            {
                throwDirection = (hit.point - crystal.transform.position).normalized;
            }

            //ChangeParent(crystal, false);
            crystal.transform.parent = null;

            crystalRB.interpolation = RigidbodyInterpolation.Interpolate;
            crystalRB.linearVelocity = throwDirection.normalized * throwForce;

            isThrowing = false;
        }
    }

}