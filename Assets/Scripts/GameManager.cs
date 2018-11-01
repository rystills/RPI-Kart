using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public bool timeFrozen = false;
	public Transform debugPoint;
	public Texture wallTex;
	public Shader wallShader;

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
		for (int i = 0; i < edgesPos.Count; ++i) {
			float vertDist = Vector2.Distance(vertsPos[(int)edgesPos[i][0]], vertsPos[(int)edgesPos[i][1]]);
			Vector2 p1 = vertsPos[(int)edgesPos[i][1]];
			Vector2 p0 = vertsPos[(int)edgesPos[i][0]];
			float vertAng = Mathf.Atan2(p1.y - p0.y, p1.x - p0.x);
			Vector2 center = new Vector2((p1.x + p0.x) / 2, (p1.y + p0.y)/2);
			//store mesh data and collider in separate objects as plane needs to rotate 90 degrees to face camera, which breaks collider
			GameObject wallParent = new GameObject();
			GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Plane);
			wallParent.name = "Wall";
			wall.name = "wall mesh";
			wall.transform.parent = wallParent.transform;
			wall.transform.rotation *= Quaternion.Euler(-90, 0, 0);
			wall.transform.localScale = new Vector3(vertDist * .1f, 1, .01f);
			DestroyImmediate(wall.GetComponent<MeshCollider>());
			wallParent.layer = 11;
			BoxCollider2D coll = wallParent.AddComponent<BoxCollider2D>();
			MeshRenderer renderer = wall.GetComponent<MeshRenderer>();
			renderer.material.shader = wallShader;
			renderer.material.mainTexture = wallTex;
			wallParent.transform.rotation *= Quaternion.Euler(0, 0,vertAng*180/Mathf.PI);
			wallParent.transform.position = center;
			coll.size = new Vector2(vertDist,.1f);
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
