﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rhythm
{
    public class TutorialManager
    {
        private readonly bool _isTutorial;
        private readonly TutorialData _data;
        private readonly PlayerInput _playerInput;
        private readonly ISoundPlayable _soundPlayable;
        private readonly ITimeProvider _timeProvider;
        private readonly ITutorialDrawable _tutorialDrawable;

        private int _next;

        public TutorialManager(bool isTutorial, TutorialData data, PlayerInput playerInput, ISoundPlayable soundPlayable, ITimeProvider timeProvider, ITutorialDrawable tutorialDrawable)
        {
            _isTutorial = isTutorial;
            _data = data;
            _playerInput = playerInput;
            _soundPlayable = soundPlayable;
            _timeProvider = timeProvider;

            _next = 0;
            _tutorialDrawable = tutorialDrawable;
        }

        public void Update()
        {
            if (_isTutorial)
            {
                if (_next < _data.Times.Length)
                {
                    if (_timeProvider.Time >= _data.Times[_next] + _data.Beatmap.Offset)
                    {
                        _playerInput.SwitchCurrentActionMap("Tutorial");
                        _soundPlayable.PauseMusic();
                        Time.timeScale = 0;

                        _tutorialDrawable.DrawTutorial(_next);
                        _next++;
                    }
                }
            }
        }

    }
}