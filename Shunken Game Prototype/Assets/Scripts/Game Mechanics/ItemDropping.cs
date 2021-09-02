using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropping : MonoBehaviour
{
    [SerializeField] LayerMask whatIsGroundLayer;
    [SerializeField] GameObject[] healthPickupItems, manaPickupItems, staminaPickupItems;
    [SerializeField] private GameObject lootBox;
   [SerializeField] private Transform lootBoxParent;
   [SerializeField] private int  lotteryNumberRange= 3;

    // Start is called before the first frame update
    void Start()
    {
        lootBoxParent = GameObject.Find("LootBoxParent").GetComponent<Transform>();
        Debug.Log("Start");
    }
    public void RunPotionLottery()
    {
        Vector3 dropPoint = FindBestPlaceToDrop();
        int lotteryNumber = 3;
      //lotteryNumber = Random.Range(0,lotteryNumberRange);
        Debug.Log($"lottery running for potions. Lottery number: " + lotteryNumber);
        switch (lotteryNumber)
        {
            case (0):
                lotteryNumber = Random.Range(0, 6);
                switch (lotteryNumber)
                {
                    case (1):
                    case (2):
                    case (3):
                        Instantiate(healthPickupItems[0], dropPoint, Quaternion.identity);
                        break;
                    case (4):
                    case (5):
                        Instantiate(healthPickupItems[1], dropPoint, Quaternion.identity);
                        break;
                    case (6):
                        Instantiate(healthPickupItems[2], dropPoint, Quaternion.identity);
                        break;
                }
                break;
            case (1):
                lotteryNumber = Random.Range(0, 6);
                switch (lotteryNumber)
                {
                    case (1):
                    case (2):
                    case (3):
                        Instantiate(manaPickupItems[0], dropPoint, Quaternion.identity);
                        break;
                    case (4):
                    case (5):
                        Instantiate(manaPickupItems[1], dropPoint, Quaternion.identity);
                        break;
                    case (6):
                        Instantiate(manaPickupItems[2], dropPoint, Quaternion.identity);
                        break;
                }
                break;
            case (2):
                lotteryNumber = Random.Range(0, 6);
                switch (lotteryNumber)
                {
                    case (1):
                    case (2):
                    case (3):
                        Instantiate(staminaPickupItems[0], dropPoint, Quaternion.identity);
                        break;
                    case (4):
                    case (5):
                        Instantiate(staminaPickupItems[1], dropPoint, Quaternion.identity);
                        break;
                    case (6):
                        Instantiate(staminaPickupItems[2], dropPoint, Quaternion.identity);
                        break;
                }
                break;
            case (3):
                Instantiate(lootBox, dropPoint, Quaternion.identity, lootBoxParent);
    
                break;

        }
       
        
    }
    private Vector3 FindBestPlaceToDrop()
    {
        RaycastHit hitUp;
        RaycastHit hitDown;
        Physics.Raycast(this.transform.parent.position, Vector3.up, out hitUp, Mathf.Infinity, whatIsGroundLayer);
        Physics.Raycast(this.transform.parent.position, Vector3.down, out hitDown, Mathf.Infinity, whatIsGroundLayer);
        if (hitDown.point != null)
        {
            Debug.Log("hitdonw");
            return hitDown.point;
        }
        else if (hitUp.point != null)
        {
            Debug.Log("hitup");
            return hitUp.point;
        }
        else return this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
