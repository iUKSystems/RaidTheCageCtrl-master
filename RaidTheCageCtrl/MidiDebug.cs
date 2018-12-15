using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaidTheCageCtrl
{
    public partial class MidiDebug : Form
    {
        public MidiDebug()
        {
            InitializeComponent();

            CreateAndClearList();
        }

        private void CreateAndClearList()
        {
            listView1.Clear();
            listView1.Columns.Add("Logging", "Logging", listView1.Width);
        }

        internal void CheckMidiNote(string p, int p_2)
        {
            string toadd = p;
            if (p_2 == -1)
                AddToList(toadd + " MidiNote: " + p_2 + " NOTUSED!");
            else
            {
                AddToList(toadd + " MidiNote: " + p_2);
            }
        }
       

        private void AddToList(string p)
        {
            DateTime test = DateTime.Now;
            string timestring = string.Format("{0:00}:{1:00}:{2:00} - ", test.Hour, test.Minute, test.Second, test.Millisecond);
            listView1.Items.Add(timestring + p);
            listView1.EnsureVisible(listView1.Items.Count - 1);
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            CreateAndClearList();
        }

        private void MidiDebug_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
