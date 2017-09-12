using System;

namespace StyleChooser.Models
{
    public class ClickCounter
    {
        public string Style { get; }
        private int _clickCount;
        private const int Size = 10;
        private readonly int[] _arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int _index;
        private readonly Random _random;

        public ClickCounter(string style, Random rand)
        {
            _random = rand;
            Style = style;
            _clickCount = 0;
            _index = 0;
            _arr = Shuffle(_arr);
        }

        public int[] Shuffle(int[] array)
        {
            for (var i = array.Length; i > 1; i--)
            {
                var j = _random.Next(i);
                var tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
            return array;
        }

        public void IncClickCount()
        {
            _clickCount++;
        }

        public int GetClickCount()
        {
            return _clickCount;
        }

        public string GetImagePath()
        {
            var path = @"/Content/styles/" + Style + @"/" + Style.ToLower() + _arr[_index] + ".jpg";
            _index = _index < Size - 1 ? ++_index : 0;
            return path;
        }
    }
}