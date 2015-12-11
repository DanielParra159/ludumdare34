using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class TeamManager : MonoBehaviour {

    public static TeamManager instance = null;

    public enum TEAMS { TEAM_1, TEAM_2, TEAM_MAX, TEAM_NEUTRAL }
    //public static int neutralTeam = (int)TEAMS.TEAM_NEUTRAL;
    public static int maxTeams = (int)TEAMS.TEAM_MAX; //numero de jugadores incluida la IA

    [Tooltip("Color del los equipos")]
    public Color [] m_teamColors;
    [Tooltip("Color neutra")]
    public Color m_neutralColor;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            Assert.AreEqual(maxTeams, m_teamColors.Length,"m_teamColors es distinto al n�mero de equipos");
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}
