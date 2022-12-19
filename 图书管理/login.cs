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
    public partial class login : Form
    {
        public string str = "Data Source=LAPTOP-VK4UBVJN;Initial Catalog=图书管理系统;Integrated Security=SSPI";
        public SqlConnection conn = null;
        public void initConn()
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
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userid = this.textBox1.Text;
            string password = this.textBox2.Text;
            initConn();
            if(userid.Trim().Equals("")||password.Trim().Equals(""))
            {
                MessageBox.Show("用户名或密码不能为空");
            }
            else
            {
                initConn();
                string sql = "select * from 用户 where 学工号='" + userid + "' and 登录密码 like '" + password + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    MessageBox.Show("登陆成功");
                }
                else
                {
                    MessageBox.Show("登陆失败");
                }
                r.Close();
                conn.Close();
                
            }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
