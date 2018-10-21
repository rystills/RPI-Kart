using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawPath : MonoBehaviour {
	List<Vector2> points = new List<Vector2>();
	public Material lineMat;
	LineRenderer lineRenderer;

	private void Start() {
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		//lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.widthMultiplier = 0.2f;

		float alpha = 1.0f;
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
			);
		lineRenderer.colorGradient = gradient;
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 scaledMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			points.Add(new Vector2(scaledMousePos.x, scaledMousePos.y));
		}
		lineRenderer.positionCount = points.Count;
		for (int i = 0; i < points.Count; ++i) {
			lineRenderer.SetPosition(i, points[i]);
		}
	}
}
