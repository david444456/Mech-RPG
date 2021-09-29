using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
using RPG.Stats;

namespace RPG.SceneManagement {
    public class MenuControlCanvas : MonoBehaviour
    {
        [SerializeField] Portal portal;
        [SerializeField] ConfigurationGame configurationGame;

        [Header("Progression")]
        [SerializeField] Progression progressionEasy;
        [SerializeField] Progression progressionMedium;
        [SerializeField] Progression progressionHard;

        public void StartNewGame()
        {
            PlayerInformationBetweenScenes.gameManager.UpdateInformationStartNewGame(progressionMedium);
            ControlChangeScenes.Instance.StartNewGame();
            portal.StartTransition();
        }

        public void LoadScene() {
            PlayerInformationBetweenScenes.gameManager.progressionPrincipal = PlayerInformationBetweenScenes.gameManager.configuration.configurationGame.progressionActual;
            ControlChangeScenes.Instance.restartScene();
        }
    }
}
