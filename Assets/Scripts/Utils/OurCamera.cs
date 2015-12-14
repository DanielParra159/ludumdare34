using UnityEngine;
using System.Collections;

public class OurCamera : MonoBehaviour {
    [SerializeField]
    [Range(0, 100)]
    private float m_cameraSpeed = 20;

    public static OurCamera instance = null;

    [System.Serializable]
    public class ValoresLimiteEyeY
    {
        public float m_limiteBajo = 20;
        public float m_limiteAlto = 30;
    }

    public ValoresLimiteEyeY m_valoresLimiteEyeY;

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
                                                                    m_valoresLimiteEyeY.m_limiteBajo,
                                                                    Camera.main.transform.parent.position.z);
            }
            
        }
        else if(dir.y > 0)
        {
            if(Camera.main.transform.parent.position.y > 30)
            {
                Camera.main.transform.parent.position = new Vector3(Camera.main.transform.parent.position.x,
                                                                    m_valoresLimiteEyeY.m_limiteAlto,
                                                                    Camera.main.transform.parent.position.z);
            }
        }
        else
        {
            Camera.main.transform.parent.Translate(dir * Time.deltaTime * m_cameraSpeed);
        }
    }
}
