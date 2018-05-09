using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace testtt
{
    public partial class Form1 : Form
    {
        string ID = string.Empty;

        public Form1()
        {
            InitializeComponent();
            btnDelete.Enabled = false;
            btn_Upload.Enabled = false;
        }
        
        public static string Reverse(string s)
        {
            char[] charArr = s.ToCharArray();
            Array.Reverse(charArr);
            return new string(charArr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Martin\source\repos\testtt\testtt\BDtesttt.mdf;Integrated Security=True;Connect Timeout=30");
            string text = string.Empty;
            string str = string.Empty;
            int count;
            int index;
            List<string> f_text = new List<string>();
            if (textBox_Word.Text != string.Empty)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "txt files(*.txt)|*.txt";
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(opf.FileName);
                    text = sr.ReadToEnd();
                    sr.Close();
                    string[] subsrt = text.Split('.');
                    for (int i = 0; i < subsrt.Length; i++)
                    {
                        if (subsrt[i].Contains(textBox_Word.Text))
                        {
                            index = 0;
                            count = 0;
                            while ((index = subsrt[i].IndexOf(textBox_Word.Text, index) + 1) != 0)
                                count++;
                            str = Reverse(subsrt[i]);
                            str += ".";
                            string query = "insert into test (sentence,count) values ('" + str.ToString() + "','" + count.ToString() + "')";
                            SqlCommand command = new SqlCommand(query, conn);
                            SqlDataReader reader;
                            try
                            {
                                conn.Open();
                                reader = command.ExecuteReader();
                                while (reader.Read())
                                {

                                }
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            string query2 = "select * from test";
                            conn.Open();
                            SqlDataAdapter SDA = new SqlDataAdapter(query2, conn);
                            DataTable dt = new DataTable();
                            SDA.Fill(dt);
                            dataGridView1.DataSource = dt;
                            conn.Close();
                            f_text.Add(str);
                            btn_Upload.Enabled = true;
                            btnDelete.Enabled = true;
                        }
                    }
                    listBox1.Items.AddRange(f_text.ToArray());

                }
            }
            else
            {
                MessageBox.Show("Please enter the word!!!");
            }


        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Martin\source\repos\testtt\testtt\BDtesttt.mdf;Integrated Security=True;Connect Timeout=30");
            string query = "delete from test where Id='" + ID + "'";
            conn.Open();
            SqlDataAdapter SDA = new SqlDataAdapter(query, conn);
            SDA.SelectCommand.ExecuteNonQuery();
            conn.Close();
        }

        private void btn_Upload_Click_1(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Martin\source\repos\testtt\testtt\BDtesttt.mdf;Integrated Security=True;Connect Timeout=30");
            string query = "select * from test";
            conn.Open();
            SqlDataAdapter SDA = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void dataGridView1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            ID = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
        }
    }
}
