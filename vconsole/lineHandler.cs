using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using static System.Math;

public class lockedList<T> { 
    readonly List<T> src = new List<T>();
    public IReadOnlyList<T> Source => src.ToArray();
    readonly object locker = new object();
    
    public void forEach(Action<T> f) {
        lock(locker) {
            foreach(var l in src) { f(l); }
        }
    }

    public void Add(T o) { lock(locker) { src.Add(o); }  }
    public void Insert(int at, T o) { lock(locker) { src.Insert(at, o); } }
    public bool Remove(T o) { lock(locker) { return src.Remove(o); } }
    public void Clear() { lock(locker) { src.Clear(); } }
    public T First() { return src[0]; }
    public T Last() { return src[src.Count - 1]; }

    public int Count => src.Count;
    public T Get(int index) { return src[index]; }
}

public class lineHandler {
    readonly vtextbox parent;
    public readonly lockedList<line> list;
    public lineHandler(vtextbox p)
    {
        parent = p;
        list = new lockedList<line>();
        list.Add(DefaultLine());
        // lines.Add(DefaultLine());
    }


    // List<line> lines = new List<line>();
    // public IReadOnlyList<line> get => lines;
    public int Count => list.Count;
    public line Current => list.Get(lineIndex);

    public int MaxInd => list.Count - 1;
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
            list.Add(l);
        }
        else{
            list.Insert(lineIndex + 1, l);
        }
        LineIndex++;
    }
    public void Remove() {
        if(Current.CursorIndex > 0) {
            Current.Remove();
        }
        else if (Count > 1) {
            var curr = Current;
            curr.Left();
            list.Remove(curr);

            if(curr.Length > 0) {
                int cindex = Current.CursorIndex;
                Current.Insert(curr.Text);
                Current.CursorIndex = cindex;
            }
        }
    }

    public void SetText(string s) {
        var ls = s.Split('\n');
        list.Clear();
        foreach(var l in ls) { AddGo(l); }
    } 

    public void GoUp() {
        if(Current == list.First()) { Current.CursorIndex = 0; }
        else { LineIndex--; }
    }
    public void GoDown() {
        if(Current == list.Last()) { Current.CursorIndex = Current.Length; }        
        else { LineIndex++; }
    } 

    internal void printToConsole() {
#if DEBUG
        Console.WriteLine(); Console.WriteLine();
        
        int pad = 0;
        list.forEach(o => pad = Max(o.Length, pad));

        int i = 0;
        list.forEach(l => 
           Console.WriteLine($"{++i}. {('\"' + l.Text + '\"').PadRight(pad + 2)} [c:{l.CursorIndex}, l:{l.Length}, s:{l.se.Length}] {(Current == l ? "<" : "")} ")
        );
#endif
    }
}