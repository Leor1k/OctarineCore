using System.Windows;
using System.Windows.Controls;
using Octarine_Core.Classic;


namespace Octarine_Core.Resource.UsersIntefeces
{
    public partial class AddUserToRoomBrick : UserControl
    {
        public int UsersID;
        bool InList = false;
        AddFriendToRoomController controller;
        public AddUserToRoomBrick(string Name, int Id, AddFriendToRoomController _controller)
        {
            InitializeComponent();
            NameUser.Text = Name;
            UsersID = Id;
            controller = _controller;
        }

        private void AddToListBTN_Click(object sender, RoutedEventArgs e)
        {
            if (InList == false)
            {
                controller.AddUser(UsersID);
                AddToListBTN.Content = "-";
                InList = true;
            }
            else
            {
                controller.DelUser(UsersID);
                AddToListBTN.Content = "+";
                InList = false;
            }
        }
    }
}
