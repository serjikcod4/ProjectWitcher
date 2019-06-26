using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DialogueSystem

{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField]
        private Canvas _dialogueCanvas;
        [SerializeField]
        private Image _dialoguePanel;
        [SerializeField]
        private Text _dialogueNPCText;
        [SerializeField]
        private PlayerAnswer[] _dialoguePlayerAnswers;
        [SerializeField]
        private GameObject _currentNPC;
        public bool _showDialogue = false;
        private string _npcStartText;
        [SerializeField]
        private Dialogue[] dialogues;


        private void Awake()
        {
            
            _dialogueCanvas = gameObject.GetComponentInChildren<Canvas>();
            _dialoguePanel = gameObject.GetComponentInChildren<Image>();
            _dialogueNPCText = gameObject.GetComponentInChildren<Text>();
            

        }
        void Start()
        {
            _dialogueCanvas.enabled = false;
        }




        void Update()
        {
            
        }
    }
}