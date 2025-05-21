using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<string> items = new List<string>();
    public void Add(string item)
    {
        items.Add(item);
        Debug.Log(item);
    }
}
