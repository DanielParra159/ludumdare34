using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ArmyManager))]
[RequireComponent(typeof(TeamManager))]
[RequireComponent(typeof(BuildingManager))]
[RequireComponent(typeof(TimeManager))]
[RequireComponent(typeof(FeedbackMessagesManager))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(ResourcesManager))]
[RequireComponent(typeof(SoundManager))]

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public enum GAME_STATES
    {
        GAME_STATE_MENU, GAME_STATE_LOADING, GAME_STATE_STARTING_LEVEL, GAME_STATE_LEVEL, GAME_STATE_PAUSE,
        GAME_STATE_GAME_OVER, GAME_STATE_MAX
    }
    public static int maxGameStates = (int)GAME_STATES.GAME_STATE_MAX;

    [SerializeField]
    private GAME_STATES m_currentState;
    private GAME_STATES m_lastState;

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            m_lastState = m_currentState;

        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
	}

    void Start()
    {
        EventManager.Start();
    }
	
	// Update is called once per frame
	void Update () {
        switch (m_currentState)
        {
            case GAME_STATES.GAME_STATE_MENU:
                updateMenu();
                break;
            case GAME_STATES.GAME_STATE_LOADING:
                updateLoading();
                break;
            case GAME_STATES.GAME_STATE_STARTING_LEVEL:
                updateStartingLevel();
                break;
            case GAME_STATES.GAME_STATE_LEVEL:
                updateLevel();
                break;
            case GAME_STATES.GAME_STATE_PAUSE:
                updatePause();
                break;
            case GAME_STATES.GAME_STATE_GAME_OVER:
                updateGameOver();
                break;
        }
	}
    private void updateMenu() { }
    private void updateLoading() { }
    private void updateStartingLevel() { }
    private void updateLevel() { }
    private void updatePause() { }
    private void updateGameOver() { }

    private void changeState(GAME_STATES nextState)
    {
        m_lastState = m_currentState;
        m_currentState = nextState;
        switch (m_currentState)
        {
            case GAME_STATES.GAME_STATE_MENU:
                break;
            case GAME_STATES.GAME_STATE_LOADING:
                break;
            case GAME_STATES.GAME_STATE_STARTING_LEVEL:
                break;
            case GAME_STATES.GAME_STATE_LEVEL:
                break;
            case GAME_STATES.GAME_STATE_PAUSE:
                break;
            case GAME_STATES.GAME_STATE_GAME_OVER:
                break;
        }
    }

    /*
     * Solo llegan eventos que se hagan dentro de la zona de acci�n
     */
    public void onMouseDown(InputManager.MOUSE_BUTTONS button, Vector3 creenPosition)
    {
        //comprobar en que sub estado estamos, construyendo, sin selecci�n...
        //activar flag y prepararse para selecci�n multiple
    }
    /*
     * Solo llegan eventos que se hagan dentro de la zona de acci�n
     */
    public void onMouseUp(InputManager.MOUSE_BUTTONS button, Vector3 creenPosition)
    {
        //comprobar en que sub estado estamos, construyendo, sin selecci�n...
        //Hacer selecci�n multiple, si no hay deferencia entre las posiciones queremos una selecci�n simple
    }
}
