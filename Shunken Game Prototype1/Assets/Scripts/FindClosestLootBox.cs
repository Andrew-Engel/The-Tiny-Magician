using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestLootBox : MonoBehaviour
{
    public static Transform closestLootBoxTransform;
    Transform playerTransform;
    Transform lootBoxParent;
    List<Transform> lootBoxes = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        lootBoxParent = GetComponent<Transform>();
        
    }
    private void SetUpLootBoxList()
    {
        lootBoxes.Clear();
        for (int i = 0; i < lootBoxParent.childCount; i++ )
        {
            lootBoxes.Add(lootBoxParent.GetChild(i));
        }
    }
    public void  GetClosestLootBox()
    {
        SetUpLootBoxList();
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = playerTransform.position;
        foreach (Transform potentialTarget in lootBoxes)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        closestLootBoxTransform = bestTarget;
    }
}
