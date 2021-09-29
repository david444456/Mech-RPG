using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
using RPG.Stats;

namespace RPG.SceneManagement {
    public class MenuControlCanvas : MonoBehaviour
    {
        [SerializeField] Portal portal;
        [SerializeField] Animator animatorPlayer;
        [SerializeField] GameObject zonaA;
        [SerializeField] GameObject options;
        [SerializeField] GameObject difficult;
        [SerializeField] ConfigurationGame configurationGame;

        [Header("UI")]
        [SerializeField] RectTransform arrow;
        [SerializeField] RectTransform[] zonaATransform;
        [SerializeField] RectTransform[] zonaBTransform;
        [SerializeField] RectTransform[] zonaCTransform;

        [SerializeField] Image imageClick;
        [SerializeField] Image imageClickJoystick;
        [SerializeField] Sprite spriteclick;

        [Header("Progression")]
        [SerializeField] Progression progressionEasy;
        [SerializeField] Progression progressionMedium;
        [SerializeField] Progression progressionHard;

        bool moveWithUPDOWN = true;
        bool useJoystick;
        float timeToMove = 0;
        int indexActual = 0;

        private void Start()
        {
            useJoystick = configurationGame.useTheJoystick;
            moveWithUPDOWN = configurationGame.moveWithUPDOWN;

            //change images sprite
            if (!useJoystick) imageClickJoystick.sprite = null;
            else imageClickJoystick.sprite = spriteclick;

            if (!moveWithUPDOWN) imageClick.sprite = null;
            else imageClick.sprite = spriteclick;
        }

        private void Update()
        {
            timeToMove += Time.deltaTime;
            if (timeToMove <= 0.2f) return;

            if (zonaA.activeSelf) {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire3"))
                {
                    confirmThisSettingA();
                    return;
                }

                //change arrow
                if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("Vertical") > 0.8f))
                {
                    if (indexActual == 0) return;
                    arrow.anchoredPosition = zonaATransform[indexActual - 1].anchoredPosition;
                    indexActual--;
                    timeToMove = 0;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("Vertical") < -0.8f))
                {
                    if (indexActual == 2) return;
                    arrow.anchoredPosition = zonaATransform[indexActual + 1].anchoredPosition;
                    indexActual++;
                    timeToMove = 0;
                }
            }
            else if (options.activeSelf) {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire3")) {
                    confirmB();
                    return;
                }

                //change arrow
                if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetAxis("Vertical") > 0.8f))
                {
                    if (indexActual == 0) return;
                    arrow.anchoredPosition = zonaBTransform[indexActual - 1].anchoredPosition;
                    indexActual--;
                    timeToMove = 0;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetAxis("Vertical") < -0.8f))
                {
                    if (indexActual == 2) return;
                    arrow.anchoredPosition = zonaBTransform[indexActual + 1].anchoredPosition;
                    indexActual++;
                    timeToMove = 0;
                }
            } else if (difficult.activeSelf) {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire3"))
                {
                    confirmC();
                    return;
                }

                //change arrow
                if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetAxis("Horizontal") < -0.8f))
                {
                    if (indexActual == 0) return;
                    arrow.anchoredPosition = zonaCTransform[indexActual - 1].anchoredPosition + new Vector2(440, 0);
                    indexActual--;
                    timeToMove = 0;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetAxis("Horizontal") > 0.8f))
                {
                    if (indexActual == 3) return;
                    arrow.anchoredPosition = zonaCTransform[indexActual + 1].anchoredPosition + new Vector2(440,0);
                    indexActual++;
                    timeToMove = 0;
                }
            }
            
        }

        private void activeOrDesactive(bool activeSystem) {
            zonaA.SetActive(activeSystem);
            options.SetActive(!activeSystem);
            indexActual = 0;

            if (activeSystem)
            {
                arrow.anchoredPosition = zonaATransform[0].anchoredPosition;
                animatorPlayer.Play("Locomotion", -1);
            }
            else
            {
                arrow.anchoredPosition = zonaBTransform[0].anchoredPosition;
                animatorPlayer.Play("Attack", -1);
            }
        }

        private void confirmThisSettingA() {
            switch (indexActual)
            {
                case 0:
                    PlayerInformationBetweenScenes.gameManager.progressionPrincipal = PlayerInformationBetweenScenes.gameManager.configuration.configurationGame.progressionActual;
                    ControlChangeScenes.Instance.restartScene();
                    break;
                case 1:
                    difficult.SetActive(true);
                    zonaA.SetActive(false);
                    indexActual = 0;
                    arrow.anchoredPosition = zonaCTransform[0].anchoredPosition + new Vector2(440, 0);
                    animatorPlayer.Play("Jumping", -1);
                    break;
                case 2:
                    activeOrDesactive(false);
                    break;

            }

        }

        private void confirmB() {

            switch (indexActual) {
                case 0:
                    //images
                    if (moveWithUPDOWN) imageClick.sprite = null;
                    else imageClick.sprite = spriteclick;

                    //config
                    moveWithUPDOWN = !moveWithUPDOWN;
                    configurationGame.moveWithUPDOWN = moveWithUPDOWN;

                    //only one
                    if (moveWithUPDOWN && useJoystick)
                    {
                        imageClickJoystick.sprite = null;
                        useJoystick = false;
                        configurationGame.useTheJoystick = useJoystick;
                    } else if (!moveWithUPDOWN && !useJoystick) {
                        useJoystick = true;
                        configurationGame.useTheJoystick = useJoystick;
                        imageClickJoystick.sprite = spriteclick;
                    }
                    break;
                case 1:
                    //images
                    if (useJoystick) imageClickJoystick.sprite = null;
                    else imageClickJoystick.sprite = spriteclick;

                    //configuration
                    useJoystick = !useJoystick;
                    configurationGame.useTheJoystick = useJoystick;

                    //only one
                    if (moveWithUPDOWN && useJoystick) {
                        imageClick.sprite = null;
                        moveWithUPDOWN = false;
                        configurationGame.moveWithUPDOWN = moveWithUPDOWN;
                    }
                    else if (!moveWithUPDOWN && !useJoystick)
                    {
                        moveWithUPDOWN = true;
                        configurationGame.moveWithUPDOWN = moveWithUPDOWN;
                        imageClick.sprite = spriteclick;
                    }

                    break;
                case 2:
                    print("Active chau");
                    activeOrDesactive(true);
                    break;

            }
        }

        private void confirmC()
        {
            switch (indexActual)
            {
                case 0:
                    print("nivel facil");
                    ConfirmProgressionManager(progressionEasy);
                    break;
                case 1:
                    ConfirmProgressionManager(progressionMedium);
                    break;
                case 2:
                    print("nivel hard");
                    ConfirmProgressionManager(progressionHard);
                    break;
                case 3:
                    difficult.SetActive(false);
                    zonaA.SetActive(true);
                    indexActual = 0;
                    arrow.anchoredPosition = zonaATransform[0].anchoredPosition;
                    animatorPlayer.Play("Locomotion", -1);
                    break;
                

            }

        }

        private void ConfirmProgressionManager(Progression progression) {
            PlayerInformationBetweenScenes.gameManager.UpdateInformationStartNewGame(progression);
            ControlChangeScenes.Instance.StartNewGame();
            portal.StartTransition();
        }
    }
}
