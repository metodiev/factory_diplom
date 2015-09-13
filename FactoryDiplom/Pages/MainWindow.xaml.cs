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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace FactoryDiplom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
               public MainWindow()
        {
            InitializeComponent();
            
           if (LoginPage.user_role != 1)
            {
                tab_adminpanel.Visibility = Visibility.Hidden;
            }
           else
            {
                FillGridView();
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string ConString = ConfigurationManager.ConnectionStrings["FactoryDiplomConnectionString"].ConnectionString;
            string CmdString = "";
            try
            {

                SqlConnection con = new SqlConnection(ConString);
                con.Open();
                CmdString = "SELECT 5*5";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MessageBox.Show(reader.GetInt32(0).ToString());
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string ConString = ConfigurationManager.ConnectionStrings["FactoryDiplomConnectionString"].ConnectionString;
            //string CmdString = "";
            try
            {

                SqlConnection con = new SqlConnection(ConString);
                con.Open();
                //CmdString = "SELECT 5*5";
                SqlCommand cmd = new SqlCommand("Procedure_test", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = "Miroslav";
                cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = "Metodiev";
                cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                //SqlDataReader reader = cmd.ExecuteReader();

                //while (reader.Read())
                //{
                //    MessageBox.Show(reader.GetInt32(0).ToString());
                //}
                cmd.ExecuteNonQuery();
                int id = Convert.ToInt32(cmd.Parameters["@id"].Value);
                MessageBox.Show(id.ToString());

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void FillGridView()
        {
            string ConString = ConfigurationManager.ConnectionStrings["FactoryDiplomConnectionString"].ConnectionString;
            string CmdString = "";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT * FROM users";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Employee");
                sda.Fill(dt);
                dg_user.ItemsSource = dt.DefaultView;

                dg_user.IsReadOnly = true;
            }
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            DataGridViewImageColumn delbut = new DataGridViewImageColumn();
            delbut.Image = System.Drawing.Image.FromFile(Environment.CurrentDirectory + "/images/delicon.png");
            //delbut.Width = 20;
            //delbut.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dg_user.Columns.Add(delbut);


        }
    }
    
}
