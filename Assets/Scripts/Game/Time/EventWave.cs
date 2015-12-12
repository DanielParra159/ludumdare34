using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class EventWave : IEvent {

    [System.Serializable]
    public class Units
    {
        public Unit.UNIT_TYPES[] m_types;
        public int [] m_num;
        public TeamManager.TEAMS m_team;
        public Transform m_meetingPoint;
    }
    public Units unitsToSpawn;
    protected static EventSpawnUnit eventSpawnUnit;

	// Use this for initialization
	void Awake () {
        Assert.AreEqual(unitsToSpawn.m_types.Length, unitsToSpawn.m_num.Length);

        eventSpawnUnit = new EventSpawnUnit();
        eventSpawnUnit.m_team = unitsToSpawn.m_team;
        eventSpawnUnit.m_position = transform.position;
        eventSpawnUnit.m_meetingPoint = unitsToSpawn.m_meetingPoint.position;
	}

    public override void play()
    {
        for (int i = 0; i < unitsToSpawn.m_types.Length; ++i)
        {
            for (int j=0; j < unitsToSpawn.m_num[i]; ++j)
            {
                eventSpawnUnit.m_type = unitsToSpawn.m_types[i];
                eventSpawnUnit.SendEvent();
            }
        }
    }
}
