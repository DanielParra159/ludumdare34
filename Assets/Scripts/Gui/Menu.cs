using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void menu()
    {
        gameObject.GetComponentInChildren<Animator>().SetTrigger("Menu");
    }
    public void game()
    {
        Application.LoadLevel("SC_Levels_Richard_00");
    }
    public void credits()
    {
        gameObject.GetComponentInChildren<Animator>().SetTrigger("Credits");
    }
    public void exit()
    {
        Application.Quit();
    }
}
