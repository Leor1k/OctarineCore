﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Octarine_Core.Apis;
using Octarine_Core.Classic;
using Octarine_Core.Models;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для SearchingFriend.xaml
    /// </summary>
    public partial class SearchingFriend : UserControl
    {
        private int _id;
        public ChatController chatController { get; set; }
        public string PhotoName {  get; set; }
        public SearchingFriend(ChatController ch,int Id, string Name, string Status, string _PhotoName)
        {
            InitializeComponent();
            _id = Id;
            chatController = ch;
            NameSrachingUser.Text = Name;
            if (Status == "В друзьях")
            {
                AddFriendBtn.Visibility = Visibility.Hidden;
                Chat_Btn.Visibility = Visibility.Visible;
                DeleteChatBTN.Visibility = Visibility.Visible;
            }    
            StatusSearchingUser.Text = Status;
            StatysRequestTb.Visibility = Visibility.Hidden;
            PhotoName = _PhotoName;
            LoadUserIcon();
        }
        public async void LoadUserIcon()
        {
            if(PhotoName != null)
            {
                MinIO minIO = new MinIO();
                BitmapImage UserBitMap = await minIO.LoadImageFromMinIO("IconUser" + _id + ".png");
                if (UserBitMap != null)
                {
                    FriendIcon.Source = UserBitMap;
                }
            }
        }

        private async void AddFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StatusSearchingUser.Text != "Хочет дружить)")
            {
                await TryAddFeind();
                AddFriendBtn.Visibility = Visibility.Hidden;
                
            }
            else
            {
                await TryAcceptFeind();
                Chat_Btn.Visibility = Visibility.Visible;
            }
        }
        private async Task TryAcceptFeind()
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
                await apir.PostAsync<object>(Properties.Settings.Default.AcceptFriend, AddFriend);
                StatysRequestTb.Text = "Вы стали друзьями";
                StatusSearchingUser.Text = "В друзьях";
                AddFriendBtn.Visibility= Visibility.Hidden;
                DeleteChatBTN.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                var eror = new ErrorAutUIController();
                eror.ShowUserError("Возникла непредвиденная ошибка", Properties.Settings.Default.BorderForEror, false);
            }
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
                var eror = new ErrorAutUIController();
                eror.ShowUserError("Запрос успешно отправлен", Properties.Settings.Default.BorderForEror, true);
            }
            catch (Exception ex)
            {
                var eror = new ErrorAutUIController();
                eror.ShowUserError("Возникла непредвиденная ошибка", Properties.Settings.Default.BorderForEror, false);
            }
        }
        public async void DeleteFriendShip()
        {
            var FriendsRequest = new
            {
                UserId = Properties.Settings.Default.UserID,
                FriendId = _id
            };
            try
            {
                var api = new ApiRequests();
                await api.PostAsync<object>(Properties.Settings.Default.DeleteFriendship, FriendsRequest);
                var eror = new ErrorAutUIController();
                eror.ShowUserError("Вы больше не друзья....", Properties.Settings.Default.BorderForEror, true);
                chatController.DeteChatBtickByFriendIf(_id);
                DeleteElement();
            }
            catch
            {
                var eror = new ErrorAutUIController();
                eror.ShowUserError("Возникла непредвиденная ошибка", Properties.Settings.Default.BorderForEror, false);
            }
            
        }
        private void DeleteElement ()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var parentContainer = VisualTreeHelper.GetParent(this) as Panel;
                parentContainer?.Children.Remove(this);
            });
        }
///Todo тут работаем
        private void Chat_Btn_Click(object sender, RoutedEventArgs e)
        {
            EnteredUserLite en = JWT.GetUserNameFromToken(Properties.Settings.Default.JwtToken);
            int id = en.GetIdUser();
            try
            {
                chatController.CreateChats(id, _id, NameSrachingUser.Text);
            }
            catch
            {

            }
        }

        private void DeleteChatBTN_Click(object sender, RoutedEventArgs e)
        {
            var ui = new MoreUI(this);
            var grid = chatController.ReturnMoreUiGrid();
            grid.Children.Add(ui);
        }
    }
}
