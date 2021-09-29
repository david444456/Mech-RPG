using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement {
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 0.2f;

        private void Awake()
        {
            //StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {
            
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>(); //call the fader to load scene better
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);

        }

        void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.P))
            {
                Load();
            }*/
            /*if (Input.GetKeyDown(KeyCode.O))
            {
                Save();
            }*/
        }
        // save the component
        public void Save() {

            GetComponent<SavingSystem>().Save(defaultSaveFile);
            
        }
        //load components
        public void Load()
        {
            //call the saving system load
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Delete() {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }

        public void loadRestartScene() {
            StartCoroutine(LoadLastScene());
        }
    }
}
