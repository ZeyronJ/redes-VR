using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NSU
{
    public class ConsoleBuffer
    {
        public int id = -1;
        private int maxCharsPerLine;
        private int maxLines;
        private List<string> lines;

        public event Action OnBufferUpdated;

        public ConsoleBuffer(int maxCharsPerLine = 64, int maxLines = 16, int id = -1)
        {
            this.maxCharsPerLine = maxCharsPerLine;
            this.maxLines = maxLines;
            this.lines =  new List<string>();
            this.id = id;
        }

        public void AddLine(string line)
        {
            if (line.Length > maxCharsPerLine)
            {
                lines.Add(line.Substring(0, maxCharsPerLine));
                lines.Add(line.Substring(maxCharsPerLine, line.Length - maxCharsPerLine));
                return;
            }
            else
            {
                lines.Add(line);
            }

            while (lines.Count > maxLines)
            {
                lines.RemoveAt(0);
            }

            OnBufferUpdated?.Invoke();
        }

        public void ClearBuffer()
        {
            lines.Clear();
            OnBufferUpdated?.Invoke();
        }

        public void ClearBufferExceptLast()
        {
            if (lines.Count <= 1)
            {
                return;
            }

            string lastLine = lines[lines.Count - 1];
            lines.Clear();
            lines.Add(lastLine);

            OnBufferUpdated?.Invoke();
        }

        public string[] GetLines()
        {
            return lines.ToArray();
        }
    }
}