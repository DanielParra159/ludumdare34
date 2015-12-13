using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HelpViking : MonoBehaviour {

	private bool eventStarted = false;
	private ControlCinematic controlCinematic;
	public GameObject canvas;
	public Sprite vikingRetrato;
	

	void OnTriggerEnter(Collider other) {
		Debug.Log ("triggered enter");
		
		if (other.tag.Equals("Unit") && !eventStarted  ) {
			Debug.Log ("entered unit");
			eventStarted = true;
			
			canvas.SetActive(true);
			Image m_image = canvas.transform.Find("Panel/Retrato").GetComponent<Image>();
			m_image.overrideSprite = vikingRetrato;
			
			Text m_texto = canvas.transform.Find("Panel/Texto").GetComponent<Text>();
			m_texto.text = "The fucking junkies man, they stabbed me and took my jacket. Find t them kick their asses, they went south of here";
			StartCoroutine(disableCanvasOntime());

			Text m_nombre = canvas.transform.Find("Panel/Nombre").GetComponent<Text>();
			m_nombre.text = "Viking";
			StartCoroutine(disableCanvasOntime());
			
		}
	}
	
	IEnumerator disableCanvasOntime()
	{
		Debug.Log("Disabling");
		yield return new WaitForSeconds(8f);
		canvas.SetActive (false);
		Debug.Log("Disabled");
		
		
	}
}
