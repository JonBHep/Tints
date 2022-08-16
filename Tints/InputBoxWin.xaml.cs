using System;
using System.Windows;

namespace Tints;

public partial class InputBoxWin
{
    public InputBoxWin(string boxTitle, string promptText, string defaultResponse)
    {
        InitializeComponent();
        Title = boxTitle;
        PromptsTextBlock.Text = promptText;
        ResponseTextBox.Text = defaultResponse;
    }
    
    public string ResponseText => ResponseTextBox.Text;

    private void buttonOkay_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void buttonCancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        Icon = this.Owner.Icon;
        ResponseTextBox.Focus();
    }
}