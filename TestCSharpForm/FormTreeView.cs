using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCSharpForm
{
    public partial class FormTreeView : Form
    {
        private int nodeCnt = 1;

        public FormTreeView()
        {
            InitializeComponent();
        }

        private void FormTreeView_Load(object sender, EventArgs e)
        {
            treeView1.Font = new Font("Arial", 15);
            treeView1.CheckBoxes = true;

            // トップレベルにノードを追加
            TreeNode parent1 = new TreeNode("1年");
            
            treeView1.Nodes.Add(parent1);

            // 子ノードを追加
            TreeNode child1 = new TreeNode("A組");
            child1.Checked = true;
            parent1.Nodes.Add(child1);
            TreeNode child2 = new TreeNode("B組");
            parent1.Nodes.Add(child2);
            TreeNode child3 = new TreeNode("C組");
            parent1.Nodes.Add(child3);

            // トップレベルにノードを追加
            TreeNode parent2 = new TreeNode("2年");

            treeView1.Nodes.Add(parent2);

            // 子ノードを追加
            TreeNode child21 = new TreeNode("A組");
            parent2.Nodes.Add(child21);
            TreeNode child22 = new TreeNode("B組");
            parent2.Nodes.Add(child22);
            TreeNode child23 = new TreeNode("C組");
            parent2.Nodes.Add(child23);

            // トップレベルにノードを追加
            TreeNode parent3 = new TreeNode("3年");

            treeView1.Nodes.Add(parent3);

            // 子ノードを追加
            TreeNode child31 = new TreeNode("A組");
            parent3.Nodes.Add(child31);
            TreeNode child32 = new TreeNode("B組");
            parent3.Nodes.Add(child32);
            TreeNode child33 = new TreeNode("C組");
            parent3.Nodes.Add(child33);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 選択中のノードと同じ階層にノードを追加する
            TreeNode node = treeView1.SelectedNode;
            if (node == null)
            {
                treeView1.Nodes.Add(new TreeNode("new node" + nodeCnt++));
            }
            else if (node.Parent != null)
            {
                node.Parent.Nodes.Add(new TreeNode("new node" + nodeCnt++));
            }
            else
            {
                treeView1.Nodes.Add(new TreeNode("new node" + nodeCnt++));
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 選択中のノードに子ノードを追加する
            if (treeView1.SelectedNode != null)
            {
                treeView1.SelectedNode.Nodes.Add(new TreeNode("new node" + nodeCnt++));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ノード情報を表示する
            foreach (TreeNode node in treeView1.Nodes)
            {
                Console.WriteLine("node:{0}", node.FullPath);
                ShowNode(node);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 選択しているノードを削除する
            if (treeView1.SelectedNode != null)
            {
                treeView1.SelectedNode.Remove();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
        }
        // ノードがクリックされた
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (node == null)
            {
                return;
            }

            Console.WriteLine("node:{0}", node.FullPath);
        }

        // 子を再帰的に表示
        private void ShowNode(TreeNode node)
        {
            foreach(TreeNode child in node.Nodes)
            {
                Console.WriteLine("node:{0}", child.FullPath);
                if (child.Nodes.Count > 0)
                {
                    ShowNode(child);
                }
            }
        }

    }
}
