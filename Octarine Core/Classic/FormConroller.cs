using System.Windows;
using System.Windows.Controls;

namespace Octarine_Core.Classic
{
    internal class FormConroller
    {
        private Grid _mainGrid;
        public FormConroller (Grid mainGrid)
        {
            _mainGrid = mainGrid;
        }
        public void SwitchOctarineBorder( Border selectedBorder)
        {
            foreach (var border in _mainGrid.Children)
            {
                if (border.GetType() == typeof(Border))
                {
                    Border border2 = (Border)border;
                    if (border2.Tag.ToString() == "dynamic")
                    {
                        border2.Visibility = Visibility.Hidden;
                    }
                }
            }
            selectedBorder.Visibility = Visibility.Visible;
        }
    }
}
