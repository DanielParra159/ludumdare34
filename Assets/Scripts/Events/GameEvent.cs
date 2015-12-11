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
