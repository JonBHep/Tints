using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tints
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private struct Couleur
        {
            public string ColourName { get; set; }
            public bool IsBlue { get; set; }
            public bool IsGreen { get; set; }
            public bool IsRed { get; set; }
            public bool IsYellow { get; set; }
            public bool IsNeutral { get; set; }
            public bool VisibleAgainstBlack { get; set; }
            public bool VisibleAgainstWhite { get; set; }
        }

        private class ColourBlock
        {
            public StackPanel Panel { get; init; }
            public int BlockIndex { get; init; }

            public ColourBlock()
            {
                Panel = new StackPanel();
            }
        }

        private readonly List<Couleur> _colours = new List<Couleur>();
        private List<ColourBlock> _blocks = new List<ColourBlock>();
        private SolidColorBrush _textColour = Brushes.Black;

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Jbh.AppManager.DataPath)) { MessageBox.Show("Path to data is not found.\n\nColours will close.", "Databank is not accessible", MessageBoxButton.OK, MessageBoxImage.Error); Close(); }

            BackCombo.Items.Clear();
            
            BackCombo.Items.Add("White");
            BackCombo.Items.Add("Silver");
            BackCombo.Items.Add("Mid-grey");
            BackCombo.Items.Add("Black");

            WrapWrapPanel.Background = Brushes.Silver;
            _textColour = Brushes.Lavender;
            
            LoadColours();
            ShowColours();

            GroupCombo.Items.Clear();
            GroupCombo.Items.Add("All");
            GroupCombo.Items.Add("Blue / Purple");
            GroupCombo.Items.Add("Green");
            GroupCombo.Items.Add("Red / Brown");
            GroupCombo.Items.Add("Yellow");
            GroupCombo.Items.Add("Neutral");
            GroupCombo.Items.Add("Visible against black");
            GroupCombo.Items.Add("Visible against white");
            GroupCombo.SelectedIndex = 0;
            BackCombo.SelectedIndex = 1;
            
        }

        private void SaveColours()
        {
            string pth = System.IO.Path.Combine(Jbh.AppManager.DataPath, "Colours.txt");
            using System.IO.StreamWriter sw = new System.IO.StreamWriter(pth);
            foreach (Couleur c in _colours)
            {
                sw.WriteLine(c.ColourName);
                sw.WriteLine(c.IsBlue);
                sw.WriteLine(c.IsGreen);
                sw.WriteLine(c.IsNeutral);
                sw.WriteLine(c.IsRed);
                sw.WriteLine(c.IsYellow);
                sw.WriteLine(c.VisibleAgainstBlack);
                sw.WriteLine(c.VisibleAgainstWhite);
            }
        }

        private void LoadColours()
        {
            string pth = System.IO.Path.Combine(Jbh.AppManager.DataPath, "Colours.txt");
            using System.IO.StreamReader sr = new System.IO.StreamReader(pth);
            _colours.Clear();
            bool inptBool;
            while (!sr.EndOfStream)
            {
                Couleur c = new Couleur();
                
                var r = sr.ReadLine();
                if (r is { })
                {
                    c.ColourName = r;    
                }
                r = sr.ReadLine();
                if (r is { })
                {
                    if (bool.TryParse(r, out inptBool)) { c.IsBlue = inptBool; }    
                }
                r = sr.ReadLine();
                if (r is { })
                {
                    if (bool.TryParse(r, out inptBool)) { c.IsGreen = inptBool; }    
                }
                r = sr.ReadLine();
                if (r is { })
                {
                    if (bool.TryParse(r, out inptBool)) { c.IsNeutral = inptBool; }    
                }
                r = sr.ReadLine();
                if (r is { })
                {
                    if (bool.TryParse(r, out inptBool)) { c.IsRed = inptBool; }    
                }
                r = sr.ReadLine();
                if (r is { })
                {
                    if (bool.TryParse(r, out inptBool)) { c.IsYellow = inptBool; }    
                }
                r = sr.ReadLine();
                if (r is { })
                {
                    if (bool.TryParse(r, out inptBool)) { c.VisibleAgainstBlack = inptBool; }    
                }
                r = sr.ReadLine();
                if (r is { })
                {
                    if (bool.TryParse(r, out inptBool)) { c.VisibleAgainstWhite = inptBool; }    
                }
                
                _colours.Add(c);
            }
        }

        private void ShowColours()
        {
            _blocks = new List<ColourBlock>();
            WrapWrapPanel.Children.Clear();
            for (int x = 0; x < _colours.Count; x++)
            {
                Couleur c = _colours[x];
                Color cc = (Color)ColorConverter.ConvertFromString(c.ColourName);
                SolidColorBrush pinceau = new SolidColorBrush(cc);

                StackPanel stkpnl = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Width = 200
                    ,
                    Height = 40
                };

                TextBlock tbColour = new TextBlock
                {
                    Width = 60,
                    Height = 20
                    ,
                    VerticalAlignment = VerticalAlignment.Center
                    ,
                    Margin = new Thickness(4)
                    ,
                    Background = pinceau
                };

                TextBlock tbCaption = new TextBlock
                {
                    Width = 120,
                    Height = 20
                };
                tbColour.VerticalAlignment = VerticalAlignment.Center;
                tbCaption.Margin = new Thickness(4);
                tbCaption.Text = c.ColourName;
                tbCaption.Foreground = _textColour;

                stkpnl.Children.Add(tbColour);
                stkpnl.Children.Add(tbCaption);
                WrapWrapPanel.Children.Add(stkpnl);

                ColourBlock blk = new ColourBlock
                {
                    BlockIndex = x,
                    Panel = stkpnl
                };
                _blocks.Add(blk);

                ComboBoxItem cbi = new ComboBoxItem() { Tag = pinceau };
                StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal, Height = 24 };
                TextBlock tbCbColour = new TextBlock() { Width = 64, Height = 20, Text = string.Empty, Background = pinceau };
                TextBlock tbCbCaption = new TextBlock() { Height = 20, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(4), Text = c.ColourName };
                sp.Children.Add(tbCbColour);
                sp.Children.Add(tbCbCaption);
                cbi.Content = sp;
                ComboBoxBk1.Items.Add(cbi);

                cbi = new ComboBoxItem() { Tag = pinceau };
                sp = new StackPanel() { Orientation = Orientation.Horizontal, Height = 24 };
                tbCbColour = new TextBlock() { Width = 64, Height = 20, Text = string.Empty, Background = pinceau };
                tbCbCaption = new TextBlock() { Height = 20, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(4), Text = c.ColourName };
                sp.Children.Add(tbCbColour);
                sp.Children.Add(tbCbCaption);
                cbi.Content = sp;
                ComboBoxBk2.Items.Add(cbi);

                cbi = new ComboBoxItem() { Tag = pinceau };
                sp = new StackPanel() { Orientation = Orientation.Horizontal, Height = 24 };
                tbCbColour = new TextBlock() { Width = 64, Height = 20, Text = string.Empty, Background = pinceau };
                tbCbCaption = new TextBlock() { Height = 20, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(4), Text = c.ColourName };
                sp.Children.Add(tbCbColour);
                sp.Children.Add(tbCbCaption);
                cbi.Content = sp;
                ComboBoxFg1.Items.Add(cbi);

                cbi = new ComboBoxItem() { Tag = pinceau };
                sp = new StackPanel() { Orientation = Orientation.Horizontal, Height = 24 };
                tbCbColour = new TextBlock() { Width = 64, Height = 20, Text = string.Empty, Background = pinceau };
                tbCbCaption = new TextBlock() { Height = 20, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(4), Text = c.ColourName };
                sp.Children.Add(tbCbColour);
                sp.Children.Add(tbCbCaption);
                cbi.Content = sp;
                ComboBoxFg2.Items.Add(cbi);

            }
            MakeSelection();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cboBack_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (BackCombo.SelectedItem.ToString())
            {
                case "White":
                    WrapWrapPanel.Background=Brushes.White;
                    _textColour=new SolidColorBrush(Colors.Black);
                    ShowColours();
                    break;
                case "Silver":
                    WrapWrapPanel.Background=Brushes.Silver;
                    _textColour=new SolidColorBrush(Colors.Black);
                    ShowColours();
                    break;
                case "Black":
                    WrapWrapPanel.Background=Brushes.Black;
                    _textColour=new SolidColorBrush(Colors.Ivory);
                    ShowColours();
                    break;
                case "Mid-grey":
                    WrapWrapPanel.Background = Brushes.Gray;
                    _textColour = new SolidColorBrush(Colors.Lavender);
                    ShowColours();
                    break;

            }
        }

        private void cboGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MakeSelection();
        }

        private void MakeSelection()
        {
            switch (GroupCombo.SelectedIndex)
            {
                case 0:
                    {
                        foreach (ColourBlock blk in _blocks)
                        { blk.Panel.Visibility = Visibility.Visible; }
                        FindTextBox.Visibility = Visibility.Visible;
                        FindButton.Visibility = Visibility.Visible;
                        break;
                    }
                case 1:
                    {
                        foreach (ColourBlock blk in _blocks)
                        {
                            blk.Panel.Visibility = _colours[blk.BlockIndex].IsBlue ? Visibility.Visible : Visibility.Collapsed;
                        }
                        FindTextBox.Visibility = Visibility.Hidden;
                        FindButton.Visibility = Visibility.Hidden;
                        break;
                    }
                case 2:
                    {
                        foreach (ColourBlock blk in _blocks)
                        {
                            blk.Panel.Visibility = _colours[blk.BlockIndex].IsGreen ? Visibility.Visible : Visibility.Collapsed;
                        }
                        FindTextBox.Visibility = Visibility.Hidden;
                        FindButton.Visibility = Visibility.Hidden;
                        break;
                    }
                case 3:
                    {
                        foreach (ColourBlock blk in _blocks)
                        {
                            blk.Panel.Visibility = _colours[blk.BlockIndex].IsRed ? Visibility.Visible : Visibility.Collapsed;
                        }
                        FindTextBox.Visibility = Visibility.Hidden;
                        FindButton.Visibility = Visibility.Hidden;
                        break;
                    }
                case 4:
                    {
                        foreach (ColourBlock blk in _blocks)
                        {
                            blk.Panel.Visibility = _colours[blk.BlockIndex].IsYellow ? Visibility.Visible : Visibility.Collapsed;
                        }
                        FindTextBox.Visibility = Visibility.Hidden;
                        FindButton.Visibility = Visibility.Hidden;
                        break;
                    }
                case 5:
                    {
                        foreach (ColourBlock blk in _blocks)
                        {
                            blk.Panel.Visibility = _colours[blk.BlockIndex].IsNeutral ? Visibility.Visible : Visibility.Collapsed;
                        }
                        FindTextBox.Visibility = Visibility.Hidden;
                        FindButton.Visibility = Visibility.Hidden;
                        break;
                    }
                case 6:
                    {
                        foreach (ColourBlock blk in _blocks)
                        {
                            blk.Panel.Visibility = _colours[blk.BlockIndex].VisibleAgainstBlack ? Visibility.Visible : Visibility.Collapsed;
                        }
                        FindTextBox.Visibility = Visibility.Hidden;
                        FindButton.Visibility = Visibility.Hidden;
                        break;
                    }
                case 7:
                    {
                        foreach (ColourBlock blk in _blocks)
                        {
                            blk.Panel.Visibility = _colours[blk.BlockIndex].VisibleAgainstWhite ? Visibility.Visible : Visibility.Collapsed;
                        }
                        FindTextBox.Visibility = Visibility.Hidden;
                        FindButton.Visibility = Visibility.Hidden;
                        break;
                    }
            }
        }
        private void textboxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            FindButton.IsEnabled = (!string.IsNullOrWhiteSpace(FindTextBox.Text));
        }

        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            GroupCombo.SelectedIndex = 0;
            string find = FindTextBox.Text.ToLower();
            foreach (ColourBlock blk in _blocks)
            {
                blk.Panel.Visibility = _colours[blk.BlockIndex].ColourName.ToLower().Contains(find) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void buttonArrange_Click(object sender, RoutedEventArgs e)
        {
            ArrangeWin w = new ArrangeWin
            {
                Owner = this
            };
            w.ShowDialog();
        }

        private void buttonIdenticals_Click(object sender, RoutedEventArgs e)
        {
            IdenticalsWin w = new IdenticalsWin
            {
                Owner = this
            };
            w.ShowDialog();
        }

        private void buttonListAll_Click(object sender, RoutedEventArgs e)
        {
            ListWin w = new ListWin() { Owner = this };
            w.ShowDialog();
        }

        private void ComboBoxBk1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxBk1.SelectedItem is ComboBoxItem {Tag: SolidColorBrush scb})
            {
                PanelBackOne.Background = scb;    
            }
        }

        private void ComboBoxBk2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxBk2.SelectedItem is ComboBoxItem {Tag: SolidColorBrush scb})
            {
                PanelBackTwo.Background = scb;    
            }
        }

        private void ComboBoxFg1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFg1.SelectedItem is ComboBoxItem {Tag: SolidColorBrush scb})
            {
                PanelFront1A.Foreground = scb;
                PanelFront1B.Foreground = scb;
            }
        }

        private void ComboBoxFg2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFg2.SelectedItem is ComboBoxItem {Tag: SolidColorBrush scb})
            {
                PanelFront2A.Foreground = scb;
                PanelFront2B.Foreground = scb;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveColours();
        }

        // TODO implement a window for editing colours
    }
}