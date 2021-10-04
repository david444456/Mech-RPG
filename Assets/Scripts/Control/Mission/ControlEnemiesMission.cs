using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class ControlEnemiesMission : MonoBehaviour
    {
        [SerializeField] GameObject _gameObjectEnemiesPacified;
        [SerializeField] GameObject _GOEnemiesAttacking;

        public void ActiveEnemies() {
            _gameObjectEnemiesPacified.SetActive(false);
            _GOEnemiesAttacking.SetActive(true);
        }
    }
}
