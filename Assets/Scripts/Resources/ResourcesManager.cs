using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Collections;

public class ResourcesManager : MonoBehaviour {

    public static ResourcesManager instance = null;
    public enum RESOURCES_TYPES
    {
        RESOURCE_TYPE_ONE, RESOURCE_TYPE_TWO, MAX_RESOURCE_TYPES
    }
    public static int maxResourceTypes = (int)RESOURCES_TYPES.MAX_RESOURCE_TYPES;

    private FeedbackMessagesManager m_feedbackMessagesManager;

    [System.Serializable]
    public class Resources
    {
        [Tooltip("Cantidad de este recurso")]
        public int[] resourcesNum;
    }
    [Tooltip("Recursos iniciales de cada equipo")]
    public Resources[] currentResources;

    [SerializeField]
    [Tooltip("Donde se pintara el recurso 1")]
    protected Text resource1;
    [SerializeField]
    [Tooltip("Donde se pintara el recurso 2")]
    protected Text resource2;

    void Awake()
    {
        Assert.IsTrue(currentResources.Length == (int)TeamManager.TEAMS.TEAM_MAX, "Tama�o de currentResources distinto a TeamManager.TEAMS.TEAM_MAX");
        Assert.IsTrue(currentResources[0].resourcesNum.Length == maxResourceTypes, "Tama�o de currentResources[0] distinto a maxResourceTypes");
        Assert.IsTrue(currentResources[1].resourcesNum.Length == maxResourceTypes, "Tama�o de currentResources[1] distinto a maxResourceTypes");
        Assert.IsNotNull(resource1, "resource1 no asignado");
        Assert.IsNotNull(resource2, "resource1 no asignado");
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

	// Use this for initialization
	void Start () {
        m_feedbackMessagesManager = FeedbackMessagesManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
        resource1.text = "" + currentResources[0].resourcesNum[0];
        resource2.text = "" + currentResources[0].resourcesNum[1];
	}

    public void addResources(int team, RESOURCES_TYPES type, int value )
    {
        currentResources[team].resourcesNum[(int)type] += value;
    }
    public void remResources(int team, RESOURCES_TYPES type, int value)
    {
        currentResources[team].resourcesNum[(int)type] -= value;
    }
    public bool canRemResources(int team, RESOURCES_TYPES type, int value)
    {
        return (currentResources[team].resourcesNum[(int)type] - value) > -1;
    }
    //MÉTODOS DE COMPROBACIÓN DE POSIBILIDAD DE CONSTRUCCIÓN POR RECURSOS Y ENVIO DE MENSAJES
    public bool haveEnoughResources(Buildng.BUILDING_TYPES building)
    {
        switch (building)
        {
            case Buildng.BUILDING_TYPES.BUILDING_URBAN_CENTER:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 300);
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, 200);
                    return true;
                }
                return false;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_HOUSE:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 50);
                    return true;
                }
                return false;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_BARRACKS:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 150);
                    return true;
                }
                return false;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_UPGRADE:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 100);
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, 200);
                    return true;
                }
                return false;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_TOWER:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 75);
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, 50);
                    return true;
                }
                return false;
        }
        return false;
    }
    public bool resourceComprobation(Buildng.BUILDING_TYPES building)
    {
        switch (building) 
        { 
            case Buildng.BUILDING_TYPES.BUILDING_URBAN_CENTER:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 300) && 
                    !canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, 200))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(2));
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 300))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(0));
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, 200))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(1));
                    return false;
                }
                return true;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_HOUSE:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 50))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(0));
                    return false;
                }
                return true;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_BARRACKS:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 150))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(0));
                    return false;
                }
                return true;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_UPGRADE:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 100) &&
                    !canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, 200))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(2));
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 100))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(0));
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, 200))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(1));
                    return false;
                }
                return true;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_TOWER:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 75) &&
                    !canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, 50))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(2));
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, 75))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(0));
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, 50))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(1));
                    return false;
                }
                return true;
        }
        return false;
    }
}
