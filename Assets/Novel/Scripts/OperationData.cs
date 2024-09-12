using Novel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Novel
{
    public interface OperationData
    {
        public void ExecuteOperation();
    }

    public class DialogueData : OperationData
    {
        public string Name { get; }
        public string Dialogue { get; }

        public DialogueData(string name, string dialogue)
        {
            Name = name;
            Dialogue = dialogue;
        }

        public void ExecuteOperation()
        {
            NovelManager.Instance.DialogueOperation.UpdateDialogue(this);
        }
    }

    public class CharacterLayoutData : OperationData
    {
        public List<string> Layout { get; }
        public List<string> Motion { get; }

        public CharacterLayoutData(string characterLayout, string characterMotion)
        {
            if (characterLayout == "Clear")
            {
                Layout = new List<string> { };
            }
            else
            {
                Layout = new List<string>(characterLayout.Split(" "));
            }

            Motion = new List<string>(characterMotion.Split(" "));
        }

        public void ExecuteOperation()
        {
            NovelManager.Instance.CharacterOperation.UpdateCharacterLayout(this);
        }
    }

    public class BackgroundData : OperationData
    {
        public string Background { get; }
        public string Motion { get; }

        public BackgroundData(string background)
        {
            string[] arguments = background.Split(" ");

            Background = arguments[0];
            
            if (arguments.Length > 1)
            {
                Motion = arguments[1];
            }
            else
            {
                Motion = "Fade";
            }
        }

        public void ExecuteOperation()
        {
            NovelManager.Instance.BackgroundOperation.UpdateBackground(this);
        }
    }

    public class SoundData : OperationData
    {
        public string Sound { get; }
        public string Motion { get; }

        public SoundData(string bgm)
        {
            string[] arguments = bgm.Split(" ");
            Sound = arguments[0];

            if (arguments.Length > 1)
            {
                Motion = arguments[1];
            }
            else
            {
                Motion = "Fade";
            }
        }

        public void ExecuteOperation()
        {
            NovelManager.Instance.SoundOperation.UpdateSound(this);
        }
    }

    public class OtherData : OperationData
    {
        // 現在は使っていないので、何の情報も保持していない(演出の拡張用)

        public OtherData()
        {

        }

        public void ExecuteOperation()
        {
            NovelManager.Instance.OtherOperation.ExecuteOtherOperation(this);
        }
    }

    public class LineOperationData
    {
        public DialogueData LineDialogueData { get; set; }
        public CharacterLayoutData LineCharacterLayoutData { get; set; }
        public BackgroundData LineBackgroundData { get; set; }
        public SoundData LineBgmData { get; set; }
        public OtherData LineOtherData { get; set; }
    }
}