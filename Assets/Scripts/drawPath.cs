using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawPath : MonoBehaviour {
	List<Vector2> points = new List<Vector2>();
	public Material lineMat;
	LineRenderer lineRenderer;
	bool drawing = false;

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
		drawing = Input.GetMouseButton(0);
		if (drawing) {
			Vector3 scaledMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (points.Count == 0 || !(points[points.Count-1].x == scaledMousePos.x && points[points.Count-1].y == scaledMousePos.y)) {
				points.Add(new Vector2(scaledMousePos.x, scaledMousePos.y));
			}
		}
		lineRenderer.positionCount = points.Count;
		for (int i = 0; i < points.Count; ++i) {
			lineRenderer.SetPosition(i, points[i]);
		}
	}
}
