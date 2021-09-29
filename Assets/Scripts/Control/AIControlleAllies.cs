using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIControlleAllies : MonoBehaviour
    {
        public WeaponConfig currentWeaponConfig = null;
        public bool cantMove = true;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform LeftHandTransform = null;

        [SerializeField] float damage = 4f;
        [SerializeField] float timeBetweenAttack = 1f;


        [SerializeField] float shoutDistance = 5;

        [Header("Rise")]
        [SerializeField] ParticleSystem particleSystemRise;
        [SerializeField] float timeLifeAllies = 4f;

        float TimeSinceLastAttack = Mathf.Infinity;

        Fighter fighter;
        Mover mover;
        Health health;
        Animator animator;
        GameObject target;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        void Start()
        {
            animator.Play("Spawn", -1);
            Invoke("AnimationDied", timeLifeAllies);
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (!cantMove) return;
            if (AggrevateNearByEnemies())
            {
                if (!GetIsInRange(target.transform))
                {
                    mover.MoveTo(target.transform.position, 6, 0.8f);
                }
                else
                {
                    mover.Cancel();
                    AttackBehaviour();
                }
            }
            else
            {
                print("Sin enemigos");
            }
            TimeSinceLastAttack += Time.deltaTime;
        }

        IEnumerator animatorChangeTransform()
        {
            while (gameObject.activeSelf)
            {
                yield return new WaitForSeconds(0.05f);
                transform.position -= new Vector3(0, 0.2f, 0);
                transform.localScale -= new Vector3(0.01f, 0.01f, .01f);
            }
        }

        private bool AggrevateNearByEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                target = ai.gameObject;
                if (CanAttack(ai.gameObject)) return true;
                continue;
            }
            return false;
        }

        private bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!GetComponent<MoverEnemy>().CanMoveTo(combatTarget.transform.position)
                && !GetIsInRange(combatTarget.transform)
                ) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            print(targetToTest != null && !targetToTest.IsDead());
            return targetToTest != null && !targetToTest.IsDead();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }

        void AnimationDied()
        {
            cantMove = false;
            mover.Cancel();
            animator.Play("Death0", -1);
        }

        void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        void CantMoveTrue()
        {
            cantMove = true;

            currentWeaponConfig.Spawn(rightHandTransform, LeftHandTransform, animator);
        }

        void AnimationDiedScale()
        {
            particleSystemRise.Play();
            StartCoroutine(animatorChangeTransform());
        }

        void AttackingFalse()
        {
            //cuando termina de atacar, para empezar a movewrme o algo asi
        }

        private void AttackBehaviour()
        {
            //
            if (TimeSinceLastAttack >= timeBetweenAttack)
            {
                transform.LookAt(target.transform.position);
                TriggetAttack();
                TimeSinceLastAttack = 0;
            }
        }

        void Hit()
        {
            Health ht = target.GetComponent<Health>();
            if (ht == null) return;
            if (ht.IsDead()) return;

            ht.TakeDamage(this.gameObject, damage);
        }

        private void TriggetAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        

    }
}
