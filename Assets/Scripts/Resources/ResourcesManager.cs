using UnityEngine;
using System.Collections;

public class ResourcesManager : MonoBehaviour {

    public static ResourcesManager instance = null;
    public enum RESOURCES_TYPES
    {
        RESOURCE_TYPE_ONE, RESOURCE_TYPE_TWO, MAX_RESOURCE_TYPES
    }
    public static int maxResourceTypes = (int)RESOURCES_TYPES.MAX_RESOURCE_TYPES;

    void Awake()
    {
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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
