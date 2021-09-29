using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Movement;

namespace RPG.Combat {
    public class BossEnemy : MonoBehaviour
    {
        [SerializeField] float timeToAttack = 4f;
        [SerializeField] ParticleSystem[] particleSystems;
        [SerializeField] ParticleSystem[] secondaryEffect;

        [Header("Giant")]
        [SerializeField] float positionDistanceBetweenNear = 3;
        [SerializeField] float positionDistanceBetweenDistance = 9;
        [SerializeField] bool verificatedDistanceByEveryAttack;
        [SerializeField] GameObject Player;

        [Header("Different Attacks")]
        [SerializeField] Transform tranformLauchProyectile;

        [Header("change position")]
        [SerializeField] GameObject robotAfter = null;
        
        Animator animator;
        Fighter fighter;
        Mover mover;

        Vector3 direction;
        float speedMoveScript = 0;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();

            speedMoveScript = mover.maxSpeed;
        }

        public void StartCouriRandomAttack() {
            StartCoroutine(randomAttack());
        }

        public void AttackingTrueBoss() => ChangeAttackFighter(true);

        public void AttackingFalseBoss()
        {
            ChangeAttackFighter(false);
            fighter.Cancel();
        }

        private void ChangeAttackFighter(bool changeValue)
        {
            fighter.CantMove = changeValue;
            fighter.EnemyBossAttack = changeValue;
            fighter.TimeSinceLastAttack = 0;
        }

        private IEnumerator randomAttack() 
        {
            yield return new WaitForSeconds(timeToAttack);
            switch (Random.Range(0, 3)) {
                case 0:
                    AttackNormal();
                    break;
                case 1:
                    AttackDistance();
                    break;
                case 2:
                    AttackNear();
                    break;
            }
            fighter.CantMove = true;

            StartCoroutine(randomAttack());
        }

        private void AttackDistance() {
            if (verificatedDistanceByEveryAttack) {
                float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
                if (distanceToPlayer < positionDistanceBetweenNear)
                {
                    mover.maxSpeed = speedMoveScript;
                    animator.SetTrigger("AttackNear");
                    return;
                }
                else if(distanceToPlayer > positionDistanceBetweenDistance) {
                    mover.maxSpeed = 12;
                    return;
                }
            }
            animator.SetTrigger("AttackDistance");
        }

        private void AttackNear()
        {
            if (verificatedDistanceByEveryAttack)
                if (Vector3.Distance(transform.position, Player.transform.position) > positionDistanceBetweenNear)
                {
                    animator.SetTrigger("AttackDistance");
                    return;
                }

            mover.maxSpeed = speedMoveScript;
            animator.SetTrigger("AttackNear");
        }

        private void AttackNormal()
        {
            animator.SetTrigger("AttackNormal");
        }

        private void spawnEffectPartycles(int index) {
            particleSystems[index].Play();
            if (secondaryEffect[index] != null) secondaryEffect[index].Play();
        }

        private void LauchProyectile(int power) {
            //calculate distance between player and enemy
            float potencyReal = Vector3.Distance(transform.position, Player.transform.position);

            //potency
            if (potencyReal > 4) potencyReal += power;
            else potencyReal += 1;

            //direction bullet
            direction = transform.forward * potencyReal + transform.position;
            direction = new Vector3(direction.x, 0, direction.z);

            //launch
            fighter.currentWeaponConfig.LauchProjectile(tranformLauchProyectile, tranformLauchProyectile, direction, gameObject, fighter.currentWeaponConfig.damageArea);
        }

    }
}
