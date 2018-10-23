using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawPath : MonoBehaviour {
	public List<Vector2> points = new List<Vector2>();
	LineRenderer lineRenderer;
	bool drawing = false;
	float playerGirth = .3f;
	public Transform debugPoint;
	public bool freshDraw = false;

	private void Start() {
		//initialie our lineRenderer with a green-red gradient
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(.5f, 0.0f), new GradientAlphaKey(.5f, 1.0f) }
			);
		lineRenderer.colorGradient = gradient;
	}
	
	void Update () {
		if (Input.GetMouseButton(0)) {
			Collider2D hit;
			if (!drawing && Input.GetMouseButtonDown(0)) {
				//check if we clicked on a player unit; if so, start a new path at his position
				hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1 << 9);
				if (hit && hit.transform == transform.parent) {
					points.Clear();
					drawing = true;
					freshDraw = true;
				}
			}
			if (drawing) {
				Vector3 scaledMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if (points.Count == 0 || !(points[points.Count - 1].x == scaledMousePos.x && points[points.Count - 1].y == scaledMousePos.y)) {
					//check collisions before adding waypoint
					Vector2 oldPoint = points.Count > 0 ? points[points.Count - 1] : (Vector2)transform.parent.transform.position;
					Vector2 newPoint = scaledMousePos;
					RaycastHit2D rch;
					rch = Physics2D.CircleCast(oldPoint, playerGirth, (newPoint - oldPoint).normalized, Vector2.Distance(oldPoint, newPoint), 1 << 8);
					if (rch.collider == null) {
						//no collisions; add the point
						points.Add(new Vector2(scaledMousePos.x, scaledMousePos.y));
					}
					else {
						//translate collider point outside of collision, with a buffer of .01f to help with float rounding
						Vector2 finalPoint = rch.point;
						float ang = Mathf.Atan2((finalPoint.y - oldPoint.y), (finalPoint.x - oldPoint.x));
						finalPoint.x -= Mathf.Cos(ang) * (playerGirth + .01f);
						finalPoint.y -= Mathf.Sin(ang) * (playerGirth + .01f);
						//failsafe: make sure the final point is outside of all collisions
						hit = Physics2D.OverlapCircle(new Vector2(finalPoint.x,finalPoint.y), playerGirth, 1 << 8);
						if (!hit) {
							points.Add(finalPoint);
						}
						//collision; try resolving on each individual axis
					}
				}
			}
		}
		else {
			drawing = false;
		}
		lineRenderer.positionCount = points.Count+1;
		lineRenderer.SetPosition(0, transform.parent.transform.position);
		for (int i = 0; i < points.Count; ++i) {
			lineRenderer.SetPosition(i+1, points[i]);
		}
	}
}
