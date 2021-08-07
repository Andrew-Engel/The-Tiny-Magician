using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LootBoxUIAndContents : MonoBehaviour
{
    [Header("Item Icons and Text Setup")]
    [Tooltip("icons for materials. Current set up order : Fiber,Meat,Antiseptic,Blood,Tinder,Water")]
    [SerializeField] Sprite[] itemIcons = new Sprite[6];
    AudioSource _audio;

    Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();
    [SerializeField] TextMeshProUGUI[] materialTitleTexts;
    [SerializeField] TextMeshProUGUI[] materialQuantityTexts;
    [SerializeField] Image[] images;
    [SerializeField] Transform itemListParent;

    [SerializeField] float animationDelay;
    public bool destroyObject = false;
    private InventorySystem _inventorySystem;
    //Two lists to hold what items and how many are in the lootbox
    private string[] lootItemsIncluded = new string[3];
    private int[] lootItemsQuantity = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        SetUpIconDictionary();
        _inventorySystem = GameObject.Find("GameManager").GetComponent<InventorySystem>();
        RunLottery();
        _audio = GetComponent<AudioSource>();
    }

    private void RunLottery()
    {
        for (int i = 0; i < 3; i++)
        {
            ChooseLootItems(i);
            ChooseLootAmount(i);
           
        }
        AssignLootIconsAndText();

    }
    private void SetUpIconDictionary()
    {
        icons.Add("Fiber", itemIcons[0]);
        icons.Add("Meat", itemIcons[1]);
        icons.Add("Antiseptic", itemIcons[2]);
        icons.Add("Blood", itemIcons[3]);
        icons.Add("Tinder", itemIcons[4]);
        icons.Add("Water", itemIcons[5]);

    }
    private void ChooseLootItems(int arrayIndex)
    {
        int lotteryNumber;
        lotteryNumber = Random.Range(1, 6);
        
        switch (lotteryNumber)
        {
            case (1):
                lootItemsIncluded[arrayIndex] = CraftingMaterials.Fiber.ToString();
                break;
            case (2):
                lootItemsIncluded[arrayIndex] = CraftingMaterials.Meat.ToString();
                break;
            case (3):
                lootItemsIncluded[arrayIndex] = CraftingMaterials.Antiseptic.ToString();
                break;
            case (4):
                lootItemsIncluded[arrayIndex] = CraftingMaterials.Blood.ToString();
                break;
            case (5):
                lootItemsIncluded[arrayIndex] = CraftingMaterials.Tinder.ToString();
                break;
            case (6):
                lootItemsIncluded[arrayIndex] = CraftingMaterials.Water.ToString();
                break;
        }


    }
    private void ChooseLootAmount(int arrayIndex)
    {
        int materialAmount;
        materialAmount = Random.Range(1, 3);
        lootItemsQuantity[arrayIndex] = materialAmount;
    }
    private void AssignLootIconsAndText()
    {

        TextMeshProUGUI materialTitleText;
        TextMeshProUGUI materialQuantityText;
        Image iconImage;
        for (int i = 0; i < 3; i++)
        {
            materialTitleText = materialTitleTexts[i];
            materialQuantityText = materialQuantityTexts[i];
            materialTitleText.text = lootItemsIncluded[i];
            materialQuantityText.text = "(" +lootItemsQuantity[i].ToString()+")";


            iconImage = images[i];
            iconImage.sprite = icons[lootItemsIncluded[i]];
        }
     

    }
    public void CollectMaterials()
    {for (int i = 0; i<3; i++)
        {

            if (!destroyObject)
            {
                Debug.Log("soundEffect1");
                _audio.Play();
            }
            int quantity = lootItemsQuantity[i];
            string lootItem = lootItemsIncluded[i];
            int prospectiveMaterialQuantity = quantity + InventorySystem.craftingMaterialAmounts[lootItem];
            if (prospectiveMaterialQuantity <= InventorySystem.maxInventorySize)
            { _inventorySystem.AddCraftingMaterial(lootItem, quantity);
                lootItemsQuantity[i] = 0;
                
            }
            else
            {
                int portionAdded = InventorySystem.maxInventorySize - InventorySystem.craftingMaterialAmounts[lootItem];
                _inventorySystem.AddCraftingMaterial(lootItem, portionAdded);
                lootItemsQuantity[i] = quantity - portionAdded;
            }

        }
 
           StartCoroutine( CraftingMaterialCollectionAnimation());
        

    }
    private IEnumerator CraftingMaterialCollectionAnimation()
    {
    
        for (int i = 0; i < itemListParent.childCount; i++)
        {
            GameObject itemListing = itemListParent.GetChild(i).gameObject;
            if (lootItemsQuantity[i] == 0)
            {
                itemListing.SetActive(false);
               
            }
            else
            {
               
                materialQuantityTexts[i].text = "(" + lootItemsQuantity[i].ToString() + ")";
            }
            yield return new WaitForSeconds(animationDelay);

        }
        if (ActiveChildCount() == 0) destroyObject = true;

    }
    private int ActiveChildCount()
    {
        int n = 0;
        foreach (Transform t in itemListParent)
        {
            if (t.gameObject.activeSelf) n++;
        }
        return n;
    }
}
