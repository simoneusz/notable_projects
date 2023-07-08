using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjInj_idz
{
    public partial class Search : Form
    {
        private XmlDocument _doc;
        private DocumentSaver currentSaver;

        public Search(XmlDocument xmlDocument)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            saveFileDialog1.Filter = "Text File (*.txt)|*.txt|HTML File (*.html)|*.html";
            _doc = xmlDocument;
            FillComboBox();
            InitTable();
        }
        private void FillComboBox()
        {
            XmlNodeList employees = _doc.SelectNodes("//employee");
            List<string> facultyList = new List<string>();
            List<string> departmentList = new List<string>();
            List<string> positionList = new List<string>();
            List<string> operators = new List<string>() { "=", ">=", "<=" };

            foreach (string op in operators)
            {
                comboBox1.Items.Add(op);
                comboBox5.Items.Add(op);
            }

            foreach (XmlNode employee in employees)
            {
                string faculty = employee.SelectSingleNode("faculty").InnerText;
                if (!facultyList.Contains(faculty))
                {
                    comboBox2.Items.Add(faculty);
                    facultyList.Add(faculty);
                }

                string department = employee.SelectSingleNode("department").InnerText;
                if (!departmentList.Contains(department))
                {
                    comboBox3.Items.Add(department);
                    departmentList.Add(department);
                }

                string position = employee.SelectSingleNode("position").InnerText;
                if (!positionList.Contains(position))
                {
                    comboBox4.Items.Add(position);
                    positionList.Add(position);
                }
            }
        }
        private void InitTable()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Name", "Имя");
            dataGridView1.Columns.Add("Faculty", "Факультет");
            dataGridView1.Columns.Add("Department", "Кафедра");
            dataGridView1.Columns.Add("Position", "Должность");
            dataGridView1.Columns.Add("Salary", "Зарплата");
            dataGridView1.Columns.Add("Years", "Стаж");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;

            InitTable();
            int count = 0;
            string name = textBox3.Text.Trim();
            string salaryStr = textBox1.Text.Trim();
            string yearsStr = textBox2.Text.Trim();
            string faculty = (string)comboBox2.SelectedItem;
            string department = (string)comboBox3.SelectedItem;
            string position = (string)comboBox4.SelectedItem;

            if (!int.TryParse(salaryStr, out int salary))
            {
                salary = int.MinValue;
            }

            if (!int.TryParse(yearsStr, out int years))
            {
                years = int.MinValue;
            }

            dataGridView1.Rows.Clear();

            XmlNodeList employees = _doc.SelectNodes("//employee");
            foreach (XmlNode employee in employees)
            {
                bool match = true;

                if (!string.IsNullOrEmpty(name))
                {
                    string employeeName = employee.SelectSingleNode("name").InnerText;
                    if (!employeeName.Contains(name))
                    {
                        match = false;
                    }
                }

                string salaryOperator = "";

                if (comboBox1.SelectedItem != null)
                {
                    salaryOperator = comboBox1.SelectedItem.ToString();
                }

                if (salary != int.MinValue)
                {
                    int employeeSalary = int.Parse(employee.SelectSingleNode("salary").InnerText);
                    switch (salaryOperator)
                    {
                        case ">=":
                            if (employeeSalary < salary)
                            {
                                match = false;
                            }
                            break;
                        case "<=":
                            if (employeeSalary > salary)
                            {
                                match = false;
                            }
                            break;
                        case "=":
                            if (employeeSalary != salary)
                            {
                                match = false;
                            }

                        break;
                    }
                }

                string yearsOperator = "";

                if (comboBox5.SelectedItem != null)
                {
                    yearsOperator = comboBox5.SelectedItem.ToString();
                }

                if (years != int.MinValue)
                {
                    int employeeYears = int.Parse(employee.SelectSingleNode("years").InnerText);
                    switch (yearsOperator)
                    {
                        case ">=":
                            if (employeeYears < years)
                            {
                                match = false;
                            }
                            break;
                        case "<=":
                            if (employeeYears > years)
                            {
                                match = false;
                            }
                            break;
                        case "=":
                            if (employeeYears != years)
                            {
                                match = false;
                            }
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(faculty))
                {
                    string employeeFaculty = employee.SelectSingleNode("faculty").InnerText;
                    if (!employeeFaculty.Contains(faculty))
                    {
                        match = false;
                    }
                }

                if (!string.IsNullOrEmpty(department))
                {
                    string employeeDepartment = employee.SelectSingleNode("department").InnerText;
                    if (!employeeDepartment.Contains(department))
                    {
                        match = false;
                    }
                }

                if (!string.IsNullOrEmpty(position))
                {
                    string employeePosition = employee.SelectSingleNode("position").InnerText;
                    if (!employeePosition.Contains(position))
                    {
                        match = false;
                    }
                }

                if (match)
                {
                    string[] row = {
                        employee.SelectSingleNode("name").InnerText,
                        employee.SelectSingleNode("faculty").InnerText,
                        employee.SelectSingleNode("department").InnerText,
                        employee.SelectSingleNode("position").InnerText,
                        employee.SelectSingleNode("salary").InnerText,
                        employee.SelectSingleNode("years").InnerText
                    };
                    dataGridView1.Rows.Add(row);
                    count++;
                }
            }
            label7.Text = "Total items: " + count;
            DateTime endTime = DateTime.Now;

            TimeSpan elapsedTime = endTime - startTime;
            double milliseconds = elapsedTime.TotalMilliseconds;

            Debug.WriteLine($"Время выполнения запроса: {milliseconds} мс");

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;

            string fileName = saveFileDialog1.FileName;
            string extension = Path.GetExtension(fileName).ToLower().ToString();
            switch (extension)
            {
                case ".txt":
                    currentSaver = new TxtDocumentSaver(dataGridView1);
                    currentSaver.Save(saveFileDialog1.FileName);
                    break;
                case ".html":
                    currentSaver = new HtmlDocumentSaver(dataGridView1);
                    currentSaver.Save(saveFileDialog1.FileName);
                    break;
                default:
                    throw new NotSupportedException("Unsupported file format: " + extension);
            }
        }
    }
}
