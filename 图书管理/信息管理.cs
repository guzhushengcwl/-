﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 信息管理
{
    public partial class 信息管理 : Form
    {
        public 信息管理()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void 添加图书信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 更新图书ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 删除用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deleteuser fm2 = new Deleteuser();
            fm2.ShowDialog();
        }

        private void 添加图书信息ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Addbook fm2 = new Addbook();
            fm2.ShowDialog();

        }

        private void 更新图书ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Updatebook fm3 = new Updatebook();
            fm3.ShowDialog();
        }

        private void 报废图书ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deletebook fm3 = new Deletebook();
            fm3.ShowDialog();
        }

        private void 密码重置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pw_reset fm3 = new pw_reset();
            fm3.ShowDialog();
        }

        private void 管理员删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteAttendant fm3 = new DeleteAttendant();
            fm3.ShowDialog();
        }

        private void 信息更改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateAttendant fm3 = new UpdateAttendant();
            fm3.ShowDialog();
        }

        
    }
}
