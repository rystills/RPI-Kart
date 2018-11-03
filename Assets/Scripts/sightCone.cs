using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sightCone : MonoBehaviour {

    SpriteRenderer sr;
    Color newcolor;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        newcolor = new Color(125, 125, 125);
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

        for (float ang = start_ang; ang <= start_ang + (Mathf.PI / 2); ang += 0.015f) {
            dir.x = transform.position.x + Mathf.Cos(ang);
            dir.y = transform.position.y + Mathf.Sin(ang);

            //third arg is distance of cast
            //fourth is layer mask of what to look for
            rch = Physics2D.Raycast(cur_pos, dir, 20, (1 << 10));
            if (rch.collider != null) {
                col_list.Add(rch.collider);
            }
        }
        sr.color = newcolor;

        //Something was found
        if (col_list.Count != 0) {
            sr.color = newcolor;
            Debug.Log("we got em");
        }

    }
}
