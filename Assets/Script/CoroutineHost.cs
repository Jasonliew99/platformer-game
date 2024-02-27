using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a singleton of CoroutineHost
public class CoroutineHost : MonoBehaviour
{
    public static CoroutineHost instance
    {
        get
        {
            if(m_Instance == null)
            {
                //we try to find the current instance of ccoroutiineHost
                m_Instance = (CoroutineHost)FindAnyObjectByType(typeof(CoroutineHost));

                //if coroutineHost doen't exist, we create a new one
                if (m_Instance == null)
                {
                    GameObject go = new GameObject();
                    m_Instance = go.AddComponent<CoroutineHost>();
                }

                DontDestroyOnLoad(m_Instance.gameObject);
            }
        }
    }

    private static CoroutineHost m_Instance = null;
}
