using Novel;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Novel
{
    public class DialogueOperation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _dialogueText;

        private float _interval = 0.05f;

        private ReplaceDictionary _replaceDictionary;

        private bool _breakFlag = false;    // これがtrueになるとコルーチンのループを抜ける

        private void Start()
        {
            _replaceDictionary = new ReplaceDictionary();
        }

        public void UpdateDialogue(DialogueData dialogueData)
        {
            NovelManager.Instance.StopDialogue = true;
            NovelManager.Instance.IsProcessingDialogue = true;
            _breakFlag = false;


            string name = dialogueData.Name;
            string dialogue = dialogueData.Dialogue;

            name = ReplaceParamater(name);
            dialogue = ReplaceParamater(dialogue);

            _nameText.text = name;

            StartCoroutine(DialogueOrder(dialogue));
        }

        private IEnumerator DialogueOrder(string dialogue)
        {
            _dialogueText.text = "";

            foreach (char c in dialogue)
            {
                if (_breakFlag)
                {
                    break;
                }

                _dialogueText.text += c;
                yield return new WaitForSeconds(_interval);
            }

            _dialogueText.text = dialogue;

            NovelManager.Instance.IsProcessingDialogue = false;
        }

        public void DisplayImmediately()
        {
            _breakFlag = true;
        }

        private string ReplaceParamater(string sentence)
        {
            int foundIndex = sentence.IndexOf("${");
            if (foundIndex >= 0)
            {
                int nextIndex = sentence.IndexOf("}", foundIndex + 2);
                if (nextIndex >= 0)
                {
                    string target = sentence.Substring(foundIndex + 2, nextIndex - (foundIndex + 2));
                    string replaced = sentence.Replace("${" + target + "}", _replaceDictionary.ReplaceDict[target]);

                    return ReplaceParamater(replaced);
                }
                else
                {
                    throw new Exception("変数置換のための\"${\"に対応する\"}\"がありません。");
                }
            }
            else
            {
                return sentence;
            }
        }
    }
}
