using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Team))]
[RequireComponent(typeof(Life))]
[RequireComponent(typeof(Selectable))]
[RequireComponent(typeof(NavMeshObstacle))]
public class Buildng : MonoBehaviour
{

    public enum BUILDING_TYPES
    {
        BUILDING_URBAN_CENTER, BUILDING_TYPE_HOUSE, BUILDING_TYPE_BARRACKS, BUILDING_TYPE_UPGRADE, BUILDING_TYPE_TOWER,
        BUILDING_TYPE_RESOURCE, BUILDING_TYPE_RESOURCE2, MAX_BUILDING_TYPES
    }
    public static int maxBuildingTypes = (int)BUILDING_TYPES.MAX_BUILDING_TYPES;
    public enum BUILDING_STATES
    {
        BUILDING_STATE_IN_CONSTRUCTION, BUILDING_STATE_BUILT, BUILDING_STATE_DESTROYED, BUILDING_STATE_BEING_REPAIRED,
        MAX_BUILDING_STATES
    }
    public static int maxBuildingStates = (int)BUILDING_STATES.MAX_BUILDING_STATES;

    [SerializeField]
    protected BUILDING_TYPES m_type;
    protected BUILDING_STATES m_currentState;
    protected BUILDING_STATES m_lastState;

    protected Selectable m_selectable;

    [Tooltip("Punto de reuni�n de las unidades")]
    public Transform m_meetingPoint;
    protected Transform m_transform;
    protected int m_team;
    protected Vector2 m_mapPos;

    [SerializeField]
    [Tooltip("Radio de la unidad, se utiliza para las pulsaciones")]
    protected float m_radius = 2.0f;
    protected float m_radius2;

    protected Pausable m_pausable;
    private bool m_initialized = false; //@todo cuando muera se tiene que poner a false
    protected Life m_life;


    void Awake()
    {
        m_pausable = new Pausable(onPause, onResume);
        m_transform = transform;

        m_radius2 = m_radius * m_radius;
    }

    // Use this for initialization
    void Start()
    {
        m_selectable = GetComponent<Selectable>();

        m_life = gameObject.GetComponent<Life>();
        m_life.registerOnDead(onDead);
        m_life.registerOnDamage(onDamage);

        m_team = gameObject.GetComponent<Team>().m_team;

        m_pausable = new Pausable();
    }

    public void init()
    {
        m_initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_pausable.Check()) return;
        Assert.IsTrue(m_initialized, "no se ha inicializado el edificio " + this);
        if (m_pausable.Check()) return;
        switch (m_currentState)
        {
            case BUILDING_STATES.BUILDING_STATE_IN_CONSTRUCTION:
                updateInConstruction();
                break;
            case BUILDING_STATES.BUILDING_STATE_BUILT:
                updateBuilt();
                break;
            case BUILDING_STATES.BUILDING_STATE_DESTROYED:
                updateDestroyed();
                break;
            case BUILDING_STATES.BUILDING_STATE_BEING_REPAIRED:
                updateRepaired();
                break;
        }
    }
    private void updateInConstruction() { }
    private void updateBuilt() { }
    private void updateDestroyed() { }
    private void updateRepaired()
    {
        m_life.setRegeneration(50.0f);
    }


    private void changeState(BUILDING_STATES nextState)
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado el edificio " + this);
        m_lastState = m_currentState;
        m_currentState = nextState;
        switch (m_currentState)
        {
            case BUILDING_STATES.BUILDING_STATE_IN_CONSTRUCTION:
                break;
            case BUILDING_STATES.BUILDING_STATE_BUILT:
                if(m_lastState == BUILDING_STATES.BUILDING_STATE_BEING_REPAIRED)
                {
                    m_life.setRegeneration(0);
                }
                break;
            case BUILDING_STATES.BUILDING_STATE_DESTROYED:
                break;
            case BUILDING_STATES.BUILDING_STATE_BEING_REPAIRED:
                break;
        }
    }

    public void onDamage(float currentLif)
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado el edificio " + this);
    }
    public void onDead()
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado el edificio " + this);
    }
    public void onHeal()
    {
        changeState(BUILDING_STATES.BUILDING_STATE_BEING_REPAIRED);
    }
    public void onStopHeal()
    {
        changeState(BUILDING_STATES.BUILDING_STATE_BUILT);
    }
    public void onPause()
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado el edificio " + this);
    }
    public void onResume()
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado el edificio " + this);
    }
    public void setMeetingPointPosition(Vector3 position)
    {
        Assert.IsTrue(m_initialized, "no se ha inicializado el edificio " + this);
        m_meetingPoint.position = position;
    }

    public void setRotation(Quaternion rotation)
    {
        m_transform.rotation = rotation;
    }
    public void setPosition(Vector3 position)
    {
        m_transform.position = position;
    }
    public Vector3 getPosition()
    {
        return m_transform.position;
    }
    public int getTeam()
    {
        return m_team;
    }
    public BUILDING_TYPES getType()
    {
        return m_type;
    }
    public void setMapXZ(int x, int z)
    {
        m_mapPos.x = x;
        m_mapPos.y = z;
    }
    public bool isPressed(Vector3 position)
    {
        if ((position - m_transform.position).sqrMagnitude < m_radius2)
            return true;
        return false;
    }
    public void selecBuilding()
    {
        m_selectable.SetSelected();
    }
    public void unselecBuilding()
    {
        m_selectable.SetDeselect();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_radius);
    }
}
