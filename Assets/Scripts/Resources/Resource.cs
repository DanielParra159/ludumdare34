using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Team))]
[RequireComponent(typeof(Selectable))]
//[RequireComponent(typeof(NavMeshObstacle))] @todo creo que es este, puede que no todas las construcciones lo necesiten activado
public class Resource : MonoBehaviour {

    [Tooltip("Numero de recursos que puede proporcionar")]
    public int m_maxNumOfResources;
    protected int m_currentNumOfResources; //recursos actuales que quedan

    [Tooltip("Tipo del recurso")]
    public ResourcesManager.RESOURCES_TYPES m_type;

    [Tooltip("Numero de unidades que pueden sacar recursos al mismo tiempo")]
    [Range(1, 20)]
    public int m_numUnitsGetResourcesAtTime;
    protected int m_currentUnitsGetingResources; //numero de unidades sacando recursos en este momento

	// Use this for initialization
	void Start () {
        m_currentNumOfResources = m_maxNumOfResources;
        m_currentUnitsGetingResources = 0;
	}


    /*
     * Pide los recursos que quiere obtener y devuelve los que realmente puede tomar
     */
    public int getResources(int resourcesToGet)
    {
        int resources = m_currentNumOfResources - resourcesToGet;
        if (resources < 0)
        {
            resources = m_currentNumOfResources;
            m_currentNumOfResources = 0;
        }
        else
        {
            resources = resourcesToGet;
        }
        return resources;
    }
    /*
     * La unidd pregunta si quedan recursos
     */
    public bool hasResources()
    {
        return m_currentNumOfResources > 0;
    }
    /*
     * La unidd pregunta si hay espacio libre
     */
    public bool canGetResources()
    {
        return m_currentUnitsGetingResources < m_numUnitsGetResourcesAtTime;
    }
    /*
     * La unidd notifica que está cogiendo recursos
     */
    public void addUnit()
    {
        ++m_currentUnitsGetingResources;
        Assert.IsTrue(m_currentUnitsGetingResources < m_numUnitsGetResourcesAtTime);
    }
    /*
     * La unidd notifica que ya no está cogiendo recursos
     */
    public void remUnit()
    {
        --m_currentUnitsGetingResources;
        Assert.IsTrue(m_currentUnitsGetingResources > -1);
    }
}
