using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SpellInventoryUI : MonoBehaviour
{ 
    private SpellEquipping spellEquipping;
    private Button button;
    [SerializeField] string spellType;

    // Start is called before the first frame update
    void Start()
    {
        spellEquipping = GameObject.Find("GameManager").GetComponent<SpellEquipping>();
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        spellEquipping.EquipSpell(spellType);
        Debug.Log("Button Clicked");
        Destroy(this.gameObject);
    }
 
}
