using RPG.Attributes;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Core;

namespace RPG.Ability
{
    public class AbilityE : Ability
    {
        [SerializeField] GameObject spawnAllies = null;
        [SerializeField] float shoutAreaRaise = 5f;

        [Header("Effects")]
        [SerializeField] ParticleSystem effectQSwythe = null;
        [SerializeField] int limitGenerationEnemy = 3;

        int enemyGeneration = 0;
        List<GameObject> alliesSpawn = new List<GameObject>();

        public override void OnActivatedAbility()
        {
            throw new System.NotImplementedException();
        }

        public void RaiseDead() {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutAreaRaise, Vector3.up, 0);
            alliesSpawn.Clear();
            foreach (RaycastHit hit in hits)
            {
                HealthEnemy ht = hit.collider.GetComponent<HealthEnemy>();

                //checkl
                if (ht == null) continue;
                if (limitGenerationEnemy <= enemyGeneration) {
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
        }

        public void MovementRaise() {
            if (alliesSpawn.Count == 0 || alliesSpawn == null) return;
            foreach (GameObject goAllies in alliesSpawn) {
                if (goAllies == null) continue;
                if (true) interactWithMovementJoystick(goAllies);
                //else InteractWithMovementRayCast(goAllies);
            }
        }

        public void CallThePartycle() {
            var effectQ = Instantiate(effectQSwythe.gameObject, PlayerInformationBetweenScenes.gameManager.player.GetComponent<PlayerController>().transformEffectAbility.position, Quaternion.identity);
            effectQ.GetComponent<ParticleSystem>().Play();
            Destroy(effectQ, 1.5f);
        }

        private bool InteractWithMovementRayCast(GameObject goMover)
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit, 100);
            if (hasHit && goMover.GetComponent<AIControlleAllies>().cantMove)
            {

                goMover.GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                
                return true;
            }

            return false;
        }

        void interactWithMovementJoystick(GameObject goMover) {
            goMover.GetComponent<Mover>().StartMoveAction(PlayerInformationBetweenScenes.gameManager.player.transform.forward * 5 + PlayerInformationBetweenScenes.gameManager.player.transform.position, 1f);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

    }
}
