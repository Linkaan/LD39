using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public float wantedEnemyCount;

	public float maxX;
	public float maxY;
	public float minXY;

	public float curEnemyCount;

	private Player player;

	void Start () {
		player = GameObject.FindObjectOfType<Player> ();
		curEnemyCount = 0;
	}

	void Update () {
		if (player.ball.dead)
			return;
		if (curEnemyCount < wantedEnemyCount) {
			EnemyAI enemy = Instantiate (enemyPrefab).GetComponent<EnemyAI> ();
			enemy.spawner = this;
			curEnemyCount += 1;
		}
	}

	public void decrementEnemyCount () {
		curEnemyCount -= 1;
		//Debug.Log (curEnemyCount);
	}
}
