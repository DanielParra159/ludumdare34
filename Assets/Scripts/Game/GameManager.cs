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
[RequireComponent(typeof(ResourcesManager))]

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public enum GAME_STATES
    {
        GAME_STATE_MENU, GAME_STATE_LOADING, GAME_STATE_STARTING_LEVEL, GAME_STATE_LEVEL, GAME_STATE_PAUSE,
        GAME_STATE_GAME_OVER, GAME_STATE_MAX
    }
    public enum SUB_LEVEL_STATES
    {
        SUBGAME_STATE_NORMAL, 
        SUBGAME_STATE_CHOOSE_TO_BUILD, 
        SUBGAME_STATE_WHERE_TO_BUILD, 
        SUBGAME_STATE_START_SELECTION,
        SUBGAME_STATE_PATROL_WHERE,
        SUBGAME_STATE_MOVE_ATTACKING_WHERE,
        SUBGAME_STATE_REPAIR_WHAT,
        SUBGAME_STATE_WHERE_TO_MEETING,
        SUBGAME_STATE_MAX
    }
    public static int maxGameStates = (int)GAME_STATES.GAME_STATE_MAX;
    public static int maxSubGameStates = (int)SUB_LEVEL_STATES.SUBGAME_STATE_MAX;

    [SerializeField]
    private GAME_STATES m_currentState;
    private GAME_STATES m_lastState;
    [SerializeField]
    private SUB_LEVEL_STATES m_currentSubLevelState;
    private SUB_LEVEL_STATES m_lastSubLevelState;

    [SerializeField]
    [Range(0,100)]
    private float m_cameraSpeed = 20;

    private BuildingManager m_buildingManager;
    private GUIManager m_guiManager;
    private ArmyManager m_armyManager;
    private Unit m_typeLeaderUnit = null;
    private Buildng m_typeLeaderBuilding = null;

    private Vector3 m_startSelectPosition;
    public GameObject m_selectRect;

    //VARIABLE QUE ALMACENA EL TIPO DE EDIFICIO A CONSTRUIR
    private Buildng.BUILDING_TYPES m_typeBuilding;

    private InputManager m_inputManager;
    private ResourcesManager m_resourceManager;
    private FeedbackMessagesManager m_feedbackMessagesManager;

    public GameObject[] m_inBuildProcess;

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
        for (int i = 0; i < m_inBuildProcess.Length; ++i )
        {
            m_inBuildProcess[i] = Instantiate<GameObject>(m_inBuildProcess[i]);
            m_inBuildProcess[i].SetActive(false);
        }
	}

    void Start()
    {
        EventManager.Start();
        m_buildingManager = BuildingManager.instance;
        m_armyManager = ArmyManager.instance;
        m_inputManager = InputManager.instance;
        m_guiManager = GUIManager.instance;
        m_resourceManager = ResourcesManager.instance;
        m_feedbackMessagesManager = FeedbackMessagesManager.instance;
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
            {
                Ray ray = Camera.main.ScreenPointToRay(m_inputManager.getScreenMousePosition());
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit)) return;
                Vector3 center = (hit.point - m_startSelectPosition);
                center.y += 0.1f;
                m_selectRect.transform.position = m_startSelectPosition + center * 0.5f;
                m_selectRect.transform.localScale = new Vector3(center.x, center.z, center.y);
                break;
            }
            case SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD:
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit)) return;

                Vector3 position = hit.point;
                m_inBuildProcess[(int)m_typeBuilding].transform.position = position;

                break;
            }
                
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

                if (m_typeLeaderUnit != null && m_typeLeaderUnit.getType() == Unit.UNIT_TYPES.UNIT_TYPE_WORKER)
                {
                    m_guiManager.activatePanel(GUIManager.PANELS.BUILDER_PANEL);
                }
                if (m_typeLeaderUnit == null)
                {
                    m_guiManager.activatePanel(GUIManager.PANELS.NOTHING_PANEL);
                }
                if (m_typeLeaderUnit != null &&
                    (m_typeLeaderUnit.getType() == Unit.UNIT_TYPES.UNIT_TYPE_WARRIOR_SWORDMAN ||
                    m_typeLeaderUnit.getType() == Unit.UNIT_TYPES.UNIT_TYPE_WARRIOR_LANCER ||
                    m_typeLeaderUnit.getType() == Unit.UNIT_TYPES.UNIT_TYPE_WARRIOR_ARCHER))
                {
                    m_guiManager.activatePanel(GUIManager.PANELS.ARMY_PANEL);
                }
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD:
                m_guiManager.activatePanel(GUIManager.PANELS.BUILDINGS_PANEL);
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD:
                m_guiManager.activatePanel(GUIManager.PANELS.NOTHING_PANEL);
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_START_SELECTION:
                m_selectRect.SetActive(true);
                break;
            case SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_MEETING:
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
            else if ((unitAuxPressed = m_armyManager.isSelectedAnyUnit()) != null)
            {
                //tenemos seleccionado alguna unidad? comprobamos si se ha pulsado sobre una unidad, sobre un edificio o sobre suelo
                if ((buildingAux = m_buildingManager.isPressedAnyEnemyBuilding(position)) != null)
                {
                    
                    m_armyManager.goToAttack(buildingAux.getPosition());
                }
                else if ((buildingAux = m_buildingManager.isPressedAnyAllyBuilding(position)) != null)
                {
                    Resource resourceAux;
                    //goto
                    if ((resourceAux = buildingAux.GetComponent<Resource>()) != null)
                    {
                        m_armyManager.goToRecollect(buildingAux.getPosition(), resourceAux);
                    }
                    else
                    {
                        m_armyManager.goTo(position);
                    }
                }
                else if ((unitTargetAux = m_armyManager.isPressedAnyEnemyUnit(position)) != null)
                {
                    m_armyManager.goToAttack(position);
                }//@todo:recurso
                else
                {
                    m_armyManager.goTo(position);
                }
            
            }

            //nos movemos con la selección o movemos el punto de encuentro si es un edificio, además si es un aldeado cancelamos la construcción
            //SI SE HACE LA LINEA DE ABAJO SE PIERDE EL PANEL DE LOS EDIFICIOS SELECCIONADOS AL ASIGNAR UN MEETING POINT
            //changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL); //@todo: se tiene que notificar al aldeano para que cambie su interfaz?
            //lanzar evento de boton derecho para que se avise a armymanager y buildingmanager
        }
        else if (button == InputManager.MOUSE_BUTTONS.MOUSE_BUTTON_LEFT && m_currentState == GAME_STATES.GAME_STATE_LEVEL )
        {
            if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL || 
                m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_CHOOSE_TO_BUILD)
            {
                //empieza selección
                m_startSelectPosition = position;
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_START_SELECTION);
            }
            else if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD)
            {
                //COMPROBAR SI LA ZONA EN LA QUE SE QUIERE COLOCAR ES VÁLIDA
                if (m_resourceManager.haveEnoughResources(m_typeBuilding))
                {
                    m_buildingManager.spawnBuilding(0, m_typeBuilding, position);
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL);
                        m_inBuildProcess[(int)m_typeBuilding].SetActive(false);
                        Cursor.visible = true;
                    }
                }
                else
                {
                    changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL);
                    m_inBuildProcess[(int)m_typeBuilding].SetActive(false);
                    Cursor.visible = true;
                }
            }
            else if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_MOVE_ATTACKING_WHERE)
            {
                m_armyManager.goToAttack(position);
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL);
            }
            else if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_PATROL_WHERE)
            {
                m_armyManager.goToPatrol(position);
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL);
            }
            else if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_REPAIR_WHAT)
            {
                Buildng building = m_buildingManager.isPressedAnyAllyBuilding(position);
                if(building != null)
                {
                    m_armyManager.goToRepair(building);
                }
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL);
            }
            else if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_MEETING)
            {
                m_buildingManager.setMeetingPoint(position);
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
                m_typeLeaderUnit = m_armyManager.selectUnits(m_startSelectPosition, position);
                if (m_typeLeaderUnit==null)
                {
                    m_typeLeaderBuilding = m_buildingManager.selectBuilding(m_startSelectPosition, position);
                }
                else 
                {
                    m_buildingManager.unselectBuilding();
                }
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_NORMAL);
            } /*if (m_currentSubLevelState == SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD)
            {
                //construimos si es posible
            }*/
            if (m_typeLeaderBuilding != null)
            {
                Buildng.BUILDING_TYPES building = m_typeLeaderBuilding.getType();
                if (building == Buildng.BUILDING_TYPES.BUILDING_URBAN_CENTER)
                {
                    m_guiManager.activatePanel(GUIManager.PANELS.URBAN_CENTRE_PANEL);
                }
                else if (building == Buildng.BUILDING_TYPES.BUILDING_TYPE_HOUSE)
                {
                    m_guiManager.activatePanel(GUIManager.PANELS.NOTHING_PANEL);
                }
                else if (building == Buildng.BUILDING_TYPES.BUILDING_TYPE_BARRACKS)
                {
                    m_guiManager.activatePanel(GUIManager.PANELS.BARRACK_PANEL);
                }
                else if (building == Buildng.BUILDING_TYPES.BUILDING_TYPE_UPGRADE)
                {
                    m_guiManager.activatePanel(GUIManager.PANELS.UPGRADE_PANEL);
                }
                else if (building == Buildng.BUILDING_TYPES.BUILDING_TYPE_TOWER)
                {
                    m_guiManager.activatePanel(GUIManager.PANELS.TOWER_PANEL);
                }
            }
        }

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
            case GUIManager.ACTION_TYPES.ACTION_MOVE_ATTACKING:
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_MOVE_ATTACKING_WHERE);
                break;
            case GUIManager.ACTION_TYPES.ACTION_PATROL:
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_PATROL_WHERE);
                break;
            case GUIManager.ACTION_TYPES.ACTION_REPAIR:
                changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_REPAIR_WHAT);
                break;
            case GUIManager.ACTION_TYPES.ACTION_STOP:
                m_armyManager.stopUnits();
                break;
        }
    }
    public void typeBuildingClicked(Buildng.BUILDING_TYPES buildingType)
    {
        m_typeBuilding = buildingType;
        m_inBuildProcess[(int)buildingType].SetActive(true);
        Cursor.visible = false;
        changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_BUILD);
    }
    public void moveCamera(Vector3 dir)
    {
        Camera.main.transform.Translate(dir * Time.deltaTime * m_cameraSpeed);
    }
    public void meetingPoint()
    {
        changeSubLevelState(SUB_LEVEL_STATES.SUBGAME_STATE_WHERE_TO_MEETING);
    }
}
