using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using static System.Math;

class line {
    string s = "";
    public string Text => s;
    public int Length => s.Length;
    public void Insert(object obj) => Insert(obj.ToString());
    public void Insert(string what) 
    {
        if(cindex >= MaxInd) {
            s += what;
        } 
        else{
            s = s.Insert(cindex, what); 
        }
        CursorIndex += what.Length; 
    }

    int cindex = 0;
    public int MaxInd => Text.Length - 1;
    public int CursorIndex {
        get => cindex;
        set => cindex = Max(0, Min(value, s.Length));
    }
    public void Right() {
        if(cindex < MaxInd) { CursorIndex++; }
        else if(lh.Current != lh.get.Last()) {
            lh.GoDown();
        }
    } 
    public void Left() {
        if(cindex > 0) { CursorIndex--; }
        else if(lh.Current != lh.get.First()) {
            lh.GoUp();
        }
    }
   

    public string Split() { // fore new line
        if(cindex <= 0) { return ""; }
        else if(cindex > MaxInd) { return ""; }
        else {
            string re = s.Substring(cindex);
            s = s.Remove(cindex);
            return re; 
        }
    }
    public void Remove() {
        Remove(1);
    }
    void Remove(int count) {
        if(CursorIndex > 0) {
            s = s.Remove(cindex - 1, count);
            CursorIndex -= count;
        }
    }

    public readonly lineHandler lh;
    public line(lineHandler lh, string initS = "", int initIndex = 0) {
        s = initS;
        CursorIndex = initIndex;
        this.lh = lh;
    }

    public Font font {get; set;} = new Font("Consolas", 12);
    public Brush brush {get; set;} = Brushes.Black;

    static readonly PointF zeroP = new PointF(0, 0);
    static readonly StringFormat measureFormat = new StringFormat( StringFormatFlags.MeasureTrailingSpaces );
    static readonly Graphics gserv = Graphics.FromImage(new Bitmap(400, 400));
    public int Width => (int) gserv.MeasureString(s, font, zeroP, measureFormat).Width;
    public int Height => (int) gserv.MeasureString(s, font).Height;
    public int WidthBeforeCursor {
        get
        {
            if (cindex <= 0) { return 0; }
            else if(cindex > MaxInd) { return Width; }
            else { return (int) gserv.MeasureString(s.Remove(cindex), font, zeroP, measureFormat ).Width; }
        }
    } 

    public Brush cursorBrush {get; set;} = Brushes.Blue;

    public void Draw(Graphics g, Point pos) {

        int offset = -Max(0, Width - ((int)g.ClipBounds.Width - pos.X - pos.X - 1));
        pos.Offset(offset,0);
        
        g.DrawString(s, font, brush, pos);

        if(lh.Current == this) {
            g.FillRectangle(cursorBrush, pos.X + WidthBeforeCursor + 1, pos.Y, 1, Height);
        }
    }
}
