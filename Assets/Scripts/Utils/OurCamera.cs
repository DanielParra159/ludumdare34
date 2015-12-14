using UnityEngine;
using System.Collections;

public class OurCamera : MonoBehaviour {
    [SerializeField]
    [Range(0, 100)]
    private float m_cameraSpeed = 20;

    public static OurCamera instance = null;
	// Use this for initialization
	void Start () {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void moveCamera(Vector3 dir)
    {
        Camera.main.transform.parent.Translate(dir * Time.deltaTime * m_cameraSpeed);
        if(dir.y < 0){
            if(Camera.main.transform.parent.position.y < 20)
            {
                Camera.main.transform.parent.position = new Vector3(Camera.main.transform.parent.position.x,
                                                                    20.0f,
                                                                    Camera.main.transform.parent.position.z);
            }
            
        }
        else if(dir.y > 0)
        {
            if(Camera.main.transform.parent.position.y > 30)
            {
                Camera.main.transform.parent.position = new Vector3(Camera.main.transform.parent.position.x,
                                                                    30.0f,
                                                                    Camera.main.transform.parent.position.z);
            }
        }
        else
        {
            Camera.main.transform.parent.Translate(dir * Time.deltaTime * m_cameraSpeed);
        }
    }
}
