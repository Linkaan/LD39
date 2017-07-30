using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public float mass;
	public float energy;

	private float dropMassSpeed = 0.0005f;
	private float dropEnergySpeed = 0.0005f;
	private float gainEnergySpeed = 0.03f;
	private float dropMassSpeedFactor = 1f;

	void Start () {
		energy = 1.0f;
	}

	void FixedUpdate () {
		if (mass == 0)
			return;
		if (energy == 0)
			dropMassSpeedFactor = 2;
		else
			dropMassSpeedFactor = 1;
		energy = Mathf.Max (0, energy - dropEnergySpeed);
		if (energy <= 0.5f) {
			mass = Mathf.Max (energy == 0 ? 0f : 0.5f, mass - dropMassSpeed * mass * dropMassSpeedFactor);
		}
	}

	void OnTriggerEnter (Collider other) {
		Ball otherBall = other.gameObject.GetComponent<Ball> ();
		if (otherBall == null)
			return;
		float otherMass = otherBall.mass;
		if (mass / otherMass >= 1.25f) {
			Player p = otherBall.GetComponent<Player> ();
			if (p != null) {
				p.ball.mass = mass;
				p.Die ();
			} else {
				otherBall.Die ();
			}
			energy = Mathf.Max (energy + gainEnergySpeed * (otherMass / mass), 1.0f);
			p = GetComponent<Player> ();
			if (p != null) {
				p.attackRollStartTime -= p.attackRollDuration;
			}
			mass += otherMass * 1.05f;
		}
	}

	public void Die () {
		mass = 0;
	}

}
