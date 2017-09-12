using System;
using System.Collections.Generic;
using System.Linq;

namespace StyleChooser.Models
{
    public class ImageHandler
    {
        private const int Min = 0;
        private int _firstImgIndex;
        private int _secondImgIndex;
        private int _step;
        private static readonly Random Rand = new Random();
        private readonly ClickCounter[] _allStyles;
        private readonly Dictionary<int, ImageData> _history = new Dictionary<int, ImageData>();
        public int Max { get; }
        public bool HasWinner { get; set; }

        public ImageHandler(IReadOnlyList<string> styles)
        {
            HasWinner = false;
            Max = styles.Count;
            _firstImgIndex = 0;
            _secondImgIndex = 0;
            _step = 1;
            _allStyles = new ClickCounter[Max];
            for (var i = 0; i < Max; i++)
            {
                _allStyles[i] = new ClickCounter(styles[i], Rand);
            }
        }

        public void ResetStep()
        {
            _step = 1;
        }

        public int GetStyleClickCount(int i)
        {
            if (i >= Min && i < Max)
                return _allStyles[i].GetClickCount();
            throw new IndexOutOfRangeException();
        }

        public string GetStyleName(int i)
        {
            if (i >= Min && i < Max)
                return _allStyles[i].Style;
            throw new IndexOutOfRangeException();
        } 

        public ImageData GetImageData()
        {
            return _history[_step-1];
        }

        public void SetUserChoice(int choice)
        {
            if (choice >= Min && choice < Max)
            {
                _allStyles[choice].IncClickCount();
            }
        }

        private List<ClickCounter> GetWinList()
        {
            var max = _allStyles.Max(c => c.GetClickCount());
            return (from m in _allStyles
                    where m.GetClickCount() == max
                    select m).ToList();
        } 

        public string GetWinner()
        {
            return GetWinList().First().Style;
        }

        public void NextStep()
        {
            if (_step >= 11)
            {
                var winList = GetWinList();
                if (winList.Count == 1)
                {
                    HasWinner = true;
                }
                else
                {
                    for (var i = Min; i < Max; i++)
                    {
                        if (_allStyles[i].Style == winList[0].Style)
                        {
                            _firstImgIndex = i;
                            break;
                        }
                    }
                    for (var i = Min; i < Max; i++)
                    {
                        if (_allStyles[i].Style == winList[1].Style)
                        {
                            _secondImgIndex = i;
                            break;
                        }
                    }

                    _history[_step] = new ImageData
                    {
                        FirstImagePath = _allStyles[_firstImgIndex].GetImagePath(),
                        SecondImagePath = _allStyles[_secondImgIndex].GetImagePath(),
                        FirstImageName = _allStyles[_firstImgIndex].Style,
                        SecondImageName = _allStyles[_secondImgIndex].Style,
                        FirstImageIndex = _firstImgIndex,
                        SecondImageIndex = _secondImgIndex,
                        Step = _step
                    };
                }
                _step++;
                return;
            }
            
            _history[_step] = new ImageData
            {
                FirstImagePath = GetFirstImage(),
                SecondImagePath = GetSecondImage(),
                FirstImageName = _allStyles[_firstImgIndex].Style,
                SecondImageName = _allStyles[_secondImgIndex].Style,
                FirstImageIndex = _firstImgIndex,
                SecondImageIndex = _secondImgIndex,
                Step = _step
            };
            _step++;
        }
        
        public string GetFirstImage()
        {
            _firstImgIndex = Rand.Next(Min, Max);
            return _allStyles[_firstImgIndex].GetImagePath();
        }

        public string GetSecondImage()
        {
            _secondImgIndex = Rand.Next(Min, Max);
            while (_secondImgIndex == _firstImgIndex)
            {
                _secondImgIndex = Rand.Next(Min, Max);
            }
            return _allStyles[_secondImgIndex].GetImagePath();
        }
    }
}