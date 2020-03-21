using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartLevel1() {
        Debug.Log("Start Scene 1");

        SceneManager.LoadScene(1);
    }

    public void StartLevel2() {
        Debug.Log("Start Scene 2");

        // SceneManager.LoadScene(2);
    }

    public void QuitGame() {
        Debug.Log("QUIT!");

        Application.Quit();
    }
}
