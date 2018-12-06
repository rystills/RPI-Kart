using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gui : MonoBehaviour {

    Texture2D guiBGText;
    Texture2D playerText;
    Texture2D enemyText;
    Texture2D playText;
    Texture2D pauseText;


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

    void OnGUI() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        GUI.DrawTexture(new Rect(0, 0, screenPos.x * 2, 32), guiBGText);

        //TODO: actually make a text style for this
        GUI.Label(new Rect(10, 0, 64, 32), "Units ");
        GUI.Label(new Rect((screenPos.x * 2)-74, 0, 64, 32), " Enemies");

        //Draw pause/play button
        GUI.DrawTexture(new Rect(screenPos.x - 16, 0, 32, 32), (Time.timeScale == 0) ? pauseText : playText);

        var startx = 64;
        GameObject[] unitsAndEnemies = GameObject.FindGameObjectsWithTag("PlayerUnit");
        //count units and enemies separately
        int numUnits = 0;
        int numEnemies = 0;
        for (int i = 0; i < unitsAndEnemies.Length; i++) {
            if (unitsAndEnemies[i].layer == 9) { //9 = player layer
                numUnits++;
            }
            else numEnemies++;
        }

        for (int i = 0; i < numUnits; i++) {
            GUI.DrawTexture(new Rect(startx, 0, 32, 32), playerText);
            startx += 32;
        }

        startx = (int)(screenPos.x * 2) - 74 - 32;
        for (int i = 0; i < numEnemies; i++) {
            GUI.DrawTexture(new Rect(startx, 0, 32, 32), enemyText);
            startx -= 32;
        }

        //TODO corner case for if there are more units than the screen can fit
        //Should conver to just one unit texture then "x #" next to it
    }
}
