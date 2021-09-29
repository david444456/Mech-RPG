using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using UnityEngine.UI;
using RPG.Attributes;
using System;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Movement
{
    public class MoverPlayer : Mover
    {
        #region Inspector variables

        [Header("Player var")]
        [SerializeField] float _multiplyDeaceleration = 0.8f;
        [SerializeField] float rotationSpeed = 0.2f;
        [SerializeField] float speedMultiplicator = 56;

        [Header("Scene")]
        [SerializeField] Transform transformCamera;

        #endregion

        #region Private var
        Health health;

        bool m_stopMove = false;

        bool animationInProcess = false;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        private Vector2 _lastMoveDirection = Vector2.zero;

        #endregion

        void Start()
        {
            health = GetComponent<Health>();
        }

        public override void Update()
        {
            if(health.IsDead()) navMeshAgent.enabled = health.IsDead();
        }

        public bool InteractWithMovement()
        {
            bool returnValueMethod = false;
            //move
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += transformCamera.forward;
                returnValueMethod = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection -= transformCamera.forward;
                returnValueMethod = true;
                // mover.StartMoveAction(moveDirection * 0.8f + transform.position, 1f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += transformCamera.right;
                returnValueMethod = true;
                //mover.StartMoveAction(moveDirection * 0.8f + transform.position, 1f);
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection -= transformCamera.right;
                returnValueMethod = true;
                //mover.StartMoveAction(moveDirection * 0.8f + transform.position, 1f);
            }

            //calcule aceleration
            Vector2 newAceleration = new Vector2(
                Math.Abs(moveDirection.x - _lastMoveDirection.x),
                Math.Abs(moveDirection.z - _lastMoveDirection.y));

            playerController.AcelerationMovement = (-1)*(newAceleration.x + newAceleration.y)*_multiplyDeaceleration;
            _lastMoveDirection = new Vector2(moveDirection.x, moveDirection.z);

            //move with moveDirection
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            moveDirection = moveDirection.normalized + transform.position;

            if (moveDirection != Vector3.zero)
            {
                StartMoveAction(moveDirection, 1f);
            }
            else
                animator.SetFloat("forwardSpeed", 0);

            //reset values
            moveDirection = new Vector3(0, 0, 0);

            return returnValueMethod;
        }

        public override void MoveTo(Vector3 destination, float maxSpeed, float SpeedFraction)
        {
            //base.MoveTo(destination, maxSpeed, SpeedFraction); //use for move with navMesh

            //transformBeforeTheUpdate = transform.position;
            if (m_stopMove) return;
            if (destination.x == 0 || destination.y == 0) return;


            Vector3 movement = new Vector3(destination.x - transform.position.x, 0, destination.z - transform.position.z);

            if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), rotationSpeed);
            }

            transform.Translate(
                movement * playerController.AcelerationMovement * speedMoveNormal * Time.deltaTime,
                Space.World);

            //animator
            animator.SetFloat("forwardSpeed", movement.magnitude * playerController.AcelerationMovement);
        }

        public override bool CanMoveTo(Vector3 destination)
        {
            if (animationInProcess) return false;
            return base.CanMoveTo(destination);
        }

        public void MoveRotateToMousePosition()
        {
            //get the mouse position
            Vector3 worldPosition = GetMouseWorldPosition();

            //normalize this direction, substract with my actual position
            Vector3 direction = Vector3.Normalize(worldPosition - transform.position);
            direction = new Vector3(direction.x, 0 , direction.z);

            //look at this place
            if (direction != Vector3.zero)
            {
                RotatePlayerToPosition(direction);
            }
        }

        #region private function

        private void RotatePlayerToPosition(Vector3 direction) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1);
        }

        private Vector3 GetMouseWorldPosition() {
            Ray ray = GetMouseRay();
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100)) {
                return raycastHit.point;
            }
            return transform.position;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        IEnumerator EndedMove(Vector3 positionValue, Transform transformToLookAt)
        {
            //wait for a few seconds
            yield return new WaitForSeconds(0.1f);

            //calculate distance between player and positionPlayer
            if (Vector3.Distance(transform.position, positionValue) <= 0.5f)
            {
                transform.LookAt(transformToLookAt.transform.position);
            }
            else
            {
                navMeshAgent.enabled = false;
                transform.position = positionValue;
                navMeshAgent.enabled = true;
                StartCoroutine(EndedMove(positionValue, transformToLookAt));
            }
        }

        #endregion
    }
}
