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
[RequireComponent(typeof(GUIManager))]
[RequireComponent(typeof(Map))]

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public enum GAME_STATES
    {
        GAME_STATE_MENU, GAME_STATE_LOADING, GAME_STATE_STARTING_LEVEL, GAME_STATE_LEVEL, GAME_STATE_PAUSE,
        GAME_STATE_GAME_OVER, GAME_STATE_MAX
    }
    public enum SUB_LEVEL_STATES
    {
        SUBGAME_STATE_NORMAL, SUBGAME_STATE_CHOOSE_TO_BUILD, SUBGAME_STATE_WHERE_TO_BUILD, SUBGAME_STATE_START_SELECTION, SUBGAME_STATE_MAX
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
    private GUIManager m_guiManager;
    private ArmyManager m_armygManager;
    private Unit.UNIT_TYPES m_typeLeaderUnit;

    private Vector3 m_startSelectPosition;
    public GameObject m_selectRect;

    //VARIABLE QUE ALMACENA EL TIPO DE EDIFICIO A CONSTRUIR
    private Buildng.BUILDING_TYPES m_typeBuilding;

    private InputManager m_inputManager;

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
        m_buildingManager = BuildingManager.instance;
        m_armygManager = ArmyManager.instance;
        m_inputManager = InputManager.instance;
        m_guiManager = GUIManager.instance;
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
    private void updateStartingLevel()
    {
        changeState(GAME_STATES.GAME_STATE_LEVEL);
    }
    private void updateLevel() 
    {
        switch(m_currentSubLevelState)
        {
            case SUB_LEVEL_STATES.SUBGAME_STATE_START_SELECTION:
                Ray ray = Camera.main.ScreenPointToRay(m_inputManager.getScreenMousePosition());
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit)) return;
                Vector3 center = (hit.point - m_startSelectPosition);
                center.y += 0.1f;
                m_selectRect.transform.position = m_startSelectPosition + center * 0.5f;
                m_selectRect.transform.localScale = center;
                break;
        }
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
                m_selectRect.SetActive(false);

                if (m_typeLeaderUnit == Unit.UNIT_TYPES.UNIT_TYPE_WORKER)
                {
                    m_guiManager.activatePanel(1);
                }
                else
                {
                    m_guiManager.activatePanel(0);
                }

                //CAMBIAR GUI Y MOSTRAR ACCIONES DE NADA SELECCIONADO SI ALDEANO NO SELECCIONADO
                //O ACCIONES DE ALDEANO SI ALDEANO SELECCIONADO
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD:
                //CAMBIAR GUI AL DE MOSTRAR GUI DE CONSTRUCCIONES
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD:
                m_guiManager.activatePanel(0);
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_START_SELECTION:
                m_selectRect.SetActive(true);
                break;
        }
    }

    /*
     * Solo llegan eventos que se hagan dentro de la zona de acci�n
     */
    public void onMouseDown(InputManager.MOUSE_BUTTONS button, Vector3 screenPosition)
    {

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;

        Vector3 position = hit.point;
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
                        m_armygManager.goTo(position);
                    }
                    else
                    {
                        //attack
                        m_armygManager.goToAttack(buildingAux.getPosition());
                    }
                }
                else if ((unitTargetAux = m_armygManager.isPressedAnyEnemyUnit(position)) != null)
                {
                    m_armygManager.goToAttack(position);
                }//@todo:recurso
                else
                {
                    m_armygManager.goTo(position);
                }
            }
            
            
            //nos movemos con la selección o movemos el punto de encuentro si es un edificio, además si es un aldeado cancelamos la construcción
            changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL); //@todo: se tiene que notificar al aldeano para que cambie su interfaz?
            //lanzar evento de boton derecho para que se avise a armymanager y buildingmanager
        }
        else if (button == InputManager.MOUSE_BUTTONS.MOUSE_BUTTON_LEFT && m_currentState == GAME_STATES.GAME_STATE_LEVEL )
        {
            if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL || m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD)
            {
                //empieza selección
                m_startSelectPosition = position;
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_START_SELECTION);
            }
            else //if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD)
            {
                //al levantar construiremos
            }
            
        }
        //comprobar en que sub estado estamos, construyendo, sin selecci�n...
        //activar flag y prepararse para selecci�n multiple
    }
    /*
     * Solo llegan eventos que se hagan dentro de la zona de acci�n
     */
    public void onMouseUp(InputManager.MOUSE_BUTTONS button, Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;

        Vector3 position = hit.point;
        //comprobar en que sub estado estamos, construyendo, sin selecci�n...
        //Hacer selecci�n multiple, si no hay deferencia entre las posiciones queremos una selecci�n simple
        if (button == InputManager.MOUSE_BUTTONS.MOUSE_BUTTON_LEFT && m_currentState == GAME_STATES.GAME_STATE_LEVEL)
        {
            //termina selección
            if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_START_SELECTION)
            {
                m_armygManager.selectUnits(m_startSelectPosition, position);
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL);
            }
            else if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD)
            {
                //construimos si es posible
            }
        }

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
            case GUIManager.ACTION_TYPES.ACTION_ESCAPE:
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL);
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
