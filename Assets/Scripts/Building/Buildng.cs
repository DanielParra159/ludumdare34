using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Team))]
[RequireComponent(typeof(Life))]
[RequireComponent(typeof(Selectable))]
[RequireComponent(typeof(BoxCollider))]
//[RequireComponent(typeof(NavMeshObstacle))] @todo creo que es este, puede que no todas las construcciones lo necesiten activado
public class Buildng : MonoBehaviour {

    public enum BUILDING_TYPES
    {
        BUILDING_TYPE_HOUSE, BUILDING_URBAN_CENTER, MAX_BUILDING_TYPES
    }
    public static int maxBuildingTypes = (int)BUILDING_TYPES.MAX_BUILDING_TYPES;
    public enum BUILDING_STATES
    {
        BUILDING_STATE_IN_CONSTRUCTION, BUILDING_STATE_BUILT, BUILDING_STATE_DESTROYED, MAX_BUILDING_STATES
    }
    public static int maxBuildingStates = (int)BUILDING_STATES.MAX_BUILDING_STATES;

    [SerializeField]
    protected BUILDING_TYPES type;
    protected BUILDING_STATES m_currentState;
    protected BUILDING_STATES m_lastState;

    [Tooltip("Punto de reuni�n de las unidades")]
    public Transform m_meetingPoint;

    protected Pausable m_pausable;
    private bool m_initialized = false; //@todo cuando muera se tiene que poner a false

    void Awake()
    {
        m_pausable = new Pausable(onPause, onResume);
    }

	// Use this for initialization
	void Start () {
        Life lifeTemp = gameObject.GetComponent<Life>();
        lifeTemp.registerOnDead(onDead);
        lifeTemp.registerOnDamage(onDamage);
	}
	
    public void init()
    {
        m_initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }
    private void updateInConstruction() { }
    private void updateBuilt() { }
    private void updateDestroyed() { }


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
                break;
            case BUILDING_STATES.BUILDING_STATE_DESTROYED:
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
}
