using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using RootMotion.Dynamics;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class AirPathSpells : MonoBehaviour
{
    //Lens stuff
    public Volume volume;
    LensDistortion thisLensDistortion;
    [SerializeField] float distortionTime, distortionIntensity;

    AudioSource audioSource;
    public AudioClip airEscapeSound;
    public PuppetMaster puppet;
    [SerializeField] CinemachineVirtualCamera followCam;
    [SerializeField] float effectFOV = 30f, effectTime = 0.8f;
    [SerializeField] Vector3 groundOffset = new Vector3(0, 3, 0);
    [SerializeField] private float airEscapeDistance = 20f;
    [SerializeField] private float airEscapeTime = 1f;
    [SerializeField] private float airEscapeHeight = 1f;
    //AirDashStats
    [SerializeField] private float airDashDistance = 20f;
    [SerializeField] private float airDashHeight = 0.5f;
    [SerializeField] private float airDashTime = 1f;
    [SerializeField] Transform playerModel;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float delayTime;
    [SerializeField] Vector3 dashGroundOffset = new Vector3(0,0.1f,0);
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator AirEscapeBackwards()
    {
        DOTween.To(() => followCam.m_Lens.FieldOfView, x => followCam.m_Lens.FieldOfView = x, effectFOV, effectTime);
        Vector3 targetEnd = FindLandingTarget(true);
        anim.SetBool("LeapBack", true);
        transform.DOJump(targetEnd,airEscapeHeight, 1 , airEscapeTime);
        puppet.enabled = false;
        FindObjectOfType<AudioManager>().Play("WindSound");
      
        CameraDistortion();
        yield return new WaitForSeconds(airEscapeTime-0.1f);
        
        puppet.enabled = true;
        anim.SetBool("LeapBack", false);
        DOTween.To(() => followCam.m_Lens.FieldOfView, x => followCam.m_Lens.FieldOfView = x, 50f, effectTime);

    }
    private Vector3 FindLandingTarget(bool backward)
    {
        Vector3 groundOffset_t;
        Vector3 targetLocation;
        Vector3 offSet;
        if (backward)
        {  offSet = new Vector3(0, 0, -1 * airEscapeDistance);
            groundOffset_t = groundOffset;
        }
        else
        {
             offSet = new Vector3(0, 0, airDashDistance);
            groundOffset_t = dashGroundOffset;
        }
        
        Vector3 targetLocalPosition = playerModel.localPosition + offSet;
        targetLocation = transform.TransformPoint(targetLocalPosition);
        RaycastHit Hit;

        Debug.DrawRay(targetLocation, Vector3.down, Color.red);
        if (Physics.Raycast(targetLocation, Vector3.up, out Hit, Mathf.Infinity, groundLayer))
        {
            Debug.DrawRay(targetLocation, Vector3.up, Color.blue);
            targetLocation = Hit.point + groundOffset_t;
            Debug.Log("HitUpAir");

        }
        else if (Physics.Raycast(targetLocation, Vector3.down, out Hit, Mathf.Infinity, groundLayer))
        {
            targetLocation = Hit.point + groundOffset_t;
            Debug.Log("HitDown");
        }
        else
        {
            targetLocation += groundOffset_t;
            Debug.Log("No Raycasts Hit");
        }

        return targetLocation;
    }
 
    void CameraDistortion()
    {
        
            
        VolumeProfile profile = volume.sharedProfile;
        volume.profile.TryGet(out thisLensDistortion);
        thisLensDistortion.active = true;
        FloatParameter intensity = thisLensDistortion.intensity;
       
          
            DOTween.Sequence()
               .Append(DOTween.To(() => intensity.value, x => intensity.value = x, -0.6f, 0.3f))
               .AppendInterval(0.3f)
               .Append(DOTween.To(() => intensity.value, x => intensity.value = x, 0f, 0.3f))
               .OnComplete(() =>
               {
                   thisLensDistortion.active = false;
                  
               });
        
    }

    public void AirDashForwards()
    {
        anim.SetTrigger("AirDash");

        Vector3 targetLocalPosition = playerModel.localPosition + new Vector3(0, 0, airDashDistance);
       Vector3  targetLocation = transform.TransformPoint(targetLocalPosition);
        Vector3 targetEnd =CheckForObstruction(targetLocation + dashGroundOffset);
        DOTween.Sequence()
            .AppendInterval(delayTime)
            .OnStepComplete(()=>
        {
            CameraDistortion();
            FindObjectOfType<AudioManager>().Play("WindSound");
        })
            .Append(transform.DOJump(targetEnd, airDashHeight, 1, airDashTime))
            ;

    }
    Vector3 CheckForObstruction(Vector3 defaultVariable)
    {
        RaycastHit hit;
        Debug.DrawRay(playerModel.position, playerModel.forward*airDashDistance, Color.green);
        if (Physics.Raycast(playerModel.position, playerModel.forward, out hit, airDashDistance, groundLayer))
        {
            return hit.point + dashGroundOffset;
            Debug.Log("obstructiondetectedairdash!!!");
        }
        else return defaultVariable;
    }

}

