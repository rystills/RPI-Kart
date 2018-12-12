using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gui : MonoBehaviour {

    Texture2D guiBGText;
    Texture2D playerText;
    Texture2D enemyText;
    Texture2D playText;
    Texture2D pauseText;
    Vector3 screenPos;
    int iconCombineNum = 3;

    // Use this for initialization
    void Start () {
        guiBGText = new Texture2D(1, 1);
        guiBGText.SetPixel(0, 0, new Color(.117f, .29f, .29f, .95f));
        guiBGText.wrapMode = TextureWrapMode.Repeat;
        guiBGText.Apply();

        playerText = Resources.Load<Texture2D>("humanText");
        enemyText = Resources.Load<Texture2D>("enemyText");
        playText = Resources.Load<Texture2D>("playText");
        pauseText = Resources.Load<Texture2D>("pauseText");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // draw unit/enemy icons
    void drawIcons(int num, bool isUnit) {
        int startx = isUnit ? 64 : (int)(screenPos.x * 2) - 74 - 32;
        if (num < iconCombineNum) {
            for (int i = 0; i < num; i++) {
                GUI.DrawTexture(new Rect(startx, 0, 32, 32), isUnit ? playerText : enemyText);
                startx += 32 * (isUnit ? 1 : -1);
            }
        }
        else {
            GUI.DrawTexture(new Rect(startx+(isUnit ? 0 : -32), 0, 32, 32), isUnit ? playerText : enemyText);
            GUI.Label(new Rect(startx+(isUnit ? 32 : 0), 0, 32, 32), "x"+num);
        }
    }

    void OnGUI() {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        GUI.DrawTexture(new Rect(0, 0, screenPos.x * 2, 32), guiBGText);

        //TODO: actually make a text style for this
        GUI.Label(new Rect(10, 0, 64, 32), "Units ");
        GUI.Label(new Rect((screenPos.x * 2)-74, 0, 64, 32), " Enemies");

        //Draw pause/play button
        GUI.DrawTexture(new Rect(screenPos.x - 16, 0, 32, 32), (Time.timeScale == 0) ? pauseText : playText);

        GameObject[] units = GameObject.FindGameObjectsWithTag("PlayerUnit");
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
        //count units and enemies separately

        drawIcons(units.Length,true);
        drawIcons(enemies.Count,false);
    }
}
