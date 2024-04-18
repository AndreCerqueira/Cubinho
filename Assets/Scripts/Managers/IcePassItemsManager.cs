using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IcePassItemsManager : MonoBehaviour
{
    // Variables
    public static IcePassItemsManager instance;
    public List<PassItem> items;
    public List<DbPassItem> dbItems;

    
    void Awake()
    {
        instance = this;

        // Create a list of db items with the same length as the items list
        dbItems = new List<DbPassItem>();
        for (int i = 0; i < items.Count; i++)
        {
            bool isUnlocked = PlayerPrefsManager.currentLevel >= items[i].level;
            dbItems.Add(new DbPassItem(items[i].itemId, false, isUnlocked));
        }

        // Sort items by level
        dbItems.Sort((x, y) => x.passItem.level.CompareTo(y.passItem.level));

        // Select the selected hat
        dbItems.Where(x => x.passItem.itemId == PlayerPrefsManager.hatEquipedId).FirstOrDefault().isEquipped = true;
    }
    

    public PassItem GetItemById(int id) => items.Find(x => x.itemId == id);
}
