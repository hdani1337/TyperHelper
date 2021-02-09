using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TyperHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void chooseFile_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chooseFile.ShowDialog();

            while (!chooseFile.FileName.EndsWith(".txt")) {
                MessageBox.Show("Kérlek .txt kiterjesztésű fájlt válassz ki!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                chooseFile.ShowDialog();
            }

            szovegInput.ReadOnly = true;
            szovegInput.Text = "Kiválaszott fájl:\n" + chooseFile.FileName;
            label1.Visible = false;
          
        }

        private void progresses_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void progresses_Click(object sender, EventArgs e)
        {          
            List<Process> processes = System.Diagnostics.Process.GetProcesses().ToList();
            List<string> processNames = new List<string>();

            foreach (Process p in processes)
                if(!string.IsNullOrEmpty(p.MainWindowTitle))
                    processNames.Add(p.ProcessName);              

            processNames.Sort();

            progresses.DataSource = processNames;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            string text = "";         

            if (chooseFile.FileName == "openFileDialog1") {
                //Nincs fájl kiválasztva
            }
        }
    }
}
