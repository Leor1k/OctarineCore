using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Octarine_Core.Apis;
using Octarine_Core.Classic;
using Octarine_Core.Models;

namespace Octarine_Core.Resource.UsersIntefeces
{
    /// <summary>
    /// Логика взаимодействия для ChatUpBur.xaml
    /// </summary>
    public partial class ChatUpBur : UserControl
    {
        private string FriendName { get; set; } = string.Empty;
        private int FriendId { get; set; } = 0;
        private CallingController Controller { get; set; }
        private List<string> FriendsList { get; set; }
        public string ChatId { get; set; }

        private FriendAddList list;

        public DispatcherTimer _callTimer;
        public readonly bool IsGroupChat;
        public int UserAdmin;
        public ChatUpBur(string friendName, int friendId, CallingController controller, FriendBrick friend)
        {
            InitializeComponent();
            FriendName = friendName;
            FriendId = friendId;
            FriendNameTextBox.Text = friendName;
            Controller = controller;
            FriendsList = new List<string>();
            ShowPartilalsBtn.Visibility = Visibility.Hidden;
            IsGroupChat = false;
            foreach (var Users in friend.FriendIds)
            {
                FriendsList.Add(Users.ToString());
            }
            ChatId = friend.ChatId.ToString();

            _callTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(20)
            };
            _callTimer.Tick += (s, e) => EndCall();
            controller.SetChutBar(this);
        }
        public void RemoveFromUserList(int UserId)
        {
            FriendsList.Remove(UserId.ToString());
        }
        public ChatUpBur(string RoomaName, int[] friendIds, CallingController controller, int chatId)
        {
            InitializeComponent();
            FriendName = RoomaName;
            FriendNameTextBox.Text = RoomaName;
            Controller = controller;
            FriendsList = new List<string>();
            IsGroupChat = true;
            ShowPartilalsBtn.Visibility = Visibility.Visible;

            foreach (int id in friendIds)
            {
                FriendsList.Add($"{id}");
            }
            ChatId = chatId.ToString();
            _ = InitializeAdminAsync(chatId);
            _callTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(20)
            };
            _callTimer.Tick += (s, e) => EndCall();
            controller.SetChutBar(this);
        }
        private async Task InitializeAdminAsync(int chatId)
        {
            UserAdmin = await GetCreatorId(chatId);

            if (UserAdmin == Properties.Settings.Default.UserID)
            {
                Dispatcher.Invoke(() =>
                {
                    CrownButton.Visibility = Visibility.Visible;
                });
            }
            else
            {
                ImageBrush newBrush = new ImageBrush();

                newBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resource/Icons/Door.png"));

                DeleteChat.Background = newBrush;
            }
        }
        private async Task<int> GetCreatorId(int chatId)
        {
            try
            {
                var api = new ApiRequests();
                var creatorID = await api.GetAsyncNoList<int>(Properties.Settings.Default.GetCreatorId + chatId.ToString());
                if (creatorID == null)
                {
                    throw new Exception("Ошибка при получении владельца комнаты");
                }
                else
                {
                    int id = Convert.ToInt32(creatorID);
                    return id;
                }
            }
            catch
            {
                var eror = new ErrorAutUIController();
                eror.ShowUserError("Не удалось загрузить данные админестратора комнаты", Properties.Settings.Default.BorderForEror, false);
                return 0;
            }
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StandartGrid.Visibility = Visibility.Hidden;
            CallGrid.Visibility = Visibility.Visible;
            CallRequest req = new CallRequest(ChatId, Properties.Settings.Default.UserID.ToString(), FriendsList);
            LoadUserDataInCall(Properties.Settings.Default.UserID);
            await Controller.StartCallAsync(req);
            Properties.Settings.Default.InColling = true;
            _callTimer.Start();
        }
        public async void LoadUserDataInCall(int userId)
        {
           var api = new ApiRequests();
            List<UserDataFoCall> list = await api.GetAsync<UserDataFoCall>(Properties.Settings.Default.GetUserData+ userId);
            if (list.Count != 0)
            {
                foreach (UserDataFoCall user in list)
                {
                    ///ToDo опять работа
                    var item = new UserCalItem(user.Id, user.Username);
                    StackWitnName.Children.Add(item);
                }
            }
        }
        public void RemoveUserFromRoom(int UserId)
        {
            foreach (UserCalItem user in StackWitnName.Children)
            {
                if (user._userId == UserId)
                {
                    StackWitnName.Children.Remove(user);
                    CheckUsersCount();
                    break;
                }
            }
        }
        public async void CheckUsersCount()
        {
            if (StackWitnName.Children.Count == 1)
            {
                await EndCall();
                MessageBox.Show("Комната пуста, завершаем звонок");
            }
        }
        private async void EndCallBtn_Click(object sender, RoutedEventArgs e)
        {
            await EndCall();
        }

        public async Task EndCall()
        {
            _callTimer.Stop();
            await Controller.EndCall(Properties.Settings.Default.UserID.ToString(), ChatId);
            StandartGrid.Visibility = Visibility.Visible;
            CallGrid.Visibility = Visibility.Hidden;
            Properties.Settings.Default.InColling = false;
            StackWitnName.Children.Clear();
        }
        private void AddUserInRoomBTN_Click(object sender, RoutedEventArgs e)
        {
            if (list != null)
            {
                Controller._octarine.MainGrid.Children.Remove(list);
                list = null;
            }
            else
            {
                if (IsGroupChat == false)
                {
                    list = new FriendAddList(FriendsList);
                }
                else
                {
                    list = new FriendAddList(FriendsList, FriendName, Convert.ToInt32(ChatId));
                }
                Controller._octarine.MainGrid.Children.Add(list);
            }

        }

        private void ShowPartilalsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Controller._octarine.OarticalsGrid.Children.Count == 0)
            {
                if (UserAdmin == Properties.Settings.Default.UserID)
                {
                    Partikals papa = new Partikals(ChatId, true);
                    Controller._octarine.OarticalsGrid.Children.Add(papa);
                }
                else
                {
                    Partikals papa = new Partikals(ChatId, false);
                    Controller._octarine.OarticalsGrid.Children.Add(papa);
                }

            }
            else
            {
                Controller._octarine.OarticalsGrid.Children.Clear();
            }

        }

        private void DeleteChat_Click(object sender, RoutedEventArgs e)
        {
            if (UserAdmin == Properties.Settings.Default.UserID)
            {
                var ui = new MoreUI(Convert.ToInt32(ChatId), true);
                Controller._octarine.MainGrid.Children.Add(ui);
            }
            else
            {
                var ui = new MoreUI(Convert.ToInt32(ChatId), false);
                Controller._octarine.MainGrid.Children.Add(ui);
            }


        }
    }
}
