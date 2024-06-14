using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rhythm
{
    public class InputManager : IColorInputProvider, IVectorInputProvider
    {
        public Vector2 Vector { get; }

        private readonly InputAction[] _attack;
        private readonly InputAction[] _defense;
        private readonly InputAction[] _move;

        private int _attackCount;
        private int _attackMaxCount;
        private int _defenseCount;
        private int _defenseMaxCount;


        public InputManager(InputAction[] attack, InputAction[] defense, InputAction[] move)
        {
            _attack = attack ?? Array.Empty<InputAction>();
            _defense = defense ?? Array.Empty<InputAction>();
            _move = move ?? Array.Empty<InputAction>();

            _attackCount = 0;
            _attackMaxCount = 0;
            _defenseCount = 0;
            _defenseMaxCount = 0;
        }

        public bool IsColorPressed(NoteColor color)
        {
            switch (color)
            {
                case NoteColor.Red:
                    foreach (var attack in _attack)
                    {
                        if (attack.IsPressed())
                        {
                            return true;
                        }
                    }
                    return false;

                case NoteColor.Blue:
                    foreach (var defense in _defense)
                    {
                        if (defense.IsPressed())
                        {
                            return true;
                        }
                    }
                    return false;
            }

            return default;
        }

        public bool IsColorPressedThisFrame(NoteColor color)
        {
            switch (color)
            {
                case NoteColor.Red:
                    foreach (var attack in _attack)
                    {
                        if (attack.WasPressedThisFrame())
                        {
                            return true;
                        }
                    }
                    return false;

                case NoteColor.Blue:
                    foreach (var defense in _defense)
                    {
                        if (defense.WasPressedThisFrame())
                        {
                            return true;
                        }
                    }
                    return false;
            }

            return default;
        }

        public bool IsColorJudged(NoteColor color)
        {
            switch (color)
            {
                case NoteColor.Red:
                    if (_attackCount >= _attackMaxCount) return true;
                    return false;
                case NoteColor.Blue:
                    if (_defenseCount >= _defenseMaxCount) return true;
                    return false;
            }

            return default;
        }

        public void CompleteColorJudge(NoteColor color)
        {
            switch (color)
            {
                case NoteColor.Red:
                    _attackCount++;
                    break;
                case NoteColor.Blue:
                    _defenseCount++;
                    break;
            }
        }
        
        public void Update()
        {
            _attackCount = 0;
            _defenseCount = 0;
            _attackMaxCount = 0;
            _defenseMaxCount = 0;

            foreach (var attack in _attack)
            {
                if (attack.WasPressedThisFrame())
                {
                    _attackMaxCount++;
                }
            }

            foreach (var defense in _defense)
            {
                if (defense.WasPressedThisFrame())
                {
                    _defenseMaxCount++;
                }
            }
        }
    }
}