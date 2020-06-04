using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Practice
{

    public partial class Form3 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\sytch\source\repos\Practice\Practice\Database1.mdf;Integrated Security=True");
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        SqlCommand cmd;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();

            try
            {
                cmd = new SqlCommand("SELECT * FROM FoodDrink", con);
                cmd.ExecuteNonQuery();
                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                da1.Fill(dt1);
                dataGridView1.DataSource = dt1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить список блюд: " + ex.ToString());
            }
            try
            {
                cmd = new SqlCommand("SELECT Id, Name, Login, Password FROM Users Where Type = 'waiter'", con);
                cmd.ExecuteNonQuery();
                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                da1.Fill(dt1);
                dataGridView2.DataSource = dt1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить список официантов: " + ex.ToString());
            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms[0].Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog1.ShowDialog();
                if (this.openFileDialog1.FileName.Equals("") == false)
                {
                    pictureBox1.Load(this.openFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить изображение: " + ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите название");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Введите стоимость");
                return;
            }
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Выберите изображение");
                return;
            }

            try
            {
                cmd = new SqlCommand("INSERT INTO [FoodDrink] (Name,Cost,Appearance) VALUES(@Name,@Cost,@Image)", con);
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Cost", SqlDbType.Float);
                cmd.Parameters.Add("@Image", SqlDbType.Image);

                cmd.Parameters["@Name"].Value = textBox1.Text;
                cmd.Parameters["@Cost"].Value = textBox2.Text;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                cmd.Parameters["@Image"].Value = ms.GetBuffer();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Блюдо успешно добавлено в меню");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить данные в базу: " + ex.ToString());
            }

            cmd = new SqlCommand("SELECT * FROM FoodDrink", con);
            cmd.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;

            textBox1.Clear();
            textBox2.Clear();
            pictureBox1.Image = null;
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            int p = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            cmd = new SqlCommand("DELETE FROM FoodDrink WHERE Id = '" + p + "';", con);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Блюдо успешно удалено");

            cmd = new SqlCommand("SELECT * FROM FoodDrink", con);
            cmd.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;

            con.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            int p = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            bool flag = false;
            try
            {
                if (textBox1.Text != "")
                {
                    cmd = new SqlCommand("UPDATE FoodDrink SET Name = @Name WHERE Id = @Id", con);
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Name"].Value = textBox1.Text;
                    cmd.Parameters["@Id"].Value = p;
                    cmd.ExecuteNonQuery();
                    flag = true;
                }
                if (textBox2.Text != "")
                {
                    cmd = new SqlCommand("UPDATE FoodDrink SET Cost = @Cost WHERE Id = @Id", con);
                    cmd.Parameters.Add("@Cost", SqlDbType.Float);
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Cost"].Value = textBox2.Text;
                    cmd.Parameters["@Id"].Value = p;
                    cmd.ExecuteNonQuery();
                    flag = true;
                }
                if (pictureBox1.Image != null)
                {
                    cmd = new SqlCommand("UPDATE FoodDrink SET Appearance = @Image WHERE Id = @Id", con);
                    cmd.Parameters.Add("@Image", SqlDbType.Image);
                    cmd.Parameters.Add("@Id", SqlDbType.Int);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    cmd.Parameters["@Image"].Value = ms.GetBuffer();
                    cmd.Parameters["@Id"].Value = p;
                    cmd.ExecuteNonQuery();
                    flag = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось обновить данные в базе: " + ex.ToString());
            }

            if (!flag)
            {
                MessageBox.Show("Данные для обновления не введены!");
            }
            else
            {
                MessageBox.Show("Запись успешно обновлена");
            }

            cmd = new SqlCommand("SELECT * FROM FoodDrink", con);
            cmd.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;

            textBox1.Clear();
            textBox2.Clear();
            pictureBox1.Image = null;
            con.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();

            if (textBox3.Text == "")
            {
                MessageBox.Show("Введите имя");
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (textBox5.Text == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            try
            {
                cmd = new SqlCommand("INSERT INTO [Users] (Name,Login,Password,Type) VALUES(@Name,@Log,@Pas,'waiter')", con);
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Log", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Pas", SqlDbType.NVarChar);

                cmd.Parameters["@Name"].Value = textBox3.Text;
                cmd.Parameters["@Log"].Value = textBox4.Text;
                cmd.Parameters["@Pas"].Value = textBox5.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Официант успешно добавлен в базу");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить данные в базу: " + ex.ToString());
            }

            cmd = new SqlCommand("SELECT Id, Name, Login, Password FROM Users Where Type = 'waiter'", con);
            cmd.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            da1.Fill(dt1);
            dataGridView2.DataSource = dt1;

            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

            con.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            int p = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);
            bool flag = false;
            try
            {
                if (textBox3.Text != "")
                {
                    cmd = new SqlCommand("UPDATE Users SET Name = @Name WHERE Id = @Id", con);
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Name"].Value = textBox3.Text;
                    cmd.Parameters["@Id"].Value = p;
                    cmd.ExecuteNonQuery();
                    flag = true;
                }
                if (textBox4.Text != "")
                {
                    cmd = new SqlCommand("UPDATE Users SET Login = @Log WHERE Id = @Id", con);
                    cmd.Parameters.Add("@Log", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Log"].Value = textBox4.Text;
                    cmd.Parameters["@Id"].Value = p;
                    cmd.ExecuteNonQuery();
                    flag = true;
                }
                if (textBox5.Text != "")
                {
                    cmd = new SqlCommand("UPDATE Users SET Password = @Pas WHERE Id = @Id", con);
                    cmd.Parameters.Add("@Pas", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters["@Pas"].Value = textBox5.Text;
                    cmd.Parameters["@Id"].Value = p;
                    cmd.ExecuteNonQuery();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось обновить данные в базе: " + ex.ToString());
            }

            if (!flag)
            {
                MessageBox.Show("Данные для обновления не введены!");
            }
            else
            {
                MessageBox.Show("Запись успешно обновлена");
            }
            cmd = new SqlCommand("SELECT Id, Name, Login, Password FROM Users Where Type = 'waiter'", con);
            cmd.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            da1.Fill(dt1);
            dataGridView2.DataSource = dt1;

            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

            con.Close();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                int p = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);
                cmd = new SqlCommand("DELETE FROM Users WHERE Id = '" + p + "';", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Официант успешно удален");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Не удалось удалить запись из базы: " + ex.ToString());
            }
            cmd = new SqlCommand("SELECT Id, Name, Login, Password FROM Users Where Type = 'waiter'", con);
            cmd.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            da1.Fill(dt1);
            dataGridView2.DataSource = dt1;
            dataGridView3.DataSource = null;

            con.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView2.CurrentRow.Cells[0].Value == null)
            {
                return;
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                int p = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);
                cmd = new SqlCommand("SELECT Id, Table_number, Date_Time FROM Orders WHERE Waiter_Id = '" + p + "';", con);
                cmd.ExecuteNonQuery();
                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                da1.Fill(dt1);
                dataGridView3.DataSource = dt1;
            }
            catch (Exception ex)
            {
                string str = "Не удалось загрузить список заказов: " + ex.ToString();
            }
            con.Close();
        }
    }
}
