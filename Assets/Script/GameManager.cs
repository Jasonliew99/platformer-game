using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int Currentlevel = 1;

    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (GameManager)FindAnyObjectByType(typeof(GameManager));

                if (m_Instance == null)
                {
                    GameObject go = new GameObject();
                    m_Instance = go.AddComponent<GameManager>();
                }

                DontDestroyOnLoad(m_Instance.gameObject);
            }

            return m_Instance;
        }
    }

    private static GameManager m_Instance = null;

    public void StartGame()
    {
        Currentlevel = 1;
        SceneManager.LoadScene(Currentlevel);
    }

    public void GoToNextLevel()
    {
        if(Currentlevel == 2)
        {
            Currentlevel = 1;
            SceneManager.LoadScene(0);
            return;
        }

        Currentlevel++;
        SceneManager.LoadScene(Currentlevel);
    }

}
