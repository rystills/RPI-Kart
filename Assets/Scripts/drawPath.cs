using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawPath : MonoBehaviour {
	List<Vector2> points = new List<Vector2>();
	public Material lineMat;
	LineRenderer lineRenderer;
	bool drawing = false;
	float playerGirth = .3f;
	public Transform debugPoint;

	private void Start() {
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(.5f, 0.0f), new GradientAlphaKey(.5f, 1.0f) }
			);
		lineRenderer.colorGradient = gradient;
	}
	
	void Update () {
		bool prevDrawing = drawing;
		drawing = Input.GetMouseButton(0);
		if (drawing) {
			if (!prevDrawing) {
				//check if we clicked on a player unit; if so, start a new path at his position

				points.Clear();
				points.Add(transform.position);
			}
			Vector3 scaledMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (points.Count == 0 || !(points[points.Count-1].x == scaledMousePos.x && points[points.Count-1].y == scaledMousePos.y)) {
				//check collisions before adding waypoint
				Vector2 oldPoint = points[points.Count - 1];
				Vector2 newPoint = scaledMousePos;
				RaycastHit2D rch;
				rch = Physics2D.CircleCast(oldPoint, playerGirth, (newPoint - oldPoint).normalized, Vector2.Distance(oldPoint, newPoint),1<<8);
				if (rch.collider == null) {
					//no collisions; add the point
					points.Add(new Vector2(scaledMousePos.x, scaledMousePos.y));
				}
				else {
					//translate collider point outside of collision
					Vector2 finalPoint = rch.point;
					float ang = Mathf.Atan2((finalPoint.y - oldPoint.y), (finalPoint.x - oldPoint.x));
					finalPoint.x -= Mathf.Cos(ang) * playerGirth;
					finalPoint.y -= Mathf.Sin(ang) * playerGirth;
					points.Add(finalPoint);
					//collision; try resolving on each individual axis
				}
			}
		}
		lineRenderer.positionCount = points.Count;
		for (int i = 0; i < points.Count; ++i) {
			lineRenderer.SetPosition(i, points[i]);
		}
	}
}
