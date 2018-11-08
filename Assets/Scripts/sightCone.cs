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

	// Use this for initialization
	void Start () {
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
			if (vertsLen < numVerts) {
				filter.mesh.vertices = vertices3D;
			}
			rebuildTriangles();
			vertsLen = vertices3D.Length;
		}

		vertices3D[0] = Vector2.zero;
		uv[0] = new Vector2((vertices3D[0].x + transform.position.x - min) / (max - min), (vertices3D[0].y + transform.position.y - min) / (max - min));
		for (int i = 1; i < vertsLen; ++i) {
			float ang = start_ang + (end_ang - start_ang) * (i/(float)vertsLen);
            dir = Quaternion.AngleAxis(ang*180/Mathf.PI, Vector3.forward) * Vector3.right;
			rch = Physics2D.Raycast(transform.position, dir, visDist, 1<<11);
			float dist = Vector2.Distance(transform.position, rch.point);
			vertices3D[i] = dir * (rch.collider ? dist : visDist);
			//calculate uvs as though it were a plane, cancelling out transform position to achieve the chowder effect
			uv[i] = new Vector2((vertices3D[i].x + transform.position.x - min) / (max - min), (vertices3D[i].y + transform.position.y - min) / (max - min));
		}
		filter.mesh.vertices = vertices3D;
		filter.mesh.uv = uv;
		transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}