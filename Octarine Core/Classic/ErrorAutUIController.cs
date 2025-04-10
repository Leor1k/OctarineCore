using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Classic
{
    public class ErrorAutUIController
    {
        public ErrorAutUIController() { }
        private readonly Grid defoltGrid;
        public ErrorAutUIController(Grid PlaceShowing)
        {
            defoltGrid = PlaceShowing;
        }
        public void ShowUserError(string errorMessage, bool IsSuccess)
        {
            ErorrShowing erorrShowing = new ErorrShowing(errorMessage, IsSuccess);
            defoltGrid.Children.Clear();
            defoltGrid.Children.Add(erorrShowing);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(4);
            timer.Tick += (sender, e) =>
            {
                defoltGrid.Children.Clear();
                timer.Stop();
            };
            timer.Start();
        }
        public void ShowUserError(string errorMessage, Grid PlaceShowing, bool IsSuccess)
        {
            ErorrShowing erorrShowing = new ErorrShowing(errorMessage, IsSuccess); 
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