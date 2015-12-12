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
    public enum SUB_LEVEL_STATES
    {
        SUBGAME_STATE_NORMAL, SUBGAME_STATE_CHOOSE_TO_BUILD, SUBGAME_STATE_WHERE_TO_BUILD, SUBGAME_STATE_MAX
    }
    public static int maxGameStates = (int)GAME_STATES.GAME_STATE_MAX;
    public static int maxSubGameStates = (int)SUB_LEVEL_STATES.SUBGAME_STATE_MAX;

    [SerializeField]
    private GAME_STATES m_currentState;
    private GAME_STATES m_lastState;
    [SerializeField]
    private SUB_LEVEL_STATES m_currentSubLevelState;
    private SUB_LEVEL_STATES m_lastSubLevelState;

    private BuildingManager m_buildingManager;
    private ArmyManager m_armygManager;

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
        m_buildingManager = BuildingManager.instance;
        m_armygManager = ArmyManager.instance;
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
    private void updateStartingLevel()
    {
        changeState(GAME_STATES.GAME_STATE_LEVEL);
    }
    private void updateLevel() 
    {
        switch(m_currentSubLevelState)
        {
            case SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL:
                if(false/*JUGADOR SELECCIONA UN ALDEANO*/){
                    changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD);
                }
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD:
                if (false/*JUGADOR SELECCIONA UN EDIFICIO*/)
                {
                    changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD);
                }
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD:
                if (false/*JUGADOR HACE CLICK EN TERRENO VÁLIDO*/)
                {
                    //UNIT BUILDER PASA A ESTADO UNIT_STATE_BUILDING
                    //VOLVER ACTIVO UNA INSTANCIA DEL BUILDING SELECCIONADO DE LA POOL
                    changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD);
                }
                break;
        }
    }
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
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL);
                break;
            case GAME_STATES.GAME_STATE_PAUSE:
                break;
            case GAME_STATES.GAME_STATE_GAME_OVER:
                break;
        }
    }

    private void changeSubLevelState(SUB_LEVEL_STATES nextSubState)
    {
        m_lastSubLevelState = m_currentSubLevelState;
        m_currentSubLevelState = nextSubState;
        switch(m_currentSubLevelState)
        {
            case SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL:
                //CAMBIAR GUI Y MOSTRAR ACCIONES DE NADA SELECCIONADO SI ALDEANO NO SELECCIONADO
                //O ACCIONES DE ALDEANO SI ALDEANO SELECCIONADO
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD:
                //CAMBIAR GUI Y MOSTRAR EDIFICIOS
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD:
                //CAMBIAR GUI Y MOSTRAR BOTON DE CANCELACIÓN
                //MOSTRAR REPRESENTACION DEL EDIFICIO SELECCIONADO EN EL CURSOR DEL JUGADOR
                break;
        }
    }

    /*
     * Solo llegan eventos que se hagan dentro de la zona de acci�n
     */
    public void onMouseDown(InputManager.MOUSE_BUTTONS button, Vector2 screenPosition)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(screenPosition);
        if (button == InputManager.MOUSE_BUTTONS.MOUSE_BUTTON_RIGH && m_currentState == GAME_STATES.GAME_STATE_LEVEL)
        {
            
            //@todo: comprobar si se ha pulsad sobre algún elemento de interes, recurso, enemigo, edificio...
            Buildng buildingAux = null;
            Unit unitAuxPressed = null;
            Unit unitTargetAux = null;
           
            if ((buildingAux = m_buildingManager.isSelectedAnyBuilding()) != null )
            {
                //tenemos una construcción activa? Solo podemos cambiar el meetingpoint
                m_buildingManager.setMeetingPoint(position);
            }
            else if ((unitAuxPressed = m_armygManager.isSelectedAnyUnit()) != null)
            {
                //tenemos seleccionado alguna unidad? comprobamos si se ha pulsado sobre una unidad, sobre un edificio o sobre suelo
                if ((buildingAux = m_buildingManager.isPressedAnyBuilding(position)) != null)
                {
                    //comprobamos el equipo
                    if (buildingAux.getTeam() == unitAuxPressed.getTeam())
                    {
                        //goto
                        unitAuxPressed.goTo(position);
                    }
                    else
                    {
                        //attack
                        unitAuxPressed.goToAttack(buildingAux.getPosition());
                    }
                }
                else if ((unitTargetAux = m_armygManager.isPressedAnyUnit(position)) != null)
                {

                }//@todo:recurso
                else
                {
                    //suelo
                    unitAuxPressed.goTo(position);
                }
            }
            
            
            //nos movemos con la selección o movemos el punto de encuentro si es un edificio, además si es un aldeado cancelamos la construcción
            changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL); //@todo: se tiene que notificar al aldeano para que cambie su interfaz?
            //lanzar evento de boton derecho para que se avise a armymanager y buildingmanager
        }
        else if (button == InputManager.MOUSE_BUTTONS.MOUSE_BUTTON_LEFT && m_currentState == GAME_STATES.GAME_STATE_LEVEL && 
            m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL || m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD )
        {
            //empieza selección
        }
        //comprobar en que sub estado estamos, construyendo, sin selecci�n...
        //activar flag y prepararse para selecci�n multiple
    }
    /*
     * Solo llegan eventos que se hagan dentro de la zona de acci�n
     */
    public void onMouseUp(InputManager.MOUSE_BUTTONS button, Vector2 screenPosition)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(screenPosition);
        //comprobar en que sub estado estamos, construyendo, sin selecci�n...
        //Hacer selecci�n multiple, si no hay deferencia entre las posiciones queremos una selecci�n simple
        if (button == InputManager.MOUSE_BUTTONS.MOUSE_BUTTON_LEFT && m_currentState == GAME_STATES.GAME_STATE_LEVEL &&
            m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL || m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD)
        {
            //termina selección
            //@todo: para probar selección simple
            m_armygManager.selectUnit(position);
        }
    }
}
