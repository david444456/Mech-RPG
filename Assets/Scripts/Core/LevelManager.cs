using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using RPG.Control;

namespace RPG.Core {
    public class LevelManager : MonoBehaviour
    {

        [SerializeField] int intLevel = 0;

        [Header("Events")]
        [SerializeField] UnityEvent Level1;
        [SerializeField] UnityEvent Level2;
        [SerializeField] UnityEvent Level3;

        int actualMision;

        void Start()
        {

        }

        public void RestartScenes() {
            ControlChangeScenes.Instance.restartScene();
            print("restart");
        }
    }
}
