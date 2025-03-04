using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shell;
using Octarine_Core.Apis;
using Octarine_Core.Autorisation;
namespace Octarine_Core

{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Properties.Settings.Default.JwtToken = null;
            CheckAutorisation();
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            RegistrationBrd.Visibility = Visibility.Hidden;
            ConfirmBrd.Visibility = Visibility.Hidden;
            ComfirmTrurBrd.Visibility = Visibility.Hidden;
            WindowChrome.SetWindowChrome(this, new WindowChrome());
        }

        private async void TryLogin()
        {
            var email = EmailTxb.Text;
            var password = PassPsb.Password;
            ApiRequests apir = new ApiRequests();

            var loginRequest = new
            {
                email,
                password
            };

            try
            {
                var tokenResponse = await apir.PostAsync<object>(Properties.Settings.Default.ApiUrl, loginRequest);
                Properties.Settings.Default.JwtToken = tokenResponse;
                Properties.Settings.Default.Save();
                Window window = new OctarineWindow();
                window.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage($"Возникла ошибка: {ex.Message}");
            }
        }
        private async void TryRegister()
        {
            ApiRequests apir = new ApiRequests();
            var registerRequest = new
            {
                users_name = RegLogintxb.Text.Trim(),
                users_email = RegEmailTxb.Text.Trim(),
                users_password = RegPassPsb.Password.Trim(),
            };
            try
            {
                var message = await apir.PostAsync<object>(Properties.Settings.Default.ApiUrlRegister, registerRequest);
                RegistrationBrd.Visibility = Visibility.Hidden;
                ConfirmBrd.Visibility = Visibility.Visible;
                ErrorBorder.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                ErrorMessage($"Возникла ошибка: {ex.Message}");
            }
        } 
        private async void TryConfirm(string code)
        {
            ApiRequests apir = new ApiRequests();
            var confirmrRequest = new
            {
                Email = RegEmailTxb.Text.Trim(),
                Code = code
            };
            try
            {
                var message = await apir.PostAsync<object>(Properties.Settings.Default.ApiUrlConfirm, confirmrRequest);
                ComfirmTrurBrd.Visibility = Visibility.Visible;
                ConfirmBrd.Visibility = Visibility.Hidden;
                ErrorBorder.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                ErrorMessage($"Возникла ошибка: {ex.Message}");
            }
        } //Вроде тоже да
        private async void TrySendCodeAgain()
        {
            ApiRequests apir = new ApiRequests();
            var emailRequest = new
            {
                Email = RegEmailTxb.Text.Trim(),
            };
            try
            {
                var message = await apir.PostAsync<object>(Properties.Settings.Default.ApiUrlSendCode, emailRequest);
                ErrorBorder.Visibility = Visibility.Visible;
                ErrorOut.Text = "Код выслан повторно";
            }
            catch (Exception ex)
            {
                ErrorMessage($"Возникла ошибка: {ex.Message}");
            }
        } //Вроде тоже да
        private void ErrorMessage(string message)
        {
            ErrorBorder.Visibility = Visibility.Visible;
            ErrorOut.Text = message;
        }
        private void CheckCheckCodeOnNull()
        {
            foreach (TextBox t in Nubers_Stack.Children)
            {
                if (String.IsNullOrEmpty(t.Text))
                {
                    throw new Exception("Все поля кода подтверждения обязательны к заполнению");
                }
            }
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
        private void UpBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = this.WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckPasswordAndEmail(EmailTxb.Text, PassPsb.Password);
                TryLogin();
            }
            catch (Exception ex)
            {
                ErrorBorder.Visibility = Visibility.Visible;
                ErrorOut.Text = ex.Message;
            }
        }
        private void CheckPasswordAndEmail(string Email, string Password)
        {
            if (String.IsNullOrEmpty(Email.Trim()))
            {
                throw new Exception("Поле \"Email\" обязятельно к заполнению");
            }
            else if (String.IsNullOrEmpty(Password.Trim()))
            {
                throw new Exception("Поле \"Пароль\" обязятельно к заполнению");
            }
            else if (IsValidEmail(Email) == false)
            {
                throw new Exception("Неверный формат электронной почты");
            }
            else if (Password.Length < 7)
            {
                throw new Exception("Пароль должен состоять мининмум из 8 символов");
            }
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
        private void PassPsb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        private void DontHaveAccBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthBrd.Visibility = Visibility.Hidden;
            RegistrationBrd.Visibility = Visibility.Visible;
            ErrorBorder.Visibility = Visibility.Hidden;
        }
        private void RegBtn_Копировать_Click(object sender, RoutedEventArgs e)
        {
            AuthBrd.Visibility = Visibility.Visible;
            RegistrationBrd.Visibility = Visibility.Hidden;
            ErrorBorder.Visibility = Visibility.Hidden;
        }
        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckPasswordAndEmail(RegEmailTxb.Text, RegPassPsb.Password);
                CheckLogin(RegLogintxb.Text);
                if (RegPassPsb.Password != RegRepeatPassPsb.Password)
                {
                    throw new Exception("Пароли не совпадают");
                }
                TryRegister();

            }
            catch (Exception ex)
            {
                ErrorBorder.Visibility = Visibility.Visible;
                ErrorOut.Text = ex.Message;
            }
        }
        private void CheckLogin(string login)
        {
            if (String.IsNullOrEmpty(login))
            {
                throw new Exception("Поле \"Логин\" обязательно к заполнению");
            }
            int countLetter = 0;
            foreach (char item in login)
            {
                if (Char.IsLetter(item))
                {
                    countLetter++;
                }
            }
            if (countLetter < 2)
            {
                throw new Exception("\"Логин\" должен содержать минимум из 3 букв");
            }
        }
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            string a = null;
            foreach (TextBox t in Nubers_Stack.Children)
            {
                a = a + t.Text;
            }
            try
            {
                CheckCheckCodeOnNull();
                TryConfirm(a);

            }
            catch (Exception ex)
            {
                ErrorBorder.Visibility = Visibility.Visible;
                ErrorOut.Text = ex.Message;
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
        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selTb = (TextBox)sender;
            int index = Nubers_Stack.Children.IndexOf(selTb);
            if (selTb.Text.Length > 0)
            {
                if (index + 1 < 6)
                {
                    Nubers_Stack.Children[index + 1].Focus();
                }
            }

        }
        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            ComfirmTrurBrd.Visibility = Visibility.Hidden;
            AuthBrd.Visibility = Visibility.Visible;
        }
        private void SendCodeAgainBtn_Click(object sender, RoutedEventArgs e)
        {
            TrySendCodeAgain();
        }
        private void CheckAutorisation()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.JwtToken))
            {
                Window window = new OctarineWindow();
                window.Show();
                this.Close();
            }
        }
    }
}
