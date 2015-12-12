using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance = null;

    [System.Serializable]
    public class Buildings
    {
        [Tooltip("Prefab de todos los edificios, el orden debe de coincidir con el tipo.\nSe utilizará para la pool.")]
        public GameObject[] buildingsPrefab;
        [Tooltip("Previsión de edificios que almacenará la pool, contando con los dos equipos.")]
        [Range(1, 200)]
        public int[] buildingsNumber;
    }

    [Tooltip("Construcciones por equipo, tiene que ser tan grande como número de equipos sin contar a los neutrales")]
    public Buildings [] buildingPrefabs;

    private Buildng m_selectedBuilding = null;

    private int m_team = 0; //@todo: simplificamos y decimos que el player es el 0

    private PoolManager[][] poolOfBuildings; //por equipo y por tipo de edificio
    private List<Buildng>[] buildings;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            buildings = new List<Buildng>[TeamManager.maxTeams];
            for (int i = 0; i < TeamManager.maxTeams; ++i)
            {
                buildings[i] = new List<Buildng>(100);
            }

            poolOfBuildings = new PoolManager[Buildng.maxBuildingTypes][];

            for (int i = 0; i < TeamManager.maxTeams; ++i)
            {
                poolOfBuildings[i] = new PoolManager[Buildng.maxBuildingTypes];
                for (int j = 0; j < Buildng.maxBuildingTypes; ++j)
                {
                    Assert.AreEqual(Buildng.maxBuildingTypes, buildingPrefabs[i].buildingsNumber.Length, "buildingsNumber distinto a Unit.maxUnitsTypes");
                    Assert.AreEqual(Buildng.maxBuildingTypes, buildingPrefabs[i].buildingsPrefab.Length, "buildingsPrefab distinto a Unit.maxUnitsTypes");

                    poolOfBuildings[i][j] = new PoolManager(buildingPrefabs[i].buildingsPrefab[j], buildingPrefabs[i].buildingsNumber[j]);
                    poolOfBuildings[i][j].Init();
                }
            }
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Buildng selectBuilding(Vector3 startPosition, Vector3 endPosition)
    {
        //esto es muy mejorable
        Vector3 center = (endPosition - startPosition);
        center.y += 0.1f;
        Vector3 centerAux = startPosition + center * 0.5f;
        Bounds selectBounds = new Bounds(centerAux, new Vector3(Mathf.Abs(center.x), 10.0f, Mathf.Abs(center.z)));
        if (selectBounds.size.x < 1.5f || selectBounds.size.z < 1.5f)
        {
            selectBounds.size = selectBounds.size + new Vector3(1.5f, 0.0f, 1.5f);
        }
        int maxType = (int)Unit.UNIT_TYPES.MAX_UNIT_TYPES;
        m_selectedBuilding = null;
        for (int i = 0; i < buildings[m_team].Count; ++i)
        {
            if (selectBounds.Contains(buildings[m_team][i].getPosition()))
            {
                buildings[m_team][i].selecBuilding();
                if (buildings[m_team][i].getTeam() < maxType)
                {
                    m_selectedBuilding = buildings[m_team][i];
                }

            }
            else
            {
                buildings[m_team][i].unselecBuilding();
            }
        }
        return m_selectedBuilding;
    }
    public Buildng isPressedAnyBuilding(Vector3 position)
    {
        Buildng pressedBuildingAux = null;
        int enemyTeam = m_team + 1 % (int)TeamManager.TEAMS.TEAM_MAX;
        for (int i = 0; i < buildings[enemyTeam].Count; ++i)
        {
            if (buildings[enemyTeam][i].isPressed(position))
                pressedBuildingAux = buildings[enemyTeam][i];
        }
        return pressedBuildingAux;
    }
    public Buildng isSelectedAnyBuilding()
    {
        return null;
    }
    public void setMeetingPoint(Vector3 position)
    {
        m_selectedBuilding.setMeetingPointPosition(position);
    }
    public void readyToRepair(Buildng building)
    {
        building.onHeal();
    }
    public void stopRepairing(Buildng building)
    {
        building.onStopHeal();
    }
    public void spawnBuilding(TeamManager.TEAMS team, Buildng.BUILDING_TYPES type, Vector3 position)
    {
        int teamAux = (int)team;
        Buildng buildingAux = poolOfBuildings[teamAux][(int)type].getObject(true).GetComponent<Buildng>();
        buildingAux.init();
        buildingAux.setPosition(position);
        buildings[teamAux].Add(buildingAux);
    }
}
