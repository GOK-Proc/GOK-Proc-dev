using Novel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Novel
{
    public interface OperationData
    {
        public OperationType OperationType { get; }

        public void ExecuteOperation(NovelOperation novelOperation);
    }

    public class DialogueData : OperationData
    {
        public OperationType OperationType { get; } = OperationType.Dialogue;
        public string Name { get; }
        public string Dialogue { get; }

        public DialogueData(string name, string dialogue)
        {
            Name = name;
            Dialogue = dialogue;
        }

        public void ExecuteOperation(NovelOperation novelOperation)
        {
            novelOperation.UpdateDialogue(this);
        }
    }

    public class CharacterLayoutData : OperationData
    {
        public OperationType OperationType { get; } = OperationType.CharacterLayout;
        public List<string> CharacterLayout { get; }

        public CharacterLayoutData(string characterLayout)
        {
            CharacterLayout = new List<string>(characterLayout.Split(" "));
        }

        public void ExecuteOperation(NovelOperation novelOperation)
        {
            novelOperation.UpdateCharacterLayout(this);
        }
    }

    public class BackgroundData : OperationData
    {
        public OperationType OperationType { get; } = OperationType.Background;
        public string Background { get; }

        public BackgroundData(string background)
        {
            Background = background;
        }

        public void ExecuteOperation(NovelOperation novelOperation)
        {
            novelOperation.UpdateBackground(this);
        }
    }

    public class BgmData : OperationData
    {
        public OperationType OperationType { get; } = OperationType.Bgm;
        public string Bgm { get; }

        public BgmData(string bgm)
        {
            Bgm = bgm;
        }

        public void ExecuteOperation(NovelOperation novelOperation)
        {
            novelOperation.UpdateBgm(this);
        }
    }

    public class OtherData : OperationData
    {
        public OperationType OperationType { get; } = OperationType.Other;
        // 現在は使っていないので、何の情報も保持していない(演出の拡張用)

        public OtherData()
        {

        }

        public void ExecuteOperation(NovelOperation novelOperation)
        {
            novelOperation.ExecuteOtherOperation(this);
        }
    }

    public class LineOperationData
    {
        public DialogueData LineDialogueData { get; set; }
        public CharacterLayoutData LineCharacterLayoutData { get; set; }
        public BackgroundData LineBackgroundData { get; set; }
        public BgmData LineBgmData { get; set; }
        public OtherData LineOtherData { get; set; }
    }
}