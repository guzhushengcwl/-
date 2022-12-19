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
    public partial class DeleteBookList : Form
    {
        public DeleteBookList()
        {
            InitializeComponent();
        }
        string connString = @"Data Source=.;Initial Catalog=Library;User ID=sa;Pwd=123456";
        SqlConnection conn;
        SqlDataReader dr;
        private void button1_Click(object sender, EventArgs e)
        {
            string isbn = textBox1.Text;
            string sql = String.Format("select * from BookList where ISBN='{0}'", isbn);
            int lendnum = 0;
            SqlConnection CONN = new SqlConnection(connString);
            DialogResult result = MessageBox.Show("是否确定删除此图书信息",
                "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //弹出窗口，确认信息
            if (result == DialogResult.OK)
            {
                try
                {
                    conn.Open();
                    CONN.Open();
                    SqlCommand comm = new SqlCommand(sql, conn);
                    dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        lendnum = (int)dr["lendnum"];
                    }
                    if (lendnum != 0)
                    {
                        MessageBox.Show("尚有在借未还的该图书！", "删除失败", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        string SQL = String.Format("delete from BookList where ISBN='{0}'", isbn);
                        SqlCommand COMM = new SqlCommand(SQL, CONN);
                        int count = COMM.ExecuteNonQuery();
                        if (count > 0)
                            MessageBox.Show("删除图书信息成功", "删除成功", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        else
                            MessageBox.Show("删除图书信息失败，请确认图书信息是否存在。", "删除失败", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "操作数据库出错！", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                finally
                {
                    conn.Close();
                    CONN.Close();
                    dr.Close();
                }
                conn = new SqlConnection(connString);
                string Sql = "select ISBN as ISBN,BookName as 书名,bCategoryid as 图书类别编号,author as 第一作者,publisher as 第一出版社,"
                    + " publishTime as 出版年份,bookstate as 状态,num as 库存数目,lendnum as 借出数目 "
                    + "from BookList";
                SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                textBox1.Text = "";
            }
        }

        private void DelectBookList_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString);
            string sql = "select ISBN as ISBN,BookName as 书名,bCategoryid as 图书类别编号,author as 第一作者,publisher as 第一出版社,"
                    + " publishTime as 出版年份,bookstate as 状态,num as 库存数目,lendnum as 借出数目 "
                    + "from BookList";
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
