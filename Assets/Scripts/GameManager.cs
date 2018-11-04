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
	public Transform wallPrefab;
	public Transform[] obstaclePrefabs;

	List<List<float>> readMapValue(ref string mapData, ref int firstBracketPos, ref int secondBracketPos) {
		//gather data into lines
		firstBracketPos = mapData.IndexOf('[', secondBracketPos + 1);
		secondBracketPos = mapData.IndexOf(']', firstBracketPos + 1);
		string dataString = mapData.Substring(firstBracketPos, secondBracketPos - firstBracketPos);
		string[] datas = dataString.Split('\n');
		List<List<float>> datasPos = new List<List<float>>();
		//extract data into 2darray
		for (int i = 0; i < datas.Length; ++i) {
			string tv = datas[i].Trim();
			if (tv.Length > 0 && tv[0] == '(') {
				string[] splitTv = tv.Trim('(').Trim(',').Trim(')').Split(',');
				List<float> curFloor = new List<float>();
				for (int r = 0; r < splitTv.Length; ++r) {
					curFloor.Add(float.Parse(splitTv[r]));
				}
				datasPos.Add(curFloor);
			}
		}
		return datasPos;
	}

	private void Start() {
		//quick and dirty map load (sorry guys, Unity didn't seem to support loading JSON into a generic Dict?)
		string mapData = (Resources.Load("DemoMap") as TextAsset).text;
		int firstBracketPos = 0;
		int secondBracketPos = -1;
		List<List<float>> verts = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);
		List<List<float>> edges = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);
		List<List<float>> floors = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);
		List<List<float>> doors = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);
		List<List<float>> obstacles = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);

		//generate map walls
		for (int i = 0; i < edges.Count; ++i) {
			Vector2 p1 = new Vector2(verts[(int)edges[i][1]][0], verts[(int)edges[i][1]][1]);
			Vector2 p0 = new Vector2(verts[(int)edges[i][0]][0], verts[(int)edges[i][0]][1]);
			float vertDist = Vector2.Distance(p1, p0);
			float vertAng = Mathf.Atan2(p1.y - p0.y, p1.x - p0.x);
			Vector2 center = new Vector2((p1.x + p0.x) / 2, (p1.y + p0.y) / 2);
			//store mesh data and collider in separate objects as plane needs to rotate 90 degrees to face camera, which breaks collider
			Transform wallParent = Instantiate(wallPrefab);
			Transform wall = wallParent.transform.GetChild(0);
			wall.localScale = new Vector3(vertDist * .1f, 1, .01f);
			BoxCollider2D coll = wallParent.GetComponent<BoxCollider2D>();
			coll.size = new Vector2(vertDist, .1f);
			wallParent.rotation *= Quaternion.Euler(0, 0, vertAng * 180 / Mathf.PI);
			wallParent.position = center;
		}

		//generate map floors
		for (int i = 0; i < floors.Count; ++i) {
			Vector2[] vertices2D = new Vector2[floors[i].Count];
			for (int r = 0; r < floors[i].Count; ++r) {
				vertices2D[r] = new Vector2(verts[(int)floors[i][r]][0], verts[(int)floors[i][r]][1]);
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

			// Set up game object with mesh;
			GameObject floor = new GameObject();
			floor.transform.position = new Vector3(0, 0, 1);
			floor.name = "floor"+i;
			MeshRenderer meshRenderer = floor.AddComponent<MeshRenderer>();
			meshRenderer.material = floorMats[i];

			MeshFilter filter = floor.AddComponent<MeshFilter>();
			filter.mesh = mesh;
		}

		//generate doors

		//generate obstacles
		for (int i = 0; i < obstacles.Count; ++i) {
			Transform obs = Instantiate(obstaclePrefabs[(int)obstacles[i][0]]);
			obs.transform.position = new Vector3(obstacles[i][1], obstacles[i][2], 0);
			obs.transform.rotation *= Quaternion.Euler(0, 0, obstacles[i][3]);
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
