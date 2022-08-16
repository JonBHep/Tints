using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Tints;

public partial class ListWin
{
    public ListWin()
    {
        InitializeComponent();
    }

    private readonly List<Colour> _palette = new List<Colour>();

    private void DisplayList()
    {
        ColoursListBox.Items.Clear();
        foreach (Colour tint in _palette)
        {
            ListBoxItem lbi = new ListBoxItem();

            StackPanel spl = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            TextBlock tbkC = new TextBlock
            {
                Height = 30,
                Width = 200
                ,
                Background = new SolidColorBrush(tint.ColorValue)
            };

            TextBlock tbkT = new TextBlock
            {
                Text = tint.Title,
                FontWeight = FontWeights.Medium
                ,
                VerticalAlignment = VerticalAlignment.Center
                ,
                Padding = new Thickness(8, 2, 8, 2)
            };

            TextBlock tbkV = new TextBlock();
            tbkV.Inlines.Add(tint.Values);
            tbkV.FontWeight = FontWeights.Normal;
            tbkV.VerticalAlignment = VerticalAlignment.Center;
            tbkV.Padding = new Thickness(8, 2, 8, 2);

            spl.Children.Add(tbkC);
            spl.Children.Add(tbkT);
            spl.Children.Add(tbkV);
            lbi.Content = spl;
            ColoursListBox.Items.Add(lbi);
        }
    }


    private void buttonClose_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        List<string> rainbow = new List<string>();
        System.Reflection.PropertyInfo[] qr = typeof(Colors).GetProperties();
        foreach (System.Reflection.PropertyInfo cx in qr)
        {
            if (!cx.Name.Equals("Transparent"))
            {
                rainbow.Add(cx.Name);
            }
        }

        foreach (string c in rainbow)
        {
            Colour colr = new Colour(c);
            _palette.Add(colr);
        }

        DisplayList();
    }

    private void listboxColours_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int i = ColoursListBox.SelectedIndex;
        if (i < 0)
        {
            return;
        }

        GroupsListBox.Items.Clear();
        Colour tint = _palette[i];
        float r = tint.ColorValue.ScR;
        float g = tint.ColorValue.ScG;
        float b = tint.ColorValue.ScB;
        float amp = 0;
        bool stopper = false;
        while (!stopper)
        {
            amp += 0.02F;
            float rr = r * amp;
            float gg = g * amp;
            float bb = b * amp;
            if ((rr <= 1) && (gg <= 1) && (bb <= 1))
            {
                Color nova = Color.FromScRgb(1, rr, gg, bb);
                ListBoxItem lbi = new ListBoxItem();

                StackPanel spl = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                TextBlock tbkC = new TextBlock
                {
                    Height = 30,
                    Width = 200
                    ,
                    Background = new SolidColorBrush(nova)
                };

                TextBlock tbkV = new TextBlock();
                Run rn = new Run() {Text = nova.R.ToString(), Foreground = Brushes.Red};
                tbkV.Inlines.Add(rn);
                rn = new Run() {Text = " " + nova.G.ToString(), Foreground = Brushes.Green};
                tbkV.Inlines.Add(rn);
                rn = new Run() {Text = " " + nova.B.ToString(), Foreground = Brushes.Blue};
                tbkV.Inlines.Add(rn);
                tbkV.FontWeight = FontWeights.Normal;
                tbkV.VerticalAlignment = VerticalAlignment.Center;
                tbkV.Padding = new Thickness(8, 2, 8, 2);

                spl.Children.Add(tbkC);
                spl.Children.Add(tbkV);
                lbi.Content = spl;
                GroupsListBox.Items.Add(lbi);
            }
            else
            {
                stopper = true;
            }
        }

        GroupsListBox.ScrollIntoView(GroupsListBox.Items[^1]);
    }
}