using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using static System.Math;

public partial class line{
    public class selection {
        readonly line parent;
        public selection(line p) {
            parent = p;
        }
        
        int start;
        bool Q;

        public int Start {
            get {
                if(Q) { return start; }
                else { return parent.CursorIndex; }
            }
        }
        public int Length => Abs(parent.CursorIndex - Start);

        public void BeginSelection() {
            start = parent.CursorIndex;
            Q = true;
        }
        public void EndSelection() {
            Q = false;
        }
    }
}

public partial class line {
    string s = "";
    public string Text => s;
    public int Length => s.Length;

    int cindex = 0;
    public int CursorIndex {
        get => cindex;
        set => cindex = Max(0, Min(value, s.Length));
    }
    public bool CursorVisible {get; set;} = true;
    public int MaxInd => Text.Length - 1;

    public readonly selection se;

    public readonly lineHandler lh;
    public line(lineHandler lh, string initS = "", int initIndex = 0) {
        s = initS;
        CursorIndex = initIndex;
        this.lh = lh;
        se = new selection(this);
    }


    // Functionality
    public void Insert(object obj) => Insert(obj.ToString());
    public void Insert(string what) 
    {
        var split = what.Split('\n'); 

        if(split.Length == 1) { insertHere(split[0]); }
        else{
            var end = this.Split();

            insertHere(split[0]);

            for(int i = 1; i < split.Length - 1; i++) {
                lh.AddGo(split[i]);
            }

            lh.AddGo(split.Last() + end);
        }
    }
    void insertHere(string what) {
        if(CursorIndex >= MaxInd) {
            s += what;
        } 
        else{
            s = s.Insert(CursorIndex, what); 
        }
        CursorIndex += what.Length; 
    }

    public void Right() {
        if(CursorIndex < Length) { CursorIndex++; }
        else if(lh.Current != lh.get.Last()) {
            lh.GoDown();
            lh.Current.CursorIndex = 0;
        }
    } 
    public void Left() {
        if(CursorIndex > 0) { CursorIndex--; }
        else if(lh.Current != lh.get.First()) {
            lh.GoUp();
            lh.Current.CursorIndex = lh.Current.Length;
        }
    }

    public string Split() { // fore new line
        if(CursorIndex <= 0) { return ""; }
        else if(CursorIndex > MaxInd) { return ""; }
        else {
            string re = s.Substring(CursorIndex);
            s = s.Remove(CursorIndex);
            return re; 
        }
    }
    public void Remove() {
        // if(SelectedLength <= 0) { SelectedLength++; }
        int len = se.Length;
        if(len <= 0) { len++; }

        if(CursorIndex > 0) {
            s = s.Remove(CursorIndex - 1, len);
            CursorIndex -= len;
            se.EndSelection();
        }
    }
    public void SelectRigth(int count) {
        // SelectedLength += count;
    }
    public void SelectLeft(int count) {
        // CursorIndex -= count;
        // SelectedLength += count;
    }

    // Graphics
    public Font font {get; set;} = new Font("Consolas", 12);
    public Brush brush {get; set;} = Brushes.Black;

    static readonly PointF zeroP = new PointF(0, 0);
    static readonly StringFormat measureFormat = new StringFormat( StringFormatFlags.MeasureTrailingSpaces );
    static readonly Graphics gserv = Graphics.FromImage(new Bitmap(400, 400));
    public int Width => (int) gserv.MeasureString(s, font, zeroP, measureFormat).Width;
    public int Height => (int) gserv.MeasureString("I", font, zeroP, measureFormat).Height;
    public int WidthBeforeCursor {
        get
        {
            if (CursorIndex <= 0) { return 0; }
            else if(CursorIndex > MaxInd) { return Width; }
            else { return (int) gserv.MeasureString(s.Remove(CursorIndex), font, zeroP, measureFormat ).Width; }
        }
    } 

    public Brush cursorBrush {get; set;} = Brushes.Blue;

    public void Draw(Graphics g, Point pos) {

        int offset = -Max(0, Width - ((int)g.ClipBounds.Width - pos.X - pos.X - 1));
        pos.Offset(offset,0);
        
        g.DrawString(s, font, brush, pos);

        if(lh.Current == this && CursorVisible) {
            g.FillRectangle(cursorBrush, pos.X + WidthBeforeCursor + 1, pos.Y, 1, Height);
        }
    }
}
