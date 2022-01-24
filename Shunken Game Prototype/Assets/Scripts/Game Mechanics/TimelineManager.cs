using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;


public class TimelineManager : MonoBehaviour
{
    [SerializeField] float walkingTime = 8f;
    [SerializeField] GameObject finishDemoPrompt;
    public GameObject UI;
    private PlayableDirector director;
    //For special events triggered in timeline
    [SerializeField] Cinemachine.CinemachineImpulseSource impulseSource;
    public Transform startLocation, objectiveTransform, characterTransform;
    // Start is called before the first frame update
    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;
    }
 
    private void Director_Stopped(PlayableDirector obj)
    {
        UI.SetActive(true);
    }
   private void Director_Played(PlayableDirector obj)
    {
        UI.SetActive(false);
    }
    public void StartTimeline()
    {
        
        director.Play();
        finishDemoPrompt.SetActive(true);
    }
    public void PutCharacterInPosition()
    {
        characterTransform.DOLookAt(startLocation.position, 0.5f);
        characterTransform.DOMove(startLocation.position, walkingTime);
    }
    public void FaceObjective()
    {
        characterTransform.DOLookAt(objectiveTransform.position, 1f);
    }
    public void CameraShake()
    {
        
        impulseSource.GenerateImpulse(Camera.main.transform.forward);
    }
}
