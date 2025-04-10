using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Octarine_Core.Classic;


namespace Octarine_Core.Resource.UsersIntefeces
{
    public partial class AddUserToRoomBrick : UserControl
    {
        public readonly bool onlyShowing = false;
        public int UsersID;
        bool InList = false;
        AddFriendToRoomController controller;
        private readonly int Chatd;
        public AddUserToRoomBrick(string Name, int Id, AddFriendToRoomController _controller)
        {
            InitializeComponent();
            NameUser.Text = Name;
            UsersID = Id;
            controller = _controller;
            LoadUserImage();


        }
        public AddUserToRoomBrick(string Name, int Id)
        {
            InitializeComponent();
            NameUser.Text = Name;
            onlyShowing = true;
            AddToListBTN.Visibility = Visibility.Hidden;
            UsersID = Id;
            LoadUserImage();
        }
        public AddUserToRoomBrick(string Name, int Id, AddFriendToRoomController _controller, int chatId)
        {
            InitializeComponent();
            NameUser.Text = Name;
            onlyShowing = true;
            UsersID = Id;
            Chatd = chatId;
            AddToListBTN.Visibility = Visibility.Hidden;
            DeleteFromRoomBtn.Visibility = Visibility.Visible;
            LoadUserImage();
        }
        private async void LoadUserImage()
        {
            var minIO = new MinIO();
            BitmapImage UserBitMap = await minIO.LoadImageFromMinIO($"IconUser{UsersID}.png");
            UsersImage.Source = UserBitMap;
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
        private async void DeleteFromRoomBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int[] id = { UsersID };
                await Properties.Settings.Default.ChatController.DeleteUserFromChat(Chatd, id);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var parentContainer = VisualTreeHelper.GetParent(this) as Panel;
                    parentContainer?.Children.Remove(this);
                });
                Properties.Settings.Default.InColling = false;
            }
            catch
            {

            }
           
        }
    }
}
