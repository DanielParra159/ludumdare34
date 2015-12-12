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
        UNIT_TYPE_WORKER, MAX_UNIT_TYPES
    }
    public enum NEUTRAL_UNIT_TYPES
    {
        NEUTRAL_UNIT_TYPE_SOLDIER, MAX_NEUTRAL_UNIT_TYPES
    }
    public enum UNIT_STATES
    {
        UNIT_STATE_IDLE, UNIT_STATE_GOING_TO, UNIT_STATE_ATTACKING, UNIT_STATE_DYING, UNIT_STATE_BUILDING, MAX_UNIT_STATES
    }
    public static int maxNeutralUnitsTypes = (int)NEUTRAL_UNIT_TYPES.MAX_NEUTRAL_UNIT_TYPES;
    public static int maxUnitsTypes = (int)UNIT_TYPES.MAX_UNIT_TYPES;
    public static int maxUnitsStates = (int)UNIT_STATES.MAX_UNIT_STATES;

    [SerializeField]
    protected UNIT_TYPES m_type;
    protected UNIT_STATES m_currentState;
    protected UNIT_STATES m_lastState;
    //protected UNITS_STATES nextState;

    

    protected int m_team;
    protected NavMeshAgent m_navMeshAgent;
    protected float m_speed;

    protected Transform m_transform;
    protected Pausable m_pausable;
    protected bool m_initialized = false; //@todo cuando muera se tiene que poner a false

    void Awake()
    {
        m_transform = transform;
        m_pausable = new Pausable(onPause, onResume);

        m_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        m_speed = m_navMeshAgent.speed;
    }

	void Start () {
        TimeManager.registerChangedTime(onTimeChanged);

        m_team = gameObject.GetComponent<Team>().m_team;
        Life lifeTemp = gameObject.GetComponent<Life>();
        lifeTemp.registerOnDead(onDead);
        lifeTemp.registerOnDamage(onDamage);

        
	}
	
    public void init()
    {
        m_initialized = true;
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
        }
	}

    protected void updateIdle() { }
    protected void updateGoing()
    {
        if ( m_navMeshAgent.destination == null )
        {
            changeState(UNIT_STATES.UNIT_STATE_IDLE);
        }
    }
    protected void updateAttacking() { }
    protected void updateDying() { }

    protected void changeState(UNIT_STATES nextState)
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Unidad " + this);
        m_lastState = m_currentState;
        m_currentState = nextState;
        switch (m_currentState)
        {
            case UNIT_STATES.UNIT_STATE_IDLE:
                break;
            case UNIT_STATES.UNIT_STATE_GOING_TO:
                break;
            case UNIT_STATES.UNIT_STATE_ATTACKING:
                break;
            case UNIT_STATES.UNIT_STATE_DYING:
                break;
        }
    }
    public void goTo(Vector3 position)
    {
        m_navMeshAgent.destination = position;
        changeState(UNIT_STATES.UNIT_STATE_GOING_TO);
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
    }
}
