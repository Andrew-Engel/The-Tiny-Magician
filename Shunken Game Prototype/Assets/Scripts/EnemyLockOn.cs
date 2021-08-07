using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using DG.Tweening;
using System;
public class EnemyLockOn : MonoBehaviour
{
    [SerializeField] EnemyPresenceCheckSampleScene presenceCheck;
    [SerializeField] Transform playerMainCamTransform, playerBodyTransform, castingTarget;
    [SerializeField] Vector3 castingTargetOriginalPosition;
    [SerializeField] ThirdPersonController thirdPersonController;
    [SerializeField] float lockonRange;
    [SerializeField] int enemyLockonIndex;
   public  List<Transform> enemyTransformsList = new List<Transform>();
    [SerializeField] Animator animator;
    [SerializeField] float lerpStrength = 0.5f;
    public static bool enemiesNearby = false;
    public bool lockedOnEnemy = false;
    [SerializeField] private bool changingLockTarget = false;
    // animation IDs
    private int _animIDSpeed;
    //Player Inputs
    PlayerControls controls;
    void Awake()
    {
        DOTween.Init();
        controls = new PlayerControls();

        controls.Player.SwitchLockon.performed += context => SwitchLockon();
        controls.Player.LockOnEnemy.performed += context => LockOnEnemy();
        // controls.Player.LockOnEnemy.canceled += context => UnlockOnEnemy(); 
        presenceCheck.OnEnemyDeath += CheckForNullsInList;
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
     

    }
    private void LockOnEnemy()
    {
        if (!lockedOnEnemy)
        {
            if (enemyTransformsList[enemyLockonIndex] != null)
            {
                thirdPersonController.LockCameraPosition = true;
                animator.SetLayerWeight(0, 0f);
                animator.SetLayerWeight(2, 1f);
                lockedOnEnemy = true;
            }
            else UnlockOnEnemy();
        }
        else
        {
            thirdPersonController.LockCameraPosition = false;
            animator.SetLayerWeight(0, 1f);
            animator.SetLayerWeight(2, 0f);
            lockedOnEnemy = false;
        }
    }
    private void UnlockOnEnemy()
    {
        animator.SetLayerWeight(0, 1f);
        animator.SetLayerWeight(2, 0f);
        lockedOnEnemy = false;
    }
    void Update()
    {
        Debug.Log($"Enemies in TRansform List: " + enemyTransformsList.Count);
       
       
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (enemyTransformsList.Count == 0) enemiesNearby = false;

        if (lockedOnEnemy)
        {
            if (enemyTransformsList[enemyLockonIndex] != null)
            {
                if (!changingLockTarget)
                {
                    playerMainCamTransform.LookAt(enemyTransformsList[enemyLockonIndex]);
                    playerBodyTransform.LookAt(enemyTransformsList[enemyLockonIndex]);

                    castingTarget.position = enemyTransformsList[enemyLockonIndex].position;
                }
                else if (changingLockTarget)
                {
                    playerMainCamTransform.DOLookAt(enemyTransformsList[enemyLockonIndex].position, lerpStrength);
                    playerBodyTransform.DOLookAt(enemyTransformsList[enemyLockonIndex].position, lerpStrength);
                }

                
              
            }
            else if (enemyTransformsList[enemyLockonIndex] == null)
            {
                enemyTransformsList.Remove(enemyTransformsList[enemyLockonIndex]);
            }
            else
                LockOnEnemy();
        }
        else if (!lockedOnEnemy && (playerBodyTransform.localEulerAngles != Vector3.zero))
        {
            playerBodyTransform.localEulerAngles = Vector3.zero;
            castingTarget.localPosition = castingTargetOriginalPosition;
        }
    }
    void OnTriggerEnter(Collider other)
    {
     
        Collider[] nearbyEnemies = Physics.OverlapSphere(this.transform.position, lockonRange);
        foreach (Collider enemy in nearbyEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemiesNearby = true;
                Transform enemyTransform = enemy.gameObject.GetComponent<Transform>();
                if (!enemyTransformsList.Contains(enemyTransform))
                { enemyTransformsList.Add(enemyTransform); }
            }
        }



       
        


    }
    void OnTriggerExit(Collider other)
    {
        
        
            if (other.tag == "Enemy")
            {
            Debug.Log("Enemy Removed from list");
                Transform enemyTransform = other.gameObject.GetComponent<Transform>();
            if (enemyTransform = enemyTransformsList[enemyLockonIndex])
            {
                
                enemyTransformsList.Remove(enemyTransform);
                enemyTransformsList.TrimExcess();
                if (lockedOnEnemy)
                LockOnEnemy();
            }
            else
            enemyTransformsList.Remove(enemyTransform);
           
            }
        
    }
        private void  SwitchLockon()
    {
        if (enemyLockonIndex < (enemyTransformsList.Count -1))
        { StartCoroutine(ChangeEnemyCamTarget()); }
        else
            enemyLockonIndex = 0;

        Debug.Log(enemyTransformsList.Count);

        
    }
    private IEnumerator ChangeEnemyCamTarget()
    {
        
        changingLockTarget = true;
        Debug.Log("EnemyLockChanged");
        enemyLockonIndex++;
        //Vector3 target = enemyTransformsList[enemyLockonIndex].position;
        //Quaternion targetRotation = Quaternion.LookRotation(this.transform.position - target);

        //float str = Mathf.Min(lerpStrength * Time.deltaTime, 1);
        
        // playerMainCamTransform.rotation = Quaternion.Lerp(playerMainCamTransform.rotation, targetRotation, str);
        // playerBodyTransform.rotation = Quaternion.Lerp(playerBodyTransform.rotation, targetRotation, str);
        yield return new WaitForSeconds(1f);
        changingLockTarget = false;
       

    }
    private void CheckForNullsInList(object sender, EventArgs e)
    {
        StartCoroutine(Check());
    }
    private IEnumerator Check()
    {
        yield return new WaitForSeconds(0.3f);
       
        foreach (Transform t in enemyTransformsList)
        {
            if (t == null) enemyTransformsList.Remove(t);
        }
       
    }
}
