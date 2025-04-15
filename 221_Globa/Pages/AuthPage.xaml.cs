using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            using(var db = new Entities())
            {
                // Извлекаем всех пользователей из базы данных
                var users = db.User.AsNoTracking().ToList();

                // Применяем хеширование к введенному паролю
                var hashedPassword = GetHash(PasswordBox.Password);

                // Ищем пользователя с введенным логином и хешированным паролем
                var user = users.FirstOrDefault(u => u.Login == LoginBox.Text && u.Password == hashedPassword);

                if (user == null)
                {
                    MessageBox.Show("Пользователя с такими данными не найден!");
                    return;
                }

                MessageBox.Show("Пользователь успешно найден!");

                switch (user.Role)
                {
                    case "user":
                        NavigationService?.Navigate(new UserPage());
                        break;
                    case "admin":
                        NavigationService?.Navigate(new AdminPage());
                        break;
                }
            }
        }
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }
    }
}