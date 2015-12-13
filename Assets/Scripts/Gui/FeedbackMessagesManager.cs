using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Collections;

public class FeedbackMessagesManager : MonoBehaviour {

    public static FeedbackMessagesManager instance = null;
    
    public enum FEEDBACK_MESSAGES
    {
        FEEDBACK_MESSAGE_WANTING_RESOURCE_ONE, FEEDBACK_MESSAGE_WANTING_RESOURCE_TWO, FEEDBACK_MESSAGE_WANTING_RESOURCE_BOTH, 
        MAX_FEEDBACK_MESSAGES
    }
    public static int maxFeedBackMessages = (int)FEEDBACK_MESSAGES.MAX_FEEDBACK_MESSAGES;
    public enum STATES
    {
        STATE_APPEARING, STATE_APPEARED, STATE_DISAPPEARING, STATE_DISAPPEARED, MAX_STATES
    }

    [Tooltip("Tiempo que serán visibles los mensajes")]
    [Range(1, 20)]
    public float m_visibleTime;
    private float m_remainingTime; //tiempo restante del mensaje
    [Tooltip("Componente en el que se muestra el texto")]
    public Text m_cameraText;
    [Tooltip("Tiempo que tarda en aparecer el texto, alfa = 1")]
    [Range(1, 5)]
    public float m_appearTime;
    [Tooltip("Tiempo que tarda en desaparecer el texto, alfa = 0")]
    [Range(1, 5)]
    public float m_disappearTime;
    private STATES m_currentState;
    private string [] m_messages; //array con todos los textos, se debe de cargar de algún archivo

    [Tooltip("Prefab del mensaje que se muestra en coordenadas del mundo")]
    public GameObject m_prefabWoldMessage;
    private int m_numWoldMessage = 10;
    private PoolManager m_worldMessages;

    void Awake()
    {
        Assert.IsNotNull(m_cameraText, "Componente Text no asignado en FeedbackMessagesManager");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            
            loadMessages();
            m_worldMessages = new PoolManager(m_prefabWoldMessage, m_numWoldMessage);
            m_worldMessages.Init();
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    private bool loadMessages()
    {
        m_messages = new string[maxFeedBackMessages];
        /*for (int i = 0; i < maxFeedBackMessages;++i )
        {
            m_messages[i] = "Menssage " + i;
        }*/
        m_messages[0] = "You need more GOLD!";
        m_messages[1] = "You need more BEER!";
        m_messages[2] = "You need more resources!";
        return true;
    }
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        m_remainingTime -= Time.deltaTime * TimeManager.currentTimeFactor;
        if ( m_remainingTime < 0.0f)
        {
            changeState(m_currentState + 1);
        }
	}

    public void showCameraMessage(FEEDBACK_MESSAGES message)
    {
        showCameraMessage(m_messages[(int)message]);
    }
    public void showCameraMessage(string message)
    {
        changeState(STATES.STATE_APPEARING);
        m_cameraText.text = message;
    }

    public void showWorldMessage(Vector3 position, FEEDBACK_MESSAGES message)
    {
        showWorldMessage(position, m_messages[(int)message]);
    }
    public void showWorldMessage( Vector3 position, string message)
    {
        FeedbackMessage feedbackMessage = m_worldMessages.getObject(true).GetComponent<FeedbackMessage>();
        feedbackMessage.setText(position, message);
    }

    private void changeState(STATES state)
    {
        m_currentState = state;
        switch(m_currentState)
        {
            case STATES.STATE_APPEARING:
                enabled = true;
                m_cameraText.CrossFadeAlpha(1.0f, m_appearTime, false);
                m_remainingTime = m_appearTime;
                break;
            case STATES.STATE_APPEARED:
                m_remainingTime = m_visibleTime - m_appearTime;
                break;
            case STATES.STATE_DISAPPEARING:
                m_cameraText.CrossFadeAlpha(0.0f, m_disappearTime, false);
                m_remainingTime = m_disappearTime;
                break;
            case STATES.STATE_DISAPPEARED:
                enabled = false;
                break;
        }
    }
    public string getMessage(int message)
    {
        return m_messages[message];
    }
}
