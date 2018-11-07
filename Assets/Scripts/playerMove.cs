using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {
	public GameObject lineDrawer;
	drawPath dp;
	public float moveSpeed;
	public float rotSpeed;

	// Use this for initialization
	void Start () {
		GameObject ld = Instantiate(lineDrawer);
		ld.transform.SetParent(transform);
		dp = ld.GetComponent<drawPath>();
	}
	
	// Update is called once per frame
	void Update () {
		//move along the drawn path
		float ang = transform.rotation.eulerAngles.z / 180 * Mathf.PI;
		float moveTick = moveSpeed * Time.deltaTime;
		while (dp.points.Count > 0) {
			float dist = Vector2.Distance(transform.position, dp.points[0]);
			ang = Mathf.Atan2((dp.points[0].y - transform.position.y), (dp.points[0].x - transform.position.x));
			if (dist > moveTick) {
				transform.Translate(new Vector2(Mathf.Cos(ang) * moveTick, Mathf.Sin(ang) * moveTick),Space.World);
				break;
			}
			transform.position = dp.points[0];
			dp.points.RemoveAt(0);
			moveTick -= dist;
		}
		//update our rotation
		float rotTick = rotSpeed * Time.deltaTime;
		float rotDiff = ang-(transform.rotation.eulerAngles.z / 180 * Mathf.PI);
		//if rotation difference exceeds 180 degrees (PI rad) add a full revolution to the calculation so we get the lesser angle
		if (Mathf.Abs(rotDiff) > Mathf.PI) {
			rotDiff = ang + Mathf.PI*2 - (transform.rotation.eulerAngles.z / 180 * Mathf.PI);
		}
		transform.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z + (rotTick >= Mathf.Abs(rotDiff) ? rotDiff : rotTick * Mathf.Sign(rotDiff)) * 180 / Mathf.PI);
	}
}
