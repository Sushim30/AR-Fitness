using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private string Scenename;
    public void SceneChange()
    {
        SceneManager.LoadScene(Scenename);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
