using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sightCone : MonoBehaviour {
	public float visDist;
	public Material sightConeMat;
	public Vector3[] vertices3D;
	public MeshFilter filter;
	Vector2[] uv;
	float min = 0;
	float max = 1;
	public Transform debugPoint;
	public int numVerts;
	public float visArc;
	public float reloadTimerMax = .5f;
	public float reloadTimer = 0;
	public float maxHealth;
	public float health;
	public float power;
	public Transform laserPrefab;
	public int healthbarLength;
	public int healthbarHeight;
	Texture2D healthbarTex;
	Texture2D healthbarBGText;

	/**
	 * rebuild the triangles array
	 **/
	void rebuildTriangles() {
		int[] indices = new int[(vertices3D.Length - 2) * 3];
		for (int i = 0; i < indices.Length; i += 3) {
			indices[i] = 0;
			indices[i + 1] = (int)(i / 3) + 1;
			indices[i + 2] = (int)(i / 3) + 2;
		}
		filter.mesh.triangles = indices;
	}

	/**
	 * take the specified amount of damage
	 * @param amount: the amount of damage to take
	 **/
	void takeDamage(float amount) {
		//TODO: this method should probably go in a dedicated script file for unit interactions
		health -= amount;
		if (health <= 0) {
			Destroy(transform.parent.gameObject);
		}
	}

	/**attack the specified enemy
	 * @param enemy: the enemy to attack
	 **/
	void attack(GameObject enemy) {
		if (reloadTimer == 0) {
			reloadTimer = reloadTimerMax;
			enemy.GetComponentInChildren<sightCone>().takeDamage(power);
			//TODO: manually set UVs for laser, door, and wall meshes
			//instantiate laser graphic
			Vector2 p1 = transform.position;
			Vector2 p0 = enemy.transform.position;
			float vertDist = Vector2.Distance(p1, p0);
			float vertAng = Mathf.Atan2(p1.y - p0.y, p1.x - p0.x);
			Vector3 center = new Vector3((p1.x + p0.x) / 2, (p1.y + p0.y) / 2, .1f);
			//store mesh data and collider in separate objects as plane needs to rotate 90 degrees to face camera, which breaks collider
			Transform laserParent = Instantiate(laserPrefab);
			Transform laser = laserParent.transform.GetChild(0);
			laser.localScale = new Vector3(vertDist * .1f, 1, .01f);
			BoxCollider2D coll = laserParent.GetComponent<BoxCollider2D>();
			coll.size = new Vector2(vertDist, .1f);
			laserParent.rotation *= Quaternion.Euler(0, 0, vertAng * 180 / Mathf.PI);
			laserParent.position = center;
			laser.GetComponent<laserUpdate>().amFriendly = transform.parent.gameObject.layer == 9;
		}
	}

	// Use this for initialization
	void Start () {
		health = maxHealth;
		//init healthbar textures
		healthbarTex = new Texture2D(1, 1);
		healthbarTex.SetPixel(0, 0, new Color(0, 1, 0, .5f));
		healthbarTex.wrapMode = TextureWrapMode.Repeat;
		healthbarTex.Apply();
		healthbarBGText = new Texture2D(1, 1);
		healthbarBGText.SetPixel(0, 0, new Color(1, 0, 0, .5f));
		healthbarBGText.wrapMode = TextureWrapMode.Repeat;
		healthbarBGText.Apply();

		//create the base sight cone mesh
		vertices3D = new Vector3[numVerts];
		uv = new Vector2[vertices3D.Length];
		
		// Create the mesh
		Mesh mesh = new Mesh {
			vertices = vertices3D,
			uv = uv
		};

		filter = GetComponent<MeshFilter>();
		filter.mesh = mesh;
		rebuildTriangles();

		// Set up game object with mesh;
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.material = sightConeMat;
	}

    // Update is called once per frame
    void Update () {
		//reload cooldown
		if (reloadTimer > 0) {
			reloadTimer -= Time.deltaTime;
			if (reloadTimer < 0) {
				reloadTimer = 0;
			}
		}
		transform.localRotation = Quaternion.identity;
        RaycastHit2D rch;
        Vector2 dir;
        float facing_ang = transform.rotation.eulerAngles.z / 180 * Mathf.PI;
        float start_ang = facing_ang - (visArc*Mathf.PI/180/2);
		float end_ang = facing_ang + (visArc*Mathf.PI/180/2);

		//allow hot-reload of numVerts during gameplay if desired
		int vertsLen = vertices3D.Length;
		if (vertsLen != numVerts) {
			vertices3D = new Vector3[(int)Mathf.Max(numVerts,3)];
			uv = new Vector2[vertices3D.Length];
			//if we added more verts, we need to update mesh.vertices with a sufficiently large vertex array before setting the new triangles
			if (vertsLen < numVerts) {
				filter.mesh.vertices = vertices3D;
			}
			rebuildTriangles();
			vertsLen = vertices3D.Length;
		}

		vertices3D[0] = Vector2.zero;
		uv[0] = new Vector2((vertices3D[0].x + transform.position.x - min) / (max - min), (vertices3D[0].y + transform.position.y - min) / (max - min));
		List<GameObject> hits = new List<GameObject>();
		for (int i = 1; i < vertsLen; ++i) {
			float ang = start_ang + (end_ang - start_ang) * (i/(float)vertsLen);
            dir = Quaternion.AngleAxis(ang*180/Mathf.PI, Vector3.forward) * Vector3.right;
			rch = Physics2D.Raycast(transform.position, dir, visDist, (1 << 11) | (1 << (transform.parent.gameObject.layer == 10 ? 9 : 10)));
			float dist = Vector2.Distance(transform.position, rch.point);
			vertices3D[i] = dir * (rch.collider ? dist : visDist);
			if (rch && rch.collider.name != "Wall(Clone)") {
				hits.Add(rch.collider.gameObject);
			}
			//calculate uvs as though it were a plane, cancelling out transform position to achieve the chowder effect
			uv[i] = new Vector2((vertices3D[i].x + transform.position.x - min) / (max - min), (vertices3D[i].y + transform.position.y - min) / (max - min));
		}
		//attack the closest enemy in the hit list, if at least one exists
		if (hits.Count > 0) {
			int closestHit = -1;
			float closestDist = float.MaxValue;
			for (int i = 0; i < hits.Count; ++i)
			{
				float curDist = Vector2.Distance(transform.position, hits[i].transform.position);
				if (curDist < closestDist)
				{
					closestDist = curDist;
					closestHit = i;
				}
			}
			attack(hits[closestHit]);
		}
		filter.mesh.vertices = vertices3D;
		filter.mesh.uv = uv;
		transform.rotation = Quaternion.Euler(0, 0, 0);
    }

	void OnGUI() {
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		GUI.DrawTexture(new Rect(screenPos.x - healthbarLength/2, Camera.main.scaledPixelHeight - screenPos.y - 30, healthbarLength, healthbarHeight), healthbarBGText);
		GUI.DrawTexture(new Rect(screenPos.x - healthbarLength/2+1, Camera.main.scaledPixelHeight - screenPos.y - 29, (healthbarLength - 2) * (health/maxHealth), healthbarHeight-2), healthbarTex);
	}
}