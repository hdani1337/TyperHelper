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

            if (chooseFile.FileName.EndsWith(".txt"))
            {
                //Sikeres fájl kiválasztva   
            }
            else {
                MessageBox.Show("Kérlek .txt kiterjesztésű fájlt válassz ki!","Hiba!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
          
        }

        private void progresses_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void progresses_Click(object sender, EventArgs e)
        {
            //progresses.DataSource = 
            List<Process> processes = System.Diagnostics.Process.GetProcesses().ToList();
            List<string> processNames = new List<string>();

            foreach (Process p in processes)
                if(!string.IsNullOrEmpty(p.MainWindowTitle))
                    processNames.Add(p.ProcessName);              

            processNames.Sort();

            progresses.DataSource = processNames;
        }
    }
}
