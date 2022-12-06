using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreData
{
    public int price;
    public string resourceName;
    public string itemName;

    public StoreData (string resource, string name, int amount)
    {
        resourceName = resource;
        itemName = name;
        price = amount;
    }
}
