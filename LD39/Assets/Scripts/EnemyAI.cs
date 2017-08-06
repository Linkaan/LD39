using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	public EnemySpawner spawner;

	public float checkForTargetFrequency = 0.5f;
	public float threatDistance;
	public float criticalThreatDistance;

	public Ball target;
	public Ball threat;

	public bool dead;

	private EnemyMovement movement;
	private Ball ball;
	private Player player;

	private float nearestDistThreat;

	private float initialMass;
	private float spawnTime;

	void Start () {		
		dead = false;
		player = GameObject.FindObjectOfType<Player> ();
		ball = GetComponent<Ball> ();
		movement = GetComponent<EnemyMovement> ();
		Vector3 finalPos = Vector3.zero;

		for (int i = 1; i < 200; i++) {
			ball.mass = Random.Range (player.ball.mass * 0.5f, player.ball.mass * 2f);
			float randX;
			float randY;
			if (Random.Range (0.0f, 1.0f) < 0.5f) {
				randX = Random.Range (-spawner.maxX * i, spawner.maxX * i);
				randY = 0.0f;
				if (Mathf.Abs (randX) <= spawner.minXY) {
					randY = Random.Range (spawner.minXY, spawner.maxY * i) * (1 - (int)Random.Range (0, 1f) * 2);
				} else {
					randY = Random.Range (-spawner.maxY * i, spawner.maxY * i);
				}
			} else {
				randX = 0.0f;
				randY = Random.Range (-spawner.maxY * i, spawner.maxY * i);
				if (Mathf.Abs (randY) <= spawner.minXY) {
					randX = Random.Range (spawner.minXY, spawner.maxX * i) * (1 - (int)Random.Range (0, 1f) * 2);
				} else {
					randX = Random.Range (-spawner.maxX * i, spawner.maxX * i);
				}
			}

			Vector3 spawnPos = Camera.main.ViewportToWorldPoint (new Vector3 (randX, randY, 1f));
			finalPos = new Vector3 (spawnPos.x, ball.mass / 2.0f, spawnPos.z);

			Collider[] hitColliders = Physics.OverlapSphere (finalPos, ball.mass*2);
			if (hitColliders.Length == 0 && Vector3.Distance (spawnPos, player.transform.position) > ball.mass * 10)
				break;
		}

		transform.position = finalPos;
		spawnTime = Time.time;
		initialMass = ball.mass;

		InvokeRepeating ("CheckForTarget", 0, checkForTargetFrequency);
	}

	void CheckForTarget () {
		target = null;
		threat = null;
		nearestDistThreat = Mathf.Infinity;
		float nearestDistTarget = Mathf.Infinity;
		Ball[] objects = GameObject.FindObjectsOfType<Ball> ();
		foreach (Ball ball in objects) {
			if (ball == this.ball || ball.dead)
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
	}

	void Update () {
		if (dead)
			return;
		if (ball.mass == 0) {
			dead = true;
			Die ();
		}
		if (Time.time - spawnTime < 1) {
			if (ball.mass != initialMass && ball.mass > 0) {
				ball.mass = initialMass;
			}
		}
	}

	void LateUpdate () {
		if (dead)
			return;
		Quaternion newRotationGoal = Quaternion.identity;
		if (threat != null && nearestDistThreat <= threatDistance * threatDistance * threat.mass) {
			Vector3 lookPos = transform.position - threat.transform.position;
			lookPos.y = 0;
			newRotationGoal = Quaternion.LookRotation (lookPos);
		}

		if (target != null && (threat == null || nearestDistThreat > criticalThreatDistance * criticalThreatDistance * threat.mass)) {
			Vector3 lookPos = target.transform.position - transform.position;
			lookPos.y = 0;
			newRotationGoal *= Quaternion.LookRotation (lookPos);
		}

		Debug.DrawRay (transform.position, transform.position + newRotationGoal.eulerAngles * nearestDistThreat, Color.red);

		movement.rotationGoal = newRotationGoal;
		if (ball.mass > 0 && Time.time - spawnTime >= 1) {
			transform.localScale = new Vector3 (ball.mass, ball.mass, ball.mass);
			Vector3 curPos = transform.position;
			transform.localPosition = new Vector3 (curPos.x, ball.mass / 2.0f, curPos.z);
		}
	}

	void FixedUpdate () {
		if (dead)
			return;
		movement.fixedMovementResponsiveness = movement.movementResponsiveness / ball.mass;
		float adjustedMovementSpeed = Mathf.Max (movement.movementSpeed, movement.movementSpeed * player.ball.mass * 0.15f);
		movement.fixedMovementSpeed = Mathf.Max (adjustedMovementSpeed / 2f, adjustedMovementSpeed * ball.energy);
	}

	void Die () {
		ball.dead = true;
		movement.enabled = false;
		spawner.decrementEnemyCount ();
		Destroy (this.gameObject);
	}
}
