using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

    public static GUIManager instance = null;
    public enum ACTION_TYPES
    {
        ACTION_BUILD, ACTION_ESCAPE, ACTION_PATROL, ACTION_STOP, ACTION_MOVE_ATTACKING, ACTION_REPAIR, MAX_ACTIONS
    }
    public enum PANELS
    {
        NOTHING_PANEL, BUILDER_PANEL, BUILDINGS_PANEL, ARMY_PANEL, MAX_PANELS
    }
    public static int maxActionTypes = (int)GUIManager.ACTION_TYPES.MAX_ACTIONS;
    public static int maxPanels = (int)GUIManager.PANELS.MAX_PANELS;

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
    
    public void activatePanel(PANELS panelToActivate)
    {
        for (int i = 0; i < panels.Length; ++i )
        {
            if (i != (int)panelToActivate)
            {
                panels[i].SetActive(false);
            }
        }
        panels[(int)panelToActivate].SetActive(true);
    }
}
