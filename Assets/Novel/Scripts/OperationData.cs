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
        public List<GameObject> CharacterLayoutList { get; }

        public CharacterLayoutData(string characterLayout)
        {
            foreach (string character in characterLayout.Split(" "))
            {

            }
        }
    }
}