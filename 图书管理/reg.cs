using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace library
{
    public partial class reg : Form
    {
        string connString = @"Data Source=LAPTOP-VK4UBVJN;Initial Catalog=图书管理系统;Integrated Security=SSPI";
        SqlConnection conn;
        SqlCommand comm;


        public reg()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void reg_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString);
            comm = new SqlCommand();
            comm.Connection = conn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == string.Empty || textBox2.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty)
            {
                MessageBox.Show("信息不完整", "信息不完整", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string username = textBox1.Text.Trim();
                string password1 = textBox2.Text.Trim();
                string password2 = textBox3.Text.Trim();
                bool midentity = false;
                string sql = String.Format("Select count(*) from Manager where managerid = '{0}' and mIdentity = 0", username);
                conn.Open();
                comm.CommandText = sql;
                int k = (int)comm.ExecuteScalar();
                conn.Close();
                if (k > 0)
                {
                    textBox1.Clear();
                    MessageBox.Show("该用户已注册", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (password1 != password2)
                {
                    MessageBox.Show("两次密码不一致", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox3.Clear();
                }
                else
                {
                    sql = String.Format("insert into Manager(managerid,managerPassword,mIdentity)values('{0}','{1}','{2}') ", username, password1, midentity);
                    try
                    {
                        conn.Open();
                        comm.CommandText = sql;
                        int count = comm.ExecuteNonQuery();
                        if (count > 0)
                            MessageBox.Show("添加成功", "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("添加成功", "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "操作数据库出错", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                }
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string sql = String.Format("Select count(*) from Manager where managerid = '{0}' and mIdentity = 0", username);
            conn.Open();
            comm.CommandText = sql;
            int k = (int)comm.ExecuteScalar();
            if (k > 0)
            {
                MessageBox.Show("该用户已注册", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
            }
            conn.Close();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string sql = String.Format("Select count(*) from Manager where managerid = '{0}' and mIdentity = 0", username);
            conn.Open();
            comm.CommandText = sql;
            int k = (int)comm.ExecuteScalar();
            if (k > 0)
            {
                MessageBox.Show("该用户已", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
            }
            conn.Close();
        }

    }
}


