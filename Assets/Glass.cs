using UnityEngine;

public class Glass : Pickable
{
    protected override void PickUp(Collision collision)
    {
        Inventory player = collision.gameObject.GetComponent<Inventory>();
        if (player != null)
        {
            player.Add("Glass");
        }
    }
}
