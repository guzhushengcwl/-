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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;

namespace 图书管理
{
    public partial class 借还书 : Form
    {
        public 借还书()
        {
            InitializeComponent();
        }

        string connString = @"Data Source = .;Initial Catalog = Library; User ID = sa; Pwd = 123456";
       /// <summary>
       SqlConnection conn; //声明连接对象
       /// </summary>
       SqlCommand comm; //声明命令对象
        string sql;
        bool flag = false;
        float pay;
        //bool Ban = true;
        string Rid;
        string BISBN;
        DateTime dt = DateTime.Now;
        bool ft = true;
        int ad;
        int KP = 0;

        /*
         private void BackBook_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString); //创建Connection对象
            comm = new SqlCommand(); //创建Commmand对象
            comm.Connection = conn; //设置command使用的Connection对象
            btnPay.Enabled = false;
            btnYes.Enabled = false;
            button2.Enabled = false;
        }
        */
        private void button1_Click(object sender, EventArgs e)
        {
           /*
            * conn.Open();
            Rid = txtReaderid.Text.Trim();
            sql = String.Format("Select count(*) from Reader where Rid = '{0}'", Rid);
            comm.CommandText = sql;
            int kkk = (int)comm.ExecuteScalar();
            conn.Close();
           */
            if (kkk <= 0)
                MessageBox.Show("身份证号查找失败！\n   请查对", "还书失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                sql = String.Format("select b.ISBN as ISBN, b.BookName as 书名,a.LendTime as 借阅时间,a.BackTime as 应还时间 "
            + "from bookLendList as a,BookList as b,Reader as c where a.ISBN=b.ISBN and a.Rid=c.Rid and a.Rid='{0}' and a.isback = 0", Rid);
                try
                {
                    conn.Open();
                    conn = new SqlConnection(connString);
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet("MyData");
                    da.Fill(ds, "MyData");
                    if (ft == true)
                    {
                        DataColumn column = new DataColumn("选择", typeof(bool));
                        ds.Tables["MyData"].Columns.Add(column);
                        dataGridView1.DataSource = ds.Tables[0];
                        dataGridView1.Columns[0].ReadOnly = true;
                        dataGridView1.Columns[1].ReadOnly = true;
                        dataGridView1.Columns[2].ReadOnly = true;
                        //dataGridView1.Columns[3].ReadOnly = true;
                        this.dataGridView1.Columns["选择"].DisplayIndex = Convert.ToInt32(0);
                        dataGridView1.Columns[4].Width = 50;
                        ft = false;
                    }
                    sql = String.Format("Select * from Reader, ReaderCategory where Reader.Rid = '{0}' and Reader.Rcategoryid = ReaderCategory.Rcategoryid", Rid);
                    comm.CommandText = sql;
                    SqlDataReader dr;
                    dr = comm.ExecuteReader();
                    dr.Read();
                    rtbRead.Clear();
                    rtbRead.Text = "身份证号：" + dr[0].ToString() + "\n姓名：" + dr["Rname"].ToString()
                        + "\n读者类别：" + dr["Rcategoryname"].ToString()
                        + "\n已借阅数目：" + dr["RbLnum"].ToString() + "\n超期数目：";
                    ad = (int)dr["Rday"];
                    dr.Close();
                    sql = String.Format("Select count(*) from BookLendList where Rid = '{0}' and BackTime < '{1}' and isBack = 0", Rid, dt);
                    comm.CommandText = sql;
                    int k = (int)comm.ExecuteScalar();
                    KP = k;
                    rtbRead.Text += k.ToString();
                    btnYes.Enabled = true;
                    button2.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "操作数据库出错！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            int select = 0;
            string ddd = "";
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "True")
                {
                    cnt++;
                }
            }
            if (cnt > 1)
                MessageBox.Show("只能选择一本书！", "还书失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (cnt == 0)
                MessageBox.Show("请选择要归还书籍！", "还书失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "True")
                    {
                        select = i;
                        ddd = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        BISBN = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        break;
                    }
                }

                string a = ddd.Substring(0, ddd.IndexOf("/"));
                int t = ddd.IndexOf("/");
                ddd = ddd.Remove(t, 1);
                ddd = ddd.Insert(t, "a");
                int t1 = ddd.IndexOf("a");
                t = ddd.IndexOf("/") - 1;
                string b = ddd.Substring(t1 + 1, t - t1);
                if (b.Length == 1)
                    b = "0" + b;
                t1 = ddd.IndexOf(" ") - 1;
                string c = ddd.Substring(t + 2, t1 - t - 1);
                if (c.Length == 1)
                    c = "0" + c;
                System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd";
                DateTime dd = Convert.ToDateTime(a + "/" + b + "/" + c, dtFormat);
                TimeSpan d3 = dt.Subtract(dd);
                textBox1.Text = "0";
                if (d3.Days > 0 && flag == false)
                {
                    textBox1.Text = (double)d3.Days * 0.1 + "";
                    pay = d3.Days * 0.1f;
                    MessageBox.Show("该书籍已超期，请先缴纳扣款！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnPay.Enabled = true;
                }
                else
                {
                    if (d3.Days <= 0)
                    {
                        textBox1.Text = "0";
                        pay = 0.0f;
                    }
                    if (d3.Days > 0)
                    {
                        pay = d3.Days * 0.1f;
                        textBox1.Text = pay + "(已付)";
                    }
                    conn.Open();
                    sql = String.Format("Update BookLendList set BackTime = '{0}', isback = 1, money += {1} where Rid = '{2}' and ISBN = '{3}'", dt.ToString(), pay, Rid, BISBN);
                    comm.CommandText = sql;
                    int kkkk = (int)comm.ExecuteNonQuery();
                    sql = String.Format("Update Reader set RbLnum = RbLnum - 1 where Rid = '{0}'", Rid);
                    comm.CommandText = sql;
                    kkkk += (int)comm.ExecuteNonQuery();
                    sql = String.Format("Update BookList set lendnum = lendnum - 1 where ISBN = '{0}'", BISBN);
                    comm.CommandText = sql;
                    kkkk += (int)comm.ExecuteNonQuery();
                    if (kkkk >= 3)
                    {
                        MessageBox.Show("还书成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.Rows.RemoveAt(select);
                        flag = false;
                        //Ban = true;
                    }
                    else
                    {
                        MessageBox.Show("还书失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        flag = false;
                    }
                    conn.Close();
                }
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            flag = true;
            MessageBox.Show("缴费成功！金额：" + pay);
            textBox1.Text = pay + "(已付)";
            btnPay.Enabled = false;
            KP--;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            int select = 0;
            string ddd = "";
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "True")
                {
                    cnt++;
                }
            }
            if (cnt > 1)
                MessageBox.Show("只能选择一本书！", "续借失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (cnt == 0)
                MessageBox.Show("请选择要续借书籍！", "续借失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                sql = String.Format("Select count(*) from BookLendList where Rid = '{0}' and BackTime < '{1}' and isBack = 0", Rid, dt);
                comm.CommandText = sql;
                KP = (int)comm.ExecuteScalar();
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[4].Value.ToString() == "True")
                    {
                        select = i;
                        ddd = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        BISBN = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        break;
                    }
                }

                string a = ddd.Substring(0, ddd.IndexOf("/"));
                int t = ddd.IndexOf("/");
                ddd = ddd.Remove(t, 1);
                ddd = ddd.Insert(t, "a");
                int t1 = ddd.IndexOf("a");
                t = ddd.IndexOf("/") - 1;
                string b = ddd.Substring(t1 + 1, t - t1);
                if (b.Length == 1)
                    b = "0" + b;
                t1 = ddd.IndexOf(" ") - 1;
                string c = ddd.Substring(t + 2, t1 - t - 1);
                if (c.Length == 1)
                    c = "0" + c;
                System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd";
                DateTime dd = Convert.ToDateTime(a + "/" + b + "/" + c, dtFormat);
                TimeSpan d3 = dt.Subtract(dd);
                textBox1.Text = "0";
                if ((d3.Days > 0))
                {
                    textBox1.Text = (double)d3.Days * 0.1 + "";
                    pay = d3.Days * 0.1f;
                    MessageBox.Show("该书籍已超期，不允许续借！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (KP > 0)
                {
                    MessageBox.Show("该读者存在超期未还书籍！", "续借失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    conn.Open();
                    sql = String.Format("SELECT count(*) from BookLendList where ISBN = '{0}' and Rid = '{1}' and isback = 0 and renew = 1", BISBN, Rid);
                    comm.CommandText = sql;
                    int kkkk = (int)comm.ExecuteScalar();
                    conn.Close();
                    if (kkkk > 0)
                        MessageBox.Show("已续借过该书！", "续借失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        conn.Open();
                        sql = String.Format("Update BookLendList set BackTime = '{0}', renew =1 where Rid = '{1}' and ISBN = '{2}'", dt.AddDays(ad).ToShortDateString(), Rid, BISBN);
                        comm.CommandText = sql;
                        kkkk = (int)comm.ExecuteNonQuery();
                        conn.Close();
                        if (kkkk > 0)
                        {
                            MessageBox.Show("续借成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dataGridView1.Rows[select].Cells[3].Value = dt.AddDays(ad).ToShortDateString();
                            this.dataGridView1.Refresh();
                        }
                        else
                            MessageBox.Show("续借失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // 借还书
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "借还书";
            this.Load += new System.EventHandler(this.借还书_Load);
            this.ResumeLayout(false);

        }

        private void 借还书_Load(object sender, EventArgs e)
        {

        }
    }
    public partial class LendBook : Form
    {
        public LendBook()
        {
            InitializeComponent();
        }
        string connString = @"Data Source = .;Initial Catalog = Library; User ID = sa; Pwd = 123456";
        SqlConnection conn; //声明连接对象
        SqlCommand comm; //声明命令对象
        string sql;

        private void LendBook_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString); //创建Connection对象
            comm = new SqlCommand(); //创建Commmand对象
            comm.Connection = conn; //设置command使用的Connection对象
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            conn.Open();
            string Rid = txtReaderid.Text.Trim();
            string BISBN = txtbookISBN.Text.Trim();
            string d = dateTimePicker1.Value.ToString();

            if (txtReaderid.Text.Trim() == "" || txtbookISBN.Text.Trim() == "")
                MessageBox.Show("请完整填写信息！", "借阅", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                sql = String.Format("Select count(*) from BookList where ISBN = '{0}'", BISBN);
                comm.CommandText = sql;
                int kkk = (int)comm.ExecuteScalar();


                if (kkk <= 0)
                    MessageBox.Show("ISBN查找失败！\n   请查对", "借阅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    sql = String.Format("Select count(*) from BookList where ISBN = '{0}' and Lendnum < num and bookstate = 1", BISBN);
                    comm.CommandText = sql;
                    kkk = (int)comm.ExecuteScalar();
                    if (kkk <= 0)
                        MessageBox.Show("该书已无库存！", "借阅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        sql = String.Format("Select count(*) from Reader where Rid = '{0}'", Rid);
                        comm.CommandText = sql;
                        kkk = (int)comm.ExecuteScalar();
                        if (kkk <= 0)
                            MessageBox.Show("身份证号查找失败！\n   请查对", "借阅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                        {
                            //dateTimePicker2.Text = DateTime.Parse(DateTime.Now.ToString(dateTimePicker1.Text)).AddDays(90).ToShortDateString();
                            DateTime dt = DateTime.Now; //获取当前的日期； 
                            SqlDataReader dr;
                            sql = String.Format("Select count(*) from BookLendList where Rid = '{0}' and ISBN = '{1}' and isback = 0", Rid, BISBN);
                            comm.CommandText = sql;
                            int k2 = (int)comm.ExecuteScalar();
                            if (k2 > 0)
                            {
                                MessageBox.Show("该读者已借此书，且并未归还！");
                            }
                            else
                            {
                                //MessageBox.Show("Pass");
                                sql = String.Format("Select count(*) from BookLendList where Rid = '{0}' and BackTime < '{1}' and isback = 0", Rid, dt);
                                comm.CommandText = sql;
                                int k = (int)comm.ExecuteScalar();
                                double pm = 0;

                                sql = String.Format("Select * from Reader, ReaderCategory where Reader.Rid = '{0}' and Reader.Rcategoryid = ReaderCategory.Rcategoryid", Rid);
                                comm.CommandText = sql;
                                dr = comm.ExecuteReader();
                                dr.Read();
                                rtbRead.Clear();
                                rtbRead.Text = "身份证号：" + dr[0].ToString() + "\n姓名：" + dr["Rname"].ToString()
                                    + "\n读者类别：" + dr["Rcategoryname"].ToString() + "\n可借阅数目：" + dr["Rbnum"].ToString()
                                    + "\n已借阅数目：" + dr["RbLnum"].ToString() + "\n超期数目：";
                                int n = (int)dr["RbLnum"];
                                int m = (int)dr["Rbnum"];
                                int ad = (int)dr["Rday"];
                                dateTimePicker2.Text = DateTime.Parse(DateTime.Now.ToString(dateTimePicker1.Text)).AddDays(ad).ToShortDateString();
                                dr.Close();
                                if (k <= 0 && n < m)
                                {
                                    rtbRead.Text += 0;
                                    sql = String.Format("Select * from BookList, Bookcategory where ISBN = '{0}' and BookList.bCategoryid = Bookcategory.bCategoryid", BISBN);
                                    comm.CommandText = sql;
                                    dr = comm.ExecuteReader();
                                    dr.Read();
                                    rtbBook.Clear();
                                    rtbBook.Text = "ISBN：" + dr[0].ToString() + "\n书名：" + dr["BookName"].ToString() + "\n图书类别：" + dr["bCategoryname"].ToString()
                                           + "\n作者：" + dr["author"].ToString() + "\n出版日期：" + dr["publishTime"];
                                    dr.Close();
                                    DialogResult result = MessageBox.Show("确定借阅？", "借阅", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                    if (result == DialogResult.OK)
                                    {
                                        sql = String.Format("Insert into BookLendList values('{0}','{1}','{2}','{3}', 0.0, 0, 0)", Rid, BISBN, d, dateTimePicker2.Value.ToString());
                                        comm.CommandText = sql;
                                        int kk = (int)comm.ExecuteNonQuery();
                                        sql = String.Format("Update BookList set lendnum = lendnum + 1  where ISBN = '{0}'", BISBN);
                                        comm.CommandText = sql;
                                        kk += (int)comm.ExecuteNonQuery();
                                        sql = String.Format("Update Reader set RbLnum = RbLnum + 1 where Rid = '{0}'", Rid);
                                        comm.CommandText = sql;
                                        kk += (int)comm.ExecuteNonQuery();
                                        if (kk == 3)
                                            MessageBox.Show("借阅成功！", "借阅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        else
                                            MessageBox.Show("借阅失败！", "借阅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else if (k > 0)
                                {
                                    rtbRead.Text += k;
                                    MessageBox.Show("该读者存在超期未还书籍", "借阅失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    sql = String.Format("Select * from BookList, BookLendList where Rid = '{0}' and BookList.ISBN = BookLendList.ISBN and BackTime < '{1}' and isback = 0", Rid, dt);
                                    comm.CommandText = sql;
                                    dr = comm.ExecuteReader();
                                    rtbBook.Clear();
                                    while (dr.Read())
                                    {
                                        rtbBook.Text += "书名：" + dr["BookName"].ToString() + "\n" + "借阅时间：" + dr["LendTime"].ToString() + "\n应还日期：" + dr["BackTime"].ToString() + "\n超期扣款：";
                                        string ddd = dr["BackTime"].ToString();
                                        string a = ddd.Substring(0, ddd.IndexOf("/"));
                                        int t = ddd.IndexOf("/");
                                        ddd = ddd.Remove(t, 1);
                                        ddd = ddd.Insert(t, "a");
                                        int t1 = ddd.IndexOf("a");
                                        t = ddd.IndexOf("/") - 1;
                                        string b = ddd.Substring(t1 + 1, t - t1);
                                        if (b.Length == 1)
                                            b = "0" + b;
                                        t1 = ddd.IndexOf(" ") - 1;
                                        string c = ddd.Substring(t + 2, t1 - t - 1);
                                        if (c.Length == 1)
                                            c = "0" + c;
                                        System.Globalization.DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
                                        dtFormat.ShortDatePattern = "yyyy/MM/dd";
                                        DateTime dd = Convert.ToDateTime(a + "/" + b + "/" + c, dtFormat);
                                        TimeSpan d3 = dt.Subtract(dd);
                                        pm += (double)d3.Days * 0.1;
                                        rtbBook.Text += (double)d3.Days * 0.1 + "\n****************************\n";
                                    }
                                    rtbRead.Text += "\n扣款总额：" + pm.ToString();
                                    dr.Close();
                                }
                                else if (n >= m)
                                {
                                    rtbRead.Text += k;
                                    MessageBox.Show("该读者借阅数目超限", "借阅失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }//正常
                        }//身份证号查阅失败
                    }//无库存
                }//ISBN查找失败
            }
            conn.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtReaderid.Clear();
            txtbookISBN.Clear();
            rtbRead.Clear();
            rtbBook.Clear();
        }
    }
}