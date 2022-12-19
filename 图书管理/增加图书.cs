using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 库存管理
    {
        public partial class AddBookList : Form
        {
            public AddBookList()
            {
                InitializeComponent();
            }
            string connString = @"Data Source=.;Initial Catalog =Library;User ID = sa;Pwd=123456";
            SqlDataReader dr;
            SqlConnection conn;
            SqlCommand comm;

            private void checkk()
            {
                if (textBox1.Text.Trim() != "")
                {
                    string ISBN = textBox1.Text.ToString().Trim();
                    int flag = 1;
                    if ((ISBN[0] >= '0' && ISBN[0] <= '9') && (ISBN[ISBN.Length - 1] >= '0' && ISBN[ISBN.Length - 1] <= '9'))
                    {
                        for (int i = 1; i < ISBN.Length; i++)
                        {
                            if (!((ISBN[i] >= '0' && ISBN[i] <= '9') || ISBN[i] == '-') || (ISBN[i] == ISBN[i - 1] && ISBN[i] == '-'))
                            {
                                flag = 0;
                                break;
                            }
                        }
                    }
                    else
                        flag = 0;
                    if (flag == 0)
                        MessageBox.Show("ISBN号格式错误", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        string sqll = String.Format("Select count(*) from BookList where ISBN = '{0}' ", ISBN);
                        conn.Open();
                        comm.CommandText = sqll;
                        int k = (int)comm.ExecuteScalar();
                        conn.Close();
                        if (k > 0)
                            MessageBox.Show("该ISBN号已存在，若新添该图书，请修改该图书库存数目", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }


            private void button1_Click(object sender, EventArgs e)
            {
                string isbn = textBox1.Text;
                string name = textBox2.Text;
                string bookname = comboBox1.Text;
                int publishTime = 0;
                string publisher = textBox4.Text;
                string author = textBox5.Text;
                int num = 0;
                bool bookstate;
                double price = 0;
                string state;
                bool flag = false;
                bool error = true;
                if (textBox1.Text == "")
                {
                    MessageBox.Show("ISBN号不能为空！", "数据出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    error = false;
                }
                else if (textBox2.Text == "")
                {
                    MessageBox.Show("书名不能为空！", "数据出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    error = false;
                }
                else if (textBox3.Text == "")
                {
                    MessageBox.Show("出版年份不能为空！", "数据出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    error = false;
                }
                else if (textBox4.Text == "")
                {
                    MessageBox.Show("第一出版社不能为空！", "数据出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    error = false;
                }
                else if (textBox5.Text == "")
                {
                    MessageBox.Show("第一作者不能为空！", "数据出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    error = false;
                }
                else if (textBox6.Text == "")
                {
                    MessageBox.Show("库存数目不能为空！", "数据出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    error = false;
                }
                else if (textBox7.Text == "")
                {
                    MessageBox.Show("单价不能为空！", "数据出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    error = false;
                }
                try
                {
                    publishTime = Convert.ToInt32(textBox3.Text);
                    num = Convert.ToInt32(textBox6.Text.Trim());
                    price = Convert.ToDouble(textBox7.Text.Trim());
                    flag = true;
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
                if ((publishTime > DateTime.Now.Year || publishTime < 1800) && flag == true)
                {
                    MessageBox.Show("出版年份超出范围（1800~Now）！", "数据出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    error = false;
                }
                if (num <= 0 && flag == true)
                {
                    MessageBox.Show("库存数必须为正整数！", "数据出错！", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    error = false;
                }
                if (price <= 0 && flag == true)
                {
                    MessageBox.Show("单价必须为正数！", "数据出错！", MessageBoxButtons.OK,
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
                            + "\n图书类别：" + bookname + "\n出版年份：" + publishTime + "\n状态：" + state + "\n库存数目：" + num + "\n价格：" + price;
                        SqlConnection conn1 = new SqlConnection(connString);
                        DialogResult result = MessageBox.Show("是否确定添加图书信息：" + information,
                        "添加确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            try
                            {
                                int Bid = 0;
                                BookCategory bookcategory = comboBox1.SelectedItem as BookCategory;
                                if (bookcategory != null) Bid = bookcategory.Bid;
                                string Sql = String.Format("INSERT INTO BookList(ISBN,BookName,bCategoryid,author,publisher,publishTime,bookstate,num,lendnum, price)"
                                    + "VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}', {9})", isbn, name, Bid, author, publisher, publishTime, bookstate, num, 0, price);
                                SqlCommand comm1 = new SqlCommand(Sql, conn1);
                                conn1.Open();
                                int count = comm1.ExecuteNonQuery();
                                if (count > 0)
                                    MessageBox.Show("添加图书信息成功", "添加成功", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                                else
                                    MessageBox.Show("添加图书信息失败", "添加失败", MessageBoxButtons.OK,
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
                        }
                    }
                    else
                    {
                        MessageBox.Show("输入信息不能为空！", "数据出错！", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }

            private void AddBookList_Load(object sender, EventArgs e)
            {
                conn = new SqlConnection(connString);
                string sql = "select * from BookCategory";
                comm = new SqlCommand(sql, conn);
                try
                {
                    conn.Open();
                    dr = comm.ExecuteReader();
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
                    conn.Close();
                }
                comboBox1.SelectedIndex = 0;
            }

            private void textBox5_Click(object sender, EventArgs e)
            {
                checkk();
            }

            private void comboBox1_Click(object sender, EventArgs e)
            {
                checkk();
            }

            private void radioButton1_Click(object sender, EventArgs e)
            {
                checkk();
            }

            private void radioButton2_Click(object sender, EventArgs e)
            {
                checkk();
            }

            private void textBox2_Click(object sender, EventArgs e)
            {
                checkk();
            }

            private void textBox3_Click(object sender, EventArgs e)
            {
                checkk();
            }

            private void textBox4_Click(object sender, EventArgs e)
            {
                checkk();
            }

            private void textBox6_Click(object sender, EventArgs e)
            {
                checkk();
            }

            private void button2_Click(object sender, EventArgs e)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
            }

            private void label1_Click(object sender, EventArgs e)
            {

            }

            private void label9_Click(object sender, EventArgs e)
            {

            }

            private void radioButton1_CheckedChanged(object sender, EventArgs e)
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

            private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

            private void textBox2_TextChanged(object sender, EventArgs e)
            {

            }

            private void textBox1_TextChanged(object sender, EventArgs e)
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

            private void textBox7_TextChanged(object sender, EventArgs e)
            {

            }
        }
        class BookCategory
        {
            public int Bid;
            public string Iname;
            public BookCategory(int id, string name)
            {
                Bid = id;
                Iname = name;
            }
            public override string ToString()
            {
                return Iname;
            }
        }
    }
