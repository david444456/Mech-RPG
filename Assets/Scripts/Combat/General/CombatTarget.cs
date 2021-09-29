using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using RPG.Control;
using RPG.Movement;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) //this is for player, not fuct correctly
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                //callingController.GetComponent<Fighter>().Attack(gameObject);
                //animacion 360
                callingController.gameObject.transform.LookAt(gameObject.transform);
                Mover mover = callingController.GetComponent<Mover>();
                //mover.Cancel();
                mover.StartMoveAction(gameObject.transform.position, 1f);
               
            }

            return true;
        }

        
    }
}
