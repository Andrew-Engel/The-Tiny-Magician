using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class FirstPersonCam : MonoBehaviour
{
    Vector2 move;
  //  public MagicCasting magic;
    public Animator animator;
    public Vector3 nextPosition;
    public Quaternion nextRotation;
    public float rotationLerp;
    PlayerControls controls;
    Vector2 look;
    //Sensitivity adjustment
    public Slider slider;
   
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public float sensitivity = 0.5f;
    GameBehavior gameBehavior;
    PlayerBehavior playerBehavior;
    
    public Transform cameraFollowTarget, aimCameraFollowTarget;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;
 

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Look.performed += context => look = context.ReadValue<Vector2>();
        controls.Player.Move.performed += context => move = context.ReadValue<Vector2>();
      
        controls.Player.Move.canceled += context => move = Vector2.zero;
        //    controls.Player.Look.performed += context => TurningAnimation();
        controls.Player.Look.canceled += context => look = Vector2.zero;
     //   controls.Player.Look.canceled += context => StopTurningAnimation();
    }


    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("CameraSensitivity", 0.6f);
        //slider.value = sensitivity; 
        gameBehavior = GameObject.Find("GameManager").GetComponent<GameBehavior>();
        playerBehavior = GameObject.Find("Player").GetComponent<PlayerBehavior>();
       
    }
   
    void LateUpdate()
    {

 

        
        if (!MagicCasting.castingAimedSpells )

        {
          
            cameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(look.x * sensitivity, Vector3.up);
            cameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(look.y * sensitivity * -1, Vector3.right);
            var angles = cameraFollowTarget.transform.localEulerAngles;
            angles.z = 0;
            var leftRightAngles = cameraFollowTarget.transform.localEulerAngles.y;
            var angle = cameraFollowTarget.transform.localEulerAngles.x;
            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }
            cameraFollowTarget.transform.localEulerAngles = angles;


            nextRotation = Quaternion.Lerp(cameraFollowTarget.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

            if (move == Vector2.zero)
            {
                nextPosition = transform.position;
                return;

            }



            float moveSpeed = sensitivity / 100f;
            Vector3 position = (transform.forward * look.y * moveSpeed) + (transform.right * look.x * moveSpeed);
            nextPosition = transform.position + position;
            transform.rotation = Quaternion.Euler(0, cameraFollowTarget.transform.rotation.eulerAngles.y, 0);
            cameraFollowTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        }
        if (MagicCasting.castingAimedSpells)
        {
            aimCameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(look.x * sensitivity, Vector3.up);
            aimCameraFollowTarget.transform.rotation *= Quaternion.AngleAxis(look.y * sensitivity * -1, Vector3.right);
            var angles = aimCameraFollowTarget.transform.localEulerAngles;
            angles.z = 0;
            var leftRightAngles = aimCameraFollowTarget.transform.localEulerAngles.y;
            var angle = aimCameraFollowTarget.transform.localEulerAngles.x;
            if (angle > 100 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 100 && angle > 40)
            {
                angles.x = 40;
            }
            aimCameraFollowTarget.transform.localEulerAngles = angles;


            nextRotation = Quaternion.Lerp(aimCameraFollowTarget.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);
            float moveSpeed = sensitivity / 100f;
            Vector3 position = (transform.forward * look.y * moveSpeed) + (transform.right * look.x * moveSpeed);
            nextPosition = transform.position + position;
            transform.rotation = Quaternion.Euler(0, aimCameraFollowTarget.transform.rotation.eulerAngles.y, 0);
            aimCameraFollowTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        }
      


       
    }
        
      
    
}
