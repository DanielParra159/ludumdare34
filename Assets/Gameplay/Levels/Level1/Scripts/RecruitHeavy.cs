using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RecruitHeavy : MonoBehaviour {

	private bool eventStarted = false;
	private ControlCinematic controlCinematic;
	public GameObject canvas;
	public GameObject spawnMetalero;

	void Start() {
		ControlCinematic controlCinematic = gameObject.GetComponent<ControlCinematic> ();
		controlCinematic.setCanvasCinematico (canvas);
	}
	void OnTriggerEnter(Collider other) {
			
		
		if (other.tag.Equals("Unit") && !eventStarted  ) {
			Debug.Log ("entered unit");
			eventStarted = true;

			canvas.SetActive(true);

			
			Text m_texto = canvas.transform.Find("Panel/Texto").GetComponent<Text>();
			m_texto.text = "BROOOO!! Lets kick some poser's ass!!";

			StartCoroutine(disableCanvasOntime());

		}
	}

	IEnumerator disableCanvasOntime()
	{
		Debug.Log("Disabling");
		yield return new WaitForSeconds(2.5f);
		canvas.SetActive (false);
		spawnMetalero.SetActive(true);
		gameObject.SetActive(false);
		Debug.Log("Disabled");
		

	}


}
