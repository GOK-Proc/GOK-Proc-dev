using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rhythm
{
    public class InputManager : IColorInputProvider, IMoveInputProvider
    {
        public float Move
        {
            get
            {
                var value = 0f;
                var isPressedThisFrame = false;

                foreach (var move in _moves)
                {
                    value += move.ReadValue<Vector2>().x;
                    isPressedThisFrame |= move.WasPressedThisFrame();
                }

                return isPressedThisFrame ? value : 0f;
            }
        }

        private readonly IList<InputAction> _attacks;
        private readonly IList<InputAction> _defenses;
        private readonly IList<InputAction> _moves;

        private int _attackCount;
        private int _attackMaxCount;
        private int _defenseCount;
        private int _defenseMaxCount;


        public InputManager(IList<InputAction> attack, IList<InputAction> defense, IList<InputAction> move)
        {
            _attacks = attack ?? Array.Empty<InputAction>();
            _defenses = defense ?? Array.Empty<InputAction>();
            _moves = move ?? Array.Empty<InputAction>();

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
                    foreach (var attack in _attacks)
                    {
                        if (attack.IsPressed())
                        {
                            return true;
                        }
                    }
                    return false;

                case NoteColor.Blue:
                    foreach (var defense in _defenses)
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
                    foreach (var attack in _attacks)
                    {
                        if (attack.WasPressedThisFrame())
                        {
                            return true;
                        }
                    }
                    return false;

                case NoteColor.Blue:
                    foreach (var defense in _defenses)
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

            foreach (var attack in _attacks)
            {
                if (attack.WasPressedThisFrame())
                {
                    _attackMaxCount++;
                }
            }

            foreach (var defense in _defenses)
            {
                if (defense.WasPressedThisFrame())
                {
                    _defenseMaxCount++;
                }
            }
        }
    }
}