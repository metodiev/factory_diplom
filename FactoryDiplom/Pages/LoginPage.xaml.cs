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
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using FactoryDiplom.Encryption;

namespace FactoryDiplom
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public static int user_role = 0;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            if (!tb_username.Text.Equals("") && tb_username.Text != "")
            {
                if (pb_password.SecurePassword.Length != 0)
                {
                 
                    String username = tb_username.Text;
                    String password = pb_password.Password;

                    string enc_password = Encrypt.EncryptString(password, true);

                    bool chk = checkUser(username, enc_password);

                }
                else
                {
                    MessageBox.Show("Моля въведете парола !");
                }
            }
            else
            {
                MessageBox.Show("Моля въведете потребителско име !");
            }
        }

        private bool checkUser(String user, String pass)
        {
            string ConString = ConfigurationManager.ConnectionStrings["FactoryDiplomConnectionString"].ConnectionString;
            string CmdString = "";
            try
            {

                SqlConnection con = new SqlConnection(ConString);
                con.Open();
                CmdString = "select count(*), users.role_id from users where lower(user_name) = '" + user.ToLower() + "' and password = '" + pass + "' and type = 'A' group by role_id;";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if(reader.GetInt32(0) != 0)
                    {
                        LoginPage.user_role = reader.GetInt32(1);
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Невалидно потребителско име или парола !");
                    }
                }
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

    }
}
