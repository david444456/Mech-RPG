using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Stats;
using System;
using UnityEngine.Events;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] UnityEvent OnSeePlayer;
        [SerializeField] bool activatedBossEnemy = false;

        [Header("Movement and fighter")]
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float agroCoolTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 5;

        [Header("AvoidBlock")]
        [SerializeField] bool AbilitiesBlockAttack = true;

        [Header("Spawn")]
        
        [SerializeField] GameObject spawnCoinGameObject = null;
        [SerializeField] GameObject weaponActualSpawn = null;

        [Header("Effects")]
        public ParticleSystem effectTreeCut = null;


        Fighter fighter;
        GameObject player;
        MoverEnemy mover;
        Health health;
        Animator animator;
        private NavMeshAgent navMeshAgent;

        Vector3 guardLocation;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArriveAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWaypointIndex = 0;
        

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<MoverEnemy>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {
            //podria tener errores
            guardLocation = transform.position;
            if (GetComponent<BaseStats>().GetLevel() > 3) {
                print("Change velocity attack");
            }
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (IsAggrevated(player) && fighter.CanAttack(player))
            {
                //boss enemy
                if (!activatedBossEnemy)
                {
                    OnSeePlayer.Invoke();
                    activatedBossEnemy = true;
                }

                //attack general enemy
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //Suspicion State
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        private void AttackingFalse()
        {
            //cuando termina de atacar, para empezar a movewrme o algo asi

            //para optimizar
            //fighter.ChangeActiveColliderWeapon(false);
            animator.SetBool("Hit1", false);
        }

        private void AttackingTrue()
        {
            //para optimizar
            //fighter.ChangeActiveColliderWeapon(true);
            //to animation event

        }

        public void Aggrevate() {
            timeSinceAggrevated = 0;
        }

        public void spawnCoin() {
            if (spawnCoinGameObject == null) return;
            spawnCoinGameObject.SetActive(true);
            spawnCoinGameObject.transform.parent = null;
            //Instantiate(spawnCoinGameObject, transform.position, Quaternion.identity);
        }

        public void spawnWeapon()
        {
            if (weaponActualSpawn != null)
            {
                Instantiate(weaponActualSpawn, transform.position, Quaternion.identity);
            }
        }

        public void blockAttackAvoid() {
            if (!AbilitiesBlockAttack) return;
            
            switch (UnityEngine.Random.Range(0, 3)) {
                case 0:
                    animator.SetTrigger("Block");
                    print("Bloqueando");
                    break;
                case 1:
                    animator.SetTrigger("AvoidAttack");
                    break;
                case 2:
                    break;
            }
            
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArriveAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation;

            if (patrolPath != null) {
                if (AtWaypoint()) {
                    timeSinceArriveAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArriveAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition , patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());

            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {

            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {

            return patrolPath.GetWaypoint(currentWaypointIndex);
            
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceArriveAtWaypoint = 0;
            fighter.Attack(player);

            AggrevateNearByEnemies();
        }

        private void AggrevateNearByEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach (RaycastHit hit in hits) {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;

                ai.Aggrevate();
            }
        }

        private bool IsAggrevated(GameObject player)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (effectTreeCut != null && (distanceToPlayer < chaseDistance)) {
                Destroy(effectTreeCut.gameObject);
            }

            return distanceToPlayer < chaseDistance || timeSinceAggrevated < agroCoolTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
