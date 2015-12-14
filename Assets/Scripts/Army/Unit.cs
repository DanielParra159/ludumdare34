using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Team))]
[RequireComponent(typeof(Life))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(Selectable))]
[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour {

    public enum UNIT_TYPES
    {
        UNIT_TYPE_WARRIOR_SWORDMAN, UNIT_TYPE_WARRIOR_LANCER,UNIT_TYPE_WARRIOR_ARCHER, UNIT_TYPE_WORKER, MAX_UNIT_TYPES
    }
    public enum NEUTRAL_UNIT_TYPES
    {
        NEUTRAL_UNIT_TYPE_SOLDIER, MAX_NEUTRAL_UNIT_TYPES
    }
    public enum UNIT_STATES
    {
        UNIT_STATE_IDLE, UNIT_STATE_GOING_TO, UNIT_STATE_ATTACKING, UNIT_STATE_DYING, UNIT_STATE_BUILDING, UNIT_STATE_PATROLLING,
        MAX_UNIT_STATES
    }
    public enum UNIT_SUB_STATES
    {
        UNIT_SUB_STATE_NORMAL, UNIT_SUB_STATE_FOLLOW_TARGET, UNIT_SUB_STATE_AGGRESSIVE, UNIT_SUB_STATE_RECOLLECTING, 
        UNIT_SUB_STATE_REPAIRING, MAX_UNIT_SUB_STATES
    }
    public static int maxNeutralUnitsTypes = (int)NEUTRAL_UNIT_TYPES.MAX_NEUTRAL_UNIT_TYPES;
    public static int maxUnitsTypes = (int)UNIT_TYPES.MAX_UNIT_TYPES;
    public static int maxUnitsStates = (int)UNIT_STATES.MAX_UNIT_STATES;

    [SerializeField]
    protected UNIT_TYPES m_type;
    protected UNIT_STATES m_currentState;
    protected UNIT_STATES m_lastState;
    protected UNIT_SUB_STATES m_currentSubState;
    protected UNIT_SUB_STATES m_lastSubState;


    protected Selectable m_selectable;
    protected Attack m_attack;
    protected int m_team;
    protected NavMeshAgent m_navMeshAgent;
    protected float m_speed;
    [SerializeField]
    [Tooltip("Radio de la unidad, se utiliza para las pulsaciones")]
    protected float m_radius = 2.0f;
    protected float m_radius2;
    [SerializeField]
    [Tooltip("Radio de detección sobre los enemigos")]
    protected float m_enemyDetectionRadius = 8.0f;
    protected float m_enemyDetectionRadius2;

    [SerializeField]
    [Tooltip("Radio de detección sobre los enemigos")]
    [Range(1,20)]
    protected float m_dyingTime = 2;
    protected float m_currentTime;

    protected Map m_map;
    protected Vector2 m_mapPos;
    protected GameObject m_target;
    protected GameObject m_recollectionPoint;

    protected Transform m_transform;
    protected Pausable m_pausable;
    protected bool m_initialized = false; //@todo cuando muera se tiene que poner a false

    //Par de atributos a usar para el patrol
    protected Vector3 m_positionInitial;
    protected Vector3 m_positionFinal;

    protected ReadyToRepairBuilding readyToRepairBuilding;
    protected Buildng buildingToRepair;
    protected Resource m_resourceToRecolect;
    public class STR_RESOURCE{
        public int amount;
        public ResourcesManager.RESOURCES_TYPES type;
        public bool recolecting;
    }
    protected STR_RESOURCE m_resourceRecolected;
    [SerializeField]
    [Tooltip("Tiempo que tarda en recolectar")]
    [Range(1, 10)]
    protected float m_timeToRecolect;
    [SerializeField]
    [Tooltip("Cantidad a recolectar")]
    [Range(1, 100)]
    protected int m_AmountToRecolect;
    protected StopRepairingBuilding stopRepairingBuilding;
    protected Animator m_animator;
    protected Life m_life;

    protected EventAddResource eventAddResource;

    void Awake()
    {
        m_transform = transform;
        m_pausable = new Pausable(onPause, onResume);

        m_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        m_speed = m_navMeshAgent.speed;
        m_radius2 = m_radius * m_radius;
        m_enemyDetectionRadius2 = m_enemyDetectionRadius * m_enemyDetectionRadius;
        m_animator = GetComponentInChildren<Animator>();

        
    }

	void Start () {
        TimeManager.registerChangedTime(onTimeChanged);
        Team teamAux = gameObject.GetComponent<Team>();
        m_team = teamAux.m_team;

        eventAddResource = new EventAddResource();
        eventAddResource.m_team = teamAux.m_myTeam;

        m_resourceRecolected = new STR_RESOURCE();
        m_resourceRecolected.amount = 0;
        m_resourceRecolected.recolecting = false;

        m_life = gameObject.GetComponent<Life>();
        m_life.registerOnDead(onDead);
        m_life.registerOnDamage(onDamage);

        m_selectable = GetComponent<Selectable>();
        m_map = Map.instance;
        m_attack = GetComponent<Attack>();

        readyToRepairBuilding = new ReadyToRepairBuilding();
        stopRepairingBuilding = new StopRepairingBuilding();
	}
	
    public void init()
    {
        m_initialized = true;
        m_map = Map.instance;
        m_map.addObjectToMap(m_transform.position, gameObject);

        m_resourceRecolected = new STR_RESOURCE();
        m_resourceRecolected.amount = 0;
        m_resourceRecolected.recolecting = false;

        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        if (m_animator != null)
        {
            m_animator.SetBool("dead", false);
        }

        if (m_life != null)
        {
            m_life.SetActive(true);
            m_life.init();
        }
    }

	// Update is called once per frame
	void Update () {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        if (m_pausable.Check()) return;
        
        switch (m_currentState)
        {
            case UNIT_STATES.UNIT_STATE_IDLE:
                updateIdle();
                break;
            case UNIT_STATES.UNIT_STATE_GOING_TO:
                updateGoing();
                break;
            case UNIT_STATES.UNIT_STATE_ATTACKING:
                updateAttacking();
                break;
            case UNIT_STATES.UNIT_STATE_DYING:
                updateDying();
                break;
            case UNIT_STATES.UNIT_STATE_PATROLLING:
                updatePatrolling();
                break;
        }
	}

    protected void updateIdle() 
    {
        switch (m_currentSubState)
        {
            case UNIT_SUB_STATES.UNIT_SUB_STATE_AGGRESSIVE:
                {
                    GameObject go = m_map.anyEnemyInRadious(transform.position, m_enemyDetectionRadius, m_team);
                    if(go!=null)
                    {
                        goToTarget(go);
                    }
                    //podemos atacar si existe
                    break;
                }
        }
    }
    protected void updateGoing()
    {
        switch(m_currentSubState)
        {
            case UNIT_SUB_STATES.UNIT_SUB_STATE_NORMAL:
            {
                if (Vector3.Distance(m_navMeshAgent.destination, m_transform.position) < 3)
                {
                    changeState(UNIT_STATES.UNIT_STATE_IDLE);
                }
                else
                {
                    m_map.moveObjectToMap((int)m_mapPos.x, (int)m_mapPos.y, m_transform.position, gameObject);
                }break;
            }
            case UNIT_SUB_STATES.UNIT_SUB_STATE_AGGRESSIVE:
            {
                GameObject go = m_map.anyEnemyInRadious(transform.position, m_enemyDetectionRadius, m_team);
                if(go!=null)
                {
                    goToTarget(go);
                }
                break;
            }
            case UNIT_SUB_STATES.UNIT_SUB_STATE_FOLLOW_TARGET:
            {
                Vector3 targetPosition = m_target.transform.position;
                //le podemos atacar
                if ((targetPosition-m_transform.position).sqrMagnitude < m_attack.getAttackRange2())
                {
                    changeState(UNIT_STATES.UNIT_STATE_ATTACKING);
                }
                else
                {
                    //comprobamos si esta vivo y lo seguimos
                    if (m_target.activeInHierarchy)
                    {
                        m_navMeshAgent.SetDestination(targetPosition);
                    }
                    else
                    {
                        m_navMeshAgent.Stop();
                        changeState(UNIT_STATES.UNIT_STATE_IDLE);
                    }
                    
                }
                break;
            }
            case UNIT_SUB_STATES.UNIT_SUB_STATE_REPAIRING:
            {
                if (m_navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    readyToRepairBuilding.building = buildingToRepair;
                    readyToRepairBuilding.SendEvent();
                }
                break;
            }
        }
    }
    protected void updateAttacking() 
    {
        //ataca al target!!!
        if ( m_attack.canAttack())
        {
            m_attack.attackTarget(m_target);
            if ( m_animator != null)
            {
                m_animator.SetTrigger("attack");
            }
        }
        changeState(UNIT_STATES.UNIT_STATE_IDLE);
    }
    protected void updateDying() {
        m_currentTime -= Time.deltaTime * TimeManager.currentTimeFactor;
        if (m_currentTime < 0.0f)
        {
            changeState(UNIT_STATES.UNIT_STATE_IDLE);
            m_initialized = false;
            gameObject.SetActive(false);
        }
    }
    protected void updatePatrolling() 
    {   
        //GITANADA PARA EVITAR LA COORD Y|MODIFICAR PARA HACER UNA COMPROBACIÓN MÁS PROFESIONAL
        if (m_currentSubState == UNIT_SUB_STATES.UNIT_SUB_STATE_RECOLLECTING && m_resourceRecolected.amount > 0)
        {
            //sumamos recursos y volvemos
            m_navMeshAgent.SetDestination(m_positionFinal);
            eventAddResource.m_amount = m_resourceRecolected.amount;
            eventAddResource.m_type = m_resourceRecolected.type;
            eventAddResource.SendEvent();
            Color color = Color.green;
            if (eventAddResource.m_type == ResourcesManager.RESOURCES_TYPES.RESOURCE_TYPE_ONE)
            {
                color = Color.yellow;
            }
            FeedbackMessagesManager.instance.showWorldMessage(m_transform.position + Vector3.up, "" + m_resourceRecolected.amount, color);
            m_resourceRecolected.amount = 0;
        }
        else if ((m_positionFinal - gameObject.transform.position).sqrMagnitude < m_radius2)
        {
            if (m_currentSubState == UNIT_SUB_STATES.UNIT_SUB_STATE_RECOLLECTING)
            {
                //esperamos a que nos den los recursos y volvemos
                if (m_resourceRecolected.amount < 1 && !m_resourceRecolected.recolecting && m_resourceToRecolect.hasResources() && m_resourceToRecolect.canGetResources())
                {
                    m_resourceToRecolect.addUnit();
                    m_resourceRecolected.recolecting = true;
                    m_currentTime = m_timeToRecolect;
                    m_resourceRecolected.type = m_resourceToRecolect.m_type;
                }
                else if (m_resourceRecolected.recolecting)
                {
                    m_currentTime -= Time.deltaTime * TimeManager.currentTimeFactor;
                    if (m_currentTime < 0.0f)
                    {
                        m_resourceRecolected.amount = m_resourceToRecolect.getResources(m_AmountToRecolect);
                        m_resourceRecolected.recolecting = false;
                        m_resourceToRecolect.remUnit();
                        m_positionInitial = m_positionFinal;
                        m_navMeshAgent.SetDestination(m_positionInitial);
                    }
                }
                else if (!m_resourceToRecolect.hasResources())
                {
                    goTo(m_positionInitial);
                }
            }
            else
            {
                m_navMeshAgent.SetDestination(m_positionInitial);
            }
        }
        else if ((m_positionInitial-gameObject.transform.position).sqrMagnitude < m_radius2)
        //if (gameObject.transform.position.x == m_positionInitial.x && gameObject.transform.position.z == m_positionInitial.z)
        {
            m_navMeshAgent.SetDestination(m_positionFinal);
            //m_navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
        }
        
    }

    protected void changeState(UNIT_STATES nextState)
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        m_lastState = m_currentState;
        m_currentState = nextState;
        switch (m_currentState)
        {
            case UNIT_STATES.UNIT_STATE_IDLE:
                m_navMeshAgent.SetDestination(m_transform.position);
                changeSubState(UNIT_SUB_STATES.UNIT_SUB_STATE_AGGRESSIVE);
                if (m_animator != null)
                {
                    m_animator.SetBool("walking", false);
                }
                break;
            case UNIT_STATES.UNIT_STATE_GOING_TO:
                if (m_animator != null)
                {
                    m_animator.SetBool("walking", true);
                }
                break;
            case UNIT_STATES.UNIT_STATE_ATTACKING:
                if (m_animator != null)
                {
                    m_animator.SetBool("walking", false);
                }
                break;
            case UNIT_STATES.UNIT_STATE_DYING:
                m_map.remObjectToMap((int)m_mapPos.x, (int)m_mapPos.y, gameObject);
                if (m_animator != null)
                {
                    m_animator.SetBool("dead", true);
                }
                m_life.SetActive(false);
                m_currentTime = m_dyingTime;
                break;
            case UNIT_STATES.UNIT_STATE_PATROLLING:
                changeSubState(UNIT_SUB_STATES.UNIT_SUB_STATE_AGGRESSIVE);
                if (m_animator != null)
                {
                    m_animator.SetBool("walking", true);
                }
                break;
        }
    }
    protected void changeSubState(UNIT_SUB_STATES nextSubState)
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        m_lastSubState = m_currentSubState;
        m_currentSubState = nextSubState;
        if(m_lastSubState == UNIT_SUB_STATES.UNIT_SUB_STATE_REPAIRING)
        {
            stopRepairingBuilding.building = buildingToRepair;
            stopRepairingBuilding.SendEvent();
        }
        switch (m_currentSubState)
        {
            case UNIT_SUB_STATES.UNIT_SUB_STATE_NORMAL:
                break;
            case UNIT_SUB_STATES.UNIT_SUB_STATE_AGGRESSIVE:
                break;
            case UNIT_SUB_STATES.UNIT_SUB_STATE_FOLLOW_TARGET:
                break;
            case UNIT_SUB_STATES.UNIT_SUB_STATE_RECOLLECTING:
                break;
            case UNIT_SUB_STATES.UNIT_SUB_STATE_REPAIRING:
                break;
        }
    }
    public void goTo(Vector3 position)
    {
        m_navMeshAgent.SetDestination(position);
        changeState(UNIT_STATES.UNIT_STATE_GOING_TO);
        changeSubState(UNIT_SUB_STATES.UNIT_SUB_STATE_NORMAL);
    }
    public void goToTarget(GameObject target)
    {
        //@todo: guardarse el target y perseguirlo
        m_navMeshAgent.SetDestination(target.transform.position);
        changeState(UNIT_STATES.UNIT_STATE_GOING_TO);
        changeSubState(UNIT_SUB_STATES.UNIT_SUB_STATE_FOLLOW_TARGET);
        m_target = target;
    }
    public void goToAttack(Vector3 position)
    {
        m_navMeshAgent.SetDestination(position);
        changeState(UNIT_STATES.UNIT_STATE_GOING_TO);
        changeSubState(UNIT_SUB_STATES.UNIT_SUB_STATE_AGGRESSIVE);
    }
    public void goToPatrol(Vector3 position)
    {
        m_positionInitial = gameObject.transform.position;
        m_positionFinal = position;
        changeState(UNIT_STATES.UNIT_STATE_PATROLLING);
    }
    public void goToRecollect(Vector3 position, Resource resource)
    {
        m_positionInitial = gameObject.transform.position;
        m_positionFinal = position;
        changeState(UNIT_STATES.UNIT_STATE_PATROLLING);
        changeSubState(UNIT_SUB_STATES.UNIT_SUB_STATE_RECOLLECTING);
        m_resourceToRecolect = resource;
    }
    public void goToRepair(Buildng building)
    {
        m_navMeshAgent.SetDestination(building.transform.position);
        buildingToRepair = building;
        changeState(UNIT_STATES.UNIT_STATE_GOING_TO);
        changeSubState(UNIT_SUB_STATES.UNIT_SUB_STATE_REPAIRING);
    }
    public void stop()
    {
        changeState(UNIT_STATES.UNIT_STATE_IDLE);
    }
    public void onDamage(float currentLif)
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
    }
    public void onDead()
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        changeState(UNIT_STATES.UNIT_STATE_DYING);
    }
    public void onPause()
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        m_navMeshAgent.Stop();
    }
    public void onResume()
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        m_navMeshAgent.Resume();
    }

    public void onTimeChanged()
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        //acelerar/desacelerar animaci�n y velocidad de mov
    }

    public UNIT_TYPES getType()
    {
        return m_type;
    }

    public void setPosition(Vector3 position)
    {
        m_transform.position = position;
        m_map.moveObjectToMap((int)m_mapPos.x, (int)m_mapPos.y, position, gameObject);
    }
    public Vector3 getPosition()
    {
        return m_transform.position;
    }

    public int getTeam()
    {
        return m_team;
    }
    public bool canBeSelected(Vector3 position)
    {
        return true;
    }
    public void selecUnit()
    {
        m_selectable.SetSelected();
    }
    public void unselecUnit()
    {
        m_selectable.SetDeselect();
    }
    public bool isPressed(Vector3 position)
    {
        if ((position - m_transform.position).sqrMagnitude < m_radius2)
            return true;
        return false;
    }
    public void setMapXZ(int x, int z)
    {
        m_mapPos.x = x;
        m_mapPos.y = z;
    }
    public float getDetectionRadius()
    {
        return m_enemyDetectionRadius;
    }
    public float getDetectionRadius2()
    {
        return m_enemyDetectionRadius2;
    }
    public int getDamage()
    {
        return m_attack.getDamage();
    }
    public int getLife()
    {
        return (int)m_life.getLife();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_radius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_enemyDetectionRadius);

    }
}
