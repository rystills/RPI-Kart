using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public bool timeFrozen = false;
	public bool debugShowMousePos = false;
    GameObject[] playerUnits;
    GameObject[] enemyUnits;
    bool showPauseScreen = false;

    float native_width = 1920;
    float native_height = 1080;

    // Use this for initialization
    void Start() {

	}

	void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            bool temp = timeFrozen;
            if (showPauseScreen == false) {
                showPauseScreen = true;
                Time.timeScale = 0;
            }
            else {
                showPauseScreen = false;
                Time.timeScale = temp ? 0 : 1;
            }
        }
        if (showPauseScreen == false) {
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
            playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");

            GameObject[] temp1 = GameObject.FindGameObjectsWithTag("MachineGun");
            GameObject[] temp2 = GameObject.FindGameObjectsWithTag("SemiAuto");
            GameObject[] temp3 = GameObject.FindGameObjectsWithTag("Sniper");

            List<GameObject> enemies = new List<GameObject>();
            foreach (GameObject temp in temp1) {
                enemies.Add(temp);
            }
            foreach (GameObject temp in temp2) {
                enemies.Add(temp);
            }
            foreach (GameObject temp in temp3) {
                enemies.Add(temp);
            }

            if (playerUnits.Length == 0) {
                SceneManager.LoadScene("GameOver");
            }
            if (enemies.Count == 0) {
                SceneManager.LoadScene("WinScene");
            }
        }
    }

    public bool getShowPauseScreen() {
        return showPauseScreen;
    }

    private void OnGUI() {
        float rx = Screen.width / native_width;
        float ry = Screen.height / native_height;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

        GUIStyle text = new GUIStyle(GUI.skin.label);
        text.normal.textColor = Color.black;
        text.fontSize = 64;

        GUIStyle button = new GUIStyle("button");
        button.fontSize = 24;
        if (showPauseScreen) {
            GUI.Label(new Rect(native_width / 2 + 200, native_height / 2 - 200, 800, 300), "Menu", text);
            if (GUI.Button(new Rect(native_width / 2 + 100, native_height / 2 + 300, 200, 100), "Main Menu", button))
                SceneManager.LoadScene("menuScene");
            if (GUI.Button(new Rect(native_width / 2 + 400, native_height / 2 + 300, 200, 100), "Restart", button)) {
                Time.timeScale = 1;
                SceneManager.LoadScene("gameScene");
            }
            if (GUI.Button(new Rect(native_width / 2 + 700, native_height / 2 + 300, 200, 100), "Quit", button))
                Application.Quit();
        }
    }
}
