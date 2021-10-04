using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control {
    public class AIConversation : MonoBehaviour
    {
        [SerializeField] UnityEvent _eventActiveThisConversation;
        [SerializeField] Conversation conversation;

        void Start()
        {
            _eventActiveThisConversation = new UnityEvent();
        }

        public void ActiveConversation() => _eventActiveThisConversation.Invoke();

        public Conversation GetConversation() => conversation;
    }
}
