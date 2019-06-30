using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.DialogueSystem

{
    public class NPC_StartDialogView : MonoBehaviour
    {
        [SerializeField]
        private Transform _canvasNPC;
        [SerializeField]
        private GameObject _dialogPanel;
        [SerializeField]
        private Text _text;
        public bool _startDialogFlag { get; private set; }
        public bool _dialogAreaEnter { get; private set; }
        public string _NpcText { get; private set; }


        DialogueSystem dialogueSystem;
        

        private void Awake()
        {
            _canvasNPC = gameObject.GetComponentInChildren<Canvas>().transform;
            _dialogPanel = GameObject.Find("NPC_use_Panel");
            _text = gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();

            dialogueSystem = FindObjectOfType<DialogueSystem>();

            _NpcText = "Text for NPC №1, bla bla bla bla!";
        }
        void Start()
        {
            _dialogPanel.SetActive(false);
            _text.text = "\"E\" Начать диалог";
            dialogueSystem._npcStartText = _NpcText;
        }


        void Update()
        {
            _canvasNPC.LookAt(Camera.main.transform);


            if (Input.GetButton("Use") & _dialogAreaEnter == true )
            {
                _startDialogFlag = true;
                dialogueSystem.gameObject.GetComponentInChildren<Canvas>().enabled = true;
               
            }
            if(Input.GetButton("Cancel") || _dialogAreaEnter == false)
            {
                _startDialogFlag = false;
                dialogueSystem.gameObject.GetComponentInChildren<Canvas>().enabled = false;
                
            }

            ShowDialogueGUI();

        }


        private void ShowDialogueGUI()
        {
            if (_startDialogFlag == false & _dialogAreaEnter == true )
            {
                _dialogPanel.SetActive(true);
            }
            else
            {
                _dialogPanel.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Player")
            {

                _dialogAreaEnter = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == "Player")
            {
                    _dialogAreaEnter = false;
            }
        }
    }
}