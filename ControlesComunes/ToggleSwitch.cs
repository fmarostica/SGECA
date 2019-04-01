using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class ToggleSwitch : UserControl
    {
        public bool Checked
        {
            get { return toolStripButton1.Checked; }
            set { toolStripButton1.Checked = value; }
        }

        public ToggleSwitch()
        {
            InitializeComponent();
            verificarImagen();
        }

        private void toolStripButton1_CheckedChanged(object sender, EventArgs e)
        {

            verificarImagen();
        }

        private void verificarImagen()
        {
            if (toolStripButton1.Checked)
                toolStripButton1.Image = SGECA.ControlesComunes.Recursos.Switch_SI;
            else
                toolStripButton1.Image = SGECA.ControlesComunes.Recursos.Switch_NO;
        }
    }
}
