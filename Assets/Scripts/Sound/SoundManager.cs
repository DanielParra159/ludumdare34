using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [Tooltip("AudioSource donde se pueden reproducir sonidos")]
    public AudioSource [] m_sfxSources;
    [Tooltip("AudioSource donde se reproduciran los sonidos al seleccionar una unidad")]
    public AudioSource m_sfxSourceSelectedUnit;


    void Awake()
    {
        //if (instance == null)
        {
            instance = this;
        //    DontDestroyOnLoad(instance);
        }
        /*else if (instance != this)
        {
            Destroy(this.gameObject);
        }*/
    }


    //Used to play single sound clips.
    public bool PlaySingle(AudioClip clip)
    {
        for (int i=0; i < m_sfxSources.Length; ++i)
        {
            if ( !m_sfxSources[i].isPlaying)
            {
                m_sfxSources[i].clip = clip;
                m_sfxSources[i].Play();
                return true;
            }
        }
        return false;
    }
    public bool PlaySingleSelectedUnit(AudioClip clip)
    {
        if (!m_sfxSourceSelectedUnit.isPlaying)
        {
            m_sfxSourceSelectedUnit.clip = clip;
            m_sfxSourceSelectedUnit.Play();
            return true;
        }
        return false;
    }
}
