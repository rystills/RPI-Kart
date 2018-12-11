using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HTPUI : MonoBehaviour {


    float native_width = 1920;
    float native_height = 1080;

    private void Start() {
        
    }

    void OnGUI() {
        float rx = Screen.width / native_width;
        float ry = Screen.height / native_height;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

        GUIStyle label = new GUIStyle(GUI.skin.label);
        label.fontSize = 128;

        GUIStyle text = new GUIStyle(GUI.skin.label);
        text.fontSize = 64;

        GUI.Label(new Rect(native_width / 2 - 800, native_height / 2 - 450, 800, 300), "How to Play:", label);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 - 300, 800, 300), "Press space to pause/unpause time", text);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 - 100, 800, 300), "Click and drag your units (blue) to move them", text);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 + 100, 800, 300), "You can draw paths for your units to follow while time is paused", text);
        GUI.Label(new Rect(native_width / 2 - 700, native_height / 2 + 375, 800, 300), "Kill all enemies to win", text);


        GUIStyle button = new GUIStyle("button");
        button.fontSize = 24;

        if (GUI.Button(new Rect(native_width / 2 + 300, native_height / 2 - 150, 200, 100), "Start Game", button))
            SceneManager.LoadScene("gameScene");
        if (GUI.Button(new Rect(native_width / 2 + 300, native_height / 2 + 150, 200, 100), "Back", button))
            SceneManager.LoadScene("menuScene");
    }
}