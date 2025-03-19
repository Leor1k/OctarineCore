
using Octarine_Core.Apis;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Models
{
    public class EnteredUserLite
    {
        public EnteredUserLite(int _id, string _name,string _status, string _pictureName)
        {
            id = _id;
            name = _name;
            status = _status;
            pictureName = _pictureName;
        }
        private int id {  get; set; }
        private string name { get; set; }
        private string status { get; set; }
        private string pictureName { get; set; }
        public void LoadUserBrick(UsersBrick ub)
        {
            ub.UserName = name;
            ub.Status = status;
            ub.IdUser = id;
            ub.PhotoName = pictureName;
            ub.LoadUserIcon();
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
