using System;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Octarine_Core.Apis;
using Octarine_Core.Classic;
using Octarine_Core.Models;


namespace Octarine_Core.Resource.UsersIntefeces
{
    public class PartikalsUser
    {
        [JsonPropertyName("userId")]
        public int Id { get; set; }
        [JsonPropertyName("userName")]
        public string Name { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
        public AddUserToRoomBrick CreateBick()
        {
            var user  = new AddUserToRoomBrick(Name, Id);
            if (Role == "Создатель")
            {
                user.CrownImage.Visibility = Visibility.Visible;
            }
            return  user;
        }
        public AddUserToRoomBrick CreateBickForAdmin(AddFriendToRoomController controller, int chatId)
        {
            return new AddUserToRoomBrick(Name, Id, controller, chatId);
        }
    }
    public partial class Partikals : UserControl
    {
        public Partikals(string ChatId, bool ForAdmin)
        {
            InitializeComponent();
            LoadChatparticants(ChatId, ForAdmin);
        }
        private async void LoadChatparticants(string id, bool thisForAdmin)
        {
            ApiRequests api = new ApiRequests();
            int id1 = Convert.ToInt16(id);
            var particants = await api.GetAsync<PartikalsUser>(Properties.Settings.Default.LoadPartikals + id1);
            if (thisForAdmin == false)
            {
                foreach (var papa in particants)
                {
                    StackPart.Children.Add(papa.CreateBick());
                }
            }
            else
            {
                var controller = new AddFriendToRoomController();
                foreach (var grandpapa in particants)
                {
                    StackPart.Children.Add(grandpapa.CreateBickForAdmin(controller, Convert.ToInt32(id)));
                }

            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CloseContol();
        }
        private void CloseContol()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var parentContainer = VisualTreeHelper.GetParent(this) as Panel;
                parentContainer?.Children.Remove(this);
            });
            Properties.Settings.Default.InColling = false;
        }
    }
}
