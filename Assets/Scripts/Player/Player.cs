using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject hatSlot;

    // Start is called before the first frame update
    void Start()
    {
        int hatId = PlayerPrefsManager.hatEquipedId;

        Debug.Log("Hat id: " + hatId);
        Debug.Log("ITEMS: " + IcePassItemsManager.instance.items);
        PassItem item = IcePassItemsManager.instance.GetItemById(hatId);
        GameObject prefab = item.prefab;
        // Instantiate the hat
        if (prefab != null)
            Instantiate(prefab, hatSlot.transform); // GameObject hat = 
        // hat.transform.localPosition = Vector3.zero;
        // hat.transform.localRotation = Quaternion.identity;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
