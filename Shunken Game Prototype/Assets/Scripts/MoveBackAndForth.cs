using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.transform.position;
    }

   private void Move()
    {
        Vector3 movementDirection = Mathf.Sin(Time.time)* movementVector;
      //  this.transform.position = startingPosition + Vector3.one *  Mathf.PingPong(5f,5f);
        this.transform.Translate(movementDirection);
    }
    void Update()
    {
        if (Time.timeScale ==1f)
        Move();
    }
}
