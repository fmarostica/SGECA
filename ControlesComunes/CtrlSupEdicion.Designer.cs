namespace SGECA.ControlesComunes
{
    partial class CtrlSupEdicion
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlSupEdicion));
            this.tsHerramientas = new System.Windows.Forms.ToolStrip();
            this.tsbEditar = new System.Windows.Forms.ToolStripButton();
            this.tsbAgregar = new System.Windows.Forms.ToolStripButton();
            this.tsbActualizar = new System.Windows.Forms.ToolStripButton();
            this.tsbEliminar = new System.Windows.Forms.ToolStripButton();
            this.tsbDeshabilitar = new System.Windows.Forms.ToolStripButton();
            this.tsbHabilitar = new System.Windows.Forms.ToolStripButton();
            this.tsbDeshacer = new System.Windows.Forms.ToolStripButton();
            this.tsbCancelar = new System.Windows.Forms.ToolStripButton();
            this.tsbCerrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExportar = new System.Windows.Forms.ToolStripButton();
            this.tsHerramientas.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsHerramientas
            // 
            this.tsHerramientas.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsHerramientas.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tsHerramientas.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbEditar,
            this.tsbAgregar,
            this.tsbActualizar,
            this.tsbEliminar,
            this.tsbDeshabilitar,
            this.tsbHabilitar,
            this.tsbDeshacer,
            this.tsbCancelar,
            this.tsbCerrar,
            this.toolStripSeparator2,
            this.tsbExportar});
            this.tsHerramientas.Location = new System.Drawing.Point(0, 0);
            this.tsHerramientas.Name = "tsHerramientas";
            this.tsHerramientas.Size = new System.Drawing.Size(579, 39);
            this.tsHerramientas.TabIndex = 0;
            this.tsHerramientas.Text = "toolStrip1";
            // 
            // tsbEditar
            // 
            this.tsbEditar.Image = ((System.Drawing.Image)(resources.GetObject("tsbEditar.Image")));
            this.tsbEditar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditar.Name = "tsbEditar";
            this.tsbEditar.Size = new System.Drawing.Size(55, 36);
            this.tsbEditar.Text = "F4";
            this.tsbEditar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbEditar.ToolTipText = "Editar ";
            this.tsbEditar.Click += new System.EventHandler(this.tsbEditar_Click);
            // 
            // tsbAgregar
            // 
            this.tsbAgregar.Image = ((System.Drawing.Image)(resources.GetObject("tsbAgregar.Image")));
            this.tsbAgregar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAgregar.Name = "tsbAgregar";
            this.tsbAgregar.Size = new System.Drawing.Size(55, 36);
            this.tsbAgregar.Text = "F5";
            this.tsbAgregar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbAgregar.ToolTipText = "Agregar";
            this.tsbAgregar.Click += new System.EventHandler(this.tsbAgregar_Click);
            // 
            // tsbActualizar
            // 
            this.tsbActualizar.Image = global::SGECA.ControlesComunes.Recursos.btnActualizar;
            this.tsbActualizar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbActualizar.Name = "tsbActualizar";
            this.tsbActualizar.Size = new System.Drawing.Size(55, 36);
            this.tsbActualizar.Text = "F6";
            this.tsbActualizar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbActualizar.ToolTipText = "Actualizar/Guardar";
            this.tsbActualizar.Click += new System.EventHandler(this.tsbActualizar_Click);
            // 
            // tsbEliminar
            // 
            this.tsbEliminar.Image = global::SGECA.ControlesComunes.Recursos.papelera;
            this.tsbEliminar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEliminar.Name = "tsbEliminar";
            this.tsbEliminar.Size = new System.Drawing.Size(55, 36);
            this.tsbEliminar.Text = "F7";
            this.tsbEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbEliminar.ToolTipText = "Eliminar";
            this.tsbEliminar.Click += new System.EventHandler(this.tsbEliminar_Click);
            // 
            // tsbDeshabilitar
            // 
            this.tsbDeshabilitar.Image = global::SGECA.ControlesComunes.Recursos.deshabilitar;
            this.tsbDeshabilitar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeshabilitar.Name = "tsbDeshabilitar";
            this.tsbDeshabilitar.Size = new System.Drawing.Size(55, 36);
            this.tsbDeshabilitar.Text = "F8";
            this.tsbDeshabilitar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbDeshabilitar.ToolTipText = "Deshabilitar";
            this.tsbDeshabilitar.Click += new System.EventHandler(this.tsbDeshabilitar_Click);
            // 
            // tsbHabilitar
            // 
            this.tsbHabilitar.Image = global::SGECA.ControlesComunes.Recursos.habilitar;
            this.tsbHabilitar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHabilitar.Name = "tsbHabilitar";
            this.tsbHabilitar.Size = new System.Drawing.Size(55, 36);
            this.tsbHabilitar.Text = "F9";
            this.tsbHabilitar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbHabilitar.ToolTipText = "Habilitar";
            this.tsbHabilitar.Click += new System.EventHandler(this.tsbHabilitar_Click);
            // 
            // tsbDeshacer
            // 
            this.tsbDeshacer.Image = global::SGECA.ControlesComunes.Recursos.btnCancelar;
            this.tsbDeshacer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeshacer.Name = "tsbDeshacer";
            this.tsbDeshacer.Size = new System.Drawing.Size(61, 36);
            this.tsbDeshacer.Text = "F10";
            this.tsbDeshacer.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbDeshacer.ToolTipText = "Deshacer";
            this.tsbDeshacer.Click += new System.EventHandler(this.tsbDeshacer_Click);
            // 
            // tsbCancelar
            // 
            this.tsbCancelar.Image = global::SGECA.ControlesComunes.Recursos.btnEliminar;
            this.tsbCancelar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancelar.Name = "tsbCancelar";
            this.tsbCancelar.Size = new System.Drawing.Size(61, 36);
            this.tsbCancelar.Text = "F11";
            this.tsbCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbCancelar.ToolTipText = "Cancelar";
            this.tsbCancelar.Click += new System.EventHandler(this.tsbCancelar_Click);
            // 
            // tsbCerrar
            // 
            this.tsbCerrar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbCerrar.Image = global::SGECA.ControlesComunes.Recursos.btnCerrar;
            this.tsbCerrar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCerrar.Name = "tsbCerrar";
            this.tsbCerrar.Size = new System.Drawing.Size(61, 36);
            this.tsbCerrar.Text = "F12";
            this.tsbCerrar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbCerrar.ToolTipText = "Cerrar";
            this.tsbCerrar.Click += new System.EventHandler(this.tsbCerrar_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // tsbExportar
            // 
            this.tsbExportar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExportar.Enabled = false;
            this.tsbExportar.Image = global::SGECA.ControlesComunes.Recursos.export_to_excel;
            this.tsbExportar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportar.Name = "tsbExportar";
            this.tsbExportar.Size = new System.Drawing.Size(36, 36);
            this.tsbExportar.Text = "Exportar";
            this.tsbExportar.Click += new System.EventHandler(this.tsbExportar_Click);
            // 
            // CtrlSupEdicion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tsHerramientas);
            this.Name = "CtrlSupEdicion";
            this.Size = new System.Drawing.Size(579, 42);
            this.tsHerramientas.ResumeLayout(false);
            this.tsHerramientas.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsHerramientas;
        private System.Windows.Forms.ToolStripButton tsbEditar;
        private System.Windows.Forms.ToolStripButton tsbAgregar;
        private System.Windows.Forms.ToolStripButton tsbActualizar;
        private System.Windows.Forms.ToolStripButton tsbEliminar;
        private System.Windows.Forms.ToolStripButton tsbCancelar;
        private System.Windows.Forms.ToolStripButton tsbCerrar;
        private System.Windows.Forms.ToolStripButton tsbDeshacer;
        private System.Windows.Forms.ToolStripButton tsbDeshabilitar;
        private System.Windows.Forms.ToolStripButton tsbHabilitar;
        private System.Windows.Forms.ToolStripButton tsbExportar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
