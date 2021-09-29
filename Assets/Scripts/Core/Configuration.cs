using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.SceneManagement;

namespace RPG.Core {
    public class Configuration : MonoBehaviour
    {
        [SerializeField] public bool[] move;
        [SerializeField] public ConfigurationGame configurationGame;

    }
}
