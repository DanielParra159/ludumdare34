using UnityEngine;
using System.Collections;

public class StartCinematic : MonoBehaviour {

	public GameObject cinematic;


	void OnTriggerEnter(Collider other) {

		if (other.tag.Equals("Unit") ) 
			cinematic.SetActive(true);
	}


}
