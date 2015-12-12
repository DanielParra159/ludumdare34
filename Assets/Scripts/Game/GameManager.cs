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

    private Unit.UNIT_TYPES m_typeLeaderUnit;

    //VARIABLE QUE ALMACENA EL TIPO DE EDIFICIO A CONSTRUIR
    private Buildng.BUILDING_TYPES m_typeBuilding;

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            m_lastState = m_currentState;
            m_lastSubLevelState = m_currentSubLevelState;

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
        //SACAR LAS COMPROBACIONES DEL UPDATE Y REALIZAR LAS ACCIONES INDIVIDUALES EN LOS EVENTOS
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
    private void updateLevel() 
    {
        //SERIAS DUDAS SOBRE LA VALIDEZ DE ESTE CÓDIGO | KAITO
        /*
        switch(m_currentSubLevelState)
        {
            case SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL:
                //SI HACEMOS UN CAMBIADOR DE GUI QUE RECIBA EL TIPO DE UNIDAD, ESTE IF ES SIMPLIFICABLE
                if (m_typeLeaderUnit == Unit.UNIT_TYPES.UNIT_TYPE_WORKER) 
                {
                    //CAMBIAR GUI A LA DEL CONSTRUCTOR
                }
                else
                {
                    //CAMBIAR GUI A LA GUI DEL TIPO DE UNIDAD SELECCIONADA
                }
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD:
                //PROBABLEMENTE NUNCA SE ENTRE AQUÍ
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD:
                if (false) //JUGADOR HACE CLICK EN TERRENO VÁLIDO
                {
                    //UNIT BUILDER PASA A ESTADO UNIT_STATE_BUILDING
                    //VOLVER ACTIVO UNA INSTANCIA DEL BUILDING SELECCIONADO DE LA POOL
                    changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD);
                }
                break;
        }
        */
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
                //CAMBIAR GUI AL DE MOSTRAR GUI DE CONSTRUCCIONES
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD:
                //CAMBIAR GUI AL DE MOSTRAR BOTON DE CANCELACIÓN
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
    public void typeLeaderUnitChosen(Unit.UNIT_TYPES typeLeaderUnit)
    {
        m_typeLeaderUnit = typeLeaderUnit;
        updateLevel();
    }
    public void actionButtonClicked(GUIManager.ACTION_TYPES actionClicked)
    {
        switch (actionClicked) 
        {
            case GUIManager.ACTION_TYPES.ACTION_BUILD:
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD);
                break;
        }
    }
    public void typeBuildingClicked(Buildng.BUILDING_TYPES buildingType)
    {
        switch (buildingType)
        {
            case Buildng.BUILDING_TYPES.BUILDING_URBAN_CENTER:
                //MOSTRAR REPRESENTACION DEL EDIFICIO URBAN_CENTER EN EL CURSOR DEL JUGADOR
                m_typeBuilding = buildingType;
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD);
                break;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_HOUSE:
                //MOSTRAR REPRESENTACION DEL EDIFICIO HOUSE EN EL CURSOR DEL JUGADOR
                m_typeBuilding = buildingType;
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD);
                break;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_BARRACKS:
                //MOSTRAR REPRESENTACION DEL EDIFICIO BARRACKS EN EL CURSOR DEL JUGADOR
                m_typeBuilding = buildingType;
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD);
                break;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_UPGRADE:
                //MOSTRAR REPRESENTACION DEL EDIFICIO UPGRADE EN EL CURSOR DEL JUGADOR
                m_typeBuilding = buildingType;
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD);
                break;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_TOWER:
                //MOSTRAR REPRESENTACION DEL EDIFICIO TOWER EN EL CURSOR DEL JUGADOR
                m_typeBuilding = buildingType;
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD);
                break;
        }
    }
}
