using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 0, 60 * Time.deltaTime);
		transform.Translate(new Vector2(2 * Time.deltaTime, 3 * Time.deltaTime), Space.World);
	}
}
