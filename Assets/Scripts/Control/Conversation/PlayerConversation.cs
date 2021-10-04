using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Control
{
    public class PlayerConversation : MonoBehaviour
    {
        public Action<bool> EventStateConversation = delegate { };

        [Header("Interact")]
        [SerializeField] GameObject uiButton = null;
        [SerializeField] int distance = 10;

        [Header("UI")]
        [SerializeField] GameObject GOconversation;
        [SerializeField] Image imageConversationType;
        [SerializeField] Text textConversation;

        private AIConversation aIConversation;
        private Conversation lastConversation;
        private bool _activeNextConversation = false;
        private bool _conversationActive = false;
        private int _indexActualDialog = 0;

        void Start()
        {

        }

        private void Update()
        {
            if (uiButton.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.E) || aIConversation.GetIfWorksWithTouchItem())
                {
                    uiButton.SetActive(false);
                    lastConversation = aIConversation.GetConversation();
                    _indexActualDialog = 0;

                    //event show new conversation
                    aIConversation.ActiveConversation();

                    //start with the conversation
                    _conversationActive = true;
                    GetComponent<MoverPlayer>().StopMoveChangeAnimation();
                    ShowConversationText();
                    ChangeStateConversation(true);
                }
                else if (Vector3.Distance(transform.position, aIConversation.gameObject.transform.position) > distance)
                {
                    aIConversation = null;
                    uiButton.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Conversation")
            {
                aIConversation = collision.GetComponent<AIConversation>();
                print(aIConversation);
                uiButton.transform.position = collision.gameObject.transform.position;
                uiButton.SetActive(true);
            }
        }

        public void NextConversation() => _activeNextConversation = true;

        public bool InteractWithConversation() {
            //input, change things, bool value
            if (_conversationActive)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _activeNextConversation = true;
                }

                //change conversation if i get more
                if (_activeNextConversation)
                {
                    if (_indexActualDialog < lastConversation.GetLimitTextConversations() - 1)
                    {
                        _indexActualDialog++;
                        ShowConversationText();
                    }
                    else
                    {
                        ChangeStateConversation(false);
                        GOconversation.SetActive(false);
                        _conversationActive = false;
                    }

                    _activeNextConversation = false;
                }
            }

            return _conversationActive;
        }

        public void ShowConversationText() {
            GOconversation.SetActive(true);
            imageConversationType.sprite = lastConversation.GetSpriteTypeByIndex(_indexActualDialog);
            textConversation.text = lastConversation.GetTextForConversationByIndex(_indexActualDialog);
        }

        private void ChangeStateConversation(bool newState) {
            EventStateConversation.Invoke(newState);
        }
    }
}
