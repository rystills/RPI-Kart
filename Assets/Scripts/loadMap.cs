using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class loadMap : MonoBehaviour {
	public TextAsset map;
	public Transform debugPoint;
	public Material[] floorMats;
	public Transform wallPrefab;
	public Transform doorPrefab;
	public Transform[] obstaclePrefabs;
	public Transform unitFriendly;
	public Transform unitEnemy;
    public Transform Sniper;
    public Transform MachineGun;
    public Transform SemiAuto;
	public Transform backgroundPrefab;
	public Material backgroundMaterial;

	/**read the next set of values from the map content string
	 * @param mapData: the map content string to read from
	 * @param firstBracketPos: the index of the starting square bracket
	 * @param secondBracketPos: the index of the ending square bracket
	 * @returns: a list of floats containing the next set of values from the map content string
	 **/
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

	/*Tuple<int, float> readFloat(string mapData, int characterPointer)
	{
		bool isNegative = mapData[characterPointer] == '-';
		if (isNegative)
			++characterPointer;
		float ans = 0;
		while (char.IsNumber(mapData[characterPointer]))
			ans = ans * 10 + char.GetNumericValue(mapData[characterPointer++]);
		if (mapData[characterPointer] == '.')
		{
			int numDigits = 0;
			++characterPointer;
			while (char.IsNumber(mapData[characterPointer]))
				ans += char.GetNumericValue(mapData[characterPointer++]) / Math.Pow(10, numDigits);
		}

		if (isNegative)
			return -1 * ans;
		else
			return ans;
	}

	Tuple<int, List<T>> readList(Func<string, int, Tuple<int,T>> elementReader, string mapData, int characterPointer)
	{
		while (char.IsWhiteSpace(mapData[characterPointer]))
			++characterPointer;
		if (mapData[characterPointer] != '[')
			; // TODO: throw error
		++characterPointer;
		List<T> ans = new List<T>();
		while (mapData[characterPointer] != ']')
		{
			Tuple<int, T> result = elementReader(mapData, characterPointer);
			characterPointer = result.Item1;
			ans.Add(result.Item2);
			while (char.IsWhiteSpace(mapData[characterPointer]) || mapData[characterPointer] == ',')
				++characterPointer;
		}
		return new Tuple<int, List<T>>(characterPointer+1, ans);
	}*/

	private void Start() {
		//quick and dirty map load (sorry guys, Unity didn't seem to support loading JSON into a generic Dict?)
		string mapData = map.text;
		int firstBracketPos = 0;
		int secondBracketPos = -1;
		List<List<float>> verts = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);
		List<List<float>> edges = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);
		List<List<float>> floors = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);
		List<List<float>> doors = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);
		List<List<float>> obstacles = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);
		List<List<float>> units = readMapValue(ref mapData, ref firstBracketPos, ref secondBracketPos);

		//generate background
		Transform bg = Instantiate(backgroundPrefab);
		bg.GetComponent<MeshRenderer>().material = backgroundMaterial;

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

		//generate map doors
		for (int i = 0; i < doors.Count; ++i) {
			Vector2 p1 = new Vector2(verts[(int)doors[i][1]][0], verts[(int)doors[i][1]][1]);
			Vector2 p0 = new Vector2(verts[(int)doors[i][0]][0], verts[(int)doors[i][0]][1]);
			float vertDist = Vector2.Distance(p1, p0);
			float vertAng = Mathf.Atan2(p1.y - p0.y, p1.x - p0.x);
			Vector3 center = new Vector3((p1.x + p0.x) / 2, (p1.y + p0.y) / 2, .1f);
			//store mesh data and collider in separate objects as plane needs to rotate 90 degrees to face camera, which breaks collider
			Transform doorParent = Instantiate(doorPrefab);
			Transform door = doorParent.transform.GetChild(0);
			door.localScale = new Vector3(vertDist * .1f, 1, .01f);
			BoxCollider2D coll = doorParent.GetComponent<BoxCollider2D>();
			coll.size = new Vector2(vertDist, .1f);
			doorParent.rotation *= Quaternion.Euler(0, 0, vertAng * 180 / Mathf.PI);
			doorParent.position = center;
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
			floor.name = "floor" + i;
			MeshRenderer meshRenderer = floor.AddComponent<MeshRenderer>();
			meshRenderer.material = floorMats[i];

			MeshFilter filter = floor.AddComponent<MeshFilter>();
			filter.mesh = mesh;
		}

		//generate obstacles
		for (int i = 0; i < obstacles.Count; ++i) {
			Transform obs = Instantiate(obstaclePrefabs[(int)obstacles[i][0]]);
			obs.transform.position = new Vector3(obstacles[i][1], obstacles[i][2], obs.transform.position.z);
			obs.transform.rotation *= Quaternion.Euler(0, 0, obstacles[i][3]);
		}

        //spawn units
        bool temp = true;
        
		for (int i = 0; i < units.Count; ++i) {
            if ((int) units[i][0] == 0){
                temp = true;
            }
            else {
                temp = false;
            }
            if (temp) {
                Transform unit = Instantiate(unitFriendly);
                unit.transform.position = new Vector2(units[i][1], units[i][2]);
                unit.transform.rotation *= Quaternion.Euler(0, 0, units[i][3]);
                unit.transform.tag = "PlayerUnit";
            }
            else {
                int random = Random.Range(0, 3);
                if (random == 0) {
                    Transform unit = Instantiate(Sniper);
                    unit.transform.position = new Vector2(units[i][1], units[i][2]);
                    unit.transform.rotation *= Quaternion.Euler(0, 0, units[i][3]);
                }
                else if (random == 1) {
                    Transform unit = Instantiate(MachineGun);
                    unit.transform.position = new Vector2(units[i][1], units[i][2]);
                    unit.transform.rotation *= Quaternion.Euler(0, 0, units[i][3]);
                }
                else if (random == 2) {
                    Transform unit = Instantiate(SemiAuto);
                    unit.transform.position = new Vector2(units[i][1], units[i][2]);
                    unit.transform.rotation *= Quaternion.Euler(0, 0, units[i][3]);
                }
            }
		}

	}
}
