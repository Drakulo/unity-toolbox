using UnityEngine;
using System.Collections;

public class TextureBillboard : MonoBehaviour
{
	/// <summary>
	/// The text renderer.
	/// </summary>
	public GameObject TextRenderer;
	
	/// <summary>
	/// The X speed.
	/// </summary>
	public float XSpeed;
	
	/// <summary>
	/// The Y speed.
	/// </summary>
	public float YSpeed;
	
	void Update ()
	{
		// Get the actual offset
		var offset = TextRenderer.renderer.material.mainTextureOffset;
		
		// Compute the offset according to elapsed time
		offset.x += XSpeed * Time.deltaTime;
		offset.y += YSpeed * Time.deltaTime;
		
		// Apply the offset movement
		TextRenderer.renderer.material.mainTextureOffset = offset;
	}
}
