using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {

    [Tooltip("Equipo al que pertenece")]
    public TeamManager.TEAMS m_myTeam;

    /* Solo utilizar después del Awake */
    [HideInInspector]
    public int m_team;

    void Awake()
    {
        m_team = (int)m_myTeam;
    }
}
