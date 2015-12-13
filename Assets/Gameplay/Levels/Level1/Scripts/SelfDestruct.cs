using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	// Use this for initialization
	void Start () {
		destruct ();

	}

	IEnumerator destruct()
	{
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);

	}
	

}
