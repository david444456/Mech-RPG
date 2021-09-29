using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;
using RPG.Movement;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Cinematics
{
    public class CinematicsControlRemove : MonoBehaviour
    {
        GameObject player;
        List<AIController> listEnemys = new List<AIController>();

        private void Awake()
        {
             player = GameObject.FindWithTag("Player");

        }

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        public void DisableControl(PlayableDirector pd) {
            GameObject player = GameObject.FindWithTag("Player");

            //player
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            changeControlPlayer(false);


            var enemys = FindObjectsOfType<AIController>();
            foreach (AIController goEnemy in enemys) {
                listEnemys.Add(goEnemy);
                goEnemy.enabled = false;
                goEnemy.GetComponent<Mover>().enabled = false;
                goEnemy.GetComponent<Fighter>().enabled = false;
            }
        }


        void EnableControl(PlayableDirector pd) {
            changeControlPlayer(true);
            foreach (AIController goEnemy in listEnemys)
            {
                goEnemy.enabled = true;
                goEnemy.GetComponent<Mover>().enabled = true;
                goEnemy.GetComponent<Fighter>().enabled = true;
            }
            listEnemys.Clear();
        }

        void changeControlPlayer(bool controlBool) {
            player.GetComponent<PlayerController>().enabled = controlBool;
           
            player.GetComponent<CapsuleCollider>().enabled = controlBool;
        }

    }
}
