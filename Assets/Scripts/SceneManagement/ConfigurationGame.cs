using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;


namespace RPG.SceneManagement {
    [CreateAssetMenu(fileName = "Config", menuName = "Conver/ Make new Config", order = 0)]
    public class ConfigurationGame : ScriptableObject
    {
        [SerializeField] public bool moveWithUPDOWN;
        [SerializeField] public bool useTheJoystick;
        public DifficultyGame difficultyGame;
        public Progression progressionActual;

        [Header("Images")]
        public Sprite spriteFJoystick;
        public Sprite spriteEJoystick;
        public Sprite spriteRJoystick;
        public Sprite fire1;
        public Sprite fire2;

        // Start is called before the first frame update
        void Start()
        {

        }

    }

    public enum DifficultyGame {
        Easy,
        Normal, 
        Hard
    }
}
