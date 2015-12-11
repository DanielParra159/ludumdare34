using UnityEngine;
using System.Collections;

public class Outline : MonoBehaviour {

    public Renderer m_renderer;
    private float a = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        a += Time.deltaTime * 0.5f;

        float r = Random.Range(0.0f, 0.9f);
        float g = Random.Range(0.0f, 0.9f);
        float b = Random.Range(0.0f, 0.9f);
        if (a>1.0f)
        {
            a = 0.0f;
        }
	    for (int i = 0; i < m_renderer.materials.Length; ++i)
        {
            m_renderer.materials[i].SetColor("_OutlineColor", new Color(r, g, b, a));
        }
	}
}
