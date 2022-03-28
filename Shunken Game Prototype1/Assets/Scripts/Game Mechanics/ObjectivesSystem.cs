using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ObjectivesSystem : MonoBehaviour
{
    
    [Tooltip("Match Order of Titles of Prefabs with Order of Prefabs!")]
    [SerializeField] string[] objectiveMenuTitles;
    [Tooltip("Prefabs Instantiated in objecive Menu")]
    [SerializeField] GameObject[] objectiveMenuPrefabsList;
    //Dictionary for instantiation
    public Dictionary<string, GameObject> objectiveMenuPrefabs = new Dictionary<string, GameObject>();

    [Tooltip("Prefabs instantiated on Screen, same order as titles!")]
    [SerializeField] GameObject[] screenOverlayPrefabs;
    public Dictionary<string, GameObject> objectiveScreenOverlayPrefabs = new Dictionary<string, GameObject>();
    [SerializeField] TextMeshProUGUI MissionDescriptionText;

    [SerializeField] Transform MenuTitleParent, ScreenOverlayParent;
    [SerializeField] AudioClip newMissionSound;
    public AudioSource _audio;
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        SetUpMenuPrefabs();
        //SetUpOverlayPrefabs();
    }

    void SetUpMenuPrefabs()
    {
        for (int i = 0;i<objectiveMenuTitles.Length; i++ )
        {
            objectiveMenuPrefabs.Add(objectiveMenuTitles[i], objectiveMenuPrefabsList[i]);
        }
    }

    void SetUpOverlayPrefabs()
    {
        for (int i = 0; i < objectiveMenuTitles.Length; i++)
        {
            objectiveMenuPrefabs.Add(objectiveMenuTitles[i], objectiveMenuPrefabsList[i]);
        }
    }

    public void NewObjective(string title, string descriptionString)
    {
       GameObject menuPrefab = Instantiate(objectiveMenuPrefabsList[0], MenuTitleParent);
        TextMeshProUGUI menuText = menuPrefab.GetComponentInChildren<TextMeshProUGUI>();
        ObjectiveButtonBehavior buttonBehavior = menuPrefab.GetComponentInChildren<ObjectiveButtonBehavior>();
        buttonBehavior.descriptionString = title + "\n" + descriptionString;
        menuText.text = title;
        MissionDescriptionText.text = title + "\n" + descriptionString;
        //  Instantiate(objectiveScreenOverlayPrefabs[title], ScreenOverlayParent);
        GameObject overlay = Instantiate(screenOverlayPrefabs[0], ScreenOverlayParent);
        TextMeshProUGUI overlayText = overlay.GetComponent<TextMeshProUGUI>();
        overlayText.text = title;
        if (_audio == null)
        {
            Debug.Log($"Audiosource missing on GameManager for Objective system!");
            _audio = GetComponent<AudioSource>();
        }
        
            _audio.PlayOneShot(newMissionSound, 1f);
    }
}
