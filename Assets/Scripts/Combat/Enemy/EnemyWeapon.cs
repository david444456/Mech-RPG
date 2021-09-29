using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat {
    public class EnemyWeapon : MonoBehaviour
    {
        [SerializeField] GameObject enemyInstigator = null;
        [SerializeField] int damageEnemy = 20;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnTriggerEnter(Collider collision)
        {
            print("colission");
            if (collision.gameObject.tag == "Player") {
                collision.gameObject.GetComponent<Health>().TakeDamage(enemyInstigator, damageEnemy);
            }
        }
    }
}
