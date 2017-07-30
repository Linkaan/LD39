using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public Transform graphics;
	public float movementSpeed;
	public float rotationSpeed;
	public float movementResponsiveness;
	public float fixedMovementResponsiveness;
	public float fixedMovementSpeed;
	public Quaternion rotationGoal;

	private float xRotation;

	private Player player;

	void Start () {
		player = GameObject.FindObjectOfType<Player> ();
		fixedMovementResponsiveness = movementResponsiveness;
		rotationGoal = Quaternion.identity;
		xRotation = 0;
	}
		
	void Update () {
		if (fixedMovementResponsiveness == Mathf.Infinity || player.ball.dead)
			return;
		transform.rotation = Quaternion.RotateTowards (transform.rotation, rotationGoal, Time.deltaTime * rotationSpeed);
		transform.Translate (0, 0, fixedMovementSpeed * Time.deltaTime);

		// update ball rotation
		graphics.LookAt (graphics.position + transform.forward);
		Vector3 newRotation = graphics.eulerAngles;
		xRotation += movementSpeed * Time.deltaTime * fixedMovementResponsiveness;
		newRotation.x = xRotation;
		graphics.eulerAngles = newRotation;
	}

}
