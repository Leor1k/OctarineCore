using System.Windows;
using System.Windows.Controls;

namespace Octarine_Core.Classic
{
    internal class FormConroller
    {
        private Grid _mainGrid;
        private Grid _settingGrid;

        public FormConroller (Grid mainGrid, Grid SettingGrid)
        {
            _mainGrid = mainGrid;
            _settingGrid = SettingGrid;
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
        public void SwitchSettingGrid (Grid selectedGrid)
        {
            foreach (Grid grid in _settingGrid.Children)
            {
                if (grid.GetType() == typeof(Grid))
                {
                    grid.Visibility = Visibility.Hidden;
                }
            }
            selectedGrid.Visibility = Visibility.Visible;
        }
    }
}
