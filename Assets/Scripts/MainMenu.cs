using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject instructionButton;

    public GameObject instructionMenu;
    public GameObject backButton;
    public GameObject instructionTitle;
    public GameObject instructionOverlay;

    public void StartLevel1() {
        Debug.Log("Start Scene 1");

        SceneManager.LoadScene(1);
    }

    public void StartLevel2() {
        Debug.Log("Start Scene 2");

        SceneManager.LoadScene(2);
    }

    public void StartLevel3() {
        Debug.Log("Start Scene 3");

        SceneManager.LoadScene(3);
    }

    public void DisplayInstructions() {
        mainMenu.SetActive(false);
        instructionButton.SetActive(false);

        instructionMenu.SetActive(true);
        backButton.SetActive(true);
        instructionTitle.SetActive(true);
        instructionOverlay.SetActive(true);
    }

    public void DisplayMainMenu() {
        mainMenu.SetActive(true);
        instructionButton.SetActive(true);

        instructionMenu.SetActive(false);
        backButton.SetActive(false);
        instructionTitle.SetActive(false);
        instructionOverlay.SetActive(false);
    }

    public void QuitGame() {
        Debug.Log("QUIT!");

        Application.Quit();
    }
}
