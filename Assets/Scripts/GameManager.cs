using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public bool timeFrozen = false;
	public Transform debugPoint;
	public Texture wallTex;
	public Material[] floorMats;
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

		//gather floor data into lines
		firstBracketPos = mapData.IndexOf('[',secondBracketPos + 1);
		secondBracketPos = mapData.IndexOf(']', firstBracketPos + 1);
		string floorString = mapData.Substring(firstBracketPos, secondBracketPos - firstBracketPos);
		string[] floors = floorString.Split('\n');
		List<List<int>> floorsPos = new List<List<int>>();
		//extract floor data into 2darray
		for (int i = 0; i < floors.Length; ++i) {
			string tv = floors[i].Trim();
			if (tv.Length > 0 && tv[0] == '(') {
				string[] splitTv = tv.Trim('(').Trim(',').Trim(')').Split(',');
				List<int> curFloor = new List<int>();
				for (int r = 0; r < splitTv.Length; ++r) {
					curFloor.Add(Int32.Parse(splitTv[r]));
				}
				floorsPos.Add(curFloor);
			}
		}

		//gather door data into lines
		firstBracketPos = mapData.IndexOf('[', secondBracketPos + 1);
		secondBracketPos = mapData.IndexOf(']', firstBracketPos + 1);
		string doorString = mapData.Substring(firstBracketPos, secondBracketPos - firstBracketPos);
		string[] doors = doorString.Split('\n');
		List<Vector2> doorsPos = new List<Vector2>();
		//extract door data into 2darray
		for (int i = 0; i < doors.Length; ++i) {
			string tv = doors[i].Trim();
			if (tv.Length > 0 && tv[0] == '(') {
				string[] splitTv = tv.Trim('(').Trim(',').Trim(')').Split(',');
				doorsPos.Add(new Vector2(Int32.Parse(splitTv[0]), Int32.Parse(splitTv[1])));
			}
		}

		//generate map walls
		for (int i = 0; i < edgesPos.Count; ++i) {
			float vertDist = Vector2.Distance(vertsPos[(int)edgesPos[i][0]], vertsPos[(int)edgesPos[i][1]]);
			Vector2 p1 = vertsPos[(int)edgesPos[i][1]];
			Vector2 p0 = vertsPos[(int)edgesPos[i][0]];
			float vertAng = Mathf.Atan2(p1.y - p0.y, p1.x - p0.x);
			Vector2 center = new Vector2((p1.x + p0.x) / 2, (p1.y + p0.y)/2);
			//store mesh data and collider in separate objects as plane needs to rotate 90 degrees to face camera, which breaks collider
			GameObject wallParent = new GameObject();
			GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Plane);
			wallParent.name = "Wall"+i;
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
			renderer.material.mainTextureScale = new Vector2(10,.1f);
			wallParent.transform.rotation *= Quaternion.Euler(0, 0,vertAng*180/Mathf.PI);
			wallParent.transform.position = center;
			coll.size = new Vector2(vertDist,.1f);
		}

		//generate map floors
		for (int i = 0; i < floorsPos.Count; ++i) {
			Vector2[] vertices2D = new Vector2[floorsPos[i].Count];
			for (int r = 0; r < floorsPos[i].Count; ++r) {
				vertices2D[r] = vertsPos[floorsPos[i][r]];
			}

			Vector3[] vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

			// Use the triangulator to get indices for creating triangles
			Triangulator triangulator = new Triangulator(vertices2D);
			int[] indices = triangulator.Triangulate();

			//calculate uvs as though it were a plane
			Vector2[] uv = new Vector2[vertices3D.Length];
			float min = 0;
			float max = 1;
			for (int r = 0; r < vertices3D.Length; ++r) {
				uv[r] = new Vector2((vertices3D[r].x - min) / (max - min), (vertices3D[r].y - min) / (max - min));
			}

			// Create the mesh
			Mesh mesh = new Mesh {
				vertices = vertices3D,
				triangles = indices,
				uv = uv
			};

			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			// Set up game object with mesh;
			GameObject floor = new GameObject();
			floor.transform.position = new Vector3(0, 0, 1);
			floor.name = "floor"+i;
			MeshRenderer meshRenderer = floor.AddComponent<MeshRenderer>();
			meshRenderer.material = floorMats[i];

			MeshFilter filter = floor.AddComponent<MeshFilter>();
			filter.mesh = mesh;
		}

	}

	void Update () {
		//freeze/unfreezes time on spacebar
		if (Input.GetKeyDown("space")) {
			timeFrozen = !timeFrozen;
			Time.timeScale = timeFrozen ? 0 : 1;
		}
	}
}
