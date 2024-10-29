using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _221_Globa.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void TextBoxPassword_Changed(object sender, RoutedEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Visible;
            if (PasswordBox.Password.Length > 0)
            {
                txtHintPassword.Visibility = Visibility.Hidden;
            }
        }

        private void LoginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (LoginBox.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginBox.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }
            using (var db = new Entities())
            {
                var user = db.User
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == LoginBox.Text && u.Password == PasswordBox.Password);
                if (user == null)
                {
                    MessageBox.Show("Пользователя с такими данными не найден!");
                    return;
                }
                MessageBox.Show("Пользователь успешно найден!");
                switch (user.Role)
                {
                    case "Пользователь":
                        NavigationService?.Navigate(new UserPage());
                        break;
                    case "Администратор":
                        NavigationService?.Navigate(new AdminPage());
                        break;
                }
            }
        }
    }
}
