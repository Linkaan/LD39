using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Ball ball;
	public ArrowPointAtEnemy threat_arrow;
	public ArrowPointAtEnemy target_arrow;
	public CameraMovement camMovement;

	public AudioClip attackrollSfx;
	public AudioClip eatSfx;
	public AudioClip losingPowerSfx;
	public AudioClip loseSfx;

	public GameObject gameoverMenu;

	public Image indicatorSlider;

	public ScoreManager scoreManager;

	public float checkForTargetFrequency = 0.2f;

	public float attackRollDuration;
	public float attackRollCost;

	public float attackRollStartTime;

	public float highestMass;
	public float timeAlive;

	private PlayerMovement movement;
	private AudioSource audioSource;

	private float lowEnergySFXInterval;
	private float lastLowEnergyTime;

	private bool attackRoll;

	private bool dead;

	void Start () {
		dead = false;
		ball = GetComponent<Ball> ();
		ball.mass = 1.0f;
		movement = GetComponent<PlayerMovement> ();
		audioSource = GetComponent<AudioSource> ();

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
		if (dead)
			return;
		if (!attackRoll && Input.GetKeyDown (KeyCode.Space)) {
			attackRoll = true;
			camMovement.smoothTime /= 2.5f;
			movement.BeginAttackRoll (ball.mass);
			playAttackRollSFX ();
			attackRollStartTime = Time.time;
			ball.energy -= attackRollCost;
		}

		if (attackRoll && Time.time - attackRollStartTime >= attackRollDuration) {
			camMovement.smoothTime *= 2.5f;
			movement.EndAttackRoll ();
			attackRoll = false;
		}

		if (ball.energy < 0.5f) {
			lowEnergySFXInterval = Mathf.Max (0.2f, ball.energy * 3);
			if (Time.time - lastLowEnergyTime >= lowEnergySFXInterval) {
				lastLowEnergyTime = Time.time;
				playLosingPowerFX ();
			}
		}

		if (ball.mass > highestMass)
			highestMass = ball.mass;
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
		//  Mathf.Min (yy, Mathf.Max (yy / 5f, yy / (player.GetComponent<Ball> ().mass * 0.15f)))
		float adjustedMovementSpeed = Mathf.Max (movement.movementSpeed, movement.movementSpeed * ball.mass * 0.15f);
		movement.fixedMovementSpeed = Mathf.Max (adjustedMovementSpeed / 2f, adjustedMovementSpeed * ball.energy);
	}

	public void playLoseSFX () {
		audioSource.clip = loseSfx;
		audioSource.Play ();
	}

	public void playEatSFX () {
		audioSource.clip = eatSfx;
		audioSource.Play ();
	}

	public void playAttackRollSFX () {
		audioSource.clip = attackrollSfx;
		audioSource.Play ();
	}

	public void playLosingPowerFX () {
		if ((audioSource.isPlaying && audioSource.clip != losingPowerSfx))
			return;
		audioSource.clip = losingPowerSfx;
		audioSource.Play ();
	}

	public void Die () {
		timeAlive = Time.time;
		scoreManager.UpdateTexts ();
		ball.dead = true;
		indicatorSlider.fillAmount = 0;
		dead = true;
		transform.localScale = new Vector3 (ball.mass, ball.mass, ball.mass);
		Vector3 curPos = transform.position;
		transform.localPosition = new Vector3 (curPos.x, ball.mass / 2.0f, curPos.z);
		movement.enabled = false;
		gameoverMenu.SetActive (true);
		Destroy (this.movement.graphics.gameObject);
	}
}
