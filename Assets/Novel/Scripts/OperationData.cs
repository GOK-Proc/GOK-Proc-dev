using Novel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novel
{
    public interface OperationData
    {
        public OperationType OperationType { get; }
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
    }

    public class CharacterLayoutData : OperationData
    {
        public OperationType OperationType { get; } = OperationType.CharacterLayout;
        public List<string> CharacterLayoutList { get; }

        public CharacterLayoutData(string characterLayout)
        {
            CharacterLayoutList = new List<string>(characterLayout.Split(" "));
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
    }
}