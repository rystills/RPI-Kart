using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour {
    GUIStyle menuButtonStyle;
    GUIStyle centeredStyle = null;

    float native_width = 1920;
    float native_height = 1080;

    private void Start() {
        menuButtonStyle = new GUIStyle("button");
        menuButtonStyle.fontSize = 24;
    }

    void OnGUI() {
        float rx = Screen.width / native_width;
        float ry = Screen.height / native_height;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

        if (centeredStyle == null) {
            centeredStyle = new GUIStyle(GUI.skin.label);
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.fontSize = 128;
        }

        GUI.Label(new Rect(native_width / 2 - 400, native_height / 2 - 400, 800, 300), "You Win!", centeredStyle);
        if (GUI.Button(new Rect(native_width / 2 - 700, native_height / 2, 200, 100), "Replay", menuButtonStyle))
            SceneManager.LoadScene("gameScene");
        if (GUI.Button(new Rect(native_width / 2 - 100, native_height / 2, 200, 100), "Main Menu", menuButtonStyle))
            SceneManager.LoadScene("menuScene");
        if (GUI.Button(new Rect(native_width / 2 + 500, native_height / 2, 200, 100), "Quit", menuButtonStyle))
            Application.Quit();
    }
}