using RPG.Attributes;
using RPG.Movement;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Core;
using UnityEngine.Events;

namespace RPG.Ability
{
    public class AbilityReviveRobotsAllies : Ability
    {
        [SerializeField] ActivatedAbilityRevived activatedAbilityRevived;
        [SerializeField] GameObject spawnAllies = null;
        [SerializeField] float shoutAreaRaise = 5f;

        [Header("Effects")]
        [SerializeField] int limitGenerationEnemy = 3;

        int enemyGeneration = 0;
        List<GameObject> alliesSpawn = new List<GameObject>();

        [Serializable]
        public class ActivatedAbilityRevived : UnityEvent<List<GameObject>> {

        }

        public override void OnActivatedAbility()
        {
            RaiseDead();
        }

        void RaiseDead()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutAreaRaise, Vector3.up, 0);
            alliesSpawn.Clear();
            foreach (RaycastHit hit in hits)
            {
                HealthEnemy ht = hit.collider.GetComponent<HealthEnemy>();

                //checkl
                if (ht == null) continue;
                if (limitGenerationEnemy <= enemyGeneration)
                {
                    enemyGeneration = 0;
                    return;
                }
                if (!ht.IsDead()) continue;
                if (tag == ht.gameObject.tag) continue;
                if (ht.isUsedForReviving) continue;


                // not  to used it anymore
                ht.isUsedForReviving = true;

                //spawn and eliminated object
                enemyGeneration++;
                var Spawn = Instantiate(spawnAllies, hit.transform.position, Quaternion.identity);
                alliesSpawn.Add(Spawn);
                Destroy(ht.enemyObjectAnimatorGeneral, 0.5f);
                print(" " + ht.gameObject.name);
            }

            //activated event
            activatedAbilityRevived.Invoke(alliesSpawn);
        }
    }
}
