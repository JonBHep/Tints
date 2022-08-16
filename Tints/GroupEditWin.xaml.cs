using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tints;

public partial class GroupEditWin
{
    public GroupEditWin(string groupTitle)
    {
        InitializeComponent();
        _editingGroup = groupTitle;
    }

    private readonly List<string> _rainbow = new List<string>();
    private readonly List<ColourGroup> _colourGroups = new List<ColourGroup>();
    private readonly string _editingGroup;
    private int _editingGroupIndex;

    private void buttonClose_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        GroupTitleTextBlock.Text = _editingGroup + " GROUP";
        LoadOrder();
        DisplayList();
    }

    private void DisplayList(int sel = -1)
    {
        int clIx = -1;
        ColourGroup cgp = _colourGroups[0];
        foreach (ColourGroup gp in _colourGroups)
        {
            if (gp.Title.Equals(_editingGroup))
            {
                cgp = gp;
            }
        }

        ColoursListBox.Items.Clear();
        foreach (string clr in cgp.Colours)
        {
            clIx++;
            ListBoxItem lbi = new ListBoxItem
            {
                Tag = clr
            };
            Colour tint = new Colour(clr);
            StackPanel spl = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            TextBlock tbkN = new TextBlock
            {
                Text = $"{_editingGroup} {clIx}",
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
                Text = $"Darkness = {tint.Lightness}",
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

        ColoursListBox.SelectedIndex = sel;

        UnusedListBox.Items.Clear();
        foreach (string clr in _rainbow)
        {
            if (!cgp.Contains(clr))
            {
                ListBoxItem lbi = new ListBoxItem
                {
                    Tag = clr
                };

                StackPanel spl = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                TextBlock tbkC = new TextBlock
                {
                    Height = 30,
                    Width = 200
                };
                Color cc = (Color) ColorConverter.ConvertFromString(clr);
                tbkC.Background = new SolidColorBrush(cc);

                TextBlock tbkT = new TextBlock
                {
                    Text = clr,
                    FontWeight = FontWeights.Medium
                    ,
                    VerticalAlignment = VerticalAlignment.Center
                    ,
                    Padding = new Thickness(8, 2, 8, 2)
                };

                spl.Children.Add(tbkC);
                spl.Children.Add(tbkT);
                lbi.Content = spl;
                UnusedListBox.Items.Add(lbi);
            }
        }
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
                string? title = sr.ReadLine();
                string? c = sr.ReadLine();
                if (title is { })
                {
                    if (c is { })
                    {
                        ColourGroup gp = new ColourGroup(title)
                        {
                            ColourListSpecification = c
                        };
                        _colourGroups.Add(gp);        
                    }
                }
            }
        }

        int g = -1;
        foreach (ColourGroup gp in _colourGroups)
        {
            g++;
            if (gp.Title.Equals(_editingGroup))
            {
                _editingGroupIndex = g;
            }
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        SaveOrder();
    }

    private void buttonMvDn_Click(object sender, RoutedEventArgs e)
    {
        int ix = ColoursListBox.SelectedIndex;
        _colourGroups[_editingGroupIndex].Demote(ix);
        DisplayList(ix + 1);
    }

    private void buttonMvUp_Click(object sender, RoutedEventArgs e)
    {
        int ix = ColoursListBox.SelectedIndex;
        _colourGroups[_editingGroupIndex].Promote(ix);
        DisplayList(ix - 1);
    }

    private void listboxColours_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RemoveColourButton.IsEnabled = false;
        MvUpButton.IsEnabled = false;
        MvDnButton.IsEnabled = false;
        int sel = ColoursListBox.SelectedIndex;

        if (sel < 0)
        {
            return;
        }

        RemoveColourButton.IsEnabled = true;
        MvUpButton.IsEnabled = (sel > 0);
        MvDnButton.IsEnabled = (sel < (ColoursListBox.Items.Count - 1));
    }

    private void buttonRemove_Click(object sender, RoutedEventArgs e)
    {
        int ix = ColoursListBox.SelectedIndex;
        _colourGroups[_editingGroupIndex].Remove(ix);
        DisplayList(ix);
    }

    private void buttonAddColour_Click(object sender, RoutedEventArgs e)
    {
        if (UnusedListBox.SelectedItem is ListBoxItem {Tag: string clr})
        {
            _colourGroups[_editingGroupIndex].Add(clr);
            DisplayList();
        }
    }

    private void listboxUnused_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        AddColourButton.IsEnabled = (UnusedListBox.SelectedIndex >= 0);
    }

    private void buttonSort_Click(object sender, RoutedEventArgs e)
    {
        _colourGroups[_editingGroupIndex].SortByLightness();
        DisplayList();
    }
}