using System.Windows;
using System.Windows.Controls;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для ErorrShowing.xaml
    /// </summary>
    public partial class ErorrShowing : UserControl
    {
        public ErorrShowing( string errorString, bool IsSuccess)
        {
            InitializeComponent();
            ErorrTB.Text = errorString;
            if (IsSuccess)
            {
                Sec.Visibility = Visibility.Hidden;
            }
            else
            {
                Err.Visibility = Visibility.Hidden;
            }
        }
    }
}
