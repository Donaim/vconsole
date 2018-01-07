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
    protected internal virtual void textChanged() {}
    public override string Text {
        get => string.Join("\n", lh.list.Source.Select(o => o.Text));
        set => lh.SetText(value);
    }
    public int TextLength => Text.Length;
    public virtual void AppendText(string s, bool refresh = true) 
    {
        lh.Current.Insert(s);
        if(refresh) { Refresh(); }
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
        lh.list.forEach(l => {
            h += l.Height;
            if(i < lh.LineIndex) { hbc = h; }
            i++;
        });

        int coff = -Max(0, (h - hbc) - clipHeight);

        int offset = -Max(0, (h + coff) - (clipHeight - 1));
        p.Offset(0, offset);
        
        lh.list.forEach(l => {
            l.Draw(e.Graphics, p);
            p.Offset(0, l.Height);
        });
    }
    protected override void OnPaintBackground(PaintEventArgs e) { } //SHOULD BE EMPTY
    

    // INPUT

    protected virtual void BeforeKeyDown(PreviewKeyDownEventArgs e, ref bool skipPress, ref bool skipNavigate, ref bool skipKeyDown) {}
    bool skipPress = false;
    private void PreviewKeyDownH(object sender, PreviewKeyDownEventArgs e)
    {
        skipPress = false;
        bool skipKeyDown = false, skipNavigate = false;
        BeforeKeyDown(e, ref skipPress, ref skipNavigate, ref skipKeyDown);
        
        if(!skipNavigate) { if(PreviewNavigate(e)) { return; }}
        if(skipKeyDown) { return; }

        switch (e.KeyCode)
        {
            case Keys.Back:
                lh.Remove();
                break;
            case Keys.Enter:
                // Close();
                lh.AddGo();
                break;

            case Keys.V when e.Control:
                AppendText(Clipboard.GetText());
                break;

            default: return;
        }

        skipPress = true;
        Refresh();
        lh.printToConsole();
    }
    bool PreviewNavigate(PreviewKeyDownEventArgs e)
    {
        switch (e.KeyCode)
        {
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

            default: return false;
        }


        skipPress = true;
        Refresh();
        return true;
    }

    protected virtual void BeforeKeyPress(KeyPressEventArgs e, ref bool skipPress) {}
    private void KeyPressH(object sender, KeyPressEventArgs e)
    {
        BeforeKeyPress(e, ref skipPress);
        if(skipPress) { return; }

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