using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawMainMenu : MonoBehaviour {
	GUIStyle menuButtonStyle;
	GUIStyle centeredStyle = null;

	private void Start() {
		menuButtonStyle = new GUIStyle("button");
		menuButtonStyle.fontSize = 24;
	}

	void OnGUI() {
		//GUI functions can only run in OnGUI, because...Unity?
		if (centeredStyle == null) {
			centeredStyle = new GUIStyle(GUI.skin.label);
			centeredStyle.alignment = TextAnchor.UpperCenter;
			centeredStyle.fontSize = 72;
		}
		GUI.Label(new Rect(Screen.width / 2-200, 120,400,100),"RPI-Kart", centeredStyle);
		if (GUI.Button(new Rect(120, 400, 134, 28), "Start Game", menuButtonStyle))
			Application.LoadLevel("gameScene");
	}
}
