using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using RPG.Control;
using UnityEngine.Video;

namespace RPG.Core {
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] VideoPlayer videoPlayer;
        [SerializeField] GameObject GODesactiveVideo;
        [SerializeField] GameObject GODesactiveCredits;
        [SerializeField] float timeToDesactiveVideo = 15f;
        [SerializeField] float timeToDesactiveCredits = 15f;

        public void WinLevel() {
            videoPlayer.Play();
            StartCoroutine(DesactiveVideo());
            print("Win");
        }

        IEnumerator DesactiveVideo() {
            yield return new WaitForSeconds(timeToDesactiveVideo);
            GODesactiveVideo.SetActive(false);
            yield return new WaitForSeconds(timeToDesactiveCredits);
            GODesactiveCredits.SetActive(false);
        }

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

        public void LoadGame()
        {
            ControlChangeScenes.Instance.LoadGame();
        }

        public void SaveGame() {

            ControlChangeScenes.Instance.SaveGame();
        }
    }
}
