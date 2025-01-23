using System.Windows.Controls;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для ChatUpBur.xaml
    /// </summary>
    public partial class ChatUpBur : UserControl
    {
        string FriendName { get; set; } =string.Empty;
        int FriendId { get; set; } = 0;
        public ChatUpBur(string friendName, int friendId)
        {
            InitializeComponent();
            FriendName = friendName;
            FriendId = friendId;
            FriendNameTextBox.Text = friendName;
        }
    }
}
