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

    public Buildng isPressedAnyBuilding(Vector3 position)
    {
        return null;
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
