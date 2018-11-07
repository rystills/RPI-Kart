using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public bool timeFrozen = false;
	public bool debugShowMousePos = false;

	// Use this for initialization
	void Start() {

	}

	void Update() {
		//freeze/unfreezes time on spacebar
		if (Input.GetKeyDown("space")) {
			timeFrozen = !timeFrozen;
			Time.timeScale = timeFrozen ? 0 : 1;
		}
		if (debugShowMousePos) {
			Vector3 scaledMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//force paths to stay within .1f of the camera
			scaledMousePos.x = Mathf.Max(Mathf.Min(scaledMousePos.x, 10.3f), -10.3f);
			scaledMousePos.y = Mathf.Max(Mathf.Min(scaledMousePos.y, 4.9f), -4.9f);
			Debug.Log("mouse position: " + scaledMousePos);
		}
	}
}
