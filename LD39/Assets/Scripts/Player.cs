using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Ball ball;
	public ArrowPointAtEnemy threat_arrow;
	public ArrowPointAtEnemy target_arrow;

	public Image indicatorSlider;

	public float checkForTargetFrequency = 0.2f;

	public float attackRollDuration;
	public float attackRollCost;

	public float attackRollStartTime;

	private PlayerMovement movement;

	private bool attackRoll;

	private bool dead;

	void Start () {
		dead = false;
		ball = GetComponent<Ball> ();
		ball.mass = 1.0f;
		movement = GetComponent<PlayerMovement> ();

		InvokeRepeating ("CheckForTarget", 0, checkForTargetFrequency);
	}

	void CheckForTarget () {
		if (dead)
			return;
		Ball target = null;
		Ball threat = null;
		float nearestDistThreat = Mathf.Infinity;
		float nearestDistTarget = Mathf.Infinity;
		Ball[] objects = GameObject.FindObjectsOfType<Ball> ();
		foreach (Ball ball in objects) {
			if (ball == this.ball)
				continue;

			float ballMassRatio = this.ball.mass / ball.mass;
			if (ballMassRatio >= 1.25) {
				float distanceSqr = (ball.transform.position - transform.position).sqrMagnitude;
				if (distanceSqr < nearestDistTarget) {
					target = ball;
					nearestDistTarget = distanceSqr;
				}
			} else if (ballMassRatio <= 0.8) {
				float distanceSqr = (ball.transform.position - transform.position).sqrMagnitude;
				if (distanceSqr < nearestDistThreat) {
					threat = ball;
					nearestDistThreat = distanceSqr;
				}
			}
		}
				
		if (threat != null) {
			threat_arrow.enemy = threat.transform;
		} else {
			threat_arrow.enemy = null;
		}

		if (target != null) {
			target_arrow.enemy = target.transform;
		} else {
			target_arrow.enemy = null;
		}
	}

	void Update () {
		if (!attackRoll && Input.GetKeyDown (KeyCode.Space)) {
			attackRoll = true;
			movement.BeginAttackRoll (ball.mass);
			attackRollStartTime = Time.time;
			ball.energy -= attackRollCost;
		}

		if (attackRoll && Time.time - attackRollStartTime >= attackRollDuration) {
			movement.EndAttackRoll ();
			attackRoll = false;
		}
	}

	void LateUpdate () {
		if (dead)
			return;
		if (ball.mass > 0) {
			transform.localScale = new Vector3 (ball.mass, ball.mass, ball.mass);
			Vector3 curPos = transform.position;
			transform.localPosition = new Vector3 (curPos.x, ball.mass / 2.0f, curPos.z);
		}
		indicatorSlider.fillAmount = ball.energy;
		if (ball.energy == 0 && !attackRoll)
			Die ();
	}

	void FixedUpdate () {
		if (dead)
			return;
		movement.fixedMovementResponsiveness = movement.movementResponsiveness / ball.mass;
		movement.fixedMovementSpeed = Mathf.Max (1f, movement.movementSpeed * ball.energy);
	}

	public void Die () {
		indicatorSlider.fillAmount = 0;
		dead = true;
		transform.localScale = new Vector3 (ball.mass, ball.mass, ball.mass);
		Vector3 curPos = transform.position;
		transform.localPosition = new Vector3 (curPos.x, ball.mass / 2.0f, curPos.z);
		movement.enabled = false;
		Destroy (this.movement.graphics.gameObject);
	}
}
