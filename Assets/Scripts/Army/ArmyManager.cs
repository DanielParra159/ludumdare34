using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class ArmyManager : MonoBehaviour {

    public static ArmyManager instance = null;
    [Tooltip("Número máximo de unidades por partida.")]
    [Range(1, 300)]
    public int maxUnits = 100;
    [Tooltip("Número máximo de unidades iniciales.")]
    [Range(1, 300)]
    public int[] maxUnitsInitially = {100,100};
    private int[] currentMaxUnits; //numero de unidads maximas actualmente, por equipo

    [System.Serializable]
    public class Units
    {
        [Tooltip("Prefab de todas las unidades, el orden debe de coincidir con el tipo.\nSe utilizará para la pool.")]
        public GameObject[] unitsPrefab;
        [Tooltip("Previsión de unidades que almacenará la pool.")]
        [Range(1, 200)]
        public int[] unitsNumber;
    }

    [Tooltip("Unidades por equipo, tiene que ser tan grande como número de equipos sin contar a los neutrales")]
    public Units [] unitsPrefabs; //unidades por equipo
    [Tooltip("Unidades neutrales")]
    public Units neutralUnitsPrefabs; //unidades por equipo

    private PoolManager[][] poolOfUnits; //por equipo y por tipo de unidad
    private List<Unit> []units; //unidades de todos los equipos

    private PoolManager[] poolOfNeutralUnits; //solo para los neutrales
    private List<Unit> neutralUnits; //solo para los neutrales

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
	}

    void Start()
    {
        Assert.AreEqual(TeamManager.maxTeams, maxUnitsInitially.Length, "maxUnitsInitially distinto a TeamManager.maxTeams");

        //inicialización de estructura para todos los equipos
        units = new List<Unit>[TeamManager.maxTeams];
        currentMaxUnits = new int[TeamManager.maxTeams];
        poolOfUnits = new PoolManager[TeamManager.maxTeams][];
        for (int i = 0; i < TeamManager.maxTeams; ++i)
        {
            units[i] = new List<Unit>(100);
            currentMaxUnits[i] = maxUnitsInitially[i];

            poolOfUnits[i] = new PoolManager[Unit.maxUnitsTypes];
            for (int j = 0; j < Unit.maxUnitsTypes; ++j)
            {
                Assert.AreEqual(Unit.maxUnitsTypes, unitsPrefabs[i].unitsNumber.Length, "unitsNumber distinto a Unit.maxUnitsTypes");
                Assert.AreEqual(Unit.maxUnitsTypes, unitsPrefabs[i].unitsPrefab.Length, "unitsPrefab distinto a Unit.maxUnitsTypes");

                poolOfUnits[i][j] = new PoolManager(unitsPrefabs[i].unitsPrefab[j], unitsPrefabs[i].unitsNumber[j]);
                poolOfUnits[i][j].Init();
            }
        }
        //Inicializacion de pool neutral
        poolOfNeutralUnits = new PoolManager[Unit.maxNeutralUnitsTypes];
        neutralUnits = new List<Unit>(10);
        for (int j = 0; j < Unit.maxNeutralUnitsTypes; ++j)
            {
                Assert.AreEqual(Unit.maxNeutralUnitsTypes, neutralUnitsPrefabs.unitsNumber.Length, "unitsNumber distinto a Unit.maxNeutralUnitsTypes");
                Assert.AreEqual(Unit.maxNeutralUnitsTypes, neutralUnitsPrefabs.unitsPrefab.Length, "unitsPrefab distinto a Unit.maxNeutralUnitsTypes");

                poolOfNeutralUnits[j] = new PoolManager(neutralUnitsPrefabs.unitsPrefab[j], neutralUnitsPrefabs.unitsNumber[j]);
                poolOfNeutralUnits[j].Init();
            }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
