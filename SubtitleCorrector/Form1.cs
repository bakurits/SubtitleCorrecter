using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleCorrector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult rs = openFileDialog1.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {
                    if (fileType(file).ToLower() != "srt")
                    {
                        MessageBox.Show(fileType(file));
                        return;
                    }
                    string text = File.ReadAllText(file);
                    correctFile(text, Double.Parse(textBox1.Text));
                }
                catch (IOException)
                {
                    MessageBox.Show("Try other file");
                }
            }
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk_1(object sender, CancelEventArgs e)
        {

        }

        private string fileType(string fileName)
        {
            string res = "";
            for (int i = fileName.Length - 1; i >= 0; i--)
            {
                if (fileName[i] == '.')
                {
                    return fileName.Substring(i + 1);
                }
            }
            return res;
        }

        private string correctFile(string text, double delay)
        {
            string[] splitedText = text.Split(new string[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            string res = "";
            foreach (string st in splitedText)
            {
                res = res + correctedLine(st, delay);
                res = res + '\n';
            }
            return res;
        }

        private string correctedLine(string ln, double dl)
        {
            String[] splittedLine = line.split(" ");
            string res = "";
            for (int i = 0; i < splittedLine.length; i++) {
                if (isDate(splittedLine[i])) {
                    splittedLine[i] = fixedDate(splittedLine[i], dl);
                }
                res = res + splittedLine[i] + (i < splittedLine.length - 1 ? " " : ""));
            }
            return res;
        }

        private bool isDate(string dt)
        {
            string[] splitedDt = dt.split(":");
            return (splitedDt.length == 3);
        }

        private string fixedDate(string dateSt, double dl)
        {
            String[] splitedDt = dateSt.split(":");

            double date = 0;

            for (int i = 0; i < splitedDt.length; i++) {
                date = date * 60 + Double.Parse(splitedDt[i].Replace(",", "."));
            }

            date += dl;
            if (date < 0)
                date = 0;

            for (int i = 0; i < splitedDt.length; i++) {
                if (i == 0)
                    splitedDt[i] = "" + formated((date % 60));
                else
                    splitedDt[i] = "" + formated((int) (date % 60));

                date /= 60;
            }
            string res = "";
            for (int i = splitedDt.length - 1; i >= 0; i--) {
                res = res + splitedDt[i] + (i > 0 ? ":" : "");
            }

            return res;
        }

        private formated(double date) 
        {
            String res = "";

            int integerPart = (int) date;

            if (integerPart < 10)
                res = res + "0" + integerPart;
            else
                res = res + integerPart;

            if (integerPart != date) {
                date -= integerPart;
                String afterThePoint = date.ToString("0.000000");
                afterThePoint = "," + afterThePoint.Substring(2);
                if (afterThePoint.Length > 15)
                    afterThePoint = afterThePoint.substring(0, 4);
                ans = ans + afterThePoint;
            }

            return ans;
        }
    }
}
