using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
    public class PersistentObjectCore : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;
        public int targetFrameRate = 60;
        static bool hasSpawned = false;

        void Start()
        {

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFrameRate;

        }

        // Update is called once per frame
        void Awake()
        {
            if (hasSpawned) return;
            SpawnPersistentObject();

            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);

            DontDestroyOnLoad(persistentObject);
        }
    }
}
