using System;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TestCSharpForm
{
    public partial class FormListView : Form
    {
        //ListViewItemSorterに指定するフィールド
        ListViewItemComparer listViewItemSorter;

        private int itemCnt = 0;

        public FormListView()
        {
            InitializeComponent();
        }

        private void FormListView_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns[0].Width = 200;

            // 列を追加
            listView1.Columns.Add("memo", 100, HorizontalAlignment.Center);

            // 項目を追加
            for (int i = 0; i < 10; i++)
            {
                addItem();
            }

            // 列の色を変更
            //listView1.BackColor = System.Drawing.Color.Black;

            //ListViewItemComparerの作成と設定
            listViewItemSorter = new ListViewItemComparer();
            listViewItemSorter.ColumnModes =
                new ListViewItemComparer.ComparerMode[]
            {
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.Integer,
                ListViewItemComparer.ComparerMode.String,
                ListViewItemComparer.ComparerMode.String
            };

            //ListViewItemSorterを指定する
            listView1.ListViewItemSorter = listViewItemSorter;
        }

        private void addItem()
        {
            ListViewItem item = new ListViewItem(String.Format("name{0}", itemCnt + 1));  // name
            item.SubItems.Add((10 + itemCnt).ToString());     // age
            item.SubItems.Add("123-4567");     // tel
            item.SubItems.Add(String.Format("memo{0}", itemCnt + 1));
            listView1.Items.Add(item);
            itemCnt++;
        }

        private void deleteItem(int index)
        {
            if (listView1.Items.Count > index)
            {
                listView1.Items.RemoveAt(index);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選択された項目を表示する
            StringBuilder sb = new StringBuilder();
            foreach (ListViewItem item in ((ListView)sender).SelectedItems) 
            {
                sb.Append( String.Format(",{0}", item.Index));
            }
            Console.WriteLine("SelectedIndexChanged {0}", sb);
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            Console.WriteLine("ColumnClick {0}", e.Column);

            //クリックされた列を設定
            listViewItemSorter.Column = e.Column;
            //並び替える
            listView1.Sort();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                // 選択された項目を表示する
                StringBuilder sb = new StringBuilder();
                foreach (ListViewItem item in ((ListView)sender).SelectedItems)
                {
                    sb.Append(String.Format(",{0}", item.Index));
                }
                Console.WriteLine("DoubleClick {0}", sb);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 項目を追加する
            addItem();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 選択項目を削除する
            if (listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    deleteItem(item.Index);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 全項目を表示する
            foreach(ListViewItem item in listView1.Items)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 全項目を削除する
            listView1.Items.Clear();
        }
    }
    /// <summary>
    /// ListViewの項目の並び替えに使用するクラス
    /// </summary>
    public class ListViewItemComparer : IComparer
    {
        /// <summary>
        /// 比較する方法
        /// </summary>
        public enum ComparerMode
        {
            /// <summary>
            /// 文字列として比較
            /// </summary>
            String,
            /// <summary>
            /// 数値（Int32型）として比較
            /// </summary>
            Integer,
            /// <summary>
            /// 日時（DataTime型）として比較
            /// </summary>
            DateTime
        };

        private int _column;
        private SortOrder _order;
        private ComparerMode _mode;
        private ComparerMode[] _columnModes;

        /// <summary>
        /// 並び替えるListView列の番号
        /// </summary>
        public int Column
        {
            set
            {
                //現在と同じ列の時は、昇順降順を切り替える
                if (_column == value)
                {
                    if (_order == SortOrder.Ascending)
                    {
                        _order = SortOrder.Descending;
                    }
                    else if (_order == SortOrder.Descending)
                    {
                        _order = SortOrder.Ascending;
                    }
                }
                _column = value;
            }
            get
            {
                return _column;
            }
        }
        /// <summary>
        /// 昇順か降順か
        /// </summary>
        public SortOrder Order
        {
            set
            {
                _order = value;
            }
            get
            {
                return _order;
            }
        }
        /// <summary>
        /// 並び替えの方法
        /// </summary>
        public ComparerMode Mode
        {
            set
            {
                _mode = value;
            }
            get
            {
                return _mode;
            }
        }
        /// <summary>
        /// 列ごとの並び替えの方法
        /// </summary>
        public ComparerMode[] ColumnModes
        {
            set
            {
                _columnModes = value;
            }
        }

        /// <summary>
        /// ListViewItemComparerクラスのコンストラクタ
        /// </summary>
        /// <param name="col">並び替える列の番号</param>
        /// <param name="ord">昇順か降順か</param>
        /// <param name="cmod">並び替えの方法</param>
        public ListViewItemComparer(
            int col, SortOrder ord, ComparerMode cmod)
        {
            _column = col;
            _order = ord;
            _mode = cmod;
        }
        public ListViewItemComparer()
        {
            _column = 0;
            _order = SortOrder.Ascending;
            _mode = ComparerMode.String;
        }

        //xがyより小さいときはマイナスの数、大きいときはプラスの数、
        //同じときは0を返す
        public int Compare(object x, object y)
        {
            if (_order == SortOrder.None)
            {
                //並び替えない時
                return 0;
            }

            int result = 0;
            //ListViewItemの取得
            ListViewItem itemx = (ListViewItem)x;
            ListViewItem itemy = (ListViewItem)y;

            //並べ替えの方法を決定
            if (_columnModes != null && _columnModes.Length > _column)
            {
                _mode = _columnModes[_column];
            }

            //並び替えの方法別に、xとyを比較する
            switch (_mode)
            {
                case ComparerMode.String:
                    //文字列をとして比較
                    result = string.Compare(itemx.SubItems[_column].Text,
                        itemy.SubItems[_column].Text);
                    break;
                case ComparerMode.Integer:
                    //Int32に変換して比較
                    //.NET Framework 2.0からは、TryParseメソッドを使うこともできる
                    result = int.Parse(itemx.SubItems[_column].Text).CompareTo(
                        int.Parse(itemy.SubItems[_column].Text));
                    break;
                case ComparerMode.DateTime:
                    //DateTimeに変換して比較
                    //.NET Framework 2.0からは、TryParseメソッドを使うこともできる
                    result = DateTime.Compare(
                        DateTime.Parse(itemx.SubItems[_column].Text),
                        DateTime.Parse(itemy.SubItems[_column].Text));
                    break;
            }

            //降順の時は結果を+-逆にする
            if (_order == SortOrder.Descending)
            {
                result = -result;
            }

            //結果を返す
            return result;
        }
    }
}
