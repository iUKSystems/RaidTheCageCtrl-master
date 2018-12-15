using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DejaVu;
using WiseGuys.WGNetWork2011;

namespace RaidTheCageCtrl
{
    public partial class ShowArticleList : Form
    {
        private DataGridViewRowCollection rowcol;
        private MainForm mParent;

        private List<Button> lButtons = new List<Button>(); 

        //private Dictionary<string,string> lArticles = new Dictionary<string, string>();
        private List<articles> larticles = new List<articles>(); 

        //private List<string> lList;

        private WGGFXEngine overlay = new WGGFXEngine("OVERLAY");
        private WGGFXEngine projector = new WGGFXEngine("PROJECTOR");

        //////////////////////////////////////////////////////////////
        // for undo... ///////////////////////////////////////////////
        //////////////////////////////////////////////////////////////       
        private UndoRedo<int> mcurrentarticlepos = new UndoRedo<int>(0);

        private UndoRedo<bool> marticle1used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle2used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle3used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle4used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle5used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle6used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle7used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle8used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle9used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle10used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle11used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle12used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle13used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle14used = new UndoRedo<bool>(false);
        private UndoRedo<bool> marticle15used = new UndoRedo<bool>(false);
        private List<UndoRedo<bool>> larticlesused = new List<UndoRedo<bool>>();

        public ShowArticleList(MainForm pParent, DataGridViewRowCollection inrowcol)
        {
            InitializeComponent();
            rowcol = inrowcol;
            mParent = pParent;

            overlay = mParent.mGFXEngines[(int)eEngines.OVERLAY];
            projector = mParent.mGFXEngines[(int)eEngines.PROJECTOR];

            lButtons.Add(button1);
            lButtons.Add(button2);
            lButtons.Add(button3);
            lButtons.Add(button4);
            lButtons.Add(button5);
            lButtons.Add(button6);
            lButtons.Add(button7);
            lButtons.Add(button8);
            lButtons.Add(button9);
            lButtons.Add(button10);
            lButtons.Add(button11);
            lButtons.Add(button12);
            lButtons.Add(button13);
            lButtons.Add(button14);
            lButtons.Add(button15);

            larticlesused.Add(marticle1used);
            larticlesused.Add(marticle2used);
            larticlesused.Add(marticle3used);
            larticlesused.Add(marticle4used);
            larticlesused.Add(marticle5used);
            larticlesused.Add(marticle6used);
            larticlesused.Add(marticle7used);
            larticlesused.Add(marticle8used);
            larticlesused.Add(marticle9used);
            larticlesused.Add(marticle10used);
            larticlesused.Add(marticle11used);
            larticlesused.Add(marticle12used);
            larticlesused.Add(marticle13used);
            larticlesused.Add(marticle14used);
            larticlesused.Add(marticle15used);

            int cnt = 0;

            foreach (DataGridViewRow row in inrowcol)
            {
                long templong;
                long.TryParse(row.Cells[1].Value.ToString(), out templong);

                larticles.Add(new articles(row.Cells[0].Value.ToString(), templong));
                //lArticles.Add(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
                //lButtons[cnt].Text = row.Cells[0].Value.ToString() +" - " + row.Cells[1].Value.ToString();
                cnt++;
            }

            for (int i = cnt; i < 15; i++)
                lButtons[i].Visible = false;

            // new sort the dictionary alphabetic..
            //lList = lArticles.Keys.ToList();
            //larticles.Sort();
            //var lsortedarticles = larticles.OrderBy(price => price).ToList();   
            larticles.Sort((x, y) => string.Compare(x.name, y.name));

            cnt = 0;
            foreach (articles art in larticles)
            {

                lButtons[cnt].Text = art.name + " - " + art.price;
                cnt++;
            }
        }

        private void ArticleClicked(int inArticle)
        {
            if (larticlesused[inArticle].Value)
                return;     // already used...

            // ok.. we need to get the text and put it 
            string name = larticles[inArticle].name;
            
            using (UndoRedoManager.Start("questionReceived"))
            {
                overlay.AddUndo();
                projector.AddUndo();

                // send it to the right control and show the item...
                //projector.SetText(string.Format("pricelist.tag{0}.txt", 14 - mcurrentarticlepos.Value), name);

                if (name.Length >= mParent.mArticlelengthbeforebreaking)
                {
                    // so its bigger.. now check if it has a space..
                    int spacepos = name.IndexOf(" ");
                    if (spacepos != -1)
                    {
                        // we have a space... change this space too breaking character
                        StringBuilder tosend = new StringBuilder(name);
                        tosend[spacepos] = '¬';
                        overlay.SetText(string.Format("pricelist.tag{0}.txt", mcurrentarticlepos.Value + 1),
                            tosend.ToString());
                        projector.SetText(string.Format("pricelist.tag{0}.txt", mcurrentarticlepos.Value + 1),
                            tosend.ToString());
                    }
                    else
                        overlay.SetText(string.Format("pricelist.tag{0}.txt", mcurrentarticlepos.Value + 1),
                            name);
                    projector.SetText(string.Format("pricelist.tag{0}.txt", mcurrentarticlepos.Value + 1),
                        name);
                }
                else
                {
                    overlay.SetText(string.Format("pricelist.tag{0}.txt", mcurrentarticlepos.Value + 1), name);
                    projector.SetText(string.Format("pricelist.tag{0}.txt", mcurrentarticlepos.Value + 1), name);
                }


                mParent.gcRaidTheCage.ShowArticle(mcurrentarticlepos.Value);


                larticlesused[inArticle].Value = true;
                mcurrentarticlepos.Value++; // use in undo later...
                UndoRedoManager.Commit();
            }

            UpdateDlg();

        }

        private void UpdateDlg()
        {
            bool onedisabled = false;
            for (int i=0;i<15;i++)
            {
                lButtons[i].Enabled = !larticlesused[i].Value;
                lButtons[i].BackColor = larticlesused[i].Value ? Color.Red : SystemColors.Control;

                if (!larticlesused[i].Value)
                    lButtons[i].UseVisualStyleBackColor = true;
                else
                {
                    onedisabled = true;
                }

            }

            btshowarticlelistundo.Enabled = onedisabled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ArticleClicked(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ArticleClicked(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ArticleClicked(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ArticleClicked(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ArticleClicked(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ArticleClicked(5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ArticleClicked(6);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ArticleClicked(7);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ArticleClicked(8);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ArticleClicked(9);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ArticleClicked(10);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ArticleClicked(11);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ArticleClicked(12);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ArticleClicked(13);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ArticleClicked(14);
        }

        private void btshowarticlelistundo_Click(object sender, EventArgs e)
        {
            if (UndoRedoManager.CanUndo)
            {             
                UndoRedoManager.Undo();             
                overlay.Undo();
                projector.Undo();

                UpdateDlg();
            }
        }
    }

    public class articles
    {
        public string name;
        public long price;

        public articles(string inName, long inPrice)
        {
            name = inName;
            price = inPrice;
        }
    }
}
