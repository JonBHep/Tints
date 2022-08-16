using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tints;

public partial class ArrangeWin
{
    public ArrangeWin()
    {
        InitializeComponent();
    }

    private readonly List<string> _rainbow = new List<string>();
    private readonly List<ColourGroup> _colourGroups = new List<ColourGroup>();

    private void buttonClose_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        LoadOrder();
        DisplayGroupList();
        DisplayList();
    }

    private void DisplayGroupList()
    {
        GroupsListBox.Items.Clear();
        foreach (ColourGroup gp in _colourGroups)
        {
            ListBoxItem lbi = new ListBoxItem();
            TextBlock tbkGpNm = new TextBlock
            {
                Text = gp.Title,
                FontWeight = FontWeights.Bold
                ,
                VerticalAlignment = VerticalAlignment.Center
                ,
                Padding = new Thickness(8, 2, 8, 2)
            };
            lbi.Content = tbkGpNm;
            GroupsListBox.Items.Add(lbi);
        }
    }

    private void DisplayList(int sel = -1)
    {
        int gpIx = -1;
        ColoursListBox.Items.Clear();
        foreach (ColourGroup gp in _colourGroups)
        {
            gpIx++;
            int clIx = -1;

            ListBoxItem lbi = new ListBoxItem
            {
                Tag = $"G{gpIx}"
            };

            TextBlock tbkGpNm = new TextBlock
            {
                Text = gp.Title,
                FontWeight = FontWeights.Bold
                ,
                VerticalAlignment = VerticalAlignment.Center
                ,
                Padding = new Thickness(8, 2, 8, 2)
            };

            lbi.Content = tbkGpNm;

            ColoursListBox.Items.Add(lbi);
            string[] cList = gp.Colours;

            foreach (string clr in cList)
            {
                clIx++;
                Colour tint = new Colour(clr);

                lbi = new ListBoxItem
                {
                    Tag = $"C{clIx:000}G{gpIx}"
                };

                StackPanel spl = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                TextBlock tbkN = new TextBlock
                {
                    Text = $"{gp.Title} {clIx + 1}",
                    Width = 120
                    ,
                    FontWeight = FontWeights.Medium
                    ,
                    VerticalAlignment = VerticalAlignment.Center
                    ,
                    Padding = new Thickness(8, 2, 8, 2)
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
                    Text = clr,
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

                TextBlock tbkH = new TextBlock
                {
                    Text = tint.HexValue,
                    FontWeight = FontWeights.Normal
                    ,
                    VerticalAlignment = VerticalAlignment.Center
                    ,
                    Padding = new Thickness(8, 2, 8, 2)
                };

                spl.Children.Add(tbkN);
                spl.Children.Add(tbkC);
                spl.Children.Add(tbkT);
                spl.Children.Add(tbkV);
                spl.Children.Add(tbkH);
                lbi.Content = spl;
                ColoursListBox.Items.Add(lbi);
            }
        }

        ColoursListBox.SelectedIndex = sel;
    }

    private void SaveOrder()
    {
        string path = System.IO.Path.Combine(Jbh.AppManager.DataPath, "Groups.txt");
        using System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
        foreach (ColourGroup gp in _colourGroups)
        {
            if (!gp.IsEmpty)
            {
                sw.WriteLine(gp.Title);
                sw.WriteLine(gp.ColourListSpecification);
            }
        }
    }

    private void LoadOrder()
    {
        // Get list of System.Windows.Media.Color names (_rainbow).
        _rainbow.Clear();
        System.Reflection.PropertyInfo[] qr = typeof(Colors).GetProperties();
        foreach (System.Reflection.PropertyInfo cx in qr)
        {
            if (!cx.Name.Equals("Transparent"))
            {
                _rainbow.Add(cx.Name);
            }
        }

        string path = System.IO.Path.Combine(Jbh.AppManager.DataPath, "Groups.txt");
        if (System.IO.File.Exists(path))
        {
            using System.IO.StreamReader sr = new System.IO.StreamReader(path);
            _colourGroups.Clear();
            while (!sr.EndOfStream)
            {
                string? g = sr.ReadLine();
                string? c = sr.ReadLine();
                if (g is { })
                {
                    if (c is { })
                    {
                        ColourGroup gp = new ColourGroup(g)
                        {
                            ColourListSpecification = c
                        };
                        _colourGroups.Add(gp);        
                    }
                }
            }
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        SaveOrder();
    }

    private void buttonGroupAdd_Click(object sender, RoutedEventArgs e)
    {
        InputBoxWin ib = new InputBoxWin("Group title", "Name the new group", "")
        {
            Owner = this
        };
        if (ib.ShowDialog() == false)
        {
            return;
        }

        string tit = ib.ResponseText;
        tit = tit.ToUpper();
        if (string.IsNullOrWhiteSpace(tit))
        {
            return;
        }

        ColourGroup gp = new ColourGroup(tit);
        foreach (string c in _rainbow)
        {
            gp.Add(c);
        }

        _colourGroups.Add(gp);
        DisplayGroupList();
        DisplayList();
    }

    private void listboxGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        GpMvUpButton.IsEnabled = false;
        GpMvDnButton.IsEnabled = false;
        GroupEditButton.IsEnabled = false;
        int ix = GroupsListBox.SelectedIndex;
        if (ix < 0)
        {
            return;
        }

        GpMvUpButton.IsEnabled = (ix > 0);
        GpMvDnButton.IsEnabled = (ix < (_colourGroups.Count() - 1));
        GroupEditButton.IsEnabled = true;
    }

    private void buttonGpMvDn_Click(object sender, RoutedEventArgs e)
    {
        int ix = GroupsListBox.SelectedIndex;
        ColourGroup cg = _colourGroups[ix];
        _colourGroups.RemoveAt(ix);
        _colourGroups.Insert(ix + 1, cg);
        DisplayGroupList();
        DisplayList();
    }

    private void buttonGpMvUp_Click(object sender, RoutedEventArgs e)
    {
        int ix = GroupsListBox.SelectedIndex;
        ColourGroup cg = _colourGroups[ix];
        _colourGroups.RemoveAt(ix);
        _colourGroups.Insert(ix - 1, cg);
        DisplayGroupList();
        DisplayList();
    }

    private void buttonGroupEdit_Click(object sender, RoutedEventArgs e)
    {
        SaveOrder();
        int ix = GroupsListBox.SelectedIndex;
        ColourGroup cg = _colourGroups[ix];
        string tint = cg.Title;
        GroupEditWin w=new GroupEditWin(tint)
        {
            Owner = this
        };
        w.ShowDialog();
        LoadOrder();
        DisplayGroupList();
        DisplayList();
    }
}