using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public static GUIManager instance = null;
    public enum ACTION_TYPES
    {
        ACTION_BUILD, ACTION_ESCAPE, MAX_ACTIONS
    }

    public static int maxActionTypes = (int)GUIManager.ACTION_TYPES.MAX_ACTIONS;

    public EventTypeBuildingClicked eventTypeBuildingClicked;
    public EventActionButtonClicked eventActionButtonClicked;

    [Tooltip("Para añadir los diferentes paneles a activar y desactivar")]
    public GameObject[] panels = null;

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
        eventTypeBuildingClicked = new EventTypeBuildingClicked();
        eventActionButtonClicked = new EventActionButtonClicked();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void typeBuildingClicked(int buildingType)
    {
        eventTypeBuildingClicked.m_buildingType = (Buildng.BUILDING_TYPES)buildingType;
        eventTypeBuildingClicked.SendEvent();
    }

    public void actionButtonClicked(int actionType)
    {
        eventActionButtonClicked.m_actionType = (ACTION_TYPES)actionType;
        eventActionButtonClicked.SendEvent();
    }
    
    public void activatePanel(int panelToActivate)
    {   
        foreach(GameObject panel in panels)
            if(panel!=panels[panelToActivate])
                panel.SetActive(false);
        panels[panelToActivate].SetActive(true);
    }
}
