using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectiveTrigger : MonoBehaviour
{
    public string playerModelName = "PlayerModel";
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
        if (other.gameObject.name == playerModelName && !triggerUsed)
        {
            Debug.Log("ObjectiveTrigger");
            if (objectivesSystem == null) objectivesSystem = GameObject.Find("GameManager").GetComponent<ObjectivesSystem>();
            objectivesSystem.NewObjective(objectiveTitle, descriptionString);
            triggerUsed = true;
            Destroy(this.gameObject);
        }
    }

}
