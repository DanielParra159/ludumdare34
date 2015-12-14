using UnityEngine;
using System.Collections;

public class EventManager {

	public enum EVENTS {
		EVENT_PLAY_SOUND,
        EVENT_MOUSE_DOWN,
        EVENT_MOUSE_UP,
        EVENT_SPAWN_UNIT,
        EVENT_SPAWN_BUILDING,
        EVENT_ADD_RESOURCE,
        EVENT_ACTION_BUTTON_CLICKED,
        EVENT_TYPE_BUILDING_CLICKED,
        EVENT_READY_TO_REPAIR,
        EVENT_STOP_REPAIRING,
        EVENT_MOVE_CAMERA,
        EVENT_MEETING_POINT,
        EVENT_SHOW_UNIT_PANEL,
        EVENT_HIDE_UNIT_PANEL,
		MAX_EVENTS
	};

    private static SoundManager soundManager;
    private static GameManager gameManager;
    private static ArmyManager armyManager;
    private static BuildingManager buildingManager;
    private static ResourcesManager resourcesManager;
    private static OurCamera ourCamera;

    // Use this for initialization
    public static void Start () {
        soundManager = SoundManager.instance;
        gameManager = GameManager.instance;
        armyManager = ArmyManager.instance;
        buildingManager = BuildingManager.instance;
        resourcesManager = ResourcesManager.instance;
        ourCamera = OurCamera.instance;
    }
	
	/*
	 * Recibe un evento y lo comunica a la clase (por lo general manager) que le corresponda
	 * bReplicable a true lo envia por la red 
	 */
	public static void newEvent(GameEvent hEvent)
	{
		switch( hEvent.type )
		{
            case EVENTS.EVENT_PLAY_SOUND:
		{
            EventPlaySound eventPlaySound = (EventPlaySound)hEvent;
            soundManager.PlaySingle(eventPlaySound.m_sound);
			break;
		}
            case EVENTS.EVENT_MOUSE_DOWN:
        {
            EventMouseClick eventMouseClick = (EventMouseClick)hEvent;
            gameManager.onMouseDown(eventMouseClick.m_button, eventMouseClick.m_screenPosition);
            break;
        }
            case EVENTS.EVENT_MOUSE_UP:
        {
            EventMouseClick eventMouseClick = (EventMouseClick)hEvent;
            gameManager.onMouseUp(eventMouseClick.m_button, eventMouseClick.m_screenPosition);
            break;
        }
            case EVENTS.EVENT_SPAWN_UNIT:
        {
            EventSpawnUnit eventSpawnUnit = (EventSpawnUnit)hEvent;
            armyManager.spawnUnit(eventSpawnUnit.m_team, eventSpawnUnit.m_type, eventSpawnUnit.m_position, eventSpawnUnit.m_meetingPoint);
            break;
        }
            case EVENTS.EVENT_SPAWN_BUILDING:
        {
            EventSpawnBuilding eventSpawnBuilding = (EventSpawnBuilding)hEvent;
            buildingManager.spawnBuilding(eventSpawnBuilding.m_team, eventSpawnBuilding.m_type, eventSpawnBuilding.m_position);
            break;
        }
            case EVENTS.EVENT_ADD_RESOURCE:
        {
            EventAddResource eventAddResource = (EventAddResource)hEvent;
            resourcesManager.addResources((int)eventAddResource.m_team, eventAddResource.m_type, eventAddResource.m_amount);
            break;
        }
            case EVENTS.EVENT_ACTION_BUTTON_CLICKED:
        {
            EventActionButtonClicked eventActionButtonClicked = (EventActionButtonClicked)hEvent;
            gameManager.actionButtonClicked(eventActionButtonClicked.m_actionType);
            break;
        }
            case EVENTS.EVENT_TYPE_BUILDING_CLICKED:
        {
            EventTypeBuildingClicked eventTypeBuildingClicked = (EventTypeBuildingClicked)hEvent;
            gameManager.typeBuildingClicked(eventTypeBuildingClicked.m_buildingType);
            break;
        }
            case EVENTS.EVENT_READY_TO_REPAIR:
        {
            ReadyToRepairBuilding readyToRepairBuilding = (ReadyToRepairBuilding)hEvent;
            buildingManager.readyToRepair(readyToRepairBuilding.building);
            break;
        }
            case EVENTS.EVENT_STOP_REPAIRING:
        {
            StopRepairingBuilding stopRepairingBuilding = (StopRepairingBuilding)hEvent;
            buildingManager.stopRepairing(stopRepairingBuilding.building);
            break;
        }
            case EVENTS.EVENT_MOVE_CAMERA:
        {
            EventMoveCamera eventMoveCamera = (EventMoveCamera)hEvent;
            ourCamera.moveCamera(eventMoveCamera.m_dir);
            break;
        }
            case EVENTS.EVENT_MEETING_POINT:
        {
            EventMeetingPointClicked meetingPointClicked = (EventMeetingPointClicked)hEvent;
            gameManager.meetingPoint();
            break;
        }
            case EVENTS.EVENT_SHOW_UNIT_PANEL:
        {
            EventShowUnitPanel eventShowUnitPanel = (EventShowUnitPanel)hEvent;
            GUIManager.instance.showUnitPanel(eventShowUnitPanel.m_type, eventShowUnitPanel.m_damage, eventShowUnitPanel.m_life);
            break;
        }
		}
		//avisar a la red
		/*if ( bReplicable )
		{
			
		}*/
	}
}
