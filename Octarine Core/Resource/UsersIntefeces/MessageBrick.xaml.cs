using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Octarine_Core.Classic;

namespace Octarine_Core.Resource.UsersIntefeces
{
    public partial class MessageBrick : UserControl
    {
        public MessageBrick(string Text, bool ThisFriend, string dateTimesSender, int UserId)
        {
            InitializeComponent();
            contetMessage.Text = Text;
            messageTime.Text = dateTimesSender;
            SerImageForMessage(UserId);
        }
        public async void SerImageForMessage(int UserId)
        {
            MinIO minIO = new MinIO();
            BitmapImage UserBitMap = await minIO.LoadImageFromMinIO("IconUser" + UserId+".png");
            if (UserBitMap != null)
            {
                userPhoto.Source = UserBitMap;
            }

        }
        
    }
}
