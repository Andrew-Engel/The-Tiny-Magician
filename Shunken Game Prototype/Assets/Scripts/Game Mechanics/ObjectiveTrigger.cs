using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectiveTrigger : MonoBehaviour
{
    bool triggerUsed = false;
    [SerializeField] string objectiveTitle;
    ObjectivesSystem objectivesSystem;
    [SerializeField] string descriptionString;
    // Start is called before the first frame update
    void Start()
    {
        objectivesSystem = GameObject.Find("GameManager").GetComponent<ObjectivesSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerUsed)
        {
            Debug.Log("ObjectiveTrigger");
            objectivesSystem.NewObjective(objectiveTitle, descriptionString);
            triggerUsed = true;
            Destroy(this.gameObject);
        }
    }

}
