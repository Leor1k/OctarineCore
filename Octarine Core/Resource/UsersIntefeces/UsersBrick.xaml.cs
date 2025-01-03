using System.Windows.Controls;


namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для UsersBrick.xaml
    /// </summary>
    public partial class UsersBrick : UserControl
    {
        public string UserName { get; set; }
        public string Status { get; set; } = "Не в сети";
        public int IdUser { get; set; }

        public UsersBrick()
        {
            InitializeComponent();
            DataContext =this;
        }

    }
}
