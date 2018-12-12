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
        target = GameObject.FindGameObjectWithTag("Playerunit").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Vector2.Distance(transform.position, target.position) > 3)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        target = GameObject.FindWithTag("Playerunit").transform;

        range = Vector2.Distance(transform.position, target.position);

        transform.LookAt(target.position, Vector3.back);
        transform.Rotate(new Vector3(0, 90, 0), Space.Self);

        if (range < minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

	}
}
