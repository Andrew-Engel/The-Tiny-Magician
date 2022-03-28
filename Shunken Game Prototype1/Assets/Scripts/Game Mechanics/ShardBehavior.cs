using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardBehavior : MonoBehaviour
{
   public GameObject audioSourceObject;
    public AudioClip pickUpSound;
    bool active = true;
    DemoObjective objective;
    // Start is called before the first frame update
    void Start()
    {
        objective = GameObject.Find("Stone").GetComponent<DemoObjective>();
       
    }

    private void OnTriggerEnter (Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && active)
        {

            StartCoroutine(CollectShard());
          
        }
    }
    void SpawnAudioSource()
    {
       GameObject newObject = Instantiate(audioSourceObject, this.transform.position, Quaternion.identity);
      AudioSource  audioSource = newObject.GetComponent<AudioSource>();
        audioSource.clip = pickUpSound;
        audioSource.Play();

    }
    IEnumerator CollectShard()
    {
        objective.fragmentsCollected++;
        SpawnAudioSource();
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
}
