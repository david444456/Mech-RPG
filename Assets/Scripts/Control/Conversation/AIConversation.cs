using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control {
    public class AIConversation : MonoBehaviour
    {
        [SerializeField] UnityEvent _eventActiveThisConversation = new UnityEvent();
        [SerializeField] UnityEvent _eventFinishThisConversation = new UnityEvent();
        [SerializeField] Conversation conversation;
        [SerializeField] bool worksWithTouchItem = false;

        private bool thisIsUsed = false;

        void Start()
        {

        }

        public bool GetIfWorksWithTouchItem() => worksWithTouchItem;

        public void ActiveConversation() {
            print("Miau");
            GetComponent<Collider>().enabled = false;
            _eventActiveThisConversation.Invoke();
        }

        public void FinishConversation()
        {
            print("Miau");
            GetComponent<Collider>().enabled = false;
            _eventFinishThisConversation.Invoke();
        }

        public Conversation GetConversation() => conversation;
    }
}
