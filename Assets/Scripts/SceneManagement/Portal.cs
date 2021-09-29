using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;
using RPG.Movement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        public enum DestinationIdentifier {
            A, B, C, D
        }

        [SerializeField] public int sceneToLoad = -1; // can't Change to public, please be carefull
        [SerializeField] Transform spawnPoint;
        [SerializeField] public DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeWaitTime = 1f;
        //[SerializeField] bool PortalMenu = false;

        [Header("Change portals")]
        [SerializeField] Vector3 positionChangePortals;
        [SerializeField] Vector3 positionChangeSpawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        public void changePositionPortals() {
            transform.position = positionChangePortals;
            spawnPoint.position = positionChangeSpawnPoint;
        }

        public void StartTransition() {
             
            StartCoroutine(Transition());
        }

        private IEnumerator Transition() // no call between other script. No llame desde otro script, por favor podría morir en el intento.
        {
            if (sceneToLoad < 0) {
                Debug.LogError("Scene no set");
                yield break;
            }


            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            
            //player
                PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                playerController.enabled = false;
            
            yield return fader.FadeOut(fadeOutTime);

            
            //wrapper is using for save 
            wrapper.Save();

            //transform.parent = fader.gameObject.transform;
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            PlayerController newplayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            if (newplayerController != null)
            {
                newplayerController.enabled = false;
            }
            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save(); // again for save the player position

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeOutTime);
            if (newplayerController != null)
            {
                newplayerController.enabled = true;
               
            }
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player= GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false; // by solution errors
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.transform.position) ;
            
            player.transform.rotation = otherPortal.spawnPoint.rotation;

            player.GetComponent<NavMeshAgent>().enabled = true;
            print(" 2" + otherPortal.name);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }

            return null;
        }
    }
}
