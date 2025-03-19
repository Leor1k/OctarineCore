using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для ErorrShowing.xaml
    /// </summary>
    public partial class ErorrShowing : UserControl
    {
        public ErorrShowing( string errorString)
        {
            InitializeComponent();
            ErorrTB.Text = errorString;
        }
    }
}
