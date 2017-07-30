using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointAtEnemy : MonoBehaviour {

	public Transform enemy;

	private new Renderer renderer;

	void Start () {
		renderer = GetComponent<MeshRenderer> ();
	}

	void Update () {
		renderer.enabled = false;

		if (enemy == null)
			return;

		Vector3 pos = Camera.main.WorldToViewportPoint (enemy.position);

		if (pos.x >= 0.0f && pos.x <= 1.0f && pos.y >= 0.0f && pos.y <= 1.0f)
			return;

		renderer.enabled = true;

		Vector3 lookPos = enemy.position - transform.position;
		lookPos.y = 0;
		transform.rotation = Quaternion.LookRotation (lookPos);

		pos.x -= 0.5f;
		pos.y -= 0.5f;
		float angle = Mathf.Atan2 (pos.x, pos.y);

		pos.x = 0.5f * Mathf.Sin (angle) + 0.5f;
		pos.y = 0.5f * Mathf.Cos (angle) + 0.5f;
		pos.z = Camera.main.nearClipPlane + 0.01f;
		transform.position = Camera.main.ViewportToWorldPoint (pos);

	}
}
