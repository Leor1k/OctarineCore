using System.Windows.Controls;


namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для FriendBrick.xaml
    /// </summary>
    public partial class FriendBrick : UserControl
    {
        public FriendBrick(string FriendName, string FriendStatus)
        {
            InitializeComponent();
            DataContext = this;
            UserNameTx.Text = FriendName;
            StatusUserTx.Text = FriendStatus;
        }
    }
}
