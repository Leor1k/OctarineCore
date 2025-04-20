using System.Windows.Controls;
using Octarine_Core.Classic;
using System.Windows.Media.Imaging;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для UserCalItem.xaml
    /// </summary>
    public partial class UserCalItem : UserControl
    {
        public readonly int _userId;
        private readonly string _name;
        public UserCalItem(int UserId, string UserName)
        {
            InitializeComponent();
            _userId = UserId;
            _name = UserName;
            UserNameTb.Text = _name;
            LoadUserIcon();
        }
        public async void LoadUserIcon()
        {
            MinIO minIO = new MinIO();
            BitmapImage UserBitMap = await minIO.LoadImageFromMinIO("IconUser" + _userId + ".png");
            if (UserBitMap != null)
            {
                UsersImage.Source = UserBitMap;
            }
        }
    }
}
