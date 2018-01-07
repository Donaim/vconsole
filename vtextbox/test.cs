using System;
using System.Drawing;
using System.Windows.Forms;

class Program{
    static void Main(string[] args){
        var window = new Form();
        var tbox = new vtextbox();
        window.Controls.Add(tbox);

        Application.Run(window);
    }
}