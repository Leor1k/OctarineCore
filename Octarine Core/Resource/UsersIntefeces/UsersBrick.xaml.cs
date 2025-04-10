using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Octarine_Core.Classic;


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
        public string PhotoName { get; set; }

        public UsersBrick()
        {
            InitializeComponent();
            DataContext =this;
        }
        public async void LoadUserIcon ()
        {
            MinIO minIO = new MinIO();
            BitmapImage UserBitMap = await minIO.LoadImageFromMinIO("IconUser" +IdUser+".png");
            if (UserBitMap != null)
            {
                UsersImage.Source = UserBitMap;
            }
        }


    }
}
