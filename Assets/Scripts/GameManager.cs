using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public bool timeFrozen = false;
	public Transform wall;

	private void Start() {
		//quick and dirty map load (sorry guys, Unity didn't seem to support loading JSON into a generic Dict?)
		string mapData = (Resources.Load("DemoMap") as TextAsset).text;
		//gather wall vert data into lines
		int firstBracketPos = mapData.IndexOf('[');
		int secondBracketPos = mapData.IndexOf(']',firstBracketPos+1);
		string vertString = mapData.Substring(firstBracketPos, secondBracketPos - firstBracketPos);
		string[] verts = vertString.Split('\n');
		List<Vector2> vertsPos = new List<Vector2>();
		//extract wall vert data into array of Vector2s
		for (int i = 0; i < verts.Length; ++i) {
			string tv = verts[i].Trim();
			if (tv.Length > 0 && tv[0] == '(') {
				string[] splitTv = tv.Trim('(').Trim(',').Trim(')').Split(',');
				vertsPos.Add(new Vector2(float.Parse(splitTv[0]),float.Parse(splitTv[1])));
			}
		}
		//gather wall edge data into lines
		firstBracketPos = mapData.IndexOf('[',secondBracketPos + 1);
		secondBracketPos = mapData.IndexOf(']', firstBracketPos + 1);
		string edgeString = mapData.Substring(firstBracketPos, secondBracketPos - firstBracketPos);
		string[] edges = edgeString.Split('\n');
		List<Vector2> edgesPos = new List<Vector2>();
		//extract wall edge data into array of Vector2s
		for (int i = 0; i < edges.Length; ++i) {
			string tv = edges[i].Trim();
			if (tv.Length > 0 && tv[0] == '(') {
				string[] splitTv = tv.Trim('(').Trim(',').Trim(')').Split(',');
				edgesPos.Add(new Vector2(Int32.Parse(splitTv[0]),Int32.Parse(splitTv[1])));
			}
		}
		
		//generate map from loaded data
		for (int i = 0; i < vertsPos.Count; ++i) {
			Instantiate(wall, new Vector3(vertsPos[i][0], vertsPos[i][1], 0), Quaternion.identity);
		}

	}

	void Update () {
		//freeze/unfreezes time on spacebar
		if (Input.GetKeyDown("space")) {
			timeFrozen = !timeFrozen;
			Time.timeScale = timeFrozen ? 0 : 1;
		}
		//Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}
}
