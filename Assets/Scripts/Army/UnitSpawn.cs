using UnityEngine;
using System.Collections;

public class UnitSpawn : MonoBehaviour {

    [Tooltip("Que unidad")]
    public Unit m_unitToSpawn;
    [Tooltip("Tiempo que tarda en nacer")]
    public float m_remainingTimeToSpawn = 1.0f;
    [Tooltip("Hacia donde se ve a dirigir una vez nazca")]
    public Transform m_meetingPoint;

    private EventSpawnUnit m_eventSpawnUnit;

	// Use this for initialization
	void Awake () {
        m_eventSpawnUnit = new EventSpawnUnit();
	}
	
	// Update is called once per frame
	void Update () {
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
