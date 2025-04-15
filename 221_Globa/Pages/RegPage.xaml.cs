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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            var context = Entities.GetContext();
            var roles = context.User.Select(u => u.Role).Distinct().ToList();

            RoleComboBox.ItemsSource = roles;

            if (roles.Count > 0)
            {
                RoleComboBox.SelectedItem = roles[0];
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            txtBoxLogin.Text = string.Empty;
            TextBoxFIO.Text = string.Empty;
            PasBox1.Password = string.Empty;
            PasBox2.Password = string.Empty;
            NavigationService.GoBack();
        }

        private void TextBoxLogin_Changed(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Visible;
            if (txtBoxLogin.Text.Length > 0)
            {
                txtHintLogin.Visibility = Visibility.Hidden;
            }
        }

        private void TextBoxFIO_Changed(object sender, TextChangedEventArgs e)
        {
            txtHintFIO.Visibility = Visibility.Visible;
            if (TextBoxFIO.Text.Length > 0)
            {
                txtHintFIO.Visibility = Visibility.Hidden;
            }
        }
        private void PasswordBox_PasswordChanged1(object sender, RoutedEventArgs e)
        {
            txtHintPassword1.Visibility = Visibility.Visible;
            if (PasBox1.Password.Length > 0)
            {
                txtHintPassword1.Visibility = Visibility.Hidden;
            }
        }
        private void PasswordBox_PasswordChanged2(object sender, RoutedEventArgs e)
        {
            txtHintPassword2.Visibility = Visibility.Visible;
            if (PasBox2.Password.Length > 0)
            {
                txtHintPassword2.Visibility = Visibility.Hidden;
            }
        }

        private void ButtonКReg_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxFIO.Text) || string.IsNullOrEmpty(txtBoxLogin.Text) ||
                string.IsNullOrEmpty(PasBox1.Password) || string.IsNullOrEmpty(PasBox2.Password))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }
            using (var db = new Entities())
            {
                var user = db.User
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == txtBoxLogin.Text);
                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует(");
                    return;
                }
                User userObject = new User
                {
                    FIO = TextBoxFIO.Text,
                    Login = txtBoxLogin.Text,
                    Password = GetHash(PasBox1.Password),
                    Role = RoleComboBox.Text
                };
                db.User.Add(userObject);
                db.SaveChanges();
                MessageBox.Show("Пользователь успешно зарегистрирован!");

            }
            if (PasBox1.Password.Length >= 6)
            {
                bool en = true; // английская раскладка
                bool number = false; // цифра
                for (int i = 0; i < PasBox1.Password.Length; i++) // перебираем символы
                {
                    if (PasBox1.Password[i] >= 'А' && PasBox1.Password[i] <= 'Я') en = false; // если русская раскладка
                    if (PasBox1.Password[i] >= '0' && PasBox1.Password[i] <= '9') number = true; // если цифры
                }

                if (!en)
                    MessageBox.Show("Доступна только английская раскладка");
                else if (!number)
                    MessageBox.Show("Добавьте хотя бы одну цифру");
                if (en && number) // проверяем соответствие
                {
                    if (!PasBox1.Password.Equals(PasBox1.Password))
                    {
                        MessageBox.Show("Пароли не совпадают!");
                        return;
                    }
                    //else
                    //{
                    //    Entities1 db = new Entities1();

                    //}
                }
            }
            else MessageBox.Show("пароль слишком короткий, минимум 6 символов");

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
//            string hashedPassword = GetHash(PasBox1.Password);

//            using (var db = new Entities())
//            {
//                if (db.User.Any(u => u.Login == txtBoxLogin.Text))
//                {
//                    MessageBox.Show("Пользователь с таким логином уже существует.");
//                    return;
//                }
//            }

//            if (!IsValidPassword(PasBox1.Password))
//            {
//                MessageBox.Show("Пароль должен содержать 6 или более символов, только английские буквы и хотя бы одну цифру.");
//                return;
//            }

//            if (PasBox1.Password != PasBox2.Password)
//            {
//                MessageBox.Show("Пароли не совпадают.");
//                return;
//            }

//            using (var db = new Entities())
//            {
//                var userObject = new User
//                {
//                    FIO = TextBoxFIO.Text,
//                    Login = txtBoxLogin.Text,
//                    Password = hashedPassword,
//                    Role = RoleComboBox.Text
//                };

//                db.User.Add(userObject);
//                db.SaveChanges();
//            }

//            MessageBox.Show("Регистрация прошла успешно.");

//        }
//        private bool IsValidPassword(string password)
//        {
//            if (password.Length < 6)
//            {
//                return false;
//            }

//            if (!Regex.IsMatch(password, "^[a-zA-Z0-9]+$"))
//            {
//                return false;
//            }

//            if (!Regex.IsMatch(password, "[0-9]"))
//            {
//                return false;
//            }

//            return true;
//        }
//        public static string GetHash(string password)
//        {
//            using (var hash = SHA1.Create())
//            {
//                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
//            }
//        }
//    }
//}