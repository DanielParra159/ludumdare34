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
    public Renderer[][] m_Quads;
    public bool[][] m_visited;
    public bool[][] m_visiting;
    public int m_xFogCell; //casillas en X
    public int m_zFogCell; // casillas en Z

    private bool execution = false;
    public bool draw = true;

    public GameObject fogQuad;
    public float fogQuadScale;

    void Awake()
    {
        //if (instance == null)
        {
            instance = this;
        //    DontDestroyOnLoad(instance);

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


            GameObject quadsMap = new GameObject("FogMap");
            quadsMap = (GameObject)Instantiate(quadsMap);

            float maxX = m_xCell * m_xSize;
            float maxZ = m_zCell * m_zSize;
            m_xFogCell = (int)(maxX / fogQuadScale);
            m_zFogCell = (int)(maxZ / fogQuadScale);
            Vector3 position = transform.position;
            position.y = 5;
            position.x += fogQuadScale * 0.5f;

            m_visited = new bool[m_xFogCell][];
            m_visiting = new bool[m_xFogCell][];
            m_Quads = new Renderer[m_xFogCell][];
            for (int i = 0; i < m_xFogCell; ++i)
            {
                m_visited[i] = new bool[m_zFogCell];
                m_visiting[i] = new bool[m_zFogCell];
                m_Quads[i] = new Renderer[m_zFogCell];
                position.z = transform.position.z + fogQuadScale * 0.5f;
                for (int j = 0; j < m_zFogCell; ++j)
                {
                    m_visited[i][j] = false;
                    m_visiting[i][j] = false;
                    m_Quads[i][j] = ((GameObject)Instantiate(fogQuad, position, fogQuad.transform.rotation)).GetComponent < Renderer>();
                    m_Quads[i][j].gameObject.transform.localScale = new Vector3(fogQuadScale, fogQuadScale, 1); 
                    m_Quads[i][j].material = new Material(m_Quads[i][j].material);
                    m_Quads[i][j].transform.parent = quadsMap.transform;
                    position.z += fogQuadScale;
                }
                position.x += fogQuadScale;
            }
        }
        /*else if (instance != this)
        {
            Destroy(this.gameObject);
        }*/
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

    public GameObject anyEnemyInRadious(Vector3 position, float radious, int team)
    {
        GameObject goAux = null;
        int nearestX = int.MaxValue;
        int nearestZ = int.MaxValue;
        int xMin = (int)((position.x - radious) / m_xSize);
        int zMin = (int)((position.z - radious) / m_zSize);

        int xMax = (int)((position.x + radious) / m_xSize);
        int zMax = (int)((position.z + radious) / m_zSize);

        for (int x = xMin; x <= xMax; ++x)
        {
            for (int z = zMin; z <= zMax; ++z)
            {
                if (x < m_xCell && z < m_zCell)
                {
                    for (int i = 0; i < m_ObjectsMap[x][z].Count; ++i)
                    {
                        //mejorable, mucho...
                        Unit unitAux = m_ObjectsMap[x][z][i].GetComponent<Unit>();
                        if ( unitAux !=null)
                        {
                            if ((unitAux.getTeam() != team) /*&& (nearestX > x || nearestZ > z )*/)
                            {
                                goAux = m_ObjectsMap[x][z][i];
                                nearestX = x;
                                nearestZ = z;
                            }
                        }
                        else
                        {
                            Buildng buildingAux = m_ObjectsMap[x][z][i].GetComponent<Buildng>();
                            if (buildingAux != null)
                            {
                                if (buildingAux.getTeam() != team && (nearestX > x || nearestZ > z))
                                {
                                    goAux = m_ObjectsMap[x][z][i];
                                    nearestX = x;
                                    nearestZ = z;
                                }
                            }
                        }
                        
                    }
                }
            }
        }
        return goAux;

    }
    
}
