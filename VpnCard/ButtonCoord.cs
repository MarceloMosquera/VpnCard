using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VpnCard
{
    public class ButtonCoord : Button
    {
        public int Col { get; set; }
        public int Row { get; set; }
        public string Key { get; set; }


        public override string Text
        {
            get
            {
                var colLetter = getLetter(Col);
                if (Key == null)
                {
                    StringCollection keys = (StringCollection)Properties.Settings.Default[colLetter];
                    Key = keys[Row];
                }
                return $"{colLetter}{Row + 1}";
            }

            set
            {
                base.Text = value;
            }
        }

        private string getLetter(int value)
        {
            return "ABCDEFG"[value].ToString();
        }
    }
}
