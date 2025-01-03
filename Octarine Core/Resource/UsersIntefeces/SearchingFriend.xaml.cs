using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Octarine_Core.Apis;
using Octarine_Core.Autorisation;
using Octarine_Core.Models;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для SearchingFriend.xaml
    /// </summary>
    public partial class SearchingFriend : UserControl
    {
        private int _id;
        public SearchingFriend(int Id, string Name, string Status)
        {
            InitializeComponent();
            _id = Id;
            NameSrachingUser.Text = Name;
            StatusSearchingUser.Text = Status;
        }

        private async void AddFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            await TryAddFeind();
        }
        private async Task TryAddFeind()
        {
            EnteredUserLite es = JWT.GetUserNameFromToken(Properties.Settings.Default.JwtToken);
            var AddFriend = new
            {
                UserId = es.GetIdUser(),
                FriendId = _id
            };
            ApiRequests apir = new ApiRequests();
            try
            {
                var tokenResponse = await apir.PostAsync<object>(Properties.Settings.Default.ApiAddFriend, AddFriend);
                StatysRequestTb.Text = "Запрос в друзя отправлен";
            }
            catch (Exception ex)
            {
               StatysRequestTb.Text = $"Возникла ошибочка :{ex.Message}";
            }
        }
    }
}
