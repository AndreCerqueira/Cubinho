using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IcePassManager : MonoBehaviour
{
    // Variables
    public static IcePassManager instance;
    public List<PassItem> items => IcePassItemsManager.instance.items;
    public List<DbPassItem> dbItems => IcePassItemsManager.instance.dbItems;
    private int currentHatId = 0;
    private int currentPageIndex = 0;

    [SerializeField] private GameObject hatSlot;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private Image nameImage;
    [SerializeField] private Image nameDeco;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button lockedButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button equipedButton;


    void Start()
    {
        instance = this;

        currentPageIndex = dbItems.FindIndex(x => x.isEquipped);
        ChangePage(currentPageIndex);
    }
    

    public void ReturnToGameScene() => SceneManager.LoadScene(0);

    
    public void NextHat()
    {
        if (currentPageIndex == items.Count - 1)
            currentPageIndex = 0;
        else
            currentPageIndex++;
        
        ChangePage(currentPageIndex);
    }

    
    public void PreviousHat()
    {
        if (currentPageIndex == 0)
            currentPageIndex = items.Count - 1;
        else
            currentPageIndex--;

        ChangePage(currentPageIndex);
    }

    
    public void ChangePage(int pageIndex)
    {
        // item
        var dbItem = dbItems[pageIndex];
        var item = dbItem.passItem;

        // set new name
        nameLabel.text = item.name;
        levelLabel.text = item.level.ToString();

        // remove all objects in hatSlot
        foreach (Transform child in hatSlot.transform)
            Destroy(child.gameObject);

        // instantiate item prefab in hatSlot
        if (item.prefab != null)
            Instantiate(item.prefab, hatSlot.transform);

        nameImage.sprite = item.color.GetLabelSpriteByColor();
        nameDeco.sprite = item.color.GetLabelDecoSpriteByColor();

        // set buttons
        lockedButton.gameObject.SetActive(!dbItem.isUnlocked);
        selectButton.gameObject.SetActive(dbItem.isUnlocked && !dbItem.isEquipped);
        equipedButton.gameObject.SetActive(dbItem.isUnlocked && dbItem.isEquipped);
    }


    // Equip Hat
    public void EquipHat()
    {
        List<DbPassItem> dbItems = IcePassItemsManager.instance.dbItems;
        
        // Unequip the current hat
        int previousPageIndex = dbItems.FindIndex(x => x.isEquipped);
        dbItems[previousPageIndex].isEquipped = false;

        // Equip the new hat
        dbItems[currentPageIndex].isEquipped = true;

        // Save the new hat id
        currentHatId = dbItems[currentPageIndex].passItem.itemId;
        PlayerPrefsManager.hatEquipedId = currentHatId;

        // Refresh page
        ChangePage(currentPageIndex);
    }
}
