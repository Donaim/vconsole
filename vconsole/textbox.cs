using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using static System.Math;

public class vtextbox : Control {
    protected readonly lineHandler lh;

    public vtextbox() {
        // BackgroundColor = Color.Black;
        // Size = new Size(200, 200);
        Location = new Point(0, 0);
        Dock = DockStyle.Fill;
        
        DoubleBuffered = true;

        KeyPress += KeyPressH;
        PreviewKeyDown += PreviewKeyDownH;

        lh = new lineHandler(this);
    }

    // FUNCTIONALITY

    public override string Text {
        get => string.Join("\n", lh.get);
        set => lh.SetText(value);
    }
    public int TextLength => Text.Length;
    public void AppendText(string s) 
    {
        lh.Current.Insert(s);
    }

    public int DX {get; set;} = 5;
    public int DY {get; set;} = 5;
    // GRAPHICS
    protected override void OnPaint(PaintEventArgs e) {
        e.Graphics.Clear(BackColor);
        Point p = new Point(DX, DY);

        int clipHeight = (int)e.Graphics.ClipBounds.Height - p.Y - p.Y;

        int h = 0, 
            hbc = 0; //height before cursor
        int i = 0;
        foreach(var l in lh.get) {
            h += l.Height;
            if(i < lh.LineIndex) { hbc = h; }
            i++;
        }

        int coff = -Max(0, (h - hbc) - clipHeight);

        int offset = -Max(0, (h + coff) - (clipHeight - 1));
        p.Offset(0, offset);
        
        // int h = 0;
        foreach (var l in lh.get)
        {
            l.Draw(e.Graphics, p);
            p.Offset(0, l.Height);
        }
    }
    protected override void OnPaintBackground(PaintEventArgs e) { } //SHOULD BE EMPTY
    

    // INPUT

    bool skip = false;
    private void PreviewKeyDownH(object sender, PreviewKeyDownEventArgs e)
    {
        skip = false;

        switch (e.KeyCode)
        {
            case Keys.Back:
                lh.Remove();
                break;
            case Keys.Escape:
                // Close();
                break;
            case Keys.Enter:
                // Close();
                lh.AddGo();
                break;

            case Keys.Left:
                lh.Current.Left();
                break;
            case Keys.Right:
                lh.Current.Right();
                break;
            case Keys.Down:
                lh.GoDown();
                break;
            case Keys.Up:
                lh.GoUp();
                break;

            case Keys.V when e.Control:
                AppendText(Clipboard.GetText());
                break;

            default: return;
        }

        // e.Handled = true;
        skip = true;
        Refresh();
        lh.printToConsole();
    }
    private void KeyPressH(object sender, KeyPressEventArgs e)
    {
        if(skip) { return; }

        switch (e.KeyChar)
        {
            // case '\b': return;
            case '\t': return;

            default: lh.Current.Insert(e.KeyChar); break;

            // default: text += e.KeyChar;
        }

        // wrong_input = false;
        // pass_chars.Add(e.KeyChar);
        Refresh();
        lh.printToConsole();
    }

}