using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Classic
{
    public class ErrorAutUIController
    {
        public void ShowUserError(string errorMessage, Grid PlaceShowing)
        {
            ErorrShowing erorrShowing = new ErorrShowing(errorMessage); 
            PlaceShowing.Children.Clear();
            PlaceShowing.Children.Add(erorrShowing);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(4); 
            timer.Tick += (sender, e) =>
            {
                PlaceShowing.Children.Clear(); 
                timer.Stop(); 
            };
            timer.Start();
        }
    }
}