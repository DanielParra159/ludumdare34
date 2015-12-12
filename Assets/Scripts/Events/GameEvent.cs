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
public class EventTypeLeaderUnitChosen : GameEvent
{
    public Unit.UNIT_TYPES m_type;
}
