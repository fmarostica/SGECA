namespace  SGECA.ColaDeImpresion
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.btnPrint = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.tmrTiempoLectura = new System.Windows.Forms.Timer(this.components);
            this.btnIniciarDetener = new System.Windows.Forms.Button();
            this.nicTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clbTrabajosPendientes = new System.Windows.Forms.CheckedListBox();
            this.controlTransacciones1 = new SGECA.ControlesComunes.ControlTransacciones();
            this.richTextBoxPrintCtrl1 = new SGECA.ColaDeImpresion.RichTextBoxPrintCtrl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(357, 380);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(109, 35);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "Im&primir";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.printDocument1.EndPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_EndPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintPreview.Location = new System.Drawing.Point(242, 380);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(109, 35);
            this.btnPrintPreview.TabIndex = 3;
            this.btnPrintPreview.Text = "&Vista Previa Impresión";
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.Document = this.printDocument1;
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // tmrTiempoLectura
            // 
            this.tmrTiempoLectura.Tick += new System.EventHandler(this.tmrTiempoLectura_Tick);
            // 
            // btnIniciarDetener
            // 
            this.btnIniciarDetener.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIniciarDetener.Location = new System.Drawing.Point(12, 380);
            this.btnIniciarDetener.Name = "btnIniciarDetener";
            this.btnIniciarDetener.Size = new System.Drawing.Size(109, 35);
            this.btnIniciarDetener.TabIndex = 1;
            this.btnIniciarDetener.Text = "Iniciar/Detener";
            this.btnIniciarDetener.UseVisualStyleBackColor = true;
            this.btnIniciarDetener.Click += new System.EventHandler(this.btnIniciarDetener_Click);
            // 
            // nicTray
            // 
            this.nicTray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.nicTray.BalloonTipText = "Puede encontrar SGECA Demonio de Impresión en el área de notificación.";
            this.nicTray.BalloonTipTitle = "SGECA";
            this.nicTray.Text = "SGECA Demonio de Impresión";
            this.nicTray.Visible = true;
            this.nicTray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.nicTray_MouseDoubleClick);
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefrescar.Location = new System.Drawing.Point(127, 380);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(109, 35);
            this.btnRefrescar.TabIndex = 2;
            this.btnRefrescar.Text = "&Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.Location = new System.Drawing.Point(472, 380);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(109, 35);
            this.btnEliminar.TabIndex = 5;
            this.btnEliminar.Text = "&Quitar de Cola";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.clbTrabajosPendientes);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(569, 213);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trabajos Pendientes";
            // 
            // clbTrabajosPendientes
            // 
            this.clbTrabajosPendientes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbTrabajosPendientes.CheckOnClick = true;
            this.clbTrabajosPendientes.FormattingEnabled = true;
            this.clbTrabajosPendientes.Location = new System.Drawing.Point(6, 19);
            this.clbTrabajosPendientes.Name = "clbTrabajosPendientes";
            this.clbTrabajosPendientes.Size = new System.Drawing.Size(557, 184);
            this.clbTrabajosPendientes.TabIndex = 1;
            this.clbTrabajosPendientes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbTrabajosPendientes_ItemCheck);
            // 
            // controlTransacciones1
            // 
            this.controlTransacciones1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlTransacciones1.Location = new System.Drawing.Point(12, 231);
            this.controlTransacciones1.Name = "controlTransacciones1";
            this.controlTransacciones1.Size = new System.Drawing.Size(569, 143);
            this.controlTransacciones1.TabIndex = 8;
            // 
            // richTextBoxPrintCtrl1
            // 
            this.richTextBoxPrintCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxPrintCtrl1.Location = new System.Drawing.Point(10, 362);
            this.richTextBoxPrintCtrl1.Name = "richTextBoxPrintCtrl1";
            this.richTextBoxPrintCtrl1.Size = new System.Drawing.Size(73, 40);
            this.richTextBoxPrintCtrl1.TabIndex = 4;
            this.richTextBoxPrintCtrl1.Text = "";
            this.richTextBoxPrintCtrl1.Visible = false;
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 427);
            this.Controls.Add(this.controlTransacciones1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnRefrescar);
            this.Controls.Add(this.btnIniciarDetener);
            this.Controls.Add(this.richTextBoxPrintCtrl1);
            this.Controls.Add(this.btnPrintPreview);
            this.Controls.Add(this.btnPrint);
            this.MinimumSize = new System.Drawing.Size(609, 466);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SGECA - Cola de Impresión";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.Resize += new System.EventHandler(this.FrmPrincipal_Resize);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrint;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private RichTextBoxPrintCtrl richTextBoxPrintCtrl1;
        private System.Windows.Forms.Timer tmrTiempoLectura;
        private System.Windows.Forms.Button btnIniciarDetener;
        private System.Windows.Forms.NotifyIcon nicTray;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox clbTrabajosPendientes;
        private ControlesComunes.ControlTransacciones controlTransacciones1;
    }
}

