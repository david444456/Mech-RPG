using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using RPG.SceneManagement;
using System;
using RPG.Cinematics;
using RPG.Stats;
using RPG.Inventory;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        public bool usedNavMeshMovement = false;

        [Tooltip("Using in game, not HideInInspector to do internal testing. Not move")]
        public bool CantMoveDirection = false;

        #region private var in inspector (tranformEffectAbility is public)

        [SerializeField] float radiusRaycast = 1f;
        [SerializeField] Transform tfArea;

        [Header("Abilities")]
        [SerializeField] public Transform transformEffectAbility;

        [Header("Change move")]
        [SerializeField] public bool moveWithUPDOWN;
        [SerializeField] Transform transformCamera;
        [SerializeField] float valueRotationCamera = 0.5f;
        [SerializeField] float velocityFront = 2f;

        [Header("Level")]
        [SerializeField] Portal portal;
        [SerializeField] CinematicsTrigger playableDirector;
        [SerializeField] GameObject cameraMap = null;
        [SerializeField] GameObject startMenu = null;
        [SerializeField] bool useMap = false;

        #endregion

        #region private var

        //inspector
        Health health;
        Animator animator;
        FighterPlayer fighter;
        MoverPlayer mover;
        PlayerUI playerUI;
        PlayerInventory playerInventory;

        //delete
        RaycastHit[] hits;
        private bool InCombo;
        private float _acelerationByPlayer = 1;

        [Serializable]
        struct CursoMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField] CursoMapping[] cursoMapping = null;

        #endregion

        #region Unity Function

        private void Awake()
        {
            mover = GetComponent<MoverPlayer>();
            fighter = GetComponent<FighterPlayer>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            playerUI = GetComponent<PlayerUI>();
            playerInventory = GetComponent<PlayerInventory>();
        }

        private void Start()
        {
            //cinemachine
            if (playableDirector != null)
            {
                playableDirector.PlayTimeline();
            }
        }

        private void FixedUpdate()
        {
            
        }

        void Update()
        {
            if (health.IsDead()) return;

            //if (useMap) if (InteractWithMap()) return;
            //if (InteractWithUI()) return;

            //if (InteractWithPray()) return;
            //if( fighter.InteractWithSelectWeapon()) AttackingFalse();

            if (playerInventory.InteractWithInventory()) return;
            if (CantMoveDirection) return; //this is for animator (pray, takeItem)

            if (fighter.IntectWithCombat()) return; 

        }

        private void LateUpdate()
        {
            MoveCameraAroundThePlayer();
            if (health.IsDead()) return;          
            if (CantMoveDirection) return;
            if (mover.InteractWithMovement())
            {
                if (_acelerationByPlayer < 6) _acelerationByPlayer += Time.deltaTime*4;
            }
            else if (_acelerationByPlayer > 1) {
                //to decrement movement
                //mover.StartMoveAction(transform.forward + transform.position, 1f);
                _acelerationByPlayer -= Time.deltaTime*4;
            }
        }

        #endregion

        #region public function

        public float AcelerationMovement
        {
            set {
                if (_acelerationByPlayer > 1)
                {
                    _acelerationByPlayer += value;
                }
                else _acelerationByPlayer = 1;
            }
            get => _acelerationByPlayer;
        }

        public void ChangeMovementControllerPlayer(bool valueToChangeCantMove) {
            mover.Cancel();
            CantMoveDirection = valueToChangeCantMove;
        }

        public void changeWeaponIndex(WeaponConfig weapon) => fighter.changeWeaponIndex(weapon);

        public void SetTheWeapon(WeaponConfig weapon)
        {

            fighter.SetTheWeapon(weapon);
            AttackingFalse();
        }

        public void SetTheWeaponByIndex(WeaponConfig weapon, int index) => fighter.SetDataTheWeapon(weapon, index);

        public void SetPrimaryWeaponInPlayer() => fighter.SetTheWeapon(fighter.GetWeaponByIndex(0));

        public void spawnActualWeaponPlayer() => fighter.spawnActualWeaponPlayer();

        public int GetIndexActualWeapon() => fighter.indexWeaponActual;

        public Vector3 GetTFPosition() => tfArea.position;

        public WeaponConfig GetActualWeaponPlayer() => fighter.weaponsEquip[fighter.indexWeaponActual];

        #endregion

        #region private function

        private bool InteractWithPray()
        {
            return false;
        }

        bool InteractWithMap() {
            //menu
            if (Input.GetButtonDown("Start"))
            {
                startMenu.SetActive(!startMenu.activeInHierarchy);
            }

            /*menu active
            if (startMenu.activeInHierarchy) {
                if (Input.GetButtonDown("Fire1"))
                {
                    startMenu.SetActive(false);
                    return true;
                }
                else if (Input.GetButtonDown("Fire3")) {
                    //pay
                    GameManager.gameManager.AugmentCoins(-75);

                    //portal
                    portal.StartTransition();
                    print("Volver al mapa");
                }
                print("activo");
                return true;
            }*/


            //map
            if (Input.GetButtonDown("Select"))
            {
                cameraMap.SetActive(true);
            } else if (Input.GetButtonUp("Select")) {
                cameraMap.SetActive(false);
            }

            return false;
        }

        void MoveCameraAroundThePlayer() {
            //rotate camera around player
            /*
            if (Input.GetAxis("HorizontalCamera") >= 0.5 || Input.GetButton("HorizontalCameraMouse")) {
                transformCamera.RotateAround(transform.position, transform.up, -valueRotationCamera);
            }
            else if (Input.GetAxis("HorizontalCamera") <= -0.5 || Input.GetButton("VerticalCameraMouse"))
            {
                transformCamera.RotateAround(transform.position, transform.up, valueRotationCamera);
            }*/
        }

        //animator methods group
        void AttackingTrue()
        {
            CantMoveDirection = true;
            fighter.attacking = true;
            mover.Cancel();
        }

        void AttackingFalse()
        {
            //if (attacking && !InCombo)
            //{
            fighter.attacking = false;
            //}
            CantMoveDirection = false;
        }

        void ResetComboE()
        {
            animator.ResetTrigger("ComboE");
        }

        //important? idk
        private bool InteractWithMovement()
        {
            if (moveWithUPDOWN) return mover.InteractWithMovement();
            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit, 100);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(hit.point)) return false;
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType cursor)
        {
            CursoMapping mapping = GetCursoMapping(cursor);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InteractWithComponent()
        {
            if (moveWithUPDOWN) return false;
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        CursoMapping GetCursoMapping(CursorType type)
        {
            foreach (CursoMapping mapping in cursoMapping)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursoMapping[0];
        }

        //interesting coding --->
        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), radiusRaycast);

            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            return hits;
        }

        #endregion
    }
}
