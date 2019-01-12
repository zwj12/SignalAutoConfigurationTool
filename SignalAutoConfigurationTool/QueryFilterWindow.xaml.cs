using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SignalAutoConfigurationTool
{
    /// <summary>
    /// Interaction logic for QueryFilterWindow.xaml
    /// </summary>
    public partial class QueryFilterWindow : Window
    {
        public DataTable myDataTable_QueryFilter;
        public DataTable myDataTable_Binding;
        public string str_Filter;

        public QueryFilterWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] str_Logic = new string[] { "AND", "OR" };
            this.comboBox_Logic.ItemsSource = str_Logic;
            this.comboBox_Logic.SelectedIndex = 0;

            string[] str_Compare = new string[] { "=", ">=", "<=", "!=", "Like", ">", "<", "<>", "is Null" };
            this.comboBox_Compare.ItemsSource = str_Compare;
            this.comboBox_Compare.SelectedIndex = 0;

            if (myDataTable_QueryFilter == null)
            {
                myDataTable_QueryFilter = new DataTable("QueryFilter");
                myDataTable_QueryFilter.Columns.Add("Logic", Type.GetType("System.String"));
                myDataTable_QueryFilter.Columns.Add("Header", Type.GetType("System.String"));
                myDataTable_QueryFilter.Columns.Add("Key", Type.GetType("System.String"));
                myDataTable_QueryFilter.Columns.Add("Compare", Type.GetType("System.String"));
                myDataTable_QueryFilter.Columns.Add("KeyValue", Type.GetType("System.String"));
                myDataTable_QueryFilter.Columns.Add("DataType", Type.GetType("System.String"));
            }
            this.dataGrid_QueryFilter.ItemsSource =new DataView(myDataTable_QueryFilter);
            
            this.comboBox_Key.DisplayMemberPath = "Header";
            this.comboBox_Key.SelectedValuePath = "Key";
            this.comboBox_Key.ItemsSource = new DataView(myDataTable_Binding);

            this.comboBox_Logic.Focus();
        }

        private void DataAdd()
        {
            DataRow myDataRow;
            myDataRow = myDataTable_QueryFilter.NewRow();
            switch (this.comboBox_Logic.Text)
            {
                case "AND":
                    myDataRow["Logic"] = "And";
                    break;
                case "OR":
                    myDataRow["Logic"] = "Or";
                    break;
            }
            myDataRow["Header"] = this.comboBox_Key.Text;
            myDataRow["Key"] = this.comboBox_Key.SelectedValue;
            myDataRow["Compare"] = this.comboBox_Compare.Text;
            myDataRow["KeyValue"] = this.textBox_KeyValue.Text;
            if (this.comboBox_Compare.Text == "Like")
            {
                myDataRow["KeyValue"] = "%" + myDataRow["KeyValue"].ToString().Replace("%", "") + "%";
            }
            //myDataRow["DataType"] = myDataRow_Temp["DataType"].ToString();
            switch (myDataRow["DataType"].ToString().ToLower())
            {
                case "string":
                    break;
                case "int":
                    int int_KeyValue;
                    int.TryParse(this.textBox_KeyValue.Text, out int_KeyValue);
                    myDataRow["KeyValue"] = int_KeyValue;
                    break;
                case "bool":
                    //bool bool_KeyValue;
                    //bool.TryParse(this.textBox_KeyValue.Text, out bool_KeyValue);
                    //myDataRow["KeyValue"]=bool_KeyValue;
                    break;
                case "double":
                    double double_KeyValue;
                    double.TryParse(this.textBox_KeyValue.Text, out double_KeyValue);
                    myDataRow["KeyValue"] = double_KeyValue;
                    break;
                case "datetime":
                    DateTime DateTime_KeyValue;
                    if (DateTime.TryParse(this.textBox_KeyValue.Text, out DateTime_KeyValue))
                    {
                        myDataRow["KeyValue"] = DateTime_KeyValue.ToShortDateString();
                    }
                    else
                    {
                        myDataRow["KeyValue"] = DateTime.Today.ToShortDateString();
                    }
                    break;
            }
            myDataTable_QueryFilter.Rows.Add(myDataRow);
            this.textBox_KeyValue.Text = "";
            this.textBox_KeyValue.Focus();
        }
    }
}
