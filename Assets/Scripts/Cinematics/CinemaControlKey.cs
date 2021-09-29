using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics {
    public class CinemaControlKey : MonoBehaviour
    {
        [Header("Play Key")]
        [SerializeField] float timeChangeScale = 0.3f;
        [SerializeField] float timeWait = 3;
        [SerializeField] PlayableAsset[] playable;
        [SerializeField] PlayableDirector[] playableDirector;

        bool display = false;
        float timeBetweenDisplay = 0;
        int indexDisplay = 0;
        

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (display) {
                if (Input.GetButtonDown("Fire3") && timeBetweenDisplay <= timeWait)
                {
                    Time.timeScale = 1;
                    timeBetweenDisplay = 0;
                    display = false;
                }
                else if (timeBetweenDisplay >= timeWait) {
                    playableDirector[indexDisplay].playableAsset = playable[indexDisplay];
                    playableDirector[indexDisplay].Play();
                }

                timeBetweenDisplay += Time.deltaTime;
            }
        }

        public void ShowKey(int index) {

            indexDisplay = index;

            if (!display)
            {
                //show ui
                display = true;
                Time.timeScale = timeChangeScale;
            }
            else {
                playableDirector[index].playableAsset = playable[index];
            }


        }

        public void EndKey() {
            if (display) {
                playableDirector[indexDisplay].playableAsset = playable[indexDisplay];
            }
        }
    }
}
