using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using RPG.Control;

namespace RPG.Core {
    public class LevelManager : MonoBehaviour
    {

        public void RestartScenes() {
            ControlChangeScenes.Instance.restartScene();
            print("restart");
        }

        public void QuitGame()
        {
            ControlChangeScenes.Instance.QuitGame();
        }

        public void ReturnToMainMenu() {
            ControlChangeScenes.Instance.ReturnToMainMenu();
        }

        public void SaveGame() {

            ControlChangeScenes.Instance.SaveNewGame();
        }
    }
}
