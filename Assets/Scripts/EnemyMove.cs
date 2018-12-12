using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

	public float moveSpeed;
    public float stoppingDistance;
    public float minDistance;
    private float range;

	private Transform target;
	// Use this for initialization
	void Start () {

        // Set the target to the player
        target = GameObject.FindGameObjectWithTag("PlayerUnit").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

        // If the enemy's distance is a certain value, move towards the player
        if(Vector2.Distance(transform.position, target.position) > 3)
        {
            // Move enemy from their position to the player's position at a rate of moveSpeed
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        // Keep setting the target to the player
        target = GameObject.FindWithTag("Playerunit").transform;

        range = Vector2.Distance(transform.position, target.position);
        // Have the enemy look at the player
        transform.LookAt(target.position, Vector3.back);
        transform.Rotate(new Vector3(0, 90, 0), Space.Self);

        if (range < minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

	}
}
