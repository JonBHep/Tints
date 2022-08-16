using System.Collections.Generic;

namespace Tints;

public class ColourGroup
{
    public string Title { get; }
        private readonly List<string> _colourList;
        public string[] Colours
        {
            get
            {
                string[] u = new string[_colourList.Count];
                _colourList.CopyTo(u);
                return u;
            }
        }
        public string ColourListSpecification
        {
            get
            {
                string[] q = _colourList.ToArray();
                return string.Join("#", q);
            }
            init
            {
                string[] interim = value.Split("#".ToCharArray());
                foreach (string c in interim) { _colourList.Add(c); }
            }
        }
        public ColourGroup(string groupTitle)
        {
            Title = groupTitle;
            _colourList = new List<string>();
        }
        public bool IsEmpty => (_colourList.Count == 0);

        public void Add(string colourName)
        {
            _colourList.Add(colourName);
        }
        public void Remove(int colourIndex)
        {
            _colourList.RemoveAt(colourIndex);
        }
        public bool Contains(string colourName)
        {
            return _colourList.Contains(colourName);
        }
        public void Promote(int colourIndex)
        {
            if (colourIndex > 0)
            {
                string colourName = _colourList[colourIndex];
                _colourList.RemoveAt(colourIndex);
                _colourList.Insert(colourIndex - 1, colourName);
            }
        }
        public void Demote(int colourIndex)
        {
            if (colourIndex < (_colourList.Count - 1))
            {
                string colourName = _colourList[colourIndex];
                _colourList.RemoveAt(colourIndex);
                _colourList.Insert(colourIndex + 1, colourName);
            }
        }
        public void SortByLightness()
        {
            List<Colour> tintList = new List<Colour>();
            foreach (string clr in _colourList)
            {
                Colour t = new Colour(clr);
                tintList.Add(t);
            }
            tintList.Sort();
            _colourList.Clear();
            foreach (Colour t in tintList) { _colourList.Add(t.Title); }
        }
}