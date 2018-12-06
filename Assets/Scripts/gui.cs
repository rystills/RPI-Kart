﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gui : MonoBehaviour {

    Texture2D guiBGText;

    // Use this for initialization
    void Start () {
        guiBGText = new Texture2D(1, 1);
        guiBGText.SetPixel(0, 0, new Color(.117f, .29f, .29f, .95f));
        guiBGText.wrapMode = TextureWrapMode.Repeat;
        guiBGText.Apply();        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        GUI.DrawTexture(new Rect(0, 0, screenPos.x * 2, 20), guiBGText);
    }
}