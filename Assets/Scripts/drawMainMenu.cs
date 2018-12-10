using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawMainMenu : MonoBehaviour {
	GUIStyle menuButtonStyle;
	GUIStyle centeredStyle = null;
	Texture2D texture;

	private void Start() {
		menuButtonStyle = new GUIStyle("button");
		menuButtonStyle.fontSize = 24;

		//create random gradient texture at runtime
		texture = new Texture2D(3, 3, TextureFormat.ARGB32, false);
		for (int i = 0; i < 9; ++i) {
			texture.SetPixel(i%3, (int)(i/3), new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f)));
		}
		texture.filterMode = FilterMode.Trilinear;
		texture.Apply();
	}

	void OnGUI() {
		//GUI functions can only run in OnGUI, because...Unity?
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),texture,ScaleMode.ScaleAndCrop,true);
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
