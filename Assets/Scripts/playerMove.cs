using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {
	public GameObject lineDrawer;
	drawPath dp;
	float moveSpeed = 1.5f;

	// Use this for initialization
	void Start () {
		GameObject ld = Instantiate(lineDrawer);
		ld.transform.SetParent(transform);
		dp = ld.GetComponent<drawPath>();
	}
	
	// Update is called once per frame
	void Update () {
		if (dp.freshDraw) {
			dp.freshDraw = false;
		}
		//move along the drawn path
		float moveTick = moveSpeed * Time.deltaTime;
		while (dp.points.Count > 0) {
			float dist = Vector2.Distance(transform.position, dp.points[0]);
			float ang = Mathf.Atan2((dp.points[0].y - transform.position.y), (dp.points[0].x - transform.position.x));
			if (dist > moveTick) {
				transform.Translate(new Vector2(Mathf.Cos(ang) * moveTick, Mathf.Sin(ang) * moveTick));
				break;
			}
			transform.position = dp.points[0];
			dp.points.RemoveAt(0);
			moveTick -= dist;
		}
	}
}
