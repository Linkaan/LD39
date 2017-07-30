using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMovement : MonoBehaviour {

	public Player player;

	private new Renderer renderer;
	private Bounds bounds;

	void Start () {
		renderer = GetComponent<Renderer> ();
		bounds = GetComponent<MeshFilter> ().mesh.bounds;
	}
	
	void LateUpdate () {
		transform.localScale = new Vector3 (player.ball.mass, player.ball.mass, player.ball.mass);
		renderer.material.mainTextureScale = new Vector2 (player.ball.mass, player.ball.mass);
		float boundsX = bounds.size.x;// / renderer.sharedMaterial.mainTextureScale.x / player.ball.mass;
		float boundsZ = bounds.size.z;// / renderer.sharedMaterial.mainTextureScale.y / player.ball.mass;
		transform.position = new Vector3 (player.transform.position.x, 0, player.transform.position.z);
		renderer.sharedMaterial.SetTextureOffset ("_MainTex", -0.5f * renderer.material.mainTextureScale + new Vector2 (-player.transform.position.x / boundsX, -player.transform.position.z / boundsZ));
	}
}
