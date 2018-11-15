using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {
	public GameObject lineDrawer;
	drawPath dp;
	public float moveSpeed;
	public float rotSpeedMax;
	public float rotAccel;
	public float smallRotAccel;
	float rotSpeed = 0;
	public Sprite[] playerSkins;

	// Use this for initialization
	void Start() {
		GameObject ld = Instantiate(lineDrawer);
		ld.transform.SetParent(transform);
		dp = ld.GetComponent<drawPath>();
		//randomly select sprite
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		sr.sprite = playerSkins[Random.Range(0, playerSkins.Length)];
	}

	// Update is called once per frame
	void Update() {
		//move along the drawn path
		float ang = transform.rotation.eulerAngles.z / 180 * Mathf.PI;
		float moveTick = moveSpeed * Time.deltaTime;
		while (dp.points.Count > 0) {
			float dist = Vector2.Distance(transform.position, dp.points[0]);
			ang = Mathf.Atan2((dp.points[0].y - transform.position.y), (dp.points[0].x - transform.position.x));
			if (dist > moveTick) {
				transform.Translate(new Vector2(Mathf.Cos(ang) * moveTick, Mathf.Sin(ang) * moveTick), Space.World);
				break;
			}
			transform.position = dp.points[0];
			dp.points.RemoveAt(0);
			moveTick -= dist;
		}
		//update our rotation
		float rotDiff = ang - (transform.rotation.eulerAngles.z / 180 * Mathf.PI);
		//if rotation difference exceeds 180 degrees (PI rad) add a full revolution to the calculation so we get the lesser angle
		if (Mathf.Abs(rotDiff) > Mathf.PI) {
			rotDiff = ang + Mathf.PI * 2 - (transform.rotation.eulerAngles.z / 180 * Mathf.PI);
		}
		//minimize jitters for small rotation differences
		float scaledRotAccel = Mathf.Abs(rotDiff) < .5f ? smallRotAccel : rotAccel;

		rotSpeed += scaledRotAccel * Mathf.Sign(rotDiff);
		float rotTick = Mathf.Abs(rotSpeed) * Time.deltaTime;
		transform.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z + (rotTick >= Mathf.Abs(rotDiff) ? rotDiff : rotTick * Mathf.Sign(rotDiff)) * 180 / Mathf.PI);

		//stop rotating immediately rather than decelerating, for now
		if (rotTick >= Mathf.Abs(rotDiff)) {
			rotSpeed = 0;
		}
		if (Mathf.Abs(rotSpeed) > rotSpeedMax) {
			rotSpeed = rotSpeedMax * Mathf.Sign(rotSpeed);
		}
	}
}
