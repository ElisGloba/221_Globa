using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddUserPage.xamlы
    /// </summary>
    public partial class AddUserPage : Page
    {
        private User _currentUser = new User();
        public AddUserPage(User selectedUser)
        {
            InitializeComponent();
            if (selectedUser != null)
                _currentUser = selectedUser;

            DataContext = _currentUser;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
                errors.AppendLine("Укажите пожалуйста логин!");
            if (string.IsNullOrWhiteSpace(_currentUser.Password))
                errors.AppendLine("Укажите пожалуйста пароль!");
            if ((_currentUser.Role == null) || (cmbRole.Text == ""))
                errors.AppendLine("Выберите пожалуйста роль!");
            else
                _currentUser.Role = cmbRole.Text;
            if (string.IsNullOrWhiteSpace(_currentUser.FIO))
                errors.AppendLine("Укажите пожалуйста ФИО!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentUser.Password = GetHash(_currentUser.Password);

            if (_currentUser.ID == 0)
                Entities.GetContext().User.Add(_currentUser);
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены, ура!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoginHD.Visibility = Visibility.Visible;
            if (Login.Text.Length > 0)
            {
                LoginHD.Visibility = Visibility.Hidden;
            }
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordHD.Visibility = Visibility.Visible;
            if (Password.Text.Length > 0)
            {
                PasswordHD.Visibility = Visibility.Hidden;
            }
        }

        private void FIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            FIOHD.Visibility = Visibility.Visible;
            if (FIO.Text.Length > 0)
            {
                FIOHD.Visibility = Visibility.Hidden;
            }
        }
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
    }
}
