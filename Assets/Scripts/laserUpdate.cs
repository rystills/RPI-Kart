using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserUpdate : MonoBehaviour {
	float transparency = 1;
	float fadeSpeed = 1;
	public bool amFriendly = false;

	// Update is called once per frame
	void Update () {
		transparency -= Time.deltaTime;
		Color color = this.GetComponent<MeshRenderer>().material.color;
		color.a -= Time.deltaTime * fadeSpeed;
		//color code lasers
		if (amFriendly) color.g = 255; else color.r = 255;
		this.GetComponent<MeshRenderer>().material.color = color;
		if (transparency <= 0) {
			Destroy(transform.parent.gameObject);
		}
	}
}
