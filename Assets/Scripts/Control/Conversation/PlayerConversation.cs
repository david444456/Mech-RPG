using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Control
{
    public class PlayerConversation : MonoBehaviour
    {
        [Header("Interact")]
        [SerializeField] GameObject uiButton = null;
        [SerializeField] int distance = 10;

        [Header("UI")]
        [SerializeField] GameObject GOconversation;
        [SerializeField] Image imageConversationType;
        [SerializeField] Text textConversation;

        private AIConversation aIConversation;
        private Conversation lastConversation;
        private bool _conversationActive = false;
        private int _indexActualDialog = 0;

        void Start()
        {

        }

        private void Update()
        {
            if (uiButton.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    uiButton.SetActive(false);
                    lastConversation = aIConversation.GetConversation();
                    _indexActualDialog = 0;

                    //start with the conversation
                    ShowConversationText();
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

        public bool InteractWithConversation() {
            //input, change things, bool value

            //change conversation if i get more

            return _conversationActive;
        }

        public void ShowConversationText() {
            GOconversation.SetActive(true);
            imageConversationType.sprite = lastConversation.GetSpriteTypeByIndex(_indexActualDialog);
            textConversation.text = lastConversation.GetTextForConversationByIndex(_indexActualDialog);
        }
    }
}
