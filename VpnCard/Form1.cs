using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VpnCard
{
    public partial class Form1 : Form
    {

        private readonly Color COLOR_BNT_PRESSED = Color.FromKnownColor(KnownColor.ActiveCaption);
        private readonly Color COLOR_BNT_NOT_PRESSED = Color.FromKnownColor(KnownColor.Control);

        private ButtonCoord Coord1 = null;
        private ButtonCoord Coord2 = null;

        private bool EsElPrimero = true; 
        public Form1()
        {
            InitializeComponent();
            CreateButtons();
            lblCoords.Text = "";
        }

        private void CreateButtons()
        {
            for (int col = 0; col < 7; col++)
            {
                for (int row = 0; row < 5; row++)
                {
                    var btn = new ButtonCoord();
                    btn.Col = col;
                    btn.Row = row;
                    btn.Dock = DockStyle.Fill;
                    btn.Click += btnClick;
                    tableLayoutPanel1.Controls.Add(btn, col, row);
                }
            }
        }



        private void btnClick(object sender, EventArgs e)
        {
            ButtonCoord btn = (ButtonCoord)sender;
            if (btn.BackColor == COLOR_BNT_NOT_PRESSED)
            {
                btn.BackColor = COLOR_BNT_PRESSED;
                if (EsElPrimero)
                {
                    if (Coord1 != null) Coord1.BackColor = COLOR_BNT_NOT_PRESSED;
                    Coord1 = btn;
                }
                else
                {
                    if (Coord2 != null) Coord2.BackColor = COLOR_BNT_NOT_PRESSED;
                    Coord2 = btn;
                }
                EsElPrimero = !EsElPrimero;

            }
            else
            {
                btn.BackColor = COLOR_BNT_NOT_PRESSED;
                if (EsElPrimero) Coord2 = null;
                else Coord1 = null;
                EsElPrimero = !EsElPrimero;
            }
            lblCoords.Text = $"1-{Coord1?.Text} 2-{Coord2?.Text}";
            if (Coord1 != null && Coord2 != null)
            {
                txtCoordKey.Text = $"{Coord1.Key}{Coord2.Key}";
                Clipboard.SetText(txtCoordKey.Text);
            }
            else
            {
                txtCoordKey.Text = "";
            }
        }
        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblCoords.Text);
        }
    }
}
