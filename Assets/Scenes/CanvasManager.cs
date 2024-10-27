using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject LogoWindow;
    [SerializeField] private List<Button> buttons;      // List of buttons
    [SerializeField] private List<string> sceneNames;   // List of scene names

    // Start is called before the first frame update
    void Start()
    {
        LogoWindow.SetActive(true);
        StartCoroutine(nameof(LogoInactive));

        // Set up button listeners
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i; // Local copy to avoid closure issue in loop
            buttons[i].onClick.AddListener(() => LoadSceneByName(sceneNames[index]));
        }
    }

    private IEnumerator LogoInactive()
    {
        yield return new WaitForSeconds(1f);
        LogoWindow.SetActive(false);
    }

    private void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
