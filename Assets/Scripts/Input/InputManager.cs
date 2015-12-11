using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public static InputManager instance = null;
    public enum MOUSE_BUTTONS { MOUSE_BUTTON_LEFT, MOUSE_BUTTON_RIGH }

    public Rect m_actionRect = new Rect(new Vector2(0,0),new Vector2(Screen.width, Screen.height));
    private Vector3 m_mousePosition;

    private static EventMouseClick m_eventMouseClick;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            m_eventMouseClick = new EventMouseClick();

        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        m_mousePosition = Input.mousePosition;
        m_eventMouseClick.m_screenPosition = m_mousePosition;
        if (Input.GetMouseButtonDown(0) && m_actionRect.Contains(m_mousePosition)) //boton izquierdo pulsado
        {
            m_eventMouseClick.m_button = MOUSE_BUTTONS.MOUSE_BUTTON_LEFT;
            m_eventMouseClick.type =  EventManager.EVENTS.EVENT_MOUSE_DOWN;
            m_eventMouseClick.SendEvent();
        }
        else if (Input.GetMouseButtonUp(0)) //boton izquierdo levantado
        {
            m_eventMouseClick.m_button = MOUSE_BUTTONS.MOUSE_BUTTON_LEFT;
            m_eventMouseClick.type = EventManager.EVENTS.EVENT_MOUSE_UP;
            m_eventMouseClick.SendEvent();
        }

        if (Input.GetMouseButtonDown(1) && m_actionRect.Contains(Input.mousePosition)) //boton derecho pulsado
        {
            m_eventMouseClick.m_button = MOUSE_BUTTONS.MOUSE_BUTTON_RIGH;
            m_eventMouseClick.type = EventManager.EVENTS.EVENT_MOUSE_DOWN;
            m_eventMouseClick.SendEvent();
        }
        else if (Input.GetMouseButtonUp(1)) //boton derecho levantado
        {
            m_eventMouseClick.m_button = MOUSE_BUTTONS.MOUSE_BUTTON_RIGH;
            m_eventMouseClick.type = EventManager.EVENTS.EVENT_MOUSE_UP;
            m_eventMouseClick.SendEvent();
        }
	}

    public Vector3 getScreenMousePosition()
    {
        return m_mousePosition;
    }
}
