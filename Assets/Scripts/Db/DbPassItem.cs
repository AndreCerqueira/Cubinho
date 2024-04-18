using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbPassItem
{
    // Variables
    public PassItem passItem;
    public bool isEquipped;
    public bool isUnlocked;


    // Constructor
    public DbPassItem(int passItemId, bool isEquipped, bool isUnlocked)
    {
        this.passItem = IcePassItemsManager.instance.GetItemById(passItemId);
        this.isEquipped = isEquipped;
        this.isUnlocked = isUnlocked;
    }
}
