using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItemBehavior : MonoBehaviour
{
    InventorySystem inventorySystem;
    Button button;
    [SerializeField] string whatTypeOfItemIsThis;
    [SerializeField] int whatLevelIsThisItem;
    // Start is called before the first frame update
    void Awake()
    {
        inventorySystem = GameObject.Find("GameManager").GetComponent<InventorySystem>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }
    private void OnButtonClicked()
    {
        UseItem();
    }
    private void UseItem()
    {
        switch (whatTypeOfItemIsThis)
        {
            case ("Health"):
                inventorySystem.UseHealthItem(whatLevelIsThisItem);
                break;
            case ("Mana"):
                inventorySystem.UseManaItem(whatLevelIsThisItem);
                break;
            case ("Stamina"):
                inventorySystem.UseStaminaItem(whatLevelIsThisItem);
                break;
        }
    }
}
