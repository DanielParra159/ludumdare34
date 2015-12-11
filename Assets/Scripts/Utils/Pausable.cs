using UnityEngine;
using System.Collections;

public delegate void NotifyPause();
public delegate void NotifyResume();


public class Pausable {
	
	public static bool pause;
	public bool isPause;
	private NotifyPause  m_notifyPause = null;
    private NotifyResume m_notifyResume = null;

    public Pausable() { }
    public Pausable(NotifyPause notifyPause, NotifyResume notifyResume)
    {
        m_notifyPause += notifyPause;
        m_notifyResume += notifyResume;
    }
	
	public bool Check()
	{
		if ( (pause && isPause) ) return true;
		if ( pause != isPause )
		{
			if ( pause ) Pause();
			else Resume ();
		}
		return isPause;
	}
	public void Pause()
	{
		isPause = true;
        if (m_notifyPause != null) m_notifyPause();
	}
	public void Resume()
	{
		isPause = false;
        if (m_notifyResume != null) m_notifyResume();
	}
    public void registerPause(NotifyPause notifyPause)
    {
        m_notifyPause += notifyPause;
    }
    public void unregisterPause(NotifyPause notifyPause)
    {
        m_notifyPause -= notifyPause;
    }

    public void registerResume(NotifyResume notifyResume)
    {
        m_notifyResume += notifyResume;
    }
    public void unregisterResume(NotifyResume notifyResume)
    {
        m_notifyResume -= notifyResume;
    }
	
}
