using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour {

    public float speed;
    private float waitTime;
    public float startWaitTime;
    public Transform[] moveSpots;
    private int startingSpot;
	// Use this for initialization
	void Start () {
        // Initialize the wait time for the pathing to start
        waitTime = startWaitTime;

        // Choose a random starting spot
        startingSpot = Random.Range(0, moveSpots.Length);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[startingSpot].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpots[startingSpot].position) < 0.2f);
            if(waitTime <= 0)
        {
            startingSpot = Random.Range(0, moveSpots.Length);
            waitTime = startWaitTime;
        }
        else
        {
            // Update our waiting time
            waitTime -= Time.deltaTime;
        }
    }
}
