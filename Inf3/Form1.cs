using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Inf3
{

    public partial class Form1 : Form
    {
        public int total = 0;
        public int now = 0;
        public long size = 0;
        public Form1()
        {
            InitializeComponent();

        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "";
            string[] path = new string[10];
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                treeView1.BeforeSelect += treeView1_BeforeSelect;
                treeView1.BeforeExpand += treeView1_BeforeExpand;
                str = FBD.SelectedPath;
                TreeNode dirNode = new TreeNode(str);
                FillTreeNode(dirNode, str);
                treeView1.Nodes.Add(dirNode);
            }
        }
        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void FolderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
        void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {
                if (Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if (dirs.Length != 0)
                    {
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {
                if (Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if (dirs.Length != 0)
                    {
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void FillTreeNode(TreeNode driveNode, string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    TreeNode dirNode = new TreeNode();
                    dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                    driveNode.Nodes.Add(dirNode);
                }
            }
            catch (Exception ex) { }
        }
        private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        public static void Function(ref TreeView treeView1, ref ListView listView1, ref Chart chart1, DirectoryInfo Dir, ref StatusStrip statusStrip1, ref int now, ref int total, ref Button button1,ref long size)
        {
            button1.Enabled = true;
            listView1.Items.Clear();
            FileInfo[] files = Dir.GetFiles();
            int count = files.Length;
            if (files.Length != 0)
            {
                size = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    if (i >= count) break;
                    long Length = files[i].Length / 1024;
                    string color = Type(files[i].Name);
                    string[] arr = { files[i].Name, Length.ToString(), color };
                    ListViewItem lv = new ListViewItem(arr);
                    lv.Checked = true;
                    if (C(color) == 1)
                    {
                        lv.BackColor = Color.Red;
                    }
                    if (C(color) == 2)
                    {
                        lv.BackColor = Color.Green;
                    }
                    if (C(color) == 3)
                    {
                        lv.BackColor = Color.Orange;
                    }
                    if (C(color) == 4)
                    {
                        lv.BackColor = Color.Yellow;
                    }
                    if (C(color) == 5)
                    {
                        lv.BackColor = Color.Blue;
                    }
                    listView1.Items.Add(lv);
                    size += Length;

                }
                total = count;
                now = total;
                
                statusStrip1.Items[0].Text = "Total Kilobytes: " + size + "KB";
                statusStrip1.Items[1].Text = now + " of " + total + " items selected";
            }
        }
        public static string Type(string S)
        {
            char[] A = S.ToCharArray();
            char[] B = new char[4];
            B[3] = ' ';
            string tmp = "";
            int i = A.Length - 1;
            int j = 0;
            while (A[i] != '.')
            {
                B[j] = A[i];
                j++;
                i--;
            }
            Array.Reverse(B);
            for (int x = 0; x < j; x++)
            {
                if (B[x] == ' ')
                {
                    j++;
                    continue;
                }
                tmp += B[x];
            }

            return tmp;
        }
        private void Chart1_Click(object sender, EventArgs e)
        {

        }
        private void TreeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            DirectoryInfo Dir = new DirectoryInfo(treeView1.SelectedNode.FullPath);
            FileInfo[] files = Dir.GetFiles();
            if (files.Length != 0)
            {
                listView1.Items.Clear();
                Function(ref treeView1, ref listView1, ref chart1, Dir, ref statusStrip1, ref now, ref total, ref button1,ref size);
            }
        }
        public static int C(string S)
        {
            string[] gf = new string[] { "png", "jpg", "bmp", "gif" };
            string[] doc = new string[] { "docx", "xlsx", "pdf", "txt" };
            string[] dat = new string[] { "zip", "rar", "7z" };
            if (S == "exe") return 4;
            if (S == "dll") return 5;
            for (int i = 0; i < gf.Length; i++)
            {
                if (S == gf[i])
                {
                    return 1;
                }
                else if (S == doc[i])
                {
                    return 2;
                }
                if (i == 3)
                {
                    continue;
                }
                else if (S == dat[i])
                {
                    return 3;
                }
            }
            return -1;
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Текстовые файлы (*.txt)|*.txt";
            file.FileName = "Data";
            if (file.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(file.FileName))
                {
                    Save(file.FileName, listView1);
                }
                else
                {
                    File.Delete(file.FileName);
                    Save(file.FileName, listView1);
                }
            }
        }
        public static void Save(string path, ListView listView1)
        {
            StreamWriter file = new StreamWriter(File.Open(path, FileMode.OpenOrCreate));
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                file.Write(listView1.Items[i].SubItems[0].Text.ToString() + " ");
                file.Write(listView1.Items[i].SubItems[1].Text.ToString() + " ");
                file.Write(listView1.Items[i].SubItems[2].Text.ToString());
                file.WriteLine();
            }
            file.Close();
        }

        private void FontToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            listView1.Font = fontDialog1.Font;
            treeView1.Font = fontDialog1.Font;
        }

        private void ColorToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            listView1.BackColor = colorDialog1.Color;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ListView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            int all = 0;
            size = 0;
            now = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked == true)
                {
                    all += listView1.Items[i].SubItems[0].Text.Length;
                    size += Convert.ToInt64(listView1.Items[i].SubItems[1].Text);
                    now++;
                }
            }
            int l, r;
            l = ((all / now) / 4) + ((all / now) / 2);
            r = l + ((all / now) / 2);
            int s = 0, m = 0, h = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked == true)
                {
                    if (listView1.Items[i].SubItems[0].Text.Length < l) { s++; }
                    if (listView1.Items[i].SubItems[0].Text.Length >= l && listView1.Items[i].SubItems[0].Text.Length <= r) { m++; }
                    if (listView1.Items[i].SubItems[0].Text.Length > r) { h++; }
                }
            }
            chart1.Series[0].Points.AddXY("малые", s);
            chart1.Series[0].Points.AddXY("средние", m);
            chart1.Series[0].Points.AddXY("большие", h);
            statusStrip1.Items[0].Text = "Total Kilobytes: " + size + "KB";
            statusStrip1.Items[1].Text = now + " of " + total + " items selected";
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

