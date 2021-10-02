using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {
    public class AIConversation : MonoBehaviour
    {
        [SerializeField] Conversation conversation;

        void Start()
        {

        }

        public Conversation GetConversation() => conversation;
    }
}
