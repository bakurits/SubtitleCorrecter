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
            return text;
        }
    }
}
