using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using static System.Math;

public class lineHandler {
    vtextbox parent;
    public lineHandler(vtextbox p)
    {
        parent = p;
        lines.Add(DefaultLine());
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

    public line DefaultLine(string s = "") => new line(this, s, 0) 
        {
            brush = new SolidBrush(parent.ForeColor),
            font = parent.Font,
        }; 
    public void AddGo() => AddGo(Current.Split());
    public void AddGo(string s) {
        var l = DefaultLine(s);

        if(lineIndex >= MaxInd) {
            lines.Add(l);
        }
        else{
            lines.Insert(lineIndex + 1, l);
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

    public void SetText(string s) {
        var ls = s.Split('\n');
        lines.Clear();
        foreach(var l in ls) { AddGo(l); }
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
        Console.WriteLine(); Console.WriteLine();
        
        int pad = 0;
        foreach(var l in lines) { pad = Max(l.Length, pad); }

        int i = 0;
        foreach (var l in lines)
        {
           Console.WriteLine($"{++i}. {('\"' + l.Text + '\"').PadRight(pad + 2)} [c:{l.CursorIndex}, l:{l.Length}, s:{l.se.Length}] {(Current == l ? "<" : "")} "); 
        }
#endif
    }
}