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

namespace 库存管理
{
    public partial class ModifyBookList : Form
    {
        public ModifyBookList()
        {
            InitializeComponent();
        }

        string connString = @"Data Source=.;Initial Catalog =Library;User ID = sa;Pwd=123456";
        SqlConnection conn;
        SqlDataReader dr;

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("图书ISBN不能为空", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                conn = new SqlConnection(connString);
                string isbn = textBox1.Text.Trim();
                string sql = "select * from BookList where ISBN='" + isbn + "'";
                SqlCommand comm = new SqlCommand(sql, conn);
                bool error = true;
                try
                {
                    conn.Open();
                    dr = comm.ExecuteReader();
                    if (!dr.Read())
                    {
                        MessageBox.Show("图书信息不存在！", "数据出错！", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        error = false;
                    }
                    dr.Close();
                    dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        textBox2.Text = dr["ISBN"].ToString().Trim();
                        textBox3.Text = dr["author"].ToString().Trim();
                        textBox4.Text = dr["BookName"].ToString().Trim();
                        textBox5.Text = dr["publishTime"].ToString().Trim();
                        textBox6.Text = dr["publisher"].ToString().Trim();
                        textBox7.Text = dr["num"].ToString().Trim();
                        if ((bool)dr["bookstate"] == true)
                        {
                            radioButton1.Checked = true;
                        }
                        else if ((bool)dr["bookstate"] == false)
                        {
                            radioButton1.Checked = false;
                        }
                        totrue();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "操作数据库出错！", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                finally
                {
                    dr.Close();
                    conn.Close();
                }
                if (error == true)
                {
                    SqlConnection CONN = new SqlConnection(connString);
                    string SQL = "select * from BookCategory";
                    SqlCommand COMM = new SqlCommand(SQL, CONN);
                    comboBox1.Items.Clear();
                    try
                    {
                        CONN.Open();
                        dr = COMM.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = (int)dr[0];
                            string bCategoryname = dr["bCategoryname"].ToString().Trim();
                            comboBox1.Items.Add(new BookCategory(id, bCategoryname));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "操作数据库出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }
                    finally
                    {
                        dr.Close();
                        CONN.Close();
                    }
                    comboBox1.SelectedIndex = 0;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ISBN = textBox1.Text.Trim();
            string isbn = textBox2.Text.Trim();
            string name = textBox4.Text.Trim();
            string bookname = comboBox1.Text.Trim();
            int publishTime = 0;
            string publisher = textBox6.Text.Trim();
            string author = textBox3.Text.Trim();
            int num = 0;
            bool bookstate;
            string state;
            bool error = true;
            if (textBox4.Text == "")
            {
                MessageBox.Show("书名不能为空！", "数据出错！", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                error = false;
            }
            else if (textBox5.Text == "")
            {
                MessageBox.Show("出版年份不能为空！", "数据出错！", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                error = false;
            }
            else if (textBox6.Text == "")
            {
                MessageBox.Show("第一出版社不能为空！", "数据出错！", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                error = false;
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("第一作者不能为空！", "数据出错！", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                error = false;
            }
            else if (textBox7.Text == "")
            {
                MessageBox.Show("库存数目不能为空！", "数据出错！", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                error = false;
            }
            try
            {
                publishTime = Convert.ToInt32(textBox5.Text);
                num = Convert.ToInt32(textBox7.Text);
            }
            catch (Exception ex)
            {
                if (error == true)
                {
                    MessageBox.Show(ex.Message, "数据出错！", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
                error = false;

            }
            if (publishTime > DateTime.Now.Year)
            {
                MessageBox.Show("出版年份不能大于当前年份！", "数据出错！", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                error = false;
            }
            if (error == true)
            {
                if (radioButton1.Checked == true)
                {
                    bookstate = true;
                    state = "正常";
                }
                else
                {
                    bookstate = false;
                    state = "报废";
                }
                if (isbn != "" || name != "" || author != "" || publisher != "")
                {
                    string information = "\nISBN:" + isbn + "\n书名：" + name + "\n第一作者：" + author + "\n第一出版社：" + publisher
                        + "\n图书类别：" + bookname + "\n出版年份：" + publishTime + "\n状态：" + state + "\n库存数目：" + num;
                    SqlConnection conn1 = new SqlConnection(connString);
                    DialogResult result = MessageBox.Show("是否确定修改图书信息：" + information,
                        "修改确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        try
                        {
                            int Bid = 0;
                            BookCategory bookcategory = comboBox1.SelectedItem as BookCategory;
                            if (bookcategory != null) Bid = bookcategory.Bid;
                            string Sql = String.Format("update BookList set ISBN='{0}',BookName='{1}',bCategoryid='{2}',"
                                + "author='{3}',publisher='{4}',publishTime='{5}',bookstate='{6}',num='{7}',lendnum='{8}'", isbn, name, Bid, author, publisher, publishTime, bookstate, num, 0);
                            Sql += " where ISBN='" + ISBN + "'";
                            SqlCommand comm1 = new SqlCommand(Sql, conn1);
                            conn1.Open();
                            int count = comm1.ExecuteNonQuery();
                            if (count > 0)
                                MessageBox.Show("修改图书信息成功", "修改成功", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            else
                                MessageBox.Show("修改图书信息失败", "修改失败", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "操作数据库出错！", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        }
                        finally
                        {
                            conn1.Close();
                        }
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";
                        //comboBox1.SelectedIndex = 0;
                    }
                }
            }

        }

        private void tofalse()
        {
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            button2.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void totrue()
        {
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            button2.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void ModifyBookList_Load(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            tofalse();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
