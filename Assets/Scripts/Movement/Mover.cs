using System;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Movement
{
    //[RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Health))]
    public abstract class Mover : MonoBehaviour, IAction, ISaveable
    {
        #region Inspector variables

        [SerializeField] public float speedMoveNormal = 2;

        [Header("NavMeshAgent")]
        [SerializeField] Transform target;
        [SerializeField] public float maxSpeed = 6f;
        [SerializeField] protected float maxAceleration = 50;
        [SerializeField] float maxNavPathLenght = 40f;

        #endregion

        #region private variables
        [HideInInspector] public string stringAnimatorForwardSpeed = "forwardSpeed";

        Ray lastRay;

        [HideInInspector] public Animator animator;
        Rigidbody rb;
        public NavMeshAgent navMeshAgent;

        [HideInInspector] public PlayerController playerController;
        #endregion

        #region Unity function
        void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerController = GetComponent<PlayerController>();
        }


        public virtual void Update()
        {
        }
        #endregion

        #region public function

        public void StartMoveAction(Vector3 destination, float speedFraction) {
            GetComponent<ActionScheduler>().StartAction(this);
            //if (!CanMoveTo(destination)) return;
            MoveTo(destination, maxSpeed, speedFraction);
        }

        public virtual bool CanMoveTo(Vector3 destination) {
            //if(!m_usedNavMeshMovement) return true;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;

            //modificate the destination
            if (GetPathLength(path) > maxNavPathLenght) return false;

            return true;
        }

       /* public bool canMoveOnlyOneDirection(ref Vector3 destination) {
            Vector3 newDirection = destination;
            NavMeshPath path = new NavMeshPath();
            print(destination);
            if (NavMesh.CalculatePath(transform.position, new Vector3(transform.position.x, 0, newDirection.z), NavMesh.AllAreas, path))
            {
                if (GetPathLength(path) < maxNavPathLenght) destination = new Vector3(transform.position.x, 0, newDirection.z);
                else if (NavMesh.CalculatePath(transform.position, new Vector3(newDirection.x, 0, transform.position.z), NavMesh.AllAreas, path))
                {
                    if (GetPathLength(path) < maxNavPathLenght) destination = new Vector3(newDirection.x, 0, 0);
                    else return false;
                }
            }    
            else
                return false;

            print(GetPathLength(path) + " " + maxNavPathLenght);
            return true;
        }*/

        public int maxDistanceColliderWalk= 100;

        /// <summary>
        /// Move the player with navMeshAgent to destination 
        /// </summary>
        public virtual void MoveTo(Vector3 destination, float maxSpeed, float SpeedFraction)
        {

            //navmeshMovement
            var tfPosition = transform.position;
            if (maxSpeed > 50)
            {
                navMeshAgent.acceleration = maxSpeed;
            }
            else
            {
                navMeshAgent.acceleration = maxAceleration;
            }
            //GetComponent<Fighter>().StopAttack();
            try
            {
                navMeshAgent.destination = destination;
            }
            catch (Exception e)
            {
                navMeshAgent.destination = tfPosition;
                print(e);
            }

            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(SpeedFraction);
            navMeshAgent.isStopped = false;
            
        }

        public void Cancel() {
            if (navMeshAgent.enabled == true)
            {
                navMeshAgent.isStopped = true;
            }
        }

        #endregion

        #region private function

        float GetPathLength(NavMeshPath path) {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++) {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }

        #endregion

        #region Save function
        public object CaptureState()
        {

            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            navMeshAgent.enabled = false;
            transform.position =  position.ToVector();

            navMeshAgent.enabled = true;
        }
        #endregion
    }

    public interface SetDifferentAnimations
    {
       void SetAnimationController();
    }
}
