using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Practice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\sytch\source\repos\Practice\Practice\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Login='" + textBox1.Text + "' AND Password='" + textBox2.Text + "';", con);
            con.Open();

            SqlDataReader re = cmd.ExecuteReader();
            if (re.Read())
            {
                if (Convert.ToString(re["Type"]) == "waiter")
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    this.Hide();
                    Form2 f = new Form2() { wi = Convert.ToInt32(re["Id"]), waiterName = Convert.ToString(re["Name"])};
                    f.Show();
                }
                else if (Convert.ToString(re["Type"]) == "admin")
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    this.Hide();
                    Form3 f = new Form3();
                    f.Show();
                }
            }
            else
            {
                MessageBox.Show("Неверный Логин или Пароль");
            }
            con.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
