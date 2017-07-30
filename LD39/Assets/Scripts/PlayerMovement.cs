using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Transform graphics;
	public float movementSpeed;
	public float fixedMovementSpeed;
	public float rotationSpeed;
	public float movementResponsiveness;
	public float fixedMovementResponsiveness;

	public float attackRollBase;
	private float attackRollCompensation;

	private float xRotation;

	void Start () {
		attackRollCompensation = 1;
	}
	
	void Update () {
		if (fixedMovementResponsiveness == Mathf.Infinity)
			return;
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = transform.position.y;
		Vector3 center = Camera.main.WorldToScreenPoint (transform.position);
		//float inputAxisVertical = attackRollCompensation > 1 ? 1 : Input.GetAxis ("Vertical");
		float inputAxisVertical = 1f; // attackRollCompensation > 1 ? 1 : Mathf.Min (1f, Mathf.Abs(center.x - mousePos.x) + Mathf.Abs(center.y - mousePos.y));
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);

		Vector3 curPos = transform.position;
		Vector3 heading = curPos - mousePos;
		Debug.Log (mousePos);
		float angle = Mathf.Atan2 (heading.x, heading.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.up);


		float translation = inputAxisVertical * fixedMovementSpeed * attackRollCompensation * Time.deltaTime;
		//float rotation = Input.GetAxis ("Horizontal") * rotationSpeed * Time.deltaTime;
		transform.Translate (0, 0, translation);
		//transform.Rotate (0, rotation, 0);
		//transform.rotation = Quaternion.AngleAxis (angle, Vector3.up);
		//transform.LookAt (mousePos, Vector3.up);
		//transform.eulerAngles = new Vector3 (0, transform.eulerAngles.x, 0);
		//Vector3 lookPos = transform.position - mousePos;
		//lookPos = new Vector3 (lookPos.x, lookPos.z, 0);
		//Debug.Log (lookPos);
		//transform.rotation = Quaternion.LookRotation (lookPos);

		// update ball rotation
		graphics.LookAt (graphics.position + transform.forward);
		Vector3 newRotation = graphics.eulerAngles;
		xRotation += translation * fixedMovementResponsiveness;
		newRotation.x = xRotation;
		graphics.eulerAngles = newRotation;
	}

	public void BeginAttackRoll (float mass) {
		attackRollCompensation = attackRollBase + attackRollBase * mass;
	}

	public void EndAttackRoll () {
		attackRollCompensation = 1;
	}
}