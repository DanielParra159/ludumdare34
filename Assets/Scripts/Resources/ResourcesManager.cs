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

    [System.Serializable]
    public class BuildingPrices
    {    
        public int m_urbanCentreRes1 = 300;
        public int m_urbanCentreRes2 = 200;
        public int m_houseRes1 = 50;
        public int m_houseRes2 = 0;
        public int m_barracksRes1 = 150;
        public int m_barracksRes2 = 0;
        public int m_upgradeRes1 = 100;
        public int m_upgradeRes2 = 200;
        public int m_towerRes1 = 75;
        public int m_towerRes2 = 50;
        public int m_resourceRes1 = 125;
        public int m_resourceRes2 = 0;
    }

    public BuildingPrices m_buildingPrices;

    [System.Serializable]
    public class UnitPrices
    {
        public int m_workerRes1 = 50;
        public int m_workerRes2 = 0;
        public int m_swordmanRes1 = 125;
        public int m_swordmanRes2 = 0;
        public int m_archerRes1 = 100;
        public int m_archerRes2 = 75;
        public int m_lancerRes1 = 150;
        public int m_lancerRes2 = 250;
    }

    public UnitPrices m_unitPrices;

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
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_urbanCentreRes1);
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_buildingPrices.m_urbanCentreRes2);
                    return true;
                }
                return false;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_HOUSE:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_houseRes1);
                    return true;
                }
                return false;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_BARRACKS:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_barracksRes1);
                    return true;
                }
                return false;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_UPGRADE:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_upgradeRes1);
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_buildingPrices.m_upgradeRes2);
                    return true;
                }
                return false;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_TOWER:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_towerRes1);
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_buildingPrices.m_towerRes2);
                    return true;
                }
                return false;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_RESOURCE:
                if (resourceComprobation(building))
                {
                    remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_resourceRes1);
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
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_urbanCentreRes1) &&
                    !canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_buildingPrices.m_urbanCentreRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_BOTH), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_urbanCentreRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_buildingPrices.m_urbanCentreRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_TWO), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return true;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_HOUSE:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_houseRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return true;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_BARRACKS:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_barracksRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return true;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_UPGRADE:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_upgradeRes1) &&
                    !canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_buildingPrices.m_upgradeRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_BOTH), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_upgradeRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_buildingPrices.m_upgradeRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_TWO), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return true;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_TOWER:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_towerRes1) &&
                    !canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_buildingPrices.m_towerRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_BOTH), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_towerRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_buildingPrices.m_towerRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_TWO), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return true;
            case Buildng.BUILDING_TYPES.BUILDING_TYPE_RESOURCE:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_buildingPrices.m_resourceRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE), 
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return false;
        }
        return false;
    }

    public bool haveEnoughResources(Unit.UNIT_TYPES unit)
    {
        switch(unit){
        case Unit.UNIT_TYPES.UNIT_TYPE_WORKER:
            if (resourceComprobation(unit))
            {
                remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_workerRes1);
                return true;
            }
            return false;
        case Unit.UNIT_TYPES.UNIT_TYPE_WARRIOR_SWORDMAN:
            if (resourceComprobation(unit))
            {
                remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_swordmanRes1);
                return true;
            }
            return false;
        case Unit.UNIT_TYPES.UNIT_TYPE_WARRIOR_ARCHER:
            if (resourceComprobation(unit))
            {
                remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_archerRes1);
                remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_unitPrices.m_archerRes2);
                return true;
            }
            return false;
        case Unit.UNIT_TYPES.UNIT_TYPE_WARRIOR_LANCER:
            if (resourceComprobation(unit))
            {
                remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_lancerRes1);
                remResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_unitPrices.m_lancerRes2);
                return true;
            }
            return false;
        }
        return false;
    }
    public bool resourceComprobation(Unit.UNIT_TYPES unit)
    {
        switch (unit)
        {
            case Unit.UNIT_TYPES.UNIT_TYPE_WORKER:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_workerRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE),
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return true;

            case Unit.UNIT_TYPES.UNIT_TYPE_WARRIOR_SWORDMAN:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_swordmanRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE),
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return true;

            case Unit.UNIT_TYPES.UNIT_TYPE_WARRIOR_ARCHER:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_archerRes1) &&
                    !canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_unitPrices.m_archerRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_BOTH),
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_archerRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE),
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_unitPrices.m_archerRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_TWO),
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return true;

            case Unit.UNIT_TYPES.UNIT_TYPE_WARRIOR_LANCER:
                if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_lancerRes1) &&
                    !canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_unitPrices.m_lancerRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_BOTH),
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_ONE, m_unitPrices.m_lancerRes1))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE),
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                else if (!canRemResources(0, RESOURCES_TYPES.RESOURCE_TYPE_TWO, m_unitPrices.m_lancerRes2))
                {
                    m_feedbackMessagesManager.showCameraMessage(m_feedbackMessagesManager.getMessage(
                        (int)FeedbackMessagesManager.FEEDBACK_MESSAGES.FEEDBACK_MESSAGE_WANTING_RESOURCE_TWO),
                        FeedbackMessagesManager.POSITIONS.POSITION_CENTER, Color.white);
                    return false;
                }
                return true;
        }
        return false;
    }
}
