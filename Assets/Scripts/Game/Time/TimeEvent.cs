using UnityEngine;
using System.Collections;

public class TimeEvent : MonoBehaviour {


    [Tooltip("Tiempo entre eventos, se hace un random entre estos dos valores")]
    public Vector2 m_delayBetweenEvents;
    [Tooltip("El evento que se reproduce")]
    public IEvent m_event;

    protected float m_currentTime;

    protected Pausable m_pausable;
	// Use this for initialization
	void Start () {
        m_currentTime = Random.Range(m_delayBetweenEvents.x, m_delayBetweenEvents.y);
        m_pausable = new Pausable();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_pausable.Check()) return;
        m_currentTime -= Time.deltaTime * TimeManager.currentTimeFactor;
        if ( m_currentTime < 0.0f)
        {
            m_event.play();
            m_currentTime = Random.Range(m_delayBetweenEvents.x, m_delayBetweenEvents.y);
        }
	}
}
