using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class AirPathSpells : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera followCam;
    [SerializeField] float effectFOV = 30f, effectTime = 0.8f;
    [SerializeField] Vector3 groundOffset = new Vector3(0, 3, 0);
    [SerializeField] private float airEscapeDistance = 20f;
    [SerializeField] private float airEscapeTime = 1f;
    [SerializeField] private float airEscapeHeight = 1f;
    [SerializeField] Transform playerModel;
    [SerializeField] LayerMask groundLayer;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public IEnumerator AirEscapeBackwards()
    {
        DOTween.To(() => followCam.m_Lens.FieldOfView, x => followCam.m_Lens.FieldOfView = x, effectFOV, effectTime);
        Vector3 targetEnd = FindAirEscapeTarget();
        anim.SetBool("LeapBack", true);
        transform.DOJump(targetEnd,airEscapeHeight, 1 , airEscapeTime);
        yield return new WaitForSeconds(airEscapeTime);
        anim.SetBool("LeapBack", false);
        DOTween.To(() => followCam.m_Lens.FieldOfView, x => followCam.m_Lens.FieldOfView = x, 50f, effectTime);

    }
    private Vector3 FindAirEscapeTarget()
    {
        Vector3 targetLocation;
        Vector3 offSet = new Vector3(0, 0, -1*airEscapeDistance);
        Vector3 targetLocalPosition = playerModel.localPosition + offSet;
        targetLocation = transform.TransformPoint(targetLocalPosition);
        RaycastHit Hit;
  
        if (Physics.Raycast(targetLocation, Vector3.up, out Hit, Mathf.Infinity,groundLayer))
        {
            Debug.DrawRay(targetLocation, Vector3.up, Color.blue);
            targetLocation = Hit.point + groundOffset;
            Debug.Log("HitUpAir");
            
        }
        else if (Physics.Raycast(targetLocation, Vector3.down, out Hit, Mathf.Infinity, groundLayer))
            {
                Debug.DrawRay(targetLocation, Vector3.down, Color.red);
                targetLocation = Hit.point + groundOffset;
            Debug.Log("HitUpDown");
        }

        return targetLocation;
    }
}
