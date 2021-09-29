using RPG.Combat;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement
{
    public class MoverEnemy : Mover
    {

        private void Start()
        {
            Fighter fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        public override void Update()
        {
            UpdateAnimator();
        }

        public override bool CanMoveTo(Vector3 destination)
        {
            return base.CanMoveTo(destination);
        }

        void UpdateAnimator()
        {
            Vector3 velocity;
            velocity = navMeshAgent.velocity;
            Vector3 localVelocit = transform.InverseTransformDirection(velocity);
            float speed = localVelocit.z;
            animator.SetFloat(stringAnimatorForwardSpeed, speed);
        }

    }
}
