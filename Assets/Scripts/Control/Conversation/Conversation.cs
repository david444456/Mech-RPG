using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Dialog/ Make new Dialog", order = 0)]
    public class Conversation : ScriptableObject
    {
        [SerializeField] structConversation[] structConversations;
        [SerializeField] Sprite spriteTypeAI;
        [SerializeField] Sprite spriteTypeSlava;

        public string GetTextForConversationByIndex(int index) {
            return structConversations[index].TextConversation;
        }

        public Sprite GetSpriteTypeByIndex(int index) {
            return GetSpriteByType(structConversations[index]._typeDialog);
        }

        public int GetLimitTextConversations() => structConversations.Length;

        private Sprite GetSpriteByType(TypeDialog typeDialog)
        {
            if (typeDialog == TypeDialog.AI) return spriteTypeAI;
            else return spriteTypeSlava;
        }
    }

    [Serializable]
    public class structConversation
    {
        [TextArea (6,14)] public string TextConversation;
        public TypeDialog _typeDialog;
    }

    public enum TypeDialog { 
        AI,
        Slava,
    }
}
