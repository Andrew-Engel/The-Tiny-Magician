using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardBehavior : MonoBehaviour
{
    AudioSource audioSource;
    bool active = true;
    DemoObjective objective;
    // Start is called before the first frame update
    void Start()
    {
        objective = GameObject.Find("Stone").GetComponent<DemoObjective>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && active)
        {
            audioSource.Play();
            objective.fragmentsCollected++;
            Destroy(this.gameObject);
        }
    }
}
