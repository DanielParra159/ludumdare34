using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public static Map instance = null;

    public int m_xCell = 4; //casillas en X
    public int m_zCell = 4; // casillas en Z

    public float m_xSize = 1.0f; //tamano de las celdas en X
    public float m_zSize = 1.0f; //tamano de las celdas en Z

    public List<GameObject>[][] m_ObjectsMap;

    private bool execution = false;
    public bool draw = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            execution = true;

            m_ObjectsMap = new List<GameObject>[m_xCell][];
            for (int x = 0; x < m_xCell; ++x)
            {
                m_ObjectsMap[x] = new List<GameObject>[m_zCell];
                for (int z = 0; z < m_zCell; ++z)
                {
                    m_ObjectsMap[x][z] = new List<GameObject>(2);
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
        init();
	}
	
    void init()
    {
        
    }

    void OnDrawGizmos()
    {
        if (!draw) return;
        // Display the explosion radius when selected
        if (execution)
        {
            for (uint x = 0; x < m_xCell; ++x)
            {
                for (uint z = 0; z < m_zCell; ++z)
                {
                    if (m_ObjectsMap[x][z].Count > 0)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                    }

                    Gizmos.DrawCube(new Vector3(x * m_xSize + m_xSize * 0.5f, -0.1f, z * m_zSize + m_zSize * 0.5f), new Vector3(m_xSize, 0.2f, m_zSize));
                }
            }
        }
        Gizmos.color = Color.white;
        for (uint z = 0; z <= m_zCell; ++z) {
            Gizmos.DrawLine(new Vector3(0, 0.1f, z * m_zSize), new Vector3(m_xCell * m_xSize, 0.1f, z * m_zSize));
        }
        for (uint x = 0; x <= m_xCell; ++x)
        {
            Gizmos.DrawLine(new Vector3(x * m_xSize, 0.11f, 0), new Vector3(x * m_xSize, 0.1f, m_zCell * m_zSize));
        }
        
    }

    public void addObjectToMap(Vector3 position, GameObject go)
    {
        int x = (int)(position.x / m_xSize);
        int z = (int)(position.z / m_zSize);

        if (x < m_xCell && z < m_zCell)
        {
            m_ObjectsMap[x][z].Add(go);
            Unit unitAux = go.GetComponent<Unit>();
            if (unitAux != null)
            {
                unitAux.setMapXZ(x, z);
            }
            else
            {
                Buildng buildAux = go.GetComponent<Buildng>();
                if (buildAux != null)
                {
                    buildAux.setMapXZ(x, z);
                }
            }
        }
    }

    public void remObjectToMap(int x, int z, GameObject go)
    {
        m_ObjectsMap[x][z].Remove(go);
    }

    public void moveObjectToMap(int x, int z, Vector3 position, GameObject go)
    {
        int newX = (int)(position.x / m_xSize);
        int newZ = (int)(position.z / m_zSize);
        if (newX == x && newZ == z) return;

        remObjectToMap(x, z, go);
        addObjectToMap(position, go);
    }

    public GameObject anyEnemyInRadious(Vector3 position, float radious)
    {
        int xMin = (int)(position.x - radious / m_xSize);
        int zMin = (int)(position.z - radious / m_zSize);

        int xMax = (int)(position.x + radious / m_xSize);
        int zMax = (int)(position.z + radious / m_zSize);

        for (int x = xMin; x <= xMax; ++x)
        {
            for (int z = zMin; z < zMax; ++z)
            {
                if (x < m_xCell && z < m_zCell)
                {
                }
            }
        }
        return null;

    }
    
}
