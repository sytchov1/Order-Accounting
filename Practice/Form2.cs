using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Practice
{
    public partial class Form2 : Form
    {
        public int wi;
        public string waiterName;
        string IdFD, sName, sCount, sCost;
        double total = 0, cost, count;
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\sytch\source\repos\Practice\Practice\Database1.mdf;Integrated Security=True");
        DataSet ds;
        SqlDataAdapter da;
        DataRow dr;
        SqlCommand cmd;
        SqlDataReader sqldr;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                cmd = new SqlCommand("Select Name FROM FoodDrink", con);
                sqldr = cmd.ExecuteReader();
                while (sqldr.Read())
                {
                    comboBox1.Items.Add(sqldr["Name"]);
                    comboBox2.Items.Add(sqldr["Name"]);
                }
                sqldr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить названия блюд: " + ex.ToString());
            }
            try
            {
                cmd = new SqlCommand("SELECT Id, Table_number, Date_Time FROM Orders WHERE Waiter_Id = '" + wi + "';", con);
                cmd.ExecuteNonQuery();
                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                da1.Fill(dt1);
                dataGridView1.DataSource = dt1;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить список заказов: " + ex.ToString());
            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms[0].Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                string name = comboBox1.Text;
                da = new SqlDataAdapter("Select * from FoodDrink where Name = N'" + name + "'", con);
                ds = new DataSet();
                da.Fill(ds, "FoodDrink");
                byte[] date = new byte[0];
                dr = ds.Tables["FoodDrink"].Rows[0];
                date = (byte[])dr["Appearance"];
                System.IO.MemoryStream ms = new System.IO.MemoryStream(date);
                pictureBox1.Image = System.Drawing.Bitmap.FromStream(ms);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить изображение: " + ex.ToString());
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                if (textBox1.Text != "")
                {
                    cmd = new SqlCommand("INSERT INTO [Orders] (Table_number,Waiter_Id,Date_Time) VALUES(@Table,@Waiter,@Dtime)", con);
                    cmd.Parameters.Add("@Table", SqlDbType.Char);
                    cmd.Parameters.Add("@Waiter", SqlDbType.Int);
                    cmd.Parameters.Add("@Dtime", SqlDbType.DateTime);
                    cmd.Parameters["@Table"].Value = textBox1.Text;
                    cmd.Parameters["@Waiter"].Value = wi;
                    cmd.Parameters["@Dtime"].Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Заказ успешно создан");
                }
                else
                {
                    MessageBox.Show("Введите № Стола!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить данные в базу " + ex.ToString());
            }
            try
            {
                cmd = new SqlCommand("SELECT Id, Table_number, Date_Time FROM Orders WHERE Waiter_Id = '" + wi + "';", con);
                cmd.ExecuteNonQuery();
                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                da1.Fill(dt1);
                dataGridView1.DataSource = dt1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить список заказов: " + ex.ToString());
            }
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                if (comboBox2.Text != "" && textBox2.Text != "")
                {
                    cmd = new SqlCommand("INSERT INTO [PositionInOrder] (Id_FoodDrink,Id_Order,Count) VALUES(@IdFD,@IdOr,@Count)", con);
                    cmd.Parameters.Add("@IdFD", SqlDbType.Int);
                    cmd.Parameters.Add("@IdOr", SqlDbType.Int);
                    cmd.Parameters.Add("@Count", SqlDbType.Int);
                    cmd.Parameters["@IdFD"].Value = IdFD;
                    cmd.Parameters["@IdOr"].Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                    cmd.Parameters["@Count"].Value = textBox2.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Блюдо добавлено в заказ");
                }
                else
                {
                    MessageBox.Show("Выберите блюдо и укажите количество!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить запись в базу " + ex.ToString());
            }
            try
            {
                int p = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                cmd = new SqlCommand("SELECT PositionInOrder.Id, FoodDrink.Name, PositionInOrder.Count FROM FoodDrink INNER JOIN PositionInOrder ON FoodDrink.Id = PositionInOrder.Id_FoodDrink WHERE PositionInOrder.Id_Order = '" + p + "';", con);
                cmd.ExecuteNonQuery();
                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                da1.Fill(dt1);
                dataGridView2.DataSource = dt1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось считать данные из базы: " + ex.ToString());
            }
            con.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                string name = comboBox2.Text;
                da = new SqlDataAdapter("Select * from FoodDrink where Name = N'" + name + "'", con);
                ds = new DataSet();
                da.Fill(ds, "FoodDrink");
                dr = ds.Tables["FoodDrink"].Rows[0];
                IdFD = dr["Id"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось считать данные из базы: " + ex.ToString());
            }
            con.Close();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[0].Value == null)
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
                int p = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                cmd = new SqlCommand("SELECT PositionInOrder.Id, FoodDrink.Name, PositionInOrder.Count FROM FoodDrink INNER JOIN PositionInOrder ON FoodDrink.Id = PositionInOrder.Id_FoodDrink WHERE PositionInOrder.Id_Order = '" + p + "';", con);
                cmd.ExecuteNonQuery();
                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                da1.Fill(dt1);
                dataGridView2.DataSource = dt1;
            }
            catch (Exception ex)
            {
                string str = "Не удалось считать данные из базы: " + ex.ToString();
            }
            con.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                int p = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);
                cmd = new SqlCommand("DELETE FROM PositionInOrder WHERE Id = '" + p + "';", con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Блюдо успешно удалено из заказа");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось удалить запись в базе: " + ex.ToString());
            }
            try
            {
                int p = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                cmd = new SqlCommand("SELECT PositionInOrder.Id, FoodDrink.Name, PositionInOrder.Count FROM FoodDrink INNER JOIN PositionInOrder ON FoodDrink.Id = PositionInOrder.Id_FoodDrink WHERE PositionInOrder.Id_Order = '" + p + "';", con);
                cmd.ExecuteNonQuery();
                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                da1.Fill(dt1);
                dataGridView2.DataSource = dt1;
            }
            catch (Exception ex)
            {
                MessageBox.Show ("Не удалось считать данные из базы " + ex.ToString());
            }
            con.Close();
        }

        private void сохранитьToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
                streamWriter.WriteLine(richTextBox1.Text, System.Text.Encoding.Default);
                streamWriter.Close();
            }
        }

        private void открытьToolStripButton_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    string strfilename = openFileDialog1.FileName;
                    string filetext = File.ReadAllText(strfilename, System.Text.Encoding.Default);
                    richTextBox1.Text = filetext;
                }
            }

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, 120, 120);
        }

        private void печатьToolStripButton_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            try
            {
                total = 0;
                richTextBox1.Clear();
                richTextBox1.AppendText("\t Кафе    " + '"' + "Летний Сад" + '"' + Environment.NewLine);
                richTextBox1.AppendText("***********************************************" + Environment.NewLine);
                richTextBox1.AppendText("                                        кол-во           цена" + Environment.NewLine);
                richTextBox1.AppendText("***********************************************" + Environment.NewLine);
                cmd =  new SqlCommand("Select FoodDrink.Name, PositionInOrder.Count, FoodDrink.Cost From FoodDrink INNER JOIN PositionInOrder ON FoodDrink.Id = PositionInOrder.Id_FoodDrink WHERE PositionInOrder.Id_Order = @nOrder", con);
                cmd.Parameters.AddWithValue("@nOrder", textBox3.Text);
                sqldr = cmd.ExecuteReader();
                while (sqldr.Read())
                {
                    count = Convert.ToDouble(sqldr["Count"]);
                    cost = Convert.ToDouble(sqldr["Cost"]);
                    sName = Convert.ToString(sqldr["Name"]);
                    sCount = Convert.ToString(sqldr["Count"]);
                    sCost = Convert.ToString(sqldr["Cost"]);
                    total += count * cost;
                    richTextBox1.AppendText(sName);
                    for (int i = 0; i < 30 - sName.Length; i++)
                        richTextBox1.AppendText(" ");
                    richTextBox1.AppendText(sCount);
                    for (int i = 0; i < 18 - sCost.Length; i++)
                        richTextBox1.AppendText(" ");
                    richTextBox1.AppendText(sCost + Environment.NewLine);
                    
                }

                richTextBox1.AppendText("***********************************************" + Environment.NewLine);
                richTextBox1.AppendText("Итого                                                     " + total.ToString() + Environment.NewLine);
                richTextBox1.AppendText("***********************************************" + Environment.NewLine);
                richTextBox1.AppendText("Официант                          " + waiterName + Environment.NewLine);
                richTextBox1.AppendText("***********************************************" + Environment.NewLine);
                richTextBox1.AppendText("Дата:  " + DateTime.Now.ToShortDateString() + "            Время:  " + DateTime.Now.ToLongTimeString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось считать данные из базы " + ex.ToString());

            }
            con.Close();
        }
    }
}
