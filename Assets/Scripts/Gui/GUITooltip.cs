using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class GUITooltip : MonoBehaviour {

    
    FeedbackMessagesManager m_feedbackMessagesManager;
    [Tooltip("Posicion donde se va a mostrar")]
    public FeedbackMessagesManager.POSITIONS position;
    [Tooltip("texto")]
    public string text;
    [Tooltip("Color")]
    public Color color = Color.white;

	// Use this for initialization
	void Start () {
        m_feedbackMessagesManager = FeedbackMessagesManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseEnter()
    {
        m_feedbackMessagesManager.showCameraMessage(text, position, color, true);
        Debug.Log("Enter");
    }
    public void OnMouseExit()
    {
        m_feedbackMessagesManager.hideCameraMessage();
        Debug.Log("Exit");
    }

}
