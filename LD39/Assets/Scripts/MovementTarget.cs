using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTarget : MonoBehaviour {

	public GameObject player;
	public float rotationSpeed;
	
	void Update () {
		//Vector3 newPos = player.transform.position;
		//newPos.z -= 1f;
		//transform.position = newPos;

		float rotation = Input.GetAxis ("Horizontal") * rotationSpeed * Time.deltaTime;
		transform.RotateAround (player.transform.position, new Vector3 (0, 1, 0), rotation);
	}
}
