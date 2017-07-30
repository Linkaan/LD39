using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public float mass;
	public float energy;
	public bool dead;

	private float dropMassSpeed = 0.0005f;
	private float dropEnergySpeed = 0.0005f;
	private float fixedDropEnergySpeed;
	private float gainEnergySpeed = 2f;
	private float dropMassSpeedFactor = 1f;

	private float massGainPercentage = 0.8f;
	private float massRatioRequired = 1.25f;

	private Player p;

	void Start () {
		fixedDropEnergySpeed = dropEnergySpeed;
		p = GameObject.FindObjectOfType<Player> ();
		dead = false;
		energy = 1.0f;
	}

	void FixedUpdate () {
		if (mass == 0)
			return;
		if (energy == 0)
			dropMassSpeedFactor = 2;
		else
			dropMassSpeedFactor = 1;
		energy = Mathf.Max (0, energy - fixedDropEnergySpeed);
		if (energy <= 0.5f) {
			mass = Mathf.Max (energy == 0 ? 0f : 0.5f, mass - dropMassSpeed * mass * dropMassSpeedFactor);
		}

		if (p.GetComponent<Ball> () != this && mass / p.ball.mass > 3) {
			fixedDropEnergySpeed = dropEnergySpeed * 2;
		}
	}

	void OnTriggerStay (Collider other) {
		Ball otherBall = other.gameObject.GetComponent<Ball> ();
		if (otherBall == null || otherBall.dead)
			return;
		float otherMass = otherBall.mass;
		if (mass / otherMass >= massRatioRequired) {
			Player p = otherBall.GetComponent<Player> ();
			if (p != null) {
				p.ball.mass = mass;
				p.playLoseSFX ();
				p.Die ();
			} else {
				otherBall.Die ();
			}
			energy = Mathf.Min (energy + gainEnergySpeed * (otherMass / mass), 1.0f);
			p = GetComponent<Player> ();
			if (p != null) {
				p.attackRollStartTime -= p.attackRollDuration;
				p.playEatSFX ();
			}
			mass += otherMass * massGainPercentage;
		}
	}

	public void Die () {
		mass = 0;
	}

}
