using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfProject
{
    public class TeamColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TeamColors TeamColor)
            {
                SolidColorBrush BrushColor = new SolidColorBrush();
                switch (TeamColor)
                {
                    case TeamColors.Blue:
                        BrushColor.Color = Color.FromArgb(255, 0, 0, 255);
                        break;
                    case TeamColors.Grey:
                        BrushColor.Color = Color.FromArgb(255, 128, 128, 128);
                        break;
                    case TeamColors.Green:
                        BrushColor.Color = Color.FromArgb(255, 0, 128, 0);
                        break;
                    case TeamColors.Red:
                        BrushColor.Color = Color.FromArgb(255, 255, 0, 0);
                        break;
                    case TeamColors.Yellow:
                        BrushColor.Color = Color.FromArgb(255, 255, 255, 0);
                        break;
                }
                return BrushColor;
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
