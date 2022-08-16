using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Tints;

public class Colour : IComparable<Colour>
{
    private readonly float _darkness;
    private readonly float _lightness;
    private Color _tint;

    public Colour(string title)
    {
        Title = title;
        _tint = (Color) ColorConverter.ConvertFromString(title);
        _darkness = _tint.ScA + _tint.ScB + _tint.ScG + _tint.ScR;
        _lightness = 4 - _darkness;
    }

    public string Title { get; }

    public int CompareTo(Colour? other)
    {
        if (other is { })
        {
            return _lightness.CompareTo(other._lightness);    
        }

        return 0;
    }

    public Color ColorValue => _tint;

    public float Lightness => _lightness;

    public float Darkness => _darkness;

    public string HexValue => _tint.ToString();

    public Span Values
    {
        get
        {
            Span sp = new Span();
            Run rrv = new Run(_tint.R.ToString())
            {
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Red)
            };
            sp.Inlines.Add(rrv);
            Run rgv = new Run(" " + _tint.G.ToString())
            {
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Green)
            };
            sp.Inlines.Add(rgv);
            Run rbv = new Run(" " + _tint.B.ToString())
            {
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Blue)
            };
            sp.Inlines.Add(rbv);
            if (_tint.A < 255)
            {
                Run rav = new Run(" " + _tint.A.ToString())
                {
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.Gray)
                };
                sp.Inlines.Add(rav);
            }

            return sp;
        }
    }

    public bool SameValueAs(Colour alien)
    {
        bool flag = ColorValue.A == alien.ColorValue.A;

        if (ColorValue.R != alien.ColorValue.R)
        {
            flag = false;
        }

        if (ColorValue.G != alien.ColorValue.G)
        {
            flag = false;
        }

        if (ColorValue.B != alien.ColorValue.B)
        {
            flag = false;
        }

        return flag;
    }
}
