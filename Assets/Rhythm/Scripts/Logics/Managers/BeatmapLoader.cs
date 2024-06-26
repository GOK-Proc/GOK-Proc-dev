using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rhythm
{
    public static class BeatmapLoader
    {
        private class Note
        {
            public int Lane;
            public NoteColor Color;
            public bool IsLarge;
            public double Length;
            public double Bpm;
            public float Scroll;

            public Note(int lane, NoteColor color, bool isLarge, double length, double bpm, float scroll)
            {
                Lane = lane;
                Color = color;
                IsLarge = isLarge;
                Length = length;
                Bpm = bpm;
                Scroll = scroll;
            }
        }

        private enum ParseMode
        {
            Lane,
            Type,
            Bpm,
            Measure,
            Scroll,
        }

        public static NoteData[] Parse(TextAsset file, double offset, float baseScroll)
        {
            var text = file.text;
            var types = new Dictionary<char, (NoteColor, bool)>()
            { 
                { 'a', (NoteColor.Red, false) },
                { 'b', (NoteColor.Blue, false) },
                { 'A', (NoteColor.Red, true) },
                { 'B', (NoteColor.Blue, true) },
            };

            var notes = new List<NoteData>();
            var data = new List<Note>();
            var mode = ParseMode.Lane;
            var numstr = string.Empty;
            var isHold = false;
            var isComment = false;

            var just = offset;
            var bpm = 0d;
            var measure1 = 4d;
            var measure2 = 4d;
            var scroll = 1f;
            var m = 0d;

            try
            {
                foreach (var c in text)
                {
                    if (isComment)
                    {
                        if (c == '\n')
                        {
                            isComment = false;
                            mode = ParseMode.Lane;
                        }
                        continue;
                    }

                    switch (c)
                    {
                        case '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9' or '.':

                            numstr += c;

                            if (mode == ParseMode.Lane && !isHold)
                            {
                                var lane = int.Parse(numstr);
                                numstr = string.Empty;

                                data.Add(new Note(lane, NoteColor.Undefined, false, 0, bpm, scroll));
                                if (lane != 0) mode = ParseMode.Type;

                            }

                            break;

                        case 'a' or 'b' or 'A' or 'B':

                            if (mode == ParseMode.Type)
                            {
                                (var color, var isLarge) = types[c];
                                data.Last().Color = color;
                                data.Last().IsLarge = isLarge;
                                mode = ParseMode.Lane;
                            }

                            break;

                        case ',':

                            if (mode == ParseMode.Lane && !isHold)
                            {
                                var beat = data.Count;

                                foreach (var d in data)
                                {
                                    if (d.Lane != 0) notes.Add(new NoteData(d.Scroll * baseScroll, d.Lane - 1, d.Color, d.IsLarge, just, d.Length, d.Bpm));
                                    just += 240 * measure1 / measure2 / beat / d.Bpm;
                                }

                                data.Clear();
                            }

                            break;

                        case '[':

                            if (mode == ParseMode.Lane && data.Count > 0) isHold = true;

                            break;

                        case ':':

                            if (mode == ParseMode.Lane && isHold)
                            {
                                m = double.Parse(numstr);
                                numstr = string.Empty;
                            }

                            break;

                        case ']':

                            if (mode == ParseMode.Lane && isHold)
                            {
                                var n = double.Parse(numstr);
                                numstr = string.Empty;

                                data.Last().Length = 240 * n / m / data.Last().Bpm;
                                isHold = false;
                            }

                            break;

                        case '(':

                            if (mode == ParseMode.Lane && !isHold) mode = ParseMode.Bpm;
                            
                            break;

                        case '{':

                            if (mode == ParseMode.Lane && !isHold) mode = ParseMode.Measure;

                            break;

                        case '<':

                            if (mode == ParseMode.Lane && !isHold) mode = ParseMode.Scroll;

                            break;

                        case '/':

                            if (mode == ParseMode.Measure)
                            {
                                measure1 = double.Parse(numstr);
                                numstr = string.Empty;
                            }

                            break;

                        case ')':

                            if (mode == ParseMode.Bpm)
                            {
                                bpm = double.Parse(numstr);
                                numstr = string.Empty;
                                mode = ParseMode.Lane;
                            }

                            break;

                        case '}':

                            if (mode == ParseMode.Measure)
                            {
                                measure2 = double.Parse(numstr);
                                numstr = string.Empty;
                                mode = ParseMode.Lane;
                            }

                            break;

                        case '>':

                            if (mode == ParseMode.Scroll)
                            {
                                scroll = float.Parse(numstr);
                                numstr = string.Empty;
                                mode = ParseMode.Lane;
                            }

                            break;

                        case '\\':

                            isComment = true;

                            break;
                    }
                }
            }
            catch (System.Exception)
            {

                Debug.LogError("Invalid Beatmap Syntax");

                return default;
            }

            return notes.ToArray();
        }
    }
}