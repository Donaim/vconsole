using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using static System.Math;

class vtextbox : Control {
    lineHandler lh = new lineHandler();

    public vtextbox() {
        // BackgroundColor = Color.Black;
        // Size = new Size(200, 200);
        Location = new Point(0, 0);
        Dock = DockStyle.Fill;
        
        DoubleBuffered = true;

        KeyPress += KeyPressH;
        PreviewKeyDown += PreviewKeyDownH;
    }

    protected override void OnPaint(PaintEventArgs e) {
        e.Graphics.Clear(Color.Red);

        // int h = 0;
        Point p = new Point(5, 5);
        foreach (var l in lh.get)
        {
            l.Draw(e.Graphics, p);
            p.Offset(0, l.Height);
        }
    }
    protected override void OnPaintBackground(PaintEventArgs e) { } //SHOULD BE EMPTY
    
    bool skip = false;
    private void PreviewKeyDownH(object sender, PreviewKeyDownEventArgs e)
    {
        skip = false;

        switch (e.KeyCode)
        {
            case Keys.Back:
                lh.Current.Remove();
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