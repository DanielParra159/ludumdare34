using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Team))]

public class Selectable : MonoBehaviour {

    public enum SELECTABLE_TYPES
    {
        SELECTABLE_TYPE_UNIT, SELECTABLE_TYPE_BUILDING, SELECTABLE_TYPE_OTHER, MAX_SELECTABLE_TYPES
    }

    public GameObject m_model;
    public AudioClip m_selectedSound;
    //private static EventPlaySound eventSound; //estatico para que todos los objetos reciclen el evento

    private SELECTABLE_TYPES m_type;
    private int m_numOfMaterials;
    private float m_outlineWidth;
    private Color m_origColor;
    private Color m_teamColor;
    private bool m_selected;


	// Use this for initialization
	void Start () {
        TeamManager.TEAMS teamAux = gameObject.GetComponent<Team>().m_myTeam;
        if (teamAux == TeamManager.TEAMS.TEAM_NEUTRAL)
        {
            m_teamColor = TeamManager.instance.m_neutralColor;
        }
        else
        {
            m_teamColor = TeamManager.instance.m_teamColors[(int)teamAux];
        }
        m_selected = false;
        //Transform transformTemp = transform.FindChild("Model");

        //Assert.IsNotNull(m_model, "No has asignado el circulo de seleccion de: " + gameObject.name);
        m_model.SetActive(false);

        /*if (modelTransform != null)
        {
            m_model = modelTransform.GetComponent<Renderer>();//Se puede reemplazar por una variable publica
        }
        if (m_model && m_model.material.HasProperty("_Outline"))
        {
            m_type = SELECTABLE_TYPES.SELECTABLE_TYPE_UNIT;
            // if there is a "model" children in the object it is a unit
            m_numOfMaterials = m_model.materials.Length;
            m_outlineWidth = m_model.material.GetFloat("_Outline");
            m_model.material.SetFloat("_Outline", 0.0f);
            for (int i = 0; i < m_numOfMaterials; ++i)
            {
                m_model.materials[i].SetColor("_OutlineColor", m_teamColor);
                //model.materials[i].SetColor("_OutlineColor", Color.black);
            }
                
        }
        else
        {
            m_model = gameObject.GetComponent<Renderer>();
            // if not, it is a building
            if (m_model.material.HasProperty("_DiffuseColor"))
            {
                m_type = SELECTABLE_TYPES.SELECTABLE_TYPE_BUILDING;
                m_origColor = m_model.material.GetColor("_DiffuseColor");
            }
            else if (m_model.material.HasProperty("_AlphaColor"))
            {
                // is a buildable (construible) building
                m_type = SELECTABLE_TYPES.SELECTABLE_TYPE_BUILDING;
                m_origColor = Color.white;
            }
            else
            {
                m_type = SELECTABLE_TYPES.SELECTABLE_TYPE_OTHER;
                m_origColor = m_model.material.color;
            }
        }*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetSelected()
    {
        m_model.SetActive(true);
        m_selected = true;
        if (m_selectedSound!=null)
        {
            SoundManager.instance.PlaySingleSelectedUnit(m_selectedSound);
        }
        /*
        
        switch (m_type)
        {
            case SELECTABLE_TYPES.SELECTABLE_TYPE_UNIT:
                m_model.material.SetFloat("_Outline", m_outlineWidth);
                break;
            case SELECTABLE_TYPES.SELECTABLE_TYPE_BUILDING:
                this.m_model.material.SetColor("_DiffuseColor", m_teamColor);
                break;
            case SELECTABLE_TYPES.SELECTABLE_TYPE_OTHER:
                this.m_model.material.color = new Color(m_teamColor.r + 0.4f, m_teamColor.g + 0.4f, m_teamColor.b + 0.4f);
                break;
        }*/
    }
    public void SetDeselect()
    {
        m_model.SetActive(false);
        m_selected = false;
        /*switch (m_type)
        {
            case SELECTABLE_TYPES.SELECTABLE_TYPE_UNIT:
                m_model.material.SetFloat("_Outline", 0.0f);
                break;
            case SELECTABLE_TYPES.SELECTABLE_TYPE_BUILDING:
                m_model.material.SetColor("_DiffuseColor", m_origColor);
                break;
            case SELECTABLE_TYPES.SELECTABLE_TYPE_OTHER:
                m_model.material.color = m_origColor;
                break;
        }*/
    }

    public bool isSelected()
    {
        return m_selected;
    }
}
