using System.Windows;
using System.Windows.Controls;

namespace Octarine_Core.Resource.UsersIntefeces
{
    public partial class MessageBrick : UserControl
    {
        public MessageBrick(string Text, bool ThisFriend)
        {
            InitializeComponent();
            contetMessage.Text = Text;
        }
        
    }
}
