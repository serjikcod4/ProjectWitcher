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
        private Button[] _AnswerButtons;

        [SerializeField]
        private GameObject _currentNPC;


        [SerializeField]
        public string _npcStartText;

        public int _currentNode;
        public Dialogue[] dialogueNode;
        

        




        private void Awake()
        {

            _dialogueCanvas = gameObject.GetComponentInChildren<Canvas>();
            _dialoguePanel = gameObject.GetComponentInChildren<Image>();
            _dialogueNPCText = gameObject.GetComponentInChildren<Text>();
            _AnswerButtons = gameObject.GetComponentsInChildren<Button>();
            

        }
        void Start()
        {
            _dialogueCanvas.enabled = false;
            foreach (Button b in _AnswerButtons)
            {
                b.gameObject.SetActive(false);
            }
        }




        void Update()
        {
            DialogueUpdate();
            
        }

        public void DialogueAnswerClear()
        {
            foreach (Button b in _AnswerButtons)
            {
                b.gameObject.SetActive(false);
            }
        }
        private void DialogueUpdate()
        {
            _dialogueNPCText.text = dialogueNode[_currentNode]._npcText;
            for (int i = 0; i < dialogueNode[_currentNode]._dialoguePlayerAnswers.Length; i++)
            {
                _AnswerButtons[i].gameObject.SetActive(true);
                _AnswerButtons[i].GetComponentInChildren<Text>().name = dialogueNode[_currentNode]._dialoguePlayerAnswers[i]._text;
                _AnswerButtons[i].GetComponentInChildren<Text>().text = dialogueNode[_currentNode]._dialoguePlayerAnswers[i]._text;
                
            }
        }
    }
   
}