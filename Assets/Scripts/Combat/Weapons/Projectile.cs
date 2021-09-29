using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour
    {

        public WeaponConfig weaponConfig;
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 7f;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] bool damagePostDie = false;

        [Header("Effect")]
        [SerializeField] ParticleSystem particleSystemToPlay;
        [SerializeField] GameObject DestroyBullet;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] UnityEvent onHit;

        Vector3 direction;
        Health targetHealth;
        GameObject instigator = null;
        float damage = 0;

        bool activeSystemPartycle;

        void Update()
        {
            //if (target == null) return;


            if (Vector3.Distance(transform.position, direction) < 1)
            {
                if (!activeSystemPartycle)
                {
                    activeSystemPartycle = true;
                    ActiveEffect();
                }
            }
            else
            {
                transform.LookAt(direction);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Calcule damage, direction and instigator
        /// </summary>

        public void SetTarget(Vector3 direction, GameObject instigator, float damage)
        {
            this.direction = direction;
            this.damage = damage;
            this.instigator = instigator;
            //Invoke("ActiveEffect", 1);
            Destroy(gameObject, maxLifeTime);
        }

        void ActiveEffect()
        {
            if (particleSystemToPlay != null)
            {
                //Vector3 targetDirection;
                DestroyBullet.SetActive(false);
                particleSystemToPlay.Play();
                onHit.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            try
            {
                targetHealth = other.GetComponent<Health>();
            }
            catch
            {
                return;
            }
            if (targetHealth == null) return;
            if (targetHealth.IsDead()) return;
            if (instigator == targetHealth.gameObject) return;
            targetHealth.TakeDamage(instigator, damage);

            speed = 0;
            ActiveEffect();


            ActiveEffect();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, other.transform.position, Quaternion.identity);
            }
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }


        bool RaycastNavMesh(out Vector3 targetDirection)
        {

            targetDirection = new Vector3();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hasHit = Physics.Raycast(ray, out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, 0.8f, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            targetDirection = navMeshHit.position;
            return true;
        }


    }//end class
}//end namespace
