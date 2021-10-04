using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control
{
    public class ControlEnemiesMission : MonoBehaviour
    {
        [SerializeField] UnityEvent eventAllEnemiesDied;
        [SerializeField] int _countEnemies = 10;

        [Header("Start with enemies")]
        [SerializeField] GameObject _gameObjectEnemiesPacified;
        [SerializeField] GameObject _GOEnemiesAttacking;

        private int internalCountEnemiesDied = 0;

        public void ActiveEnemies() {
            _gameObjectEnemiesPacified.SetActive(false);
            _GOEnemiesAttacking.SetActive(true);
        }

        public void NewEnemyDied() {
            internalCountEnemiesDied++;
            if (internalCountEnemiesDied >= _countEnemies) {
                eventAllEnemiesDied.Invoke();
            }
        }

    }
}
