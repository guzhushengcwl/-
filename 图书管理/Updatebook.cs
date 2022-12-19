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
    public partial class Updatebook : Form
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
        public Updatebook()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Updatebook_Load(object sender, EventArgs e)
        {

        }
        public static void update(string sql)
        {
            SqlCommand cmd = null;
            try
            {
                cmd = get_SqlCommand(sql);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("成功更新：" + rows + "条图书信息");
                }
                else
                {
                    MessageBox.Show("更新失败，请输入正确的图书编号");
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("出错原因：" + ex.Message);
            }
        }//Update
        private void button1_Click(object sender, EventArgs e)
        {
            string oldbookid = this.textBox1.Text;
            string newbookid = this.textBox2.Text;
            string place = this.textBox6.Text;
            string quantity = this.textBox8.Text;
            int q = int.Parse(quantity);
            string sql = "update 图书 set 图书编号='" + newbookid + "',图书所在位置='" + place + "',数量='" + quantity + "' where 图书编号='" + oldbookid + "'";//更新语句
            update(sql);
        }
    }
}
