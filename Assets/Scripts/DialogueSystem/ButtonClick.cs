using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.DialogueSystem

{
    public class ButtonClick : MonoBehaviour
    {
        DialogueSystem dialogueSystem;

       // int _toNode;

        

        public  void OnButtonClick(int _toNode)
        {
            
            dialogueSystem = FindObjectOfType<DialogueSystem>();
            dialogueSystem._currentNode = _toNode;
            dialogueSystem.DialogueAnswerClear();
        }
    }
}