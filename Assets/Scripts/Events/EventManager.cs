using UnityEngine;
using System.Collections;

public class EventManager {

	public enum EVENTS {
		EVENT_PLAY_SOUND,
        EVENT_MOUSE_DOWN,
        EVENT_MOUSE_UP,
        EVENT_SPAWN_UNIT,
		MAX_EVENTS
	};

    private static SoundManager soundManager;
    private static GameManager gameManager;
    private static ArmyManager armyManager;

    // Use this for initialization
    public static void Start () {
        soundManager = SoundManager.instance;
        gameManager = GameManager.instance;
        armyManager = ArmyManager.instance;
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

		}
		//avisar a la red
		/*if ( bReplicable )
		{
			
		}*/
	}
}
