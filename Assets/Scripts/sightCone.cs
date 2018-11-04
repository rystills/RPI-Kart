using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sightCone : MonoBehaviour {
	float visDist = 10;
	public Material sightConeMat;
	public Vector3[] vertices3D;
	public MeshFilter filter;

    // Use this for initialization
    void Start () {
		//create the base sight cone mesh
		vertices3D = new Vector3[106];
		vertices3D[0] = Vector3.zero;
		int[] indices = new int[(vertices3D.Length - 2)*3];
		for (int i = 0; i < indices.Length; i+=3) {
			indices[i] = 0;
			indices[i + 1] = (int)(i / 3) + 1;
			indices[i + 2] = (int)(i / 3) + 2;
		}

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
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.material = sightConeMat;

		filter = GetComponent<MeshFilter>();
		filter.mesh = mesh;
	}

    // Update is called once per frame
    void Update () {
        RaycastHit2D rch;
        Vector2 dir;
        Vector2 cur_pos;
        List<Collider2D> col_list = new List<Collider2D>();

        float facing_ang = transform.rotation.eulerAngles.z / 180 * Mathf.PI;
        float start_ang = facing_ang - (Mathf.PI / 4);

        cur_pos.x = transform.position.x;
        cur_pos.y = transform.position.y;
        col_list.Clear();
		int i = 0;
        for (float ang = start_ang; ang <= start_ang + (Mathf.PI / 2); ang += 0.015f) {
            dir = Quaternion.AngleAxis(ang*180/Mathf.PI, Vector3.forward) * Vector3.right;

			//third arg is distance of cast
			//fourth is layer mask of what to look for
			rch = Physics2D.Raycast(cur_pos, dir, visDist, 1<<11);
            if (rch.collider != null) {
                col_list.Add(rch.collider);
				
			}
			vertices3D[++i] = (Vector2)transform.position + (Vector2)dir * (rch.collider ? Vector2.Distance(transform.position, rch.point) : visDist);
		}
		filter.mesh.vertices = vertices3D;


		//Something was found
		if (col_list.Count != 0) {
			//TODO: resolve sight collision
        }

    }
}
