using UnityEngine;
using System.Collections;

public class BuildingSpawn : MonoBehaviour {

    [Tooltip("Que edificio")]
    public Buildng m_unitToSpawn;

    private static EventSpawnBuilding m_eventSpawnBuilding;

    protected Pausable m_pausable;
	// Use this for initialization
	void Start () {
        m_pausable = new Pausable();

	}
	
	// Update is called once per frame
	void Update () {
        if (m_pausable.Check()) return;
        m_eventSpawnBuilding = new EventSpawnBuilding();
        m_eventSpawnBuilding.m_position = transform.position;
        m_eventSpawnBuilding.m_rotation = transform.rotation;
        m_eventSpawnBuilding.m_team = m_unitToSpawn.GetComponent<Team>().m_myTeam;
        m_eventSpawnBuilding.m_type = m_unitToSpawn.getType();
        m_eventSpawnBuilding.SendEvent();
        Destroy(gameObject);
        //gameObject.SetActive(false);
	}   
}
