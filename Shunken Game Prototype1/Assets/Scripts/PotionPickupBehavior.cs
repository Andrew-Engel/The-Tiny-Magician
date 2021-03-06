using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPickupBehavior : MonoBehaviour
{
    private InventorySystem inventory;

    [SerializeField] int itemLevel = 1;
    [SerializeField] string itemType;

    //sounds
    [SerializeField] private AudioClip pickUpSound;
    AudioSource _audio;

    // private AudioSource itemPickupSound;
    void Start()
    {

        _audio = GetComponent<AudioSource>();
        inventory = GameObject.Find("GameManager").GetComponent<InventorySystem>();

    }


    //1
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "PlayerModel")
        {

            _audio.PlayOneShot(pickUpSound, 1f);
            if (inventory == null) inventory = GameObject.Find("GameManager").GetComponent<InventorySystem>();
            inventory.AddItem(itemType, itemLevel);
            Destroy(this.gameObject);


        }
    }
}
