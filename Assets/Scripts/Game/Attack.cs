using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

    [SerializeField]
    [Tooltip("Da�o que hace el ataque")]
    [Range(1, 1000)]
    protected float m_damage;
    [SerializeField]
    [Tooltip("Tiempo entre ataques")]
    [Range(1, 30)]
    protected float m_delayBetweenAttacks;
    [SerializeField]
    [Tooltip("Rango de ataque")]
    [Range(1, 30)]
    protected float m_attackRange = 4.0f;
    protected float m_attackRange2;

    protected float m_currentTime;
	// Use this for initialization
	void Awake () {
        m_attackRange2 = m_attackRange * m_attackRange;
        m_currentTime = m_delayBetweenAttacks; //@todo: necesita un init donde se reinicie
	}
	
	// Update is called once per frame
	void Update () {
        m_currentTime -= Time.deltaTime * TimeManager.currentTimeFactor;;
        if (m_currentTime< 0.0f)
        {
            this.enabled = false;
        }
	}

    public void attackTarget(GameObject target)
    {
        target.GetComponent<Life>().OnDamage(m_damage);
        m_currentTime = m_delayBetweenAttacks;
        this.enabled = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange2);
    }

    public float getAttackRange2()
    {
        return m_attackRange2;
    }
    public bool canAttack()
    {
        return m_currentTime < 0.0f;
    }
}
