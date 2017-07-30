using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public Transform player;
	public float smoothTime;

	private float yy;
	private Vector3 velocity;

	void Start () {
		yy = transform.position.y;
		velocity = Vector3.zero;
	}

	void Update () {
		Vector3 targetPosition = player.TransformPoint (new Vector3 (0, Mathf.Min (yy, Mathf.Max (yy / 5f, yy / (player.GetComponent<Ball> ().mass * 0.15f))), 0));
		Vector3 curPos = transform.position;
		//curPos.y = targetPosition.y;
		//transform.position = curPos;
		transform.position = Vector3.SmoothDamp (curPos, targetPosition, ref velocity, smoothTime);
	}
}
