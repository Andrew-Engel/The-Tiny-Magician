using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SafetyNet : MonoBehaviour
{
    [SerializeField] float moveTime,jumpPower;
    [SerializeField] Vector3 offSet;
    [SerializeField] LayerMask ragDollLayer,playerLayer;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform startingLocation;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"safety Net encounterred:" + collision.gameObject.name);
        playerTransform.DOJump(startingLocation.position, jumpPower, 1, moveTime);
        /*
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == ragDollLayer || collision.gameObject.layer == playerLayer)
        {
            playerTransform.DOJump(startingLocation.position, jumpPower, 1, moveTime);
        }*/
    }
  /*  private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"safety Net encounterred:" + other.gameObject.name);
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == ragDollLayer)
        {
            playerTransform.DOMove(startingLocation.TransformPoint(startingLocation.position), moveTime);
            //playerTransform.position = startingLocation.TransformPoint(startingLocation.position);
        }
    }*/
  
}
