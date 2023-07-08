using System;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjInj_idz
{
    public partial class Form1 : Form
    {
        private XmlDocument doc;
        public Form1()
        {
            InitializeComponent();
            InitTable();
            saveFileDialog1.Filter = "XML (*.xml)|*.xml";
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            doc = new XmlDocument();
            XmlElement root = doc.CreateElement("employees");
            doc.AppendChild(root);
        }

        private void InitTable()
        {
            dataGridView1.Columns.Add("Name", "Имя");
            dataGridView1.Columns.Add("Faculty", "Факультет");
            dataGridView1.Columns.Add("Department", "Кафедра");
            dataGridView1.Columns.Add("Position", "Должность");
            dataGridView1.Columns.Add("Salary", "Зарплата");
            dataGridView1.Columns.Add("Years", "Стаж");
        }
        private void AddDataToTable(XmlElement root)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            InitTable();
            foreach (XmlNode employee in root.SelectNodes("//employee"))
            {
                string name = employee.SelectSingleNode("name").InnerText;
                string faculty = employee.SelectSingleNode("faculty").InnerText;
                string department = employee.SelectSingleNode("department").InnerText;
                string position = employee.SelectSingleNode("position").InnerText;
                int salary = int.Parse(employee.SelectSingleNode("salary").InnerText);
                int yearsOfService = int.Parse(employee.SelectSingleNode("years").InnerText);

                dataGridView1.Rows.Add(name, faculty, department, position, salary, yearsOfService);
            }
        }
        private void SaveDocument()
        {
            string filePath = Path.Combine(Application.StartupPath, "employees.xml");
            doc.Save(filePath);
        }
        private XmlDocument CreateXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("employees");
            doc.AppendChild(root);
            AddDataToTable(root);
            string filePath = Path.Combine(Application.StartupPath, "employees.xml");
            doc.Save(filePath);
            return doc;
        }
        private static XmlElement GenerateEmployee(XmlDocument doc)
        {

            XmlElement employee = doc.CreateElement("employee");

            string[] firstNames = { "Liam", "Noah", "William", "James", "Oliver", "Benjamin", "Elijah", "Lucas", "Mason", "Logan" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Taylor", "Wilson", "Clark", "Lewis", "Walker", "Hall", "Moore" };
            Random rand = new Random();
            string name = $"{firstNames[rand.Next(firstNames.Length)]} {lastNames[rand.Next(lastNames.Length)]}";

            string[] faculties = { "Physics", "Mathematics", "Computer Science", "Biology", "History" };
            string faculty = faculties[rand.Next(faculties.Length)];

            string[] departments = { "Theoretical Physics", "Applied Mathematics", "Programming", "Microbiology", "History" };
            string department = departments[rand.Next(departments.Length)];

            string[] positions = { "Assistant", "Lecturer", "Senior Lecturer", "Associate Professor", "Professor" };
            string position = positions[rand.Next(positions.Length)];

            int salary = rand.Next(2000, 5000);
            int years = rand.Next(1, 10);

            XmlElement nameElem = doc.CreateElement("name");
            nameElem.InnerText = name;

            XmlElement facultyElem = doc.CreateElement("faculty");
            facultyElem.InnerText = faculty;

            XmlElement departmentElem = doc.CreateElement("department");
            departmentElem.InnerText = department;

            XmlElement positionElem = doc.CreateElement("position");
            positionElem.InnerText = position;

            XmlElement salaryElem = doc.CreateElement("salary");
            salaryElem.InnerText = salary.ToString();

            XmlElement yearsElem = doc.CreateElement("years");
            yearsElem.InnerText = years.ToString();

            employee.AppendChild(nameElem);
            employee.AppendChild(facultyElem);
            employee.AppendChild(departmentElem);
            employee.AppendChild(positionElem);
            employee.AppendChild(salaryElem);
            employee.AppendChild(yearsElem);
            return employee;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDocument();
            MessageBox.Show("Doc have been saved", "OK");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string xmlFilePath = openFileDialog1.FileName;
                doc = new XmlDocument();
                doc.Load(xmlFilePath);
                AddDataToTable(doc.DocumentElement);
            }
        }
        private bool IsDocEmpty()
        {
            if (doc == null) return true;
            return false;
        }
        private void randomDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsDocEmpty())
            {
                MessageBox.Show("Doc is empty", "Error");
                return;
            }
            int count;
            try { 
                count = Int32.Parse(Microsoft.VisualBasic.Interaction.InputBox("Number of collections", "", "1"));
            }
            catch(FormatException)
            {
                count = 0;
            }
            if (count < 0)
            {
                count = 0;
                return;
            }
            for (int i = 0; i < count; i++)
            {
                XmlElement employee = GenerateEmployee(doc);
                doc.DocumentElement.AppendChild(employee);
            }
            SaveDocument();
            AddDataToTable(doc.DocumentElement);
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doc = CreateXmlDocument();
            doc.Load("employees.xml");
        }

        private void findToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var searchForm = new Search(doc);
            searchForm.Show();
        }
    }
}
