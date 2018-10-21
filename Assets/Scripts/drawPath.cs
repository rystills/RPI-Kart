using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawPath : MonoBehaviour {
	List<Vector2> points = new List<Vector2>();
	public Material lineMat;
	LineRenderer lineRenderer;
	bool drawing = false;
	float playerGirth = 1f;

	private void Start() {
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(.5f, 0.0f), new GradientAlphaKey(.5f, 1.0f) }
			);
		lineRenderer.colorGradient = gradient;
		points.Add(transform.position);
	}
	
	void Update () {
		drawing = Input.GetMouseButton(0);
		if (drawing) {
			Vector3 scaledMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (points.Count == 0 || !(points[points.Count-1].x == scaledMousePos.x && points[points.Count-1].y == scaledMousePos.y)) {
				//check collisions before adding waypoint
				Vector2 oldPoint = points[points.Count - 1];
				Vector2 newPoint = scaledMousePos;
				//angle between points in radians
				float ang = Mathf.Atan2((newPoint.y - oldPoint.y), (newPoint.x - oldPoint.x));
				if (Physics2D.CircleCastAll(oldPoint, playerGirth, new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)),Vector2.Distance(oldPoint, newPoint)).Length == 0) {
					points.Add(new Vector2(scaledMousePos.x, scaledMousePos.y));
				}
			}
		}
		lineRenderer.positionCount = points.Count;
		for (int i = 0; i < points.Count; ++i) {
			lineRenderer.SetPosition(i, points[i]);
		}
	}
}
