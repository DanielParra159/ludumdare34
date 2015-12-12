using UnityEngine;
using System.Collections;

public class GameEvent {
	
	public EventManager.EVENTS										type;
	public float 													fTime;	
	public void SendEvent()
	{
		fTime = Time.time;
		EventManager.newEvent( this );
	}
}
public class EventPlaySound : GameEvent {
	public AudioClip						 			m_sound;
    public EventPlaySound() { type = EventManager.EVENTS.EVENT_PLAY_SOUND; }	
}
public class EventMouseClick : GameEvent
{
    public InputManager.MOUSE_BUTTONS m_button;
    public Vector3 m_screenPosition;
    public EventMouseClick() { type = EventManager.EVENTS.EVENT_MOUSE_DOWN; }
}
public class EventSpawnUnit : GameEvent
{
    public TeamManager.TEAMS m_team;
    public Unit.UNIT_TYPES m_type;
    public Vector3 m_position;
    public Vector3 m_meetingPoint;
    public EventSpawnUnit() { type = EventManager.EVENTS.EVENT_SPAWN_UNIT; }
}
public class EventActionButtonClicked : GameEvent
{
    public GUIManager.ACTION_TYPES m_actionType;
    public EventActionButtonClicked() { type = EventManager.EVENTS.EVENT_ACTION_BUTTON_CLICKED; }
}
public class EventTypeBuildingClicked : GameEvent
{
    public Buildng.BUILDING_TYPES m_buildingType;
    public EventTypeBuildingClicked() { type = EventManager.EVENTS.EVENT_TYPE_BUILDING_CLICKED; }
}
public class ReadyToRepairBuilding : GameEvent
{
    public Buildng building;
    public ReadyToRepairBuilding() { type = EventManager.EVENTS.EVENT_READY_TO_REPAIR; }
}
public class StopRepairingBuilding : GameEvent
{
    public Buildng building;
    public StopRepairingBuilding() { type = EventManager.EVENTS.EVENT_STOP_REPAIRING; }
}
public class EventMoveCamera : GameEvent
{
    public Vector3 m_dir;
    public EventMoveCamera() { type = EventManager.EVENTS.EVENT_MOVE_CAMERA; }
}
