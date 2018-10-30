using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public bool timeFrozen = false;

	void Update () {
		//freeze/unfreezes time on spacebar
		if (Input.GetKeyDown("space")) {
			timeFrozen = !timeFrozen;
			Time.timeScale = timeFrozen ? 0 : 1;
		}
	}
}
