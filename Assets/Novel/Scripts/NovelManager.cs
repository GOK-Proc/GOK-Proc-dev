using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Novel
{
    public class NovelManager : SingletonMonoBehaviour<NovelManager>
    {
        [SerializeField] private float _defaultDuration = 0.75f;
        [SerializeField] private NovelId _novelId;
        [SerializeField] private NovelData _novelData;
        [SerializeField] private NovelMaterialData _novelMaterialData;

        [field: SerializeField] public DialogueOperation DialogueOperation { get; set; }
        [field: SerializeField] public CharacterOperation CharacterOperation { get; set;  }
        [field: SerializeField] public BackgroundOperation BackgroundOperation { get; set; }
        [field: SerializeField] public SoundOperation SoundOperation { get; set; }
        [field: SerializeField] public OtherOperation OtherOperation { get; set; }

        [SerializeField] private int _currentLine = 0;

        private ScenarioData _scenarioData;

        public float Duration { get; private set; }

        private bool _completeInitialize = false;
        private bool _notFirstLine = false;

        public bool StopDialogue { get; set; } = false;      // 前の行で会話文が更新されたか
        public bool IsProcessingCharacter { get; set; } = false;
        public bool IsProcessingBackground { get; set; } = false;

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            // 1行目の場合
            if (!_notFirstLine)
            {
                // 初期化処理が終わったら自動で実行
                if (_completeInitialize)
                {
                    CallLineOperation();

                    _notFirstLine = true;
                }
            }
            else
            {
                // すべての処理が完了しており、かつ最後の行に達していない場合
                if (IsFinishedOperation() && _scenarioData.ScenarioLines.Count > _currentLine)
                {
                    // 前の行で会話文が更新されていた場合(入力待ち)
                    if (StopDialogue)
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            CallLineOperation();
                        }
                    }
                    else
                    {
                        CallLineOperation();
                    }
                }
            }
        }

        private void CallLineOperation()
        {
            StopDialogue = false;

            Duration = _defaultDuration;

            List<OperationData> lineOperationList = _scenarioData.ScenarioLines[_currentLine];

            foreach (OperationData lineOperation in lineOperationList)
            {
                lineOperation.ExecuteOperation();
            }

            _currentLine++;
        }

        private void Initialize()
        {
            // シナリオデータのロード
            ScenarioLoader.MakeScenarioData(_novelData.NovelDict[_novelId.ToString()]);
            _scenarioData = ScenarioLoader._ScenarioData;

            // キャラクターアセットのロード
            CharacterOperation.CharacterMaterialDict = _novelMaterialData.CharacterMaterialDict;

            // 背景アセットのロード
            BackgroundOperation.BackgroundImageDict = _novelMaterialData.BackgroundMaterialDict;

            _completeInitialize = true;
        }

        private bool IsFinishedOperation()
        {
            return !IsProcessingCharacter && !IsProcessingBackground;
        }
    }
}
