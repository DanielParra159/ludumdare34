using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Collections;

public class FeedbackMessage : MonoBehaviour {

    public Text m_text;
    [Tooltip("Tiempo que serán visibles los mensajes")]
    [Range(1, 20)]
    public float m_visibleTime;
    protected float m_remainingTime; //tiempo restante del mensaje
    [Tooltip("Tiempo que tarda en aparecer el texto, alfa = 1")]
    [Range(0, 5)]
    public float m_appearTime;
    [Tooltip("Tiempo que tarda en desaparecer el texto, alfa = 0")]
    [Range(0, 5)]
    public float m_disappearTime;
    [Tooltip("Tiempo durante el que se desplaza, 0 desactivado desplazamiento.")]
    [Range(1, 20)]
    public float m_movTime;
    private float m_remainingMovTime;
    [Tooltip("Velocidad de movimiento")]
    public Vector3 m_speed;

    protected Transform m_transform;


    private FeedbackMessagesManager.STATES m_currentState;

	// Use this for initialization
	void Awake () {
        Assert.IsNotNull(m_text);
        m_transform = gameObject.GetComponent<Transform>();
        
	}
	
	// Update is called once per frame
	void Update () {
        m_remainingTime -= Time.deltaTime * TimeManager.currentTimeFactor;
        m_remainingMovTime -= Time.deltaTime * TimeManager.currentTimeFactor;
        if (m_remainingMovTime > 0.0f)
        {
            m_transform.Translate(m_speed * Time.deltaTime * TimeManager.currentTimeFactor);
        }
        if (m_remainingTime < 0.0f)
        {
            changeState(m_currentState + 1);
        }
	}

    public void setText(Vector3 position, string text, Color color)
    {
        m_transform.position = position;
        m_text.text = text;
        m_text.color = color;
        changeState(FeedbackMessagesManager.STATES.STATE_APPEARING);
    }
    
    private void changeState(FeedbackMessagesManager.STATES state)
    {
        m_currentState = state;
        switch (m_currentState)
        {
            case FeedbackMessagesManager.STATES.STATE_APPEARING:
                enabled = true;
                m_text.CrossFadeAlpha(1.0f, m_appearTime, true);
                m_remainingTime = m_appearTime;
                m_remainingMovTime = m_movTime;
                break;
            case FeedbackMessagesManager.STATES.STATE_APPEARED:
                m_remainingTime = m_visibleTime - m_appearTime;
                break;
            case FeedbackMessagesManager.STATES.STATE_DISAPPEARING:
                m_text.CrossFadeAlpha(0.0f, m_disappearTime, true);
                m_remainingTime = m_disappearTime;
                break;
            case FeedbackMessagesManager.STATES.STATE_DISAPPEARED:
                gameObject.SetActive(false);
                break;
        }
    }
}
