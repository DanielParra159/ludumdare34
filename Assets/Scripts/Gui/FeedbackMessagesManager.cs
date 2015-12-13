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
    public enum POSITIONS
    {
        POSITION_LEFT, POSITION_CENTER, POSITION_RIGHT
    }

    [Tooltip("Tiempo que serán visibles los mensajes")]
    [Range(1, 20)]
    public float m_visibleTime;
    private float m_visibleTimeAux;
    private float m_remainingTime; //tiempo restante del mensaje
    [Tooltip("Componente en el que se muestra el texto")]
    public Text m_cameraTextCenter;
    [Tooltip("Componente en el que se muestra el texto")]
    public Text m_cameraTextLeft;
    [Tooltip("Componente en el que se muestra el texto")]
    public Text m_cameraTextRight;
    private Text m_currentText;
    [Tooltip("Tiempo que tarda en aparecer el texto, alfa = 1")]
    [Range(0, 5)]
    public float m_appearTime;
    [Tooltip("Tiempo que tarda en desaparecer el texto, alfa = 0")]
    [Range(0, 5)]
    public float m_disappearTime;
    private STATES m_currentState;
    private string [] m_messages; //array con todos los textos, se debe de cargar de algún archivo

    [Tooltip("Prefab del mensaje que se muestra en coordenadas del mundo")]
    public GameObject m_prefabWoldMessage;
    private int m_numWoldMessage = 10;
    private PoolManager m_worldMessages;

    void Awake()
    {
        Assert.IsNotNull(m_cameraTextCenter, "Componente Text no asignado en FeedbackMessagesManager");
        Assert.IsNotNull(m_cameraTextLeft, "Componente Text no asignado en FeedbackMessagesManager");
        Assert.IsNotNull(m_cameraTextRight, "Componente Text no asignado en FeedbackMessagesManager");
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            
            loadMessages();
            m_worldMessages = new PoolManager(m_prefabWoldMessage, m_numWoldMessage);
            m_worldMessages.Init();

            m_currentText = m_cameraTextCenter;
            m_visibleTimeAux = m_visibleTime;
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

    public void showCameraMessage(FEEDBACK_MESSAGES message, POSITIONS position, Color color, bool inf = false)
    {
        showCameraMessage(m_messages[(int)message], position,color, inf);
    }
    public void hideCameraMessage()
    {
        m_currentText.CrossFadeAlpha(0,1,true);
    }
    public void showCameraMessage(string message, POSITIONS position, Color color, bool inf = false)
    {
        if (inf)
        {
            m_visibleTimeAux = float.MaxValue;
        }
        else
        {
            m_visibleTimeAux = m_visibleTime;
        }
        switch(position)
        {
            case POSITIONS.POSITION_LEFT:
                m_currentText = m_cameraTextLeft;
                break;
            case POSITIONS.POSITION_CENTER:
                m_currentText = m_cameraTextCenter;
                break;
            case POSITIONS.POSITION_RIGHT:
                m_currentText = m_cameraTextRight;
                break;
        }
        m_currentText.text = message;
        m_currentText.color = color;
        changeState(STATES.STATE_APPEARING);
    }

    public void showWorldMessage(Vector3 position, FEEDBACK_MESSAGES message, Color color)
    {
        showWorldMessage(position, m_messages[(int)message], color);
    }
    public void showWorldMessage( Vector3 position, string message, Color color)
    {
        FeedbackMessage feedbackMessage = m_worldMessages.getObject(true).GetComponent<FeedbackMessage>();
        feedbackMessage.setText(position, message, color);
    }

    private void changeState(STATES state)
    {
        m_currentState = state;
        switch(m_currentState)
        {
            case STATES.STATE_APPEARING:
                enabled = true;
                m_currentText.CrossFadeAlpha(1.0f, m_appearTime, false);
                m_remainingTime = m_appearTime;
                break;
            case STATES.STATE_APPEARED:
                m_remainingTime = m_visibleTime - m_appearTime;
                break;
            case STATES.STATE_DISAPPEARING:
                m_currentText.CrossFadeAlpha(0.0f, m_disappearTime, false);
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
