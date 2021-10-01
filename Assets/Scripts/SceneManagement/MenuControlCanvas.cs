using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
using RPG.Stats;
using UnityEngine.Events;

namespace RPG.SceneManagement {
    public class MenuControlCanvas : MonoBehaviour
    {
        [SerializeField] Portal portal;
        [SerializeField] ConfigurationGame configurationGame;
        [SerializeField] float timeToStartGame = 40;
        [SerializeField] UnityEvent startNewGame = new UnityEvent();

        [Header("Progression")]
        [SerializeField] Progression progressionEasy;
        [SerializeField] Progression progressionMedium;
        [SerializeField] Progression progressionHard;

        public void StartNewGame()
        {
            PlayerInformationBetweenScenes.gameManager.UpdateInformationStartNewGame(progressionMedium);
            startNewGame.Invoke();
            StartCoroutine(StartGameByTime(timeToStartGame));
        }

        public void LoadScene() {
            PlayerInformationBetweenScenes.gameManager.progressionPrincipal = PlayerInformationBetweenScenes.gameManager.configuration.configurationGame.progressionActual;
            ControlChangeScenes.Instance.restartScene();
        }

        public void QuitGame() {
            Application.Quit();
        }

        IEnumerator StartGameByTime(float time) {
            yield return new WaitForSeconds(time);
            ControlChangeScenes.Instance.StartNewGame();
            portal.StartTransition();
        }
    }
}
