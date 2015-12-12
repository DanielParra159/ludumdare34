using UnityEngine;
using System.Collections;

public class TextureOffset : MonoBehaviour {
	
	public float scrollSpeedX = 1.5f;
	public float scrollSpeedY = 1.5f;
	
	void Update () {
		var offsetX = scrollSpeedX * Time.deltaTime;
		var offsetY = scrollSpeedY * Time.deltaTime;
		Renderer r = GetComponent<Renderer> ();
		r.material.mainTextureOffset += new Vector2(offsetX,offsetY);
	}
	
}
