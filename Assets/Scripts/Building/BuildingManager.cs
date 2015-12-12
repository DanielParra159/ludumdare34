using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance = null;

    [Tooltip("Prefab de todos los edificios, el orden debe de coincidir con el tipo.\nSe utilizará para la pool.")]
    public GameObject[] buildingsPrefab;
    [Tooltip("Previsión de edificios que almacenará la pool, contando con los dos equipos.")]
    public int[] buildingsNumber;

    private Buildng m_selectedBuilding = null;


    private PoolManager[] poolOfBuildings; //por equipo y por tipo de edificio
    private List<Buildng>[] buildings;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            Assert.AreEqual(Buildng.maxBuildingTypes, buildingsNumber.Length, "buildingsNumber distinto a Buildng.maxBuildingTypes");
            Assert.AreEqual(Buildng.maxBuildingTypes, buildingsPrefab.Length, "buildingsPrefab distinto a Buildng.maxBuildingTypes");

            buildings = new List<Buildng>[TeamManager.maxTeams];
            for (int i = 0; i < TeamManager.maxTeams; ++i)
            {
                buildings[i] = new List<Buildng>(100);
            }

            poolOfBuildings = new PoolManager[Buildng.maxBuildingTypes];
            for (int i = 0; i < Buildng.maxBuildingTypes; ++i)
            {
                poolOfBuildings[i] = new PoolManager(buildingsPrefab[i], buildingsNumber[i]);
                poolOfBuildings[i].Init();
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
}
