namespace SGECA.ControlesComunes
{
    partial class BusquedaConFiltro
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
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.grpFiltrar = new System.Windows.Forms.GroupBox();
            this.btnQuitarTodos = new System.Windows.Forms.Button();
            this.lstLista = new System.Windows.Forms.ListBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radAnd = new System.Windows.Forms.RadioButton();
            this.lblValor = new System.Windows.Forms.Label();
            this.lblCondicion = new System.Windows.Forms.Label();
            this.lblCampo = new System.Windows.Forms.Label();
            this.cboCondicion = new System.Windows.Forms.ComboBox();
            this.cboCampo = new System.Windows.Forms.ComboBox();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnQuitar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.grpBusquedaDescripcion = new System.Windows.Forms.GroupBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.grpGeneral.SuspendLayout();
            this.grpFiltrar.SuspendLayout();
            this.grpBusquedaDescripcion.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpGeneral
            // 
            this.grpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGeneral.Controls.Add(this.grpFiltrar);
            this.grpGeneral.Controls.Add(this.grpBusquedaDescripcion);
            this.grpGeneral.Location = new System.Drawing.Point(1, -2);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(760, 136);
            this.grpGeneral.TabIndex = 33;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "Búsqueda y/o Filtrado";
            // 
            // grpFiltrar
            // 
            this.grpFiltrar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.grpFiltrar.Controls.Add(this.btnQuitarTodos);
            this.grpFiltrar.Controls.Add(this.lstLista);
            this.grpFiltrar.Controls.Add(this.radioButton2);
            this.grpFiltrar.Controls.Add(this.radAnd);
            this.grpFiltrar.Controls.Add(this.lblValor);
            this.grpFiltrar.Controls.Add(this.lblCondicion);
            this.grpFiltrar.Controls.Add(this.lblCampo);
            this.grpFiltrar.Controls.Add(this.cboCondicion);
            this.grpFiltrar.Controls.Add(this.cboCampo);
            this.grpFiltrar.Controls.Add(this.txtValor);
            this.grpFiltrar.Controls.Add(this.btnAplicar);
            this.grpFiltrar.Controls.Add(this.btnQuitar);
            this.grpFiltrar.Controls.Add(this.btnAgregar);
            this.grpFiltrar.Location = new System.Drawing.Point(238, 13);
            this.grpFiltrar.Name = "grpFiltrar";
            this.grpFiltrar.Size = new System.Drawing.Size(516, 121);
            this.grpFiltrar.TabIndex = 30;
            this.grpFiltrar.TabStop = false;
            this.grpFiltrar.Text = "Filtrar";
            this.grpFiltrar.Enter += new System.EventHandler(this.grpFiltrar_Enter);
            // 
            // btnQuitarTodos
            // 
            this.btnQuitarTodos.Location = new System.Drawing.Point(151, 94);
            this.btnQuitarTodos.Name = "btnQuitarTodos";
            this.btnQuitarTodos.Size = new System.Drawing.Size(82, 23);
            this.btnQuitarTodos.TabIndex = 6;
            this.btnQuitarTodos.Text = "Quitar Todos";
            this.btnQuitarTodos.UseVisualStyleBackColor = true;
            this.btnQuitarTodos.Click += new System.EventHandler(this.btnQuitarTodos_Click);
            // 
            // lstLista
            // 
            this.lstLista.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstLista.FormattingEnabled = true;
            this.lstLista.Location = new System.Drawing.Point(311, 13);
            this.lstLista.Name = "lstLista";
            this.lstLista.Size = new System.Drawing.Size(199, 82);
            this.lstLista.TabIndex = 8;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(415, 99);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(99, 17);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Cualquier cond.";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Click += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radAnd
            // 
            this.radAnd.AutoSize = true;
            this.radAnd.Checked = true;
            this.radAnd.Location = new System.Drawing.Point(309, 99);
            this.radAnd.Name = "radAnd";
            this.radAnd.Size = new System.Drawing.Size(109, 17);
            this.radAnd.TabIndex = 9;
            this.radAnd.TabStop = true;
            this.radAnd.Text = "Todas las condic.";
            this.radAnd.UseVisualStyleBackColor = true;
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Location = new System.Drawing.Point(6, 71);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(34, 13);
            this.lblValor.TabIndex = 43;
            this.lblValor.Text = "Valor:";
            // 
            // lblCondicion
            // 
            this.lblCondicion.AutoSize = true;
            this.lblCondicion.Location = new System.Drawing.Point(6, 43);
            this.lblCondicion.Name = "lblCondicion";
            this.lblCondicion.Size = new System.Drawing.Size(57, 13);
            this.lblCondicion.TabIndex = 42;
            this.lblCondicion.Text = "Condición:";
            // 
            // lblCampo
            // 
            this.lblCampo.AutoSize = true;
            this.lblCampo.Location = new System.Drawing.Point(6, 16);
            this.lblCampo.Name = "lblCampo";
            this.lblCampo.Size = new System.Drawing.Size(43, 13);
            this.lblCampo.TabIndex = 41;
            this.lblCampo.Text = "Campo:";
            // 
            // cboCondicion
            // 
            this.cboCondicion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCondicion.FormattingEnabled = true;
            this.cboCondicion.Location = new System.Drawing.Point(67, 40);
            this.cboCondicion.Name = "cboCondicion";
            this.cboCondicion.Size = new System.Drawing.Size(237, 21);
            this.cboCondicion.TabIndex = 2;
            // 
            // cboCampo
            // 
            this.cboCampo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCampo.FormattingEnabled = true;
            this.cboCampo.Location = new System.Drawing.Point(67, 13);
            this.cboCampo.Name = "cboCampo";
            this.cboCampo.Size = new System.Drawing.Size(237, 21);
            this.cboCampo.TabIndex = 1;
            this.cboCampo.SelectedIndexChanged += new System.EventHandler(this.cboCampo_SelectedIndexChanged);
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(67, 68);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(237, 20);
            this.txtValor.TabIndex = 3;
            // 
            // btnAplicar
            // 
            this.btnAplicar.Location = new System.Drawing.Point(239, 94);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(66, 23);
            this.btnAplicar.TabIndex = 7;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // btnQuitar
            // 
            this.btnQuitar.Location = new System.Drawing.Point(80, 94);
            this.btnQuitar.Name = "btnQuitar";
            this.btnQuitar.Size = new System.Drawing.Size(65, 23);
            this.btnQuitar.TabIndex = 5;
            this.btnQuitar.Text = "Quitar";
            this.btnQuitar.UseVisualStyleBackColor = true;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new System.Drawing.Point(6, 94);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(65, 23);
            this.btnAgregar.TabIndex = 4;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // grpBusquedaDescripcion
            // 
            this.grpBusquedaDescripcion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBusquedaDescripcion.Controls.Add(this.lblDescripcion);
            this.grpBusquedaDescripcion.Controls.Add(this.txtDescripcion);
            this.grpBusquedaDescripcion.Controls.Add(this.btnLimpiar);
            this.grpBusquedaDescripcion.Controls.Add(this.btnBuscar);
            this.grpBusquedaDescripcion.Location = new System.Drawing.Point(3, 13);
            this.grpBusquedaDescripcion.Name = "grpBusquedaDescripcion";
            this.grpBusquedaDescripcion.Size = new System.Drawing.Size(229, 120);
            this.grpBusquedaDescripcion.TabIndex = 32;
            this.grpBusquedaDescripcion.TabStop = false;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(3, 13);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(120, 13);
            this.lblDescripcion.TabIndex = 29;
            this.lblDescripcion.Text = "Buscar por Descripción:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescripcion.Location = new System.Drawing.Point(6, 37);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(217, 20);
            this.txtDescripcion.TabIndex = 1;
            this.txtDescripcion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDescripcion_KeyPress);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpiar.Location = new System.Drawing.Point(158, 93);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(65, 23);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBuscar.Location = new System.Drawing.Point(6, 93);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(65, 23);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // BusquedaConFiltro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpGeneral);
            this.MinimumSize = new System.Drawing.Size(702, 0);
            this.Name = "BusquedaConFiltro";
            this.Size = new System.Drawing.Size(761, 134);
            this.Load += new System.EventHandler(this.BusquedaConFiltro_Load);
            this.grpGeneral.ResumeLayout(false);
            this.grpFiltrar.ResumeLayout(false);
            this.grpFiltrar.PerformLayout();
            this.grpBusquedaDescripcion.ResumeLayout(false);
            this.grpBusquedaDescripcion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBusquedaDescripcion;
        private System.Windows.Forms.Label lblDescripcion;
        public System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Button btnLimpiar;
        public System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.GroupBox grpFiltrar;
        private System.Windows.Forms.Label lblValor;
        private System.Windows.Forms.Label lblCondicion;
        private System.Windows.Forms.Label lblCampo;
        private System.Windows.Forms.ComboBox cboCondicion;
        private System.Windows.Forms.ComboBox cboCampo;
        public System.Windows.Forms.TextBox txtValor;
        public System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.RadioButton radAnd;
        private System.Windows.Forms.RadioButton radioButton2;
        public System.Windows.Forms.ListBox lstLista;
        private System.Windows.Forms.Button btnQuitarTodos;
        private System.Windows.Forms.GroupBox grpGeneral;
    }
}
