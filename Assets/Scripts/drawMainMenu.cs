using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class drawMainMenu : MonoBehaviour {
    GUIStyle centeredStyle = null;
	Texture2D texture;

	float native_width = 1920;
    float native_height = 1080;

    private void Start() {
		//create random gradient texture at runtime
		texture = new Texture2D(3, 3, TextureFormat.ARGB32, false);
		for (int i = 0; i < 9; ++i) {
			texture.SetPixel(i % 3, (int)(i / 3), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
		}
		texture.filterMode = FilterMode.Trilinear;
		texture.Apply();
	}

    void OnGUI() {
		//update texture colors by random amounts
		for (int i = 0; i < 9; ++i) {
			Color curColor = texture.GetPixel(i % 3, (int)(i / 3));
			texture.SetPixel(i % 3, (int)(i / 3), new Color(curColor.r + Random.Range(-.002f, .002f), curColor.g + Random.Range(-.002f, .002f), curColor.b + Random.Range(-.002f, .002f)));
		}
		texture.Apply();
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture, ScaleMode.ScaleAndCrop, true);

		float rx = Screen.width / native_width;
        float ry = Screen.height / native_height;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

        GUIStyle button = new GUIStyle("button");
        button.fontSize = 24;

		//GUI functions can only run in OnGUI, because...Unity?
		if (centeredStyle == null) {
            centeredStyle = new GUIStyle(GUI.skin.label);
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.fontSize = 128;
        }
        GUI.Label(new Rect(native_width / 2 - 400, native_height / 2 - 400, 800, 300), "RPI-Kart", centeredStyle);
        if (GUI.Button(new Rect(native_width / 2 - 300, native_height / 2, 200, 100), "Start Game",button)) {
            Time.timeScale = 1;
            SceneManager.LoadScene("gameScene");
        }
        if (GUI.Button(new Rect(native_width / 2 - 300, native_height / 2 + 200, 200, 100), "How to Play", button))
            SceneManager.LoadScene("HTP");
        if (GUI.Button(new Rect(native_width / 2 + 200, native_height / 2, 200, 100), "Credits",button))
            SceneManager.LoadScene("credits");
        if (GUI.Button(new Rect(native_width / 2 + 200, native_height / 2 + 200, 200, 100), "Quit",button))
            Application.Quit();
    }
}
