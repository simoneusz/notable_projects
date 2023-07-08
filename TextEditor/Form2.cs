using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextEditor
{
    public sealed partial class Form1 : Form
    {
        private string text;
        private static readonly EventLogger eventLogger = EventLogger.Instance;
        private DocumentSaver currentSaver;
        private readonly IFileLoaderFactory _fileLoaderFactory;
        private readonly IObserver _authorSheetObserver;
        private readonly ObservableTextControl _observableTextControl;
        public Form1()
        {
            InitializeComponent();
            _fileLoaderFactory = new FileLoaderFactory();
            saveFileDialog1.Filter = "Text File (*.txt)|*.txt|HTML File (*.html)|*.html|Binary File (*.bin)|*.bin";
            saveFileDialog2.Filter = "JSON (*.json)|*.json";
            _authorSheetObserver = new AuthorSheetObserver(1800);
            _observableTextControl = new ObservableTextControl(MainText);
            _observableTextControl.RegisterObserver(_authorSheetObserver);

        }
        private void MainText_TextChanged(object sender, EventArgs e)
        {
            int line = MainText.GetLineFromCharIndex(MainText.SelectionStart);
            int column = MainText.SelectionStart - MainText.GetFirstCharIndexOfCurrentLine();
            eventLogger.LogEvent(EventType.TextAdded, line, column);
            text = MainText.Text;
        }
        private void MainText_KeyPress(object sender, KeyPressEventArgs e)
        {
            int line = MainText.GetLineFromCharIndex(MainText.SelectionStart);
            int column = MainText.SelectionStart - MainText.GetFirstCharIndexOfCurrentLine();
            eventLogger.LogEvent(EventType.CharacterDeleted, line, column);
        }
        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string textFilePath = saveFileDialog1.FileName;
                string reportFilePath = Path.ChangeExtension(textFilePath, ".json");

                var eventLogs = eventLogger.GetEventLogs();
                var reportBuilder = new ReportBuilder(eventLogs);
                var reportJson = reportBuilder.BuildReport();

                File.WriteAllText(reportFilePath, reportJson);
            }
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextStatistics stats = new(text);

            string message = "Statistics for the text:\n" +
                             "Size: " + stats.SizeInKb.ToString("F2") + " KB\n" +
                             "Symbols: " + stats.Symbols + "\n" +
                             "Paragraphs: " + stats.Paragraphs + "\n" +
                             "Empty Lines: " + stats.EmptyLineCount + "\n" +
                             "Author Pages: " + stats.AuthorPages + "\n" +
                             "Vowels: " + stats.CountVowels + "\n" +
                             "Consonants: " + stats.CountConsonants + "\n" +
                             "Numeric Characters: " + stats.CountNumeric + "\n" +
                             "Special Symbols: " + stats.CountSpecialSymbols;

            MessageBox.Show(message, "Text Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private string RemoveWhiteSpace(string pattern)
        {
            string tempstr = pattern.Replace('\t', ' ');

            StringBuilder SB = new StringBuilder();
            bool PrevChar = false;
            foreach (char item in tempstr)
            {
                bool NextChar = item == ' ';
                if (!NextChar || !PrevChar)
                {
                    SB.Append(item);
                    PrevChar = NextChar;
                }
            }
            return SB.ToString();
        }
        private void whiteSpacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            TableLayoutPanel table = new TableLayoutPanel();
            table.ColumnCount = 2;

            RichTextBox richTextBox1 = new RichTextBox();
            richTextBox1.Text = MainText.Text;
            RichTextBox richTextBox2 = new RichTextBox();
            richTextBox2.Text = RemoveWhiteSpace(MainText.Text);

            richTextBox1.Dock = DockStyle.Fill;
            richTextBox2.Dock = DockStyle.Fill;

            table.Controls.Add(richTextBox1, 0, 0);
            table.Controls.Add(richTextBox2, 1, 0);

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            table.Dock = DockStyle.Fill;

            if (MainText.Text == richTextBox2.Text)
            {
                MessageBox.Show("Nothing to change", "MessageBox Title");
                return;
            }
            MainText.Controls.Add(table);


            var a = MessageBox.Show("Save?", "", MessageBoxButtons.OKCancel);
            if(a == DialogResult.OK)
            {
                MainText.Text = richTextBox2.Text;
            }
            MainText.Controls.Remove(table);
            MainText.Dock = DockStyle.Fill;
        }
        private string RemoveEmptyLines(string text)
        {
            while (text.Contains("\n\n"))
            {
                text = text.Replace("\n\n", "\n");
            }
            return text;
        }
        private void emptyLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            TableLayoutPanel table = new TableLayoutPanel();
            table.ColumnCount = 2;

            RichTextBox richTextBox1 = new RichTextBox();
            richTextBox1.Text = MainText.Text;
            RichTextBox richTextBox2 = new RichTextBox();
            richTextBox2.Text = RemoveEmptyLines(MainText.Text);

            richTextBox1.Dock = DockStyle.Fill;
            richTextBox2.Dock = DockStyle.Fill;

            table.Controls.Add(richTextBox1, 0, 0);
            table.Controls.Add(richTextBox2, 1, 0);

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            table.Dock = DockStyle.Fill;

            if (MainText.Text == richTextBox2.Text)
            {
                MessageBox.Show("Nothing to change", "MessageBox Title");
                return;
            }
            MainText.Controls.Add(table);


            var a = MessageBox.Show("Save?", "", MessageBoxButtons.OKCancel);
            if (a == DialogResult.OK)
            {
                MainText.Text = richTextBox2.Text;
            }
            MainText.Controls.Remove(table);
            MainText.Dock = DockStyle.Fill;

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;

            string fileName = saveFileDialog1.FileName;
            string extension = Path.GetExtension(fileName).ToLower().ToString();
            switch (extension)
            {
                case ".txt":
                    currentSaver = new TxtDocumentSaver(text);
                    currentSaver.Save(saveFileDialog1.FileName);
                    break;
                case ".html":
                    currentSaver = new HtmlDocumentSaver(text);
                    currentSaver.Save(saveFileDialog1.FileName);
                    break;
                case ".bin":
                    currentSaver = new BinaryDocumentSaver(text);
                    currentSaver.Save(saveFileDialog1.FileName);
                    break;
                default:
                    throw new NotSupportedException("Unsupported file format: " + extension);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;

            string fileName = openFileDialog1.FileName;
            string extension = Path.GetExtension(fileName).ToLower();

            try
            {
                IFileLoader loader = _fileLoaderFactory.Create(fileName);
                MainText.Text = loader.Load(fileName);
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show(ex.Message, "Unsupported File Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void requestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebRequester request = new();
            MainText.Text += request.MakeRequest();
        }
       

        private void keywordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] CSharpKeywords = { "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while" };

            bool containsCSharpKeyword = false;

            foreach (var keyword in CSharpKeywords)
            {
                if (MainText.Text.Contains(keyword))
                {
                    containsCSharpKeyword = true;
                    break;
                }
            }

            var messageBoxText = containsCSharpKeyword ? "There is at least one C# keyword" : "There is no C# keyword.";
            MessageBox.Show(messageBoxText);
        }

        private void tenLongestWordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            char[] separators = MainText.Text.Where(c => !Char.IsLetterOrDigit(c)).Distinct().ToArray();
            string[] words = MainText.Text.Split(separators);
            var longestWords = words.OrderByDescending(w => w.Length).Take(10);
            var message = string.Join(Environment.NewLine, longestWords.ToArray());
            MessageBox.Show(longestWords.ToArray().Length + " longest words is:\n" + message);

        }
    }
}
