using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public delegate void NotifyChangedTime();

public class TimeManager : MonoBehaviour {

    public static TimeManager instance = null;

    public enum TIME_FACTORS
    {
        TIME_FACTOR_VERY_SLOW, TIME_FACTOR_SLOW, TIME_FACTOR_NORMAL, TIME_FACTOR_QUICK, TIME_FACTOR_VERY_QUICK, MAX_TIME_FACTORS
    }

    [Tooltip("Factores de tiempo emepezando por el más lento.\n0 VERY_SLOW\n1 SLOW\n2 NORMAL\n3 QUICK\n4 VERY_QUICK")]
    public float[] m_timeFactors = {0.5f,0.75f,1.0f,1.25f,1.5f};

    public static float currentTimeFactor;
    private static TIME_FACTORS currentFactor = TIME_FACTORS.TIME_FACTOR_NORMAL;
    private static NotifyChangedTime m_notifyChangedTime;

    void Awake()
    {
        Assert.AreEqual((int)TIME_FACTORS.MAX_TIME_FACTORS, m_timeFactors.Length, "Factores de tiempo no asignados");
        //if (TimeManager.instance == null)
        {
            TimeManager.instance = this;
        //    DontDestroyOnLoad(instance);

            TimeManager.currentTimeFactor = m_timeFactors[(int)currentFactor];

        }
        /*else if (instance != this)
        {
            Destroy(this.gameObject);
        }*/
    }

    public static void increasesSpeed()
    {
        if ( currentFactor != TIME_FACTORS.TIME_FACTOR_VERY_QUICK)
        {
            ++TimeManager.currentFactor;
            TimeManager.instance.updateTime();
        }
    }
    public static void decreasesSpeed()
    {
        if (currentFactor != TIME_FACTORS.TIME_FACTOR_VERY_SLOW)
        {
            --TimeManager.currentFactor;
            TimeManager.instance.updateTime();
        }
    }
    private void updateTime()
    {
        TimeManager.currentTimeFactor = m_timeFactors[(int)currentFactor];
        if (TimeManager.m_notifyChangedTime != null)
        {
            TimeManager.m_notifyChangedTime();
        }
    }
    public static void registerChangedTime(NotifyChangedTime notifyChangedTime)
    {
        TimeManager.m_notifyChangedTime += notifyChangedTime;
    }
    public static void unregisterChangedTime(NotifyChangedTime notifyChangedTime)
    {
        TimeManager.m_notifyChangedTime -= notifyChangedTime;
    }
}
