using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SpellEquipping : MonoBehaviour
{
    public static string[] spellsEquiped = new string[]{"Nothing","Nothing","Nothing"};
    [SerializeField] private GameObject[] iconsEquiped;
    [SerializeField] Transform spellIconsAvailableParent, spellIconsEquippedParent;
    [SerializeField] List<GameObject> spellIconsList = new List<GameObject>();
    SkillTreeSystem skillTreeSystem;
    Dictionary<string, GameObject> spellIcons = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        SetUpIconDictionary();
        skillTreeSystem = GameObject.Find("GameManager").GetComponent<SkillTreeSystem>();
        skillTreeSystem.OnSpellUnlock += SkillTreeSystem_OnSpellUnlock;
    }
    private void SetUpIconDictionary()
    {
        for (int i = 0; i < spellIconsList.Count; i++ )
        {
            spellIcons.Add(spellIconsList[i].name.ToString(), spellIconsList[i]);
           
        }
    }
    private void SkillTreeSystem_OnSpellUnlock(object sender, SkillTreeSystem.OnSpellUnlockEventArgs e)
    {
        if (e.addToInventory)
        AddIconToUnlocked(e.spellToBeUnlocked);
    }
    private void AddIconToUnlocked(string spellName)
    {
        
        Instantiate(spellIcons[spellName], spellIconsAvailableParent);
    }
    public void EquipSpell(string spellName)
    {
        GameObject lastSpellOnList;
        string iconString = spellsEquiped[spellsEquiped.Length - 1];
        lastSpellOnList = spellIcons[iconString];

        for (int i = spellsEquiped.Length - 1; i > 0; i--)
        {
          
            spellsEquiped[i] = spellsEquiped[i - 1];
            if (spellsEquiped[i] == "Nothing") spellsEquiped[i] =spellName;
            GameObject equippedIcon = spellIconsEquippedParent.GetChild(i).gameObject;
            Destroy(equippedIcon);
            GameObject newIcon = spellIcons[spellsEquiped[i]];
           Transform instantiatedIcon = Instantiate(newIcon, spellIconsEquippedParent).transform;
            Button spellButtonOne = instantiatedIcon.gameObject.GetComponent<Button>();
            spellButtonOne.interactable = false;
            instantiatedIcon.SetSiblingIndex(i);

        }
        if (spellIconsAvailableParent.Find(lastSpellOnList.name + "(Clone)") == null && lastSpellOnList.name != "Nothing" && !spellsEquiped.Contains(iconString))
        { Instantiate(lastSpellOnList, spellIconsAvailableParent); }
        spellsEquiped[0] = spellName;
        Destroy(spellIconsEquippedParent.GetChild(0).gameObject);
       Transform firstIcon = Instantiate(spellIcons[spellsEquiped[0]], spellIconsEquippedParent).transform;
        Button spellButton = firstIcon.gameObject.GetComponent<Button>();
        spellButton.interactable = false;
        firstIcon.SetAsFirstSibling();

    }
    
}
