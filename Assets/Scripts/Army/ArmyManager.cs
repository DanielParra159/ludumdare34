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

    private List<Unit> m_selectedUnits;
    private Unit m_selectedUnit;
    private int m_team = 0; //@todo: simplificamos y decimos que el player es el 0

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
        //if (instance == null)
        {
            instance = this;
         //   DontDestroyOnLoad(instance);
        }
        /*else if (instance != this)
        {
            Destroy(this.gameObject);
        }*/
	}

    void Start()
    {
        Assert.AreEqual(TeamManager.maxTeams, maxUnitsInitially.Length, "maxUnitsInitially distinto a TeamManager.maxTeams");

        //inicialización de estructura para todos los equipos
        units = new List<Unit>[TeamManager.maxTeams];
        m_selectedUnits = new List<Unit>(100);
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
	
    public void spawnUnit(TeamManager.TEAMS team, Unit.UNIT_TYPES type, Vector3 position, Vector3 meetingPoint)
    {
        int teamAux = (int)team;
        Unit unitAux = poolOfUnits[teamAux][(int)type].getObject(true).GetComponent<Unit>();
        unitAux.init();
        unitAux.setPosition(position);
        unitAux.goTo(meetingPoint);
        units[teamAux].Add(unitAux);
    }
    public Unit isPressedAnyEnemyUnit(Vector3 position)
    {
        Unit pressedUnitAux = null;
        int enemyTeam = m_team + 1 % (int)TeamManager.TEAMS.TEAM_MAX;
        for (int i = 0; i < units[enemyTeam].Count; ++i)
        {
            if (units[enemyTeam][i].isPressed(position))
                pressedUnitAux = units[enemyTeam][i];
        }
        return pressedUnitAux;
    }
    public Unit selectUnits(Vector3 startPosition, Vector3 endPosition)
    {
        //esto es muy mejorable
        Vector3 center = (endPosition - startPosition);
        center.y += 0.1f;
        Vector3 centerAux = startPosition + center * 0.5f;
        Bounds selectBounds = new Bounds(centerAux, new Vector3(Mathf.Abs(center.x),20.0f,Mathf.Abs(center.z)));
        if (selectBounds.size.x < 1.5f || selectBounds.size.z < 1.5f)
        {
            selectBounds.size = selectBounds.size + new Vector3(1.5f, 0.0f, 1.5f);
        }
        int maxType = (int)Unit.UNIT_TYPES.MAX_UNIT_TYPES;
        m_selectedUnits.Clear();
        m_selectedUnit = null;
        for (int i = 0; i < units[m_team].Count; ++i )
        {
            if (selectBounds.Contains(units[m_team][i].getPosition()))
            {
                units[m_team][i].selecUnit();
                m_selectedUnits.Add(units[m_team][i]);
                if (units[m_team][i].getTeam() < maxType)
                {
                    m_selectedUnit = units[m_team][i];
                }
                
            }
            else
            {
                units[m_team][i].unselecUnit();
            }
        }
        //if (m_selectedUnits.Count > 0 && m_selectedUnit!=null)
        //m_selectedUnits.Add(m_selectedUnit);
        return m_selectedUnit;
    }
    public Unit isSelectedAnyUnit()
    {
        return m_selectedUnit;
    }
    public void goTo(Vector3 position)
    {
        for (int i = 0; i < m_selectedUnits.Count; ++i)
        {
            m_selectedUnits[i].goTo(position);
        }
    }
    public void goToAttack(Vector3 position)
    {
        for (int i = 0; i < m_selectedUnits.Count; ++i)
        {
            m_selectedUnits[i].goToAttack(position);
        }
    }
    public void stopUnits()
    {
        for (int i = 0; i < m_selectedUnits.Count; ++i)
        {
            m_selectedUnits[i].stop();
        }
    }
    public void goToPatrol(Vector3 position)
    {
        for (int i = 0; i < m_selectedUnits.Count; ++i)
        {
            m_selectedUnits[i].goToPatrol(position);
        }
    }
    public void goToRecollect(Vector3 position, Resource resource)
    {
        for (int i = 0; i < m_selectedUnits.Count; ++i)
        {
            m_selectedUnits[i].goToRecollect(position, resource);
        }
    }
    public void goToRepair(Buildng building)
    {
        for (int i = 0; i < m_selectedUnits.Count; ++i)
        {
            m_selectedUnits[i].goToRepair(building);
        }
    }

    void Update()
    {
        Map map = Map.instance;
        for (int i = 0; i < units[0].Count;++i )
        {
            float radius = units[0][i].getDetectionRadius();
            float radius2 = units[0][i].getDetectionRadius2();
            Vector3 position = units[0][i].getPosition();
            int xMin = (int)((position.x - radius) / map.fogQuadScale);
            int zMin = (int)((position.z - radius + 4) / map.fogQuadScale);

            int xMax = (int)((position.x + radius) / map.fogQuadScale);
            int zMax = (int)((position.z + radius + 4) / map.fogQuadScale);
            Vector3 worldPosition = Vector3.zero;
            worldPosition.y = position.y;
            for (int x = xMin; x < xMax; ++x)
            {
                for (int z = zMin; z < zMax; ++z)
                {
                    worldPosition.x = x * map.fogQuadScale;
                    worldPosition.z = z * map.fogQuadScale;
                    if ((worldPosition-position).sqrMagnitude < radius2 && x < map.m_xFogCell && z < map.m_zFogCell)
                    {
                        map.m_visited[x][z] = true;
                        map.m_visiting[x][z] = true;
                    }
                }
            }
        }

        for (uint x = 0; x < map.m_xFogCell; ++x)
        {
            for (uint z = 0; z < map.m_zFogCell; ++z)
            {
                if (map.m_visited[x][z] && map.m_visiting[x][z])
                {
                    map.m_Quads[x][z].material.color = new Color(0, 0, 0, 0);
                }
                else if (map.m_visited[x][z])
                {
                    map.m_Quads[x][z].material.color = new Color(0, 0, 0, 0.3f);
                }
                else
                {
                    map.m_Quads[x][z].material.color = new Color(0, 0, 0, 1.0f);
                }
                map.m_visiting[x][z] = false;
            }
        }
    }
}
