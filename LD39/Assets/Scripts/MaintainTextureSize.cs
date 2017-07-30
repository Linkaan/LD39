using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaintainTextureSize : MonoBehaviour {

	public Texture texture;
	public float textureToMeshZ = 2f;

	private Vector3 prevScale = Vector3.one;
	private float prevTextureToMeshZ = 1f;

	private new Renderer renderer;

	void Start () {
		this.renderer = GetComponent<Renderer> ();
		this.prevScale = gameObject.transform.localScale;
		this.prevTextureToMeshZ = this.textureToMeshZ;

		this.UpdateTiling ();
	}
	
	void Update () {
		if(gameObject.transform.lossyScale != prevScale || !Mathf.Approximately(this.textureToMeshZ, prevTextureToMeshZ))
			this.UpdateTiling();

		// Maintain previous state variables
		this.prevScale = gameObject.transform.lossyScale;
		this.prevTextureToMeshZ = this.textureToMeshZ;
	}

	[ContextMenu("UpdateTiling")]
	void UpdateTiling()
	{
		// A Unity plane is 10 units x 10 units
		float planeSizeX = 10f;
		float planeSizeZ = 10f;

		// Figure out texture-to-mesh width based on user set texture-to-mesh height
		float textureToMeshX = ((float)this.texture.width/this.texture.height)*this.textureToMeshZ;

		renderer.sharedMaterial.mainTextureScale = new Vector2(planeSizeX*gameObject.transform.lossyScale.x/textureToMeshX, planeSizeZ*gameObject.transform.lossyScale.z/textureToMeshZ);
	}
}
