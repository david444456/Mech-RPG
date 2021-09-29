
using UnityEngine;
using RPG.Stats;

namespace RPG.Core
{
    public class PlayerInformationBetweenScenes : MonoBehaviour
    {
        public static PlayerInformationBetweenScenes gameManager;

        [HideInInspector] public GameObject player;
        public Progression progressionPrincipal;

        [Header("Change move")]
        [SerializeField] public Configuration configuration;

        [Header("More")]
        [SerializeField] public Abilities abilities;

        void Awake() {
            if (gameManager == null)
            {
                gameManager = this;
            }
        }

        void Start()
        {
           // UpdateThisWeapon();
            UpdatePlayer(null);

            //fps
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;
        }

        public void UpdatePlayer(GameObject gameObjectPlayer)
        {
            if (gameObjectPlayer == null)
                player = GameObject.FindWithTag("Player");
            else
                player = gameObjectPlayer;
        }

        public void UpdateInformationStartNewGame(Progression progression)
        {
            configuration.configurationGame.progressionActual = progression;
            progressionPrincipal = progression;
        }

        public bool returnAbilities(int indexAbility)
        {
            return abilities.AbilitiesPlayer[indexAbility];

        }



    }
}
