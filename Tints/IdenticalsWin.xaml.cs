using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tints;

public partial class IdenticalsWin
{
    public IdenticalsWin()
    {
        InitializeComponent();
    }
    
    private void Window_ContentRendered(object sender, EventArgs e)
    {
        List<string> rainbow=new List<string>();
        System.Reflection.PropertyInfo[] qr = typeof(Colors).GetProperties();
        foreach (System.Reflection.PropertyInfo cx in qr)
        {
            if (!cx.Name.Equals("Transparent"))
            {
                rainbow.Add(cx.Name);
            }
        }
        List<Colour> palette = new List<Colour>();
        foreach(string c in rainbow)
        {
            Colour colr = new Colour(c);
            palette.Add(colr);
        }
        for (int a = 0; a < (palette.Count-1); a++)
        {
            for (int b = a + 1; b < palette.Count; b++)
            {
                if (palette[a].SameValueAs(palette[b]))
                {
                    string q = palette[a].Title + " is the same as " + palette[b].Title;
                    ListBoxItem lbi = new ListBoxItem();
                    TextBlock tbk = new TextBlock
                    {
                        Text = q
                    };
                    lbi.Content = tbk;
                    IdenticalsListBox.Items.Add(lbi);
                }
            }
        }
    }
}