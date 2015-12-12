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
        /*BORRAR*/UNIT_TYPE_POTATO,/*BORRAR*/ UNIT_TYPE_WORKER, UNIT_TYPE_ARMY, MAX_UNIT_TYPES
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
        UNIT_SUB_STATE_NORMAL, UNIT_SUB_STATE_FOLLOW_TARGET, UNIT_SUB_STATE_AGGRESSIVE, UNIT_SUB_STATE_RECOLLECTING, MAX_UNIT_SUB_STATES
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

    void Awake()
    {
        m_transform = transform;
        m_pausable = new Pausable(onPause, onResume);

        m_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        m_speed = m_navMeshAgent.speed;
        m_radius2 = m_radius * m_radius;
        m_enemyDetectionRadius2 = m_enemyDetectionRadius * m_enemyDetectionRadius;
    }

	void Start () {
        TimeManager.registerChangedTime(onTimeChanged);

        m_team = gameObject.GetComponent<Team>().m_team;
        Life lifeTemp = gameObject.GetComponent<Life>();
        lifeTemp.registerOnDead(onDead);
        lifeTemp.registerOnDamage(onDamage);

        m_selectable = GetComponent<Selectable>();
        m_map = Map.instance;
        m_attack = GetComponent<Attack>();

        
	}
	
    public void init()
    {
        m_initialized = true;
        m_map = Map.instance;
        m_map.addObjectToMap(m_transform.position, gameObject);
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
                    goToTarget(go);
                    //podemos atacar si existe
                    break;
                }
        }
    }
    protected void updateGoing()
    {
        if ( m_navMeshAgent.destination == null )
        {
            changeState(UNIT_STATES.UNIT_STATE_IDLE);
        }
        else
        {
            m_map.moveObjectToMap((int)m_mapPos.x, (int)m_mapPos.y, m_transform.position, gameObject);
        }
        switch(m_currentSubState)
        {
            case UNIT_SUB_STATES.UNIT_SUB_STATE_AGGRESSIVE:
            {
                GameObject go = m_map.anyEnemyInRadious(transform.position, m_enemyDetectionRadius, m_team);
                goToTarget(go);
                //podemos atacar si existe
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
            //TO DO PROCESO DE RECOLECCIÓN 
            /*case UNIT_SUB_STATES.UNIT_SUB_STATE_RECOLLECTING:
            {
                Vector3 targetPosition = m_recollectionPoint.transform.position;

                break;
            }*/
        }
    }
    protected void updateAttacking() 
    {
        //ataca al target!!!
        if ( m_attack.canAttack())
        {
            m_attack.attackTarget(m_target);
        }
        changeState(UNIT_STATES.UNIT_STATE_IDLE);
    }
    protected void updateDying() { }
    protected void updatePatrolling() 
    {   
        //GITANADA PARA EVITAR LA COORD Y
        if (gameObject.transform.position.x == m_positionInitial.x && gameObject.transform.position.z == m_positionInitial.z)
        {
            m_navMeshAgent.SetDestination(m_positionFinal);
        }
        else if (gameObject.transform.position.x == m_positionFinal.x && gameObject.transform.position.z == m_positionFinal.z)
        {
            m_navMeshAgent.SetDestination(m_positionInitial);
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
                break;
            case UNIT_STATES.UNIT_STATE_GOING_TO:
                break;
            case UNIT_STATES.UNIT_STATE_ATTACKING:
                break;
            case UNIT_STATES.UNIT_STATE_DYING:
                break;
            case UNIT_STATES.UNIT_STATE_PATROLLING:
                changeSubState(UNIT_SUB_STATES.UNIT_SUB_STATE_AGGRESSIVE);
                break;
        }
    }
    protected void changeSubState(UNIT_SUB_STATES nextSubState)
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        m_lastSubState = m_currentSubState;
        m_currentSubState = nextSubState;
        switch (m_currentSubState)
        {
            case UNIT_SUB_STATES.UNIT_SUB_STATE_NORMAL:
                break;
            case UNIT_SUB_STATES.UNIT_SUB_STATE_AGGRESSIVE:
                break;
            case UNIT_SUB_STATES.UNIT_SUB_STATE_FOLLOW_TARGET:
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
    }

    public void onPause()
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
    }
    public void onResume()
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_radius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_enemyDetectionRadius);

    }
}
