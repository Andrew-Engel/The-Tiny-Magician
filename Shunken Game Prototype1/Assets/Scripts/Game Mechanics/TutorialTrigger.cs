using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{

    public Vector3 promptLocation;
    public string messageText;
    [SerializeField] TutorialBehavior tutorialBehavior;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& !TutorialBehavior.promptOpen)
        {
            tutorialBehavior.TriggerPrompt(promptLocation,messageText);
            Destroy(this.gameObject);
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && TutorialBehavior.promptOpen)
        {
            tutorialBehavior.DestroyPrompt();
        }
    }
}
