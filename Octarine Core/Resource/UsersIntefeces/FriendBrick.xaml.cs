using System;
using System.Windows.Controls;
using System.Windows.Input;
using Octarine_Core.Classic;
using System.Windows.Media.Imaging;
using Octarine_Core.Models;


namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для FriendBrick.xaml
    /// </summary>
    public partial class FriendBrick : UserControl
    {
        public int ChatId { get; set; }
        public string FriendName { get; set; }
        public string FriendStatus { get; set; }
        public int[] FriendIds { get; set; }

        public event EventHandler ChatClicked;
        public FriendBrick(int chatId,string friendName, string friendStatus, int[] friendsId)
        {
            InitializeComponent();
            DataContext = this;
            ChatId = chatId;
            FriendName = friendName;
            FriendStatus = friendStatus;
            UserNameTx.Text = FriendName;
            StatusUserTx.Text = FriendStatus;
            FriendIds = friendsId;
            LoadUserIcon();
        }
        public async void LoadUserIcon()
        {
            if (FriendIds.Length > 2)
            {
                GroupImage.Visibility = System.Windows.Visibility.Visible;
                UserICon.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                MinIO minIO = new MinIO();
                BitmapImage UserBitMap = await minIO.LoadImageFromMinIO("IconUser" + FriendIds[0] + ".png");
                if (UserBitMap != null)
                {
                    UserICon.Source = UserBitMap;
                }
            }
            
        }
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChatClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
