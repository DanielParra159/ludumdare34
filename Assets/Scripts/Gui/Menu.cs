using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public GameObject m_menu;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void showPause()
    {
        Pausable.pause = true;
        m_menu.SetActive(true);
    }
    public void resume()
    {
        m_menu.SetActive(false);
        Pausable.pause = false;
    }
    public void backMainMenu()
    {
        Application.LoadLevel("SC_Menu");
    }

    public void goLevel(int level)
    {
        Application.LoadLevel("SC_Levels_Richard_0"+level);
    }
    public void tutorial()
    {
        gameObject.GetComponentInChildren<Animator>().SetTrigger("Tutorial");
    }
    public void back()
    {
        gameObject.GetComponentInChildren<Animator>().SetTrigger("Back");
    }
    public void levels()
    {
        gameObject.GetComponentInChildren<Animator>().SetTrigger("Levels");
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
