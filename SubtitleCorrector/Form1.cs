using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SubtitleCorrector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string _result = "";
        private string _fileDir = "";

        private void button1_Click(object sender, EventArgs e)
        {
            using (var sw = File.CreateText( _fileDir+ "\\generated.srt"))
            {
                sw.Write(_result);
                sw.Flush();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var rs = openFileDialog1.ShowDialog();
            if (rs != DialogResult.OK) return;
            var file = openFileDialog1.FileName;
            try
            {
                if (FileType(file).ToLower() != "srt")
                {
                    MessageBox.Show(FileType(file));
                    return;
                }

                _result = CorrectFile(File.ReadAllText(file), double.Parse(textBox1.Text));
                _fileDir = Path.GetDirectoryName(file);
            }
            catch (IOException)
            {
                MessageBox.Show("Try other file");
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void openFileDialog1_FileOk_1(object sender, CancelEventArgs e)
        {
        }

        private static string FileType(string fileName)
        {
            
            for (var i = fileName.Length - 1; i >= 0; i--)
                if (fileName[i] == '.')
                    return fileName.Substring(i + 1);
            return "";
        }

        private string CorrectFile(string text, double delay)
        {
            var spitedText = text.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var res = "";
            foreach (var st in spitedText)
            {
                res += CorrectedLine(st, delay);
                res += '\n';
            }

            return res;
        }

        private string CorrectedLine(string ln, double dl)
        {
            var spittedLine = ln.Split(' ');
            var res = "";
            for (var i = 0; i < spittedLine.Length; i++)
            {
                if (IsDate(spittedLine[i])) spittedLine[i] = FixedDate(spittedLine[i], dl);
                res = res + spittedLine[i] + (i < spittedLine.Length - 1 ? " " : "");
            }

            return res;
        }

        private static bool IsDate(string dt)
        {
            var spitedDt = dt.Split(':');
            return spitedDt.Length == 3;
        }

        private string FixedDate(string dateSt, double dl)
        {
            var spitedDt = dateSt.Split(':');

            var date = spitedDt.Aggregate<string, double>(0, (current, t) => current * 60 + double.Parse(t.Replace(",", ".")));

            date += dl;
            if (date < 0)
                date = 0;

            for (var i = 0; i < spitedDt.Length; i++)
            {
                if (i == 0)
                    spitedDt[i] = "" + Formatted(date % 60);
                else
                    spitedDt[i] = "" + Formatted((int) (date % 60));

                date /= 60;
            }

            var res = "";
            for (var i = spitedDt.Length - 1; i >= 0; i--) res = res + spitedDt[i] + (i > 0 ? ":" : "");

            return res;
        }

        private static string Formatted(double date)
        {
            var res = "";

            var integerPart = (int) date;

            if (integerPart < 10)
                res = res + "0" + integerPart;
            else
                res += integerPart;

            if (!(Math.Abs(integerPart - date) > 0.00001)) return res;
            date -= integerPart;
            var afterThePoint = date.ToString("0.000000");
            afterThePoint = "," + afterThePoint.Substring(2);
            if (afterThePoint.Length > 15)
                afterThePoint = afterThePoint.Substring(0, 4);
            res += afterThePoint;

            return res;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}