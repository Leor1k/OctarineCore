
using Octarine_Core.Apis;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Models
{
    public class EnteredUserLite
    {
        public EnteredUserLite(int _id, string _name)
        {
            id = _id;
            name = _name;
            status = "В сети";
        }
        private int id {  get; set; }
        private string name { get; set; }
        private string status { get; set; }
        public void LoadUserBrick (UsersBrick ub)
        {
            ub.UserName = name;
            ub.Status = status;
            ub.IdUser = id;
            Properties.Settings.Default.UserName = name;
        }
        public string GetPersonalApi()
        {
            return Properties.Settings.Default.ApiAll + id.ToString() + "/list";
        }
        public string GetFriendsRequestApi()
        {
            return Properties.Settings.Default.GetUsersRuquests + id;
        }
        public int GetIdUser ()
        {
            return id;
        }
    }
}
