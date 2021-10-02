using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlChangeScenes : MonoBehaviour
{
    public static ControlChangeScenes Instance;

    [SerializeField] SavingWrapper savingWrapper;
    [SerializeField] GameObject CanvasPersistingGO = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void restartScene()
    {
        ReturnNormalState();
        savingWrapper.loadRestartScene();
    }

    public void StartNewGame()
    {
        ReturnNormalState();
        savingWrapper.Delete();
    }

    public void QuitGame()
    {
        ReturnNormalState();
        Application.Quit();
    }

    public void ReturnToMainMenu() {
        ReturnNormalState();
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        savingWrapper.loadRestartScene();
    }

    public void SaveGame() {
        savingWrapper.Save();
        StartCoroutine(ChangeVisibilitySavingUI());
    }

    IEnumerator ChangeVisibilitySavingUI() {
        CanvasPersistingGO.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        CanvasPersistingGO.SetActive(false);
    }

    private void ReturnNormalState() => Time.timeScale = 1;

    private void ActiveCanvas() => CanvasPersistingGO.SetActive(true);
}
