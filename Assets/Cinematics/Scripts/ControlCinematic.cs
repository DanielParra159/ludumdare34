using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlCinematic : MonoBehaviour {

	public GameObject canvasCinematico;
	public GameObject HUD;
	public GameObject MainCamera;
	private GameObject[] units;


	public void destroyUnitsForCinematic()
	{
	
		units = GameObject.FindGameObjectsWithTag ("Unit");
		foreach (GameObject unit in units) {

			unit.SetActive (false);
		}
	}

	public void setCanvasCinematico(GameObject myCanvasCinematico){
	
			canvasCinematico = myCanvasCinematico;

	}

	
	public void enableHUD(){
		
		HUD.SetActive(true);
		
	}

		
	public void loadLevel(){
		
		Application.LoadLevel ("SC_Levels_Richard_01");
		
	}

	public void disableHUD(){
		
		HUD.SetActive(false);
		
	}

	public void enableMainCamera(){
		
		MainCamera.SetActive(true);
		
	}

	


	public void enableCanvasCinematico(){

		canvasCinematico.SetActive(true);
		
	}

	public void disableCanvasCinematico(){
		
		canvasCinematico.SetActive(false);
		
	}

	public void cambiarRetrato(Sprite retrato){
		
		Image m_image = canvasCinematico.transform.Find("Panel/Retrato").GetComponent<Image>();
		m_image.overrideSprite = retrato;
		
	}

	
	public void cambiarNombre(string nombre){

		Text m_nombre = canvasCinematico.transform.Find("Panel/Nombre").GetComponent<Text>();
		m_nombre.text = nombre;

	}

	
	public void cambiarTexto(string texto){


		Text m_texto = canvasCinematico.transform.Find("Panel/Texto").GetComponent<Text>();
		m_texto.text = texto;
	}



	IEnumerator FadeScreenToBlack ()
	{
		Image m_image = canvasCinematico.transform.Find("BlackScreen").GetComponent<Image>();
		Color c = m_image.color;
		float alpha = 0f;
		
		while(alpha < 1f)
		{
			Debug.Log ("current alpha: "+alpha);
			
			alpha += 0.02f;
			c.a = alpha;
			m_image.color = c;
			yield return new WaitForSeconds(0.025f);
		}

		canvasCinematico.SetActive (false);
		c.a = 0;
		m_image.color = c;
		GameObject.Find ("CinemaController").SetActive (false);
		GameObject.Find ("HUD").SetActive (true);
	}


	
	IEnumerator shakePanel ()
	{
		Debug.Log ("Enter shake panel");
		RectTransform m_rectTransform = canvasCinematico.transform.Find("Panel").GetComponent<RectTransform>();
		Vector3 stationaryPosition = m_rectTransform.localPosition;
		bool moveUp = true;
		
		for(int i=0;i<20;i++){
			//Debug.Log ("Moved "+i+" times");
			
			if(moveUp)
				m_rectTransform.localPosition = m_rectTransform.localPosition + new Vector3 (0, 15, 0);
			else
				m_rectTransform.localPosition = m_rectTransform.localPosition + new Vector3 (0, -15, 0);
	
			moveUp = !moveUp;
			yield return new WaitForSeconds(0.035f);
			//yield return null;
			
		}
		m_rectTransform.localPosition = stationaryPosition;
		
		yield return new WaitForSeconds(0.3f);
		
	
	}


}
