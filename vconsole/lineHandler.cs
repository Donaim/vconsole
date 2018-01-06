using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using static System.Math;

class lineHandler {
    public lineHandler()
    {
        lines.Add(new line(this, "", 0));
    }

    List<line> lines = new List<line>();
    public IReadOnlyList<line> get => lines;
    public line Current => lines[lineIndex];

    public int MaxInd => lines.Count - 1;
    int lineIndex = 0;
    public int LineIndex { 
        get => lineIndex; 
        set => lineIndex = Max(0, Min(value, MaxInd)); 
    }

    public void AddGo() {
        var l = new line(this, Current.Split(), 0);

        if(lineIndex >= MaxInd) {
            lines.Add(l);
        }
        else{
            lines.Insert(lineIndex, l);
        }
        LineIndex++;
    }
    public void Remove() {
        if(Current.Length > 0) {
            Current.Remove();
        }
        else if (lines.Count > 0) {
            lines.Remove(Current);
            GoUp();
        }
    }

    public void GoUp() {
        if(Current == lines.First()) { Current.CursorIndex = 0; }
        else { LineIndex--; }
    }
    public void GoDown() {
        if(Current == lines.Last()) { Current.CursorIndex = Current.Length; }        
        else { LineIndex++; }
    } 

    internal void printToConsole() {
#if DEBUG
        foreach (var l in lines)
        {
           Console.WriteLine("> \"{0}\" [{1}] {2}", l.Text, l.CursorIndex, (Current == l ? "<" : "")); 
        }
#endif
    }
}