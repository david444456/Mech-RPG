using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat {
    public class Weapon : MonoBehaviour
    {
        public WeaponConfig weaponConfig;
        public BoxCollider boxCollider;

        [SerializeField] UnityEvent onHit;
        [Header("Ability")]
        [SerializeField] UnityEvent onHitE;
        [SerializeField] UnityEvent onHitR;
        [SerializeField] UnityEvent onHitQ;

        public void OnHit()
        {
            onHit.Invoke();
        }


        public void OnHitE()
        {
            onHitE.Invoke();
        }

        public void OnHitR()
        {
            onHitR.Invoke();
        }

        public void OnHitQ()
        {
            onHitQ.Invoke();
        }
    }
}
