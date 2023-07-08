using System;
using System.Diagnostics;
using System.Text.Json;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace ProjInj_idz
{
    public abstract class DocumentSaver
    {
        protected DataGridView dataGridView;
        public DocumentSaver(DataGridView dataGridView)
        {
            this.dataGridView = dataGridView;
        }

        public abstract void Save(string fileName);
    }

    public class TxtDocumentSaver : DocumentSaver
    {
        public TxtDocumentSaver(DataGridView dataGridView) : base(dataGridView)
        {
        }

        public override void Save(string fileName)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("There are no data to save!", "Error");
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    sb.Append(cell.Value + " ");
                }
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(fileName, sb.ToString());
        }
    }

    public class HtmlDocumentSaver : DocumentSaver
    {
        public HtmlDocumentSaver(DataGridView dataGridView) : base(dataGridView)
        {
        }

        public override void Save(string fileName)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("There are no data to save!", "Error");
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border=\"1\">");
            sb.Append("<tr>");
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                sb.Append("<th>" + column.HeaderText + "</th>");
            }
            sb.Append("</tr>");
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                sb.Append("<tr>");
                foreach (DataGridViewCell cell in row.Cells)
                {
                    sb.Append("<td>" + cell.Value + "</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            File.WriteAllText(fileName, sb.ToString());

        }
    }

}