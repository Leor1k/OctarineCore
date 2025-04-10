using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octarine_Core.Apis;
using Octarine_Core.Autorisation;

namespace Octarine_Core.Classic
{
    internal class SettingController
    {
        private readonly OctarineWindow octarine;
        private readonly ErrorAutUIController ErrorUi;
        public SettingController(OctarineWindow octarine)
        {
            this.octarine = octarine;
            ErrorUi = new ErrorAutUIController(Properties.Settings.Default.BorderForEror);
        }
        public async void  ChangeUserName(string newUserName, int UserId)
        {
            try
            {
                ApiRequests ap = new ApiRequests();
                var ChangeNameRequest = new
                {
                    UserUd = UserId,
                    NewUserName = newUserName
                };
                var message = await ap.PostAsync<object>(Properties.Settings.Default.ChangeNameApi, ChangeNameRequest);
                octarine.UsersEnteredBrick.UserNameTx.Text = newUserName;
                Properties.Settings.Default.UserName = newUserName;
            }
            catch
            {
                ErrorUi.ShowUserError("Произошла ошибка при попытке изменить ник", false);
            }       
          
        }
    }
}
