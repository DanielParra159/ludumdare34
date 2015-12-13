using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public static InputManager instance = null;
    public enum MOUSE_BUTTONS { MOUSE_BUTTON_LEFT, MOUSE_BUTTON_RIGH }

    public Rect m_actionRect;
    public float m_margin_Percentage = 0.1f;
    private Rect m_left;
    private Rect m_up;
    private Rect m_right;
    private Rect m_down;
    private Vector3 m_mousePosition;

    private static EventMouseClick m_eventMouseClick;
    private static EventMoveCamera m_eventMoveCamera;

    /*void OnGUI()
    {
        GUI.Button(m_left, "");
        GUI.Button(m_up, "");
        GUI.Button(m_right, "");
        GUI.Button(m_down, "");
        GUI.Button(m_actionRect, "");
    }*/

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            m_eventMouseClick = new EventMouseClick();
            m_eventMoveCamera = new EventMoveCamera();

            m_left = new Rect(0, 0, Screen.width * m_margin_Percentage, Screen.height);
            m_up = new Rect(0, 0, Screen.width, Screen.height * m_margin_Percentage);
            m_right = new Rect(Screen.width - (Screen.width * m_margin_Percentage), 0, Screen.width * m_margin_Percentage, Screen.height);
            m_down = new Rect(0, Screen.height - Screen.height * m_margin_Percentage, Screen.width, Screen.height * m_margin_Percentage);

            m_actionRect = new Rect(new Vector2(0, 0), new Vector2(Screen.width, Screen.height - Screen.height * 0.26f));
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

	
	// Update is called once per frame
	void Update () {
        m_mousePosition = Input.mousePosition;
        m_eventMouseClick.m_screenPosition = m_mousePosition;
        Vector3 cameraDir = Vector3.zero;
        if (m_left.Contains(m_mousePosition) || Input.GetKey(KeyCode.LeftArrow))
        {
            cameraDir.x = -1;
        }
        else if (m_right.Contains(m_mousePosition) || Input.GetKey(KeyCode.RightArrow))
        {
            cameraDir.x = +1;
        }
        if (m_up.Contains(m_mousePosition) || Input.GetKey(KeyCode.DownArrow))
        {
            cameraDir.z = -1; 
        }
        else if (m_down.Contains(m_mousePosition) || Input.GetKey(KeyCode.UpArrow))
        {
            cameraDir.z = +1;
        }
        if (Input.GetKey(KeyCode.PageDown) || Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            cameraDir.y = -1;
        }
        else if (Input.GetKey(KeyCode.PageUp) || Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            cameraDir.y = +1; 
        }
        m_eventMoveCamera.m_dir = cameraDir.normalized;
        m_eventMoveCamera.SendEvent();
        Vector3 mousePosAux = m_mousePosition;
        mousePosAux.y = Screen.height - mousePosAux.y;
        if (Input.GetMouseButtonDown(0) && m_actionRect.Contains(mousePosAux)) //boton izquierdo pulsado
        {
            m_eventMouseClick.m_button = MOUSE_BUTTONS.MOUSE_BUTTON_LEFT;
            m_eventMouseClick.type =  EventManager.EVENTS.EVENT_MOUSE_DOWN;
            m_eventMouseClick.SendEvent();
        }
        else if (Input.GetMouseButtonUp(0) /*&& m_actionRect.Contains(mousePosAux)*/) //boton izquierdo levantado
        {
            m_eventMouseClick.m_button = MOUSE_BUTTONS.MOUSE_BUTTON_LEFT;
            m_eventMouseClick.type = EventManager.EVENTS.EVENT_MOUSE_UP;
            m_eventMouseClick.SendEvent();
        }

        if (Input.GetMouseButtonDown(1) && m_actionRect.Contains(mousePosAux)) //boton derecho pulsado
        {
            m_eventMouseClick.m_button = MOUSE_BUTTONS.MOUSE_BUTTON_RIGH;
            m_eventMouseClick.type = EventManager.EVENTS.EVENT_MOUSE_DOWN;
            m_eventMouseClick.SendEvent();
        }
        else if (Input.GetMouseButtonUp(1) /*&& m_actionRect.Contains(mousePosAux)*/) //boton derecho levantado
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
