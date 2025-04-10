using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Octarine_Core.Apis;
using Octarine_Core.Models;

namespace Octarine_Core.Classic
{
    public class AddFriendToRoomController
    {
        public List<int> UsersIdList = new List<int>();
        private int UsersCaunt;
        public async void LoadUsersFriend(StackPanel stackPanel, List<string> UsersInChat)
        {
            UsersCaunt = UsersInChat.Count;
            CreateListWithUsers(UsersInChat);
            ApiRequests apiRequests = new ApiRequests();
            var friends = await apiRequests.GetAsync<Friends>(Properties.Settings.Default.ApiAll + Properties.Settings.Default.UserID.ToString() + "/list");
            if (friends != null)
            {
                foreach (var friend in friends)
                {
                    if (!CheckUserInChat(friend.FriendId, UsersInChat))
                    {
                        stackPanel.Children.Add(friend.CreateUserBrick(this));
                    }
                }
            }
        }
        public GroupChat ReturnChat ()
        {
            GroupChat chatDto = new GroupChat();
            chatDto.Particals = UsersIdList.ToArray();
            return chatDto;
        }
        public AddUserInChat ReturnNewParticants ()
        {
            var newParticants = new AddUserInChat();
            newParticants.UsersId = UsersIdList.ToArray();
            return newParticants;
        }
        public bool ChangesUsersCaunt ()
        {
            if (UsersIdList.Count == UsersCaunt)
            {
                return false;
            }
            return true;
        }
        public void CreateListWithUsers (List<string> Users)
        {
            foreach (var user in Users)
            {
                UsersIdList.Add(Convert.ToInt32(user));
            }
        }
        public bool CheckUserInChat(int FriendId, List<string> UsersInChat)
        {
            foreach (string UserId in UsersInChat)
            {
                if (Convert.ToInt32(UserId) == FriendId)
                {
                    return true;
                }
            }
            return false;
        }
        public void AddUser (int UserId)
        {
            UsersIdList.Add(UserId);
            ShowUser();
        }
        public void DelUser (int UserId)
        {
            UsersIdList.Remove(UserId);
            ShowUser();
        }
        private void ShowUser ()
        {
            string message = "В листе:";
            foreach (int id in UsersIdList)
            {
                message += $"{id}, ";
            }
        }

    }
}
