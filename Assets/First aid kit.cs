using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firstaidkit : Pickable
{
    protected override void PickUp(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Heal(3);
        }
    }
}
