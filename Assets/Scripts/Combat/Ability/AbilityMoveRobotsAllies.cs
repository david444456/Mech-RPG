using RPG.Attributes;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Core;

namespace RPG.Ability
{
    public class AbilityMoveRobotsAllies : Ability
    {
        List<GameObject> alliesSpawn = new List<GameObject>();

        public override void OnActivatedAbility()
        {
            MovementRaise();
        }

        public void SetTheAlliesSpawn(List<GameObject> newList) {
            alliesSpawn.Clear();
            foreach (var i in newList) {
                print(i.name);
            }
            alliesSpawn = newList;
        }

        private void MovementRaise()
        {
            if (alliesSpawn.Count == 0 || alliesSpawn == null) return;
            foreach (GameObject goAllies in alliesSpawn)
            {
                if (goAllies == null) continue;
                if(true) interactWithMovementJoystick(goAllies);
                else InteractWithMovementRayCast(goAllies);
            }
        }

        private bool InteractWithMovementRayCast(GameObject goMover)
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit, 100);
            if (hasHit && goMover.GetComponent<AIControlleAllies>().cantMove)
            {

                goMover.GetComponent<MoverEnemy>().StartMoveAction(hit.point, 1f);

                return true;
            }

            return false;
        }

        void interactWithMovementJoystick(GameObject goMover)
        {
            print(goMover.name);
            goMover.GetComponent<MoverEnemy>().StartMoveAction(PlayerInformationBetweenScenes.gameManager.player.transform.forward * 5 + PlayerInformationBetweenScenes.gameManager.player.transform.position, 1f);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
