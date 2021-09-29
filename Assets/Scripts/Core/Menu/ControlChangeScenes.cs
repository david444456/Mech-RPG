using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //ActiveCanvas();
        savingWrapper.loadRestartScene();
    }

    public void StartNewGame()
    {
        //ActiveCanvas();
        savingWrapper.Delete();
    }

    private void ActiveCanvas() => CanvasPersistingGO.SetActive(true);
}
