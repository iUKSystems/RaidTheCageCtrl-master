using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RaidTheCageCtrl.GameParts;

namespace RaidTheCageCtrl
{
    public partial class TotalPrices : Form
    {
        private ucRaidTheCage mParent;

        public TotalPrices(ucRaidTheCage pParent)
        {
            InitializeComponent();
            mParent = pParent;
        }

        private void TotalPrices_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            mParent.totalpricesshowing = false;
        }
    }
}
