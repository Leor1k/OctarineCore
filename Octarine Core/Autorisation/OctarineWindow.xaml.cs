using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Octarine_Core.Apis;
using Octarine_Core.Models;

namespace Octarine_Core.Autorisation
{
    public partial class OctarineWindow : Window
    {
        private EnteredUserLite EnteredUserData;
        public OctarineWindow()
        {
            InitializeComponent();
            WindowChrome.SetWindowChrome(this, new WindowChrome());
            LoadUserData();
            SearchFrindBrd.Visibility = Visibility.Hidden;
        }
        private async void LoadUserData()
        {
            EnteredUserData = JWT.GetUserNameFromToken(Properties.Settings.Default.JwtToken.ToString());
            EnteredUserData.LoadUserBrick(UsersEnteredBrick);
            await LoadUsersFriendRequests();
            await LoadUesrsFriends();

        }
        private async Task LoadUsersFriendRequests()
        {
            ApiRequests ap = new ApiRequests();
            var friends = await ap.GetAsync<WhantsBeFriend>(EnteredUserData.GetFriendsRequestApi());
            if(friends!= null)
            {
                foreach (var friend in friends)
                {
                    FriendsAllStack.Children.Add(friend.CreateAcceptBrick());
                }
            }
        }
        private async Task LoadUesrsFriends ()
        {
            ApiRequests ap = new ApiRequests();
            var friends = await ap.GetAsync<Friends>(EnteredUserData.GetPersonalApi());
            if (friends != null)
            {
                foreach (var friend in friends)
                {
                    FriendsStack.Children.Add(friend.CreateFriendBrick());
                    FriendsAllStack.Children.Add(friend.CreateSearchBrick());
                }
            }   
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void UpBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void HideBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void SearchFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchFrindBrd.Visibility = Visibility.Visible;
            InfoBorder.Visibility = Visibility.Hidden;
        }

        private async void SearchBt_Click(object sender, RoutedEventArgs e)
        {
            await SearchUser(Convert.ToInt32(SearchingUserIdTb.Text));
        }
        private async Task SearchUser(int id)
        {
            ApiRequests ap = new ApiRequests();
            var friends = await ap.GetAsync<FriendDto>(Properties.Settings.Default.SearchFriend+$"{UsersEnteredBrick.IdUser}&userIds={id}");
            foreach (var friend in friends)
            {
                SearchStack.Children.Clear();
                SearchStack.Children.Add(friend.CreateSearchBrick());
            }
        }
        private void NumberOnlyImput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }
        }

        private void YourFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchFriendGrd.Visibility = Visibility.Hidden;
            AllFriendsGrid.Visibility = Visibility.Visible;
        }

        private void SearchYourFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchFriendGrd.Visibility = Visibility.Visible;
            AllFriendsGrid.Visibility = Visibility.Hidden;
        }
    }
}
