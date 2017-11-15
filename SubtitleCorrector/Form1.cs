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
                String file = openFileDialog1.FileName;
                try
                {
                    if (fileType(file).ToLower() != "srt")
                    {
                        MessageBox.Show(fileType(file));
                        return;
                    }
                    String text = File.ReadAllText(file);
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

        private String fileType(String fileName)
        {
            String res = "";
            for (int i = fileName.Length - 1; i >= 0; i--)
            {
                if (fileName[i] == '.')
                {
                    return fileName.Substring(i + 1);
                }
            }
            return res;
        }

        private String correctFile(String text, double delay)
        {
            String[] splitedText = text.Split(new String[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            String res = "";
            foreach (String st in splitedText)
            {
                res = res + correctedLine(st, delay);
                res = res + '\n';
            }
            return res;
        }

        private String correctedLine(String ln, double dl)
        {
            String[] splittedLine = ln.Split(' ');
            String res = "";
            for (int i = 0; i < splittedLine.Length; i++)
            {
                if (IsDate(splittedLine[i]))
                {
                    splittedLine[i] = FixedDate(splittedLine[i], dl);
                }
                res = res + splittedLine[i] + (i < splittedLine.Length - 1 ? " " : "");
            }
            return res;
        }

        private bool IsDate(String dt)
        {
            String[] splitedDt = dt.Split(':');
            return (splitedDt.Length == 3);
        }

        private String FixedDate(String dateSt, double dl)
        {
            String[] splitedDt = dateSt.Split(':');

            double date = 0;

            for (int i = 0; i < splitedDt.Length; i++) {
                date = date * 60 + Double.Parse(splitedDt[i].Replace(",", "."));
            }

            date += dl;
            if (date < 0)
                date = 0;

            for (int i = 0; i < splitedDt.Length; i++) {
                if (i == 0)
                    splitedDt[i] = "" + Formated((date % 60));
                else
                    splitedDt[i] = "" + Formated((int)(date % 60));

                date /= 60;
            }
            String res = "";
            for (int i = splitedDt.Length - 1; i >= 0; i--) {
                res = res + splitedDt[i] + (i > 0 ? ":" : "");
            }

            return res;
        }

        private String Formated(double date)
        {
            String res = "";

            int integerPart = (int)date;

            if (integerPart < 10)
                res = res + "0" + integerPart;
            else
                res = res + integerPart;

            if (integerPart != date) {
                date -= integerPart;
                String afterThePoint = date.ToString("0.000000");
                afterThePoint = "," + afterThePoint.Substring(2);
                if (afterThePoint.Length > 15)
                    afterThePoint = afterThePoint.Substring(0, 4);
                res = res + afterThePoint;
            }

            return res;
        }
    }
}
