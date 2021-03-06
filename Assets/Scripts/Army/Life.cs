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
    [Tooltip("Regeneración por segundo")]
    [Range(1, 100)]
    public float m_yPositionOfDamageMessage = 1.0f;

    public AudioClip m_onDamageSound;
    public AudioClip m_onDeadSound;

    [Tooltip("Slider donde se mostrara la vida")]
    public Slider m_slider;

    private NotifyOnDead m_onDead = null;
    private NotifyOnDamage m_onDamage = null;

    private FeedbackMessagesManager m_feedbackMessagesManager;

    protected Pausable m_pausable;
    // Use this for initialization
    void Start()
    {
        init();
        m_feedbackMessagesManager = FeedbackMessagesManager.instance;
        m_slider.transform.parent.transform.parent = null;
        m_pausable = new Pausable();
    }
    public void init()
    {
        m_currentLife = m_maxLife;
        if (m_slider)
        {
            m_slider.maxValue = m_maxLife;
            m_slider.value = m_currentLife;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_pausable.Check()) return;
        m_currentLife += m_regeneration * Time.deltaTime * TimeManager.currentTimeFactor;
        if ( m_currentLife > m_maxLife)
        {
            m_currentLife = m_maxLife;
            //this.enabled = false;
        }
        if (m_slider)
        {
            m_slider.transform.parent.position = gameObject.transform.position + Vector3.forward * 1.0f;
            m_slider.value = m_currentLife;
        }
    }
    public bool OnDamage(float damage)
    {
        bool dead = false;
        this.enabled = true;
        m_currentLife -= damage;
        Vector3 offset = new Vector3(Random.Range(1.0f, 3.0f), m_yPositionOfDamageMessage, Random.Range(1.0f, 3.0f));
        m_feedbackMessagesManager.showWorldMessage(gameObject.transform.position + offset, "" + damage, Color.red);
        if (m_currentLife <= 0.0f)
        {
            //gameObject.SendMessage("OnDead", SendMessageOptions.DontRequireReceiver);
            if (m_onDead != null)
                m_onDead();
            m_currentLife = 0.0f;
            dead = true;

            if (m_onDeadSound != null)
            {
                SoundManager.instance.PlaySingle(m_onDeadSound);
            }
        }
        else if (m_onDamage != null)
        {
            if (m_onDamageSound != null)
            {
                SoundManager.instance.PlaySingle(m_onDamageSound);
            }
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
    public void SetActive(bool active)
    {
        m_slider.gameObject.SetActive(active);
    }
}
