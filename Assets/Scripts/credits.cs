using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class credits : MonoBehaviour {

    float native_width = 1920;
    float native_height = 1080;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI() {
        float rx = Screen.width / native_width;
        float ry = Screen.height / native_height;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

        GUIStyle label = new GUIStyle(GUI.skin.label);
        label.fontSize = 128;

        GUIStyle name = new GUIStyle(GUI.skin.label);
        name.fontSize = 96;

        GUI.Label(new Rect(native_width / 2 - 800, native_height / 2 - 500, 800, 300), "Made by: ", label);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 - 350, 800, 300), "Ryan Stillings", name);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 - 200, 800, 300), "Vincent Ferrera", name);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 - 50, 800, 300), "Jordan Alligood", name);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 + 100, 800, 300), "Tim Kim", name);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 + 250, 800, 300), "Tyler Chang", name);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 + 400, 800, 300), "Tommy Olney", name);

        GUIStyle button = new GUIStyle("button");
        button.fontSize = 24;

        if (GUI.Button(new Rect(native_width / 2 + 300, native_height / 2 - 150, 200, 100), "Start Game", button))
            SceneManager.LoadScene("gameScene");
        if (GUI.Button(new Rect(native_width / 2 + 300, native_height / 2 + 150, 200, 100), "Back", button))
            SceneManager.LoadScene("menuScene");
    }
}
