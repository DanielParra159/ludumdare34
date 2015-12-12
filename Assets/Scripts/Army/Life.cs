using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public delegate void NotifyOnDead();
public delegate void NotifyOnDamage(float currentLif);

public class Life : MonoBehaviour
{
    [Tooltip("Vida maxima")]
    [Range(1, 1000)]
    public float m_maxLife = 100.0f;
    [HideInInspector]
    private float m_currentLife;
    [Tooltip("Regeneración por segundo")]
    [Range(0, 1000)]
    public float m_regeneration= 0.0f;

    [Tooltip("Slider donde se mostrara la vida")]
    public Slider m_slider;

    private NotifyOnDead m_onDead = null;
    private NotifyOnDamage m_onDamage = null;

    // Use this for initialization
    void Start()
    {
        reset(); 
    }
    void reset()
    {
        m_currentLife = m_maxLife;
        if (m_slider)
        {
            m_slider.maxValue = m_maxLife;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_currentLife += m_regeneration * Time.deltaTime * TimeManager.currentTimeFactor;
        if ( m_currentLife>m_maxLife)
        {
            m_currentLife = m_maxLife;
            this.enabled = false;
        }
    }
    public bool OnDamage(float damage)
    {
        bool dead = false;
        this.enabled = true;
        m_currentLife -= damage;
        if (m_currentLife <= 0.0f)
        {
            //gameObject.SendMessage("OnDead", SendMessageOptions.DontRequireReceiver);
            if (m_onDead != null)
                m_onDead();
            m_currentLife = 0.0f;
            dead = true;
        }
        else if (m_onDamage != null)
        {
            m_onDamage(m_currentLife);
        }

        if (m_slider)
        {
            m_slider.value = m_currentLife;
        }

        return dead;
    }
    public bool isAlive()
    {
        return m_currentLife > 0.0f;
    }

    public float getLife()
    {
        return m_currentLife;
    }

    public float getMaxLife()
    {
        return m_maxLife;
    }

    /*
     * Registra la funcion que ser� llamada cuando se quede sin vida
     */
    public void registerOnDead( NotifyOnDead onDead)
    {
        m_onDead += onDead;
    }
    public void unregisterOnDead(NotifyOnDead onDead)
    {
        m_onDead -= onDead;
    }

    /*
     * Registra la funcion que ser� llamada cuando reciba un da�o y no se muera
     */
    public void registerOnDamage(NotifyOnDamage onDamage)
    {
        m_onDamage += onDamage;
    }
    public void unregisterOnDamage(NotifyOnDamage onDamage)
    {
        m_onDamage -= onDamage;
    }
    public void showSlider()
    {
        m_slider.enabled = true;
    }
    public void hideSlider()
    {
        m_slider.enabled = false;
    }

    public void setRegeneration(float regeneration)
    {
        m_regeneration = regeneration;
    }
}
