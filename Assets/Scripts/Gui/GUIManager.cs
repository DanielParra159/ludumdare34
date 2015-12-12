using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public static GUIManager instance = null;
    public enum ACTION_TYPES
    {
        ACTION_BUILD, MAX_ACTIONS
    }

    public static int maxActionTypes = (int)GUIManager.ACTION_TYPES.MAX_ACTIONS;

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
