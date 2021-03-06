using UnityEngine;
using System.Collections;

public class UnitSpawn : MonoBehaviour {

    public enum BARRACK_UNITS
    {
        SWORDMAN, ARCHER, LANCER
    }
    [Tooltip("Que unidad")]
    public Unit m_unitToSpawn;
    [Tooltip("Tiempo que tarda en nacer")]
    public float m_remainingTimeToSpawn = 1.0f;
    [Tooltip("Hacia donde se ve a dirigir una vez nazca")]
    public Transform m_meetingPoint;

    private EventSpawnUnit m_eventSpawnUnit;
    [Tooltip("Si false, las criaturas se crean cada x tiempo|true para las construcciones")]
    public bool m_spawnType = false;

    public Unit[] m_unitsToSpawnBarracks;

    private ResourcesManager m_resourceManager;

    protected Pausable m_pausable;

    void Start()
    {
        m_resourceManager = ResourcesManager.instance;
        m_pausable = new Pausable();
    }
	// Use this for initialization
	void Awake () {
        m_eventSpawnUnit = new EventSpawnUnit();
        if (gameObject.tag == "Building")
        {
            m_spawnType = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (m_pausable.Check()) return;
        if (!m_spawnType)
        {
            m_remainingTimeToSpawn -= Time.deltaTime;
            if (m_remainingTimeToSpawn < 0.0f)
            {
                m_eventSpawnUnit.m_position = transform.position;
                m_eventSpawnUnit.m_meetingPoint = m_meetingPoint.position;
                m_eventSpawnUnit.m_team = m_unitToSpawn.GetComponent<Team>().m_myTeam;
                m_eventSpawnUnit.m_type = m_unitToSpawn.getType();
                m_eventSpawnUnit.SendEvent();
                this.enabled = false;
            }
        }
	}

    public void buildUnit()
    {
        if(m_resourceManager.haveEnoughResources(Unit.UNIT_TYPES.UNIT_TYPE_WORKER)){
            m_eventSpawnUnit.m_position = transform.position;
            m_eventSpawnUnit.m_meetingPoint = m_meetingPoint.position;
            m_eventSpawnUnit.m_team = m_unitToSpawn.GetComponent<Team>().m_myTeam;
            m_eventSpawnUnit.m_type = m_unitToSpawn.getType();
            m_eventSpawnUnit.SendEvent();
            this.enabled = false;
        }
    }

    public void barrackUnits(int unit)
    {
        if(m_resourceManager.haveEnoughResources((Unit.UNIT_TYPES) unit)){
            m_eventSpawnUnit.m_position = transform.position;
            m_eventSpawnUnit.m_meetingPoint = m_meetingPoint.position;
            m_eventSpawnUnit.m_team = m_unitsToSpawnBarracks[unit].GetComponent<Team>().m_myTeam;
            m_eventSpawnUnit.m_type = m_unitsToSpawnBarracks[unit].getType();
            m_eventSpawnUnit.SendEvent();
            this.enabled = false;
        }
    }
}
