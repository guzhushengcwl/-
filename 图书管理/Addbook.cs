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


namespace 信息管理
{
    public partial class Addbook : Form
    {
        public static string str = "Data Source=LAPTOP-VK4UBVJN;Initial Catalog=图书管理系统;Integrated Security=True";
        public static SqlConnection conn = null;
        public static void initConn()
        {
            if (conn == null)
            {
                conn = new SqlConnection(str);
            }
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            if (conn.State == System.Data.ConnectionState.Broken)
            {
                conn.Close();
                conn.Open();
            }
        }//initConn
        public static SqlCommand get_SqlCommand(string sql)
        {
            initConn();
            SqlCommand cmd = new SqlCommand(sql, conn);
            return cmd;
        }
        public static void insert(string sql)
        {
            SqlCommand cmd = null;
            try
            {
                cmd = get_SqlCommand(sql);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("成功添加" + rows + "条图书信息");
                }
                else
                {
                    MessageBox.Show("添加失败");
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("出错原因：" + ex.Message);
            }
        }//insert
        public Addbook()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bookid = this.textBox1.Text;
            string bookname = this.textBox2.Text;
            string author = this.textBox3.Text;
            string classification = this.textBox5.Text;
            string place = this.textBox6.Text;
            string publish = this.textBox7.Text;
            string quantity = this.textBox4.Text;
            if (bookid == "" || bookname == "" || author == "" || classification == "" || place == "" || publish == "" || quantity=="")
            {
                MessageBox.Show("提示：输入不能为空");
            }
            else
            {
                int q = int.Parse(quantity);
                string sql = "insert into 图书 values('" + bookid + "','" + bookname + "','" + author + "','" + classification + "','" + place + "','" + publish + "','" + q + "')";//增加记录
                insert(sql);
            }
        }
    }
}
