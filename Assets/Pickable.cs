using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : NetworkBehaviour
{
    public bool reusable = false;
    private void OnCollisionEnter(Collision collision)
    {
        PickUp(collision);
        if (!reusable) Destroy(gameObject);
    }

    protected abstract void PickUp(Collision collision);
}
