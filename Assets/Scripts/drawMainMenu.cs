using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class drawMainMenu : MonoBehaviour {
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

        //GUI functions can only run in OnGUI, because...Unity?
        if (centeredStyle == null) {
            centeredStyle = new GUIStyle(GUI.skin.label);
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.fontSize = 128;
        }
        GUI.Label(new Rect(native_width / 2 - 400, native_height / 2 - 400, 800, 300), "RPI-Kart", centeredStyle);
        if (GUI.Button(new Rect(native_width / 2 - 300, native_height / 2, 200, 100), "Start Game", menuButtonStyle)) {
            Time.timeScale = 1;
            SceneManager.LoadScene("gameScene");
        }
        if (GUI.Button(new Rect(native_width / 2 - 300, native_height / 2 + 200, 200, 100), "How to Play", menuButtonStyle))
            SceneManager.LoadScene("HTP");
        if (GUI.Button(new Rect(native_width / 2 + 200, native_height / 2, 200, 100), "Credits", menuButtonStyle))
            SceneManager.LoadScene("credits");
        if (GUI.Button(new Rect(native_width / 2 + 200, native_height / 2 + 200, 200, 100), "Quit", menuButtonStyle))
            Application.Quit();
    }
}
