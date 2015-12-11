using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class PoolManager {

	public GameObject 										m_goPool; //el objeto que contendra nuestra pool
	public int 												m_poolAmount; //numero de objetos que contendra la pool al iniciar

	private List<GameObject> 								m_poolList; //lista que mantiene los objetos
    private bool m_initialized = false;

    public PoolManager() { }
    public PoolManager(GameObject goPool, int poolAmount)
    {
        m_goPool = goPool;
        m_poolAmount = poolAmount;
    }

	// Use this for initialization
	public void Init () {
        m_initialized = true;
		m_poolList = new List<GameObject>(m_poolAmount);
		for ( int i = 0; i < m_poolAmount; i++ )
		{
			m_poolList.Add( GameObject.Instantiate(m_goPool) );
            m_poolList[i].SetActive(false);
		}
	}

	public void Reset () {
        Assert.IsTrue(m_initialized, "no se ha inicializado la Pool");
		for ( int i = 0; i < m_poolAmount; i++ )
		{
			m_poolList[i].SetActive( false );
		}
	}
	

	/*
	 * Busca un item inactivo, si no lo encuentra lo crea y lo devuelve, bActive = true activa el GameObject
	 */
	public GameObject getObject(bool bActive)
	{
        Assert.IsTrue(m_initialized, "no se ha inicializado la Pool");
		bool bFind = false;
		int i = 0;
		GameObject goToReturn = null;
		while ( i < m_poolAmount && !bFind  )
		{
			if ( !m_poolList[i].activeInHierarchy )
			{
				bFind = true;
				goToReturn = m_poolList[i];
			}
			else
				i++;
		}
		if ( !bFind )
		{
			goToReturn = GameObject.Instantiate(m_goPool);
			m_poolList.Add( goToReturn );
			m_poolAmount++;
		}
		goToReturn.SetActive( bActive );
		return goToReturn;
	}

}
