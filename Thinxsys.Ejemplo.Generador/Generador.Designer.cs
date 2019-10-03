namespace Thinxsys.Ejemplo.Generador
{
    partial class F_Generador
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.B_Crear_RN = new System.Windows.Forms.Button();
            this.B_Crear_Entidad = new System.Windows.Forms.Button();
            this.T_Ruta_DB = new System.Windows.Forms.TextBox();
            this.G_DB = new System.Windows.Forms.DataGridView();
            this.B_Crear_DB = new System.Windows.Forms.Button();
            this.B_Cargar_DB = new System.Windows.Forms.Button();
            this.folderBrowserDialog_DB = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.G_DB)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(7, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(809, 334);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.B_Crear_RN);
            this.tabPage1.Controls.Add(this.B_Crear_Entidad);
            this.tabPage1.Controls.Add(this.T_Ruta_DB);
            this.tabPage1.Controls.Add(this.G_DB);
            this.tabPage1.Controls.Add(this.B_Crear_DB);
            this.tabPage1.Controls.Add(this.B_Cargar_DB);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(801, 308);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Clases DB";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // B_Crear_RN
            // 
            this.B_Crear_RN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_Crear_RN.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.B_Crear_RN.Location = new System.Drawing.Point(178, 35);
            this.B_Crear_RN.Name = "B_Crear_RN";
            this.B_Crear_RN.Size = new System.Drawing.Size(80, 23);
            this.B_Crear_RN.TabIndex = 7;
            this.B_Crear_RN.Text = "Crear RN";
            this.B_Crear_RN.UseVisualStyleBackColor = true;
            this.B_Crear_RN.Click += new System.EventHandler(this.B_Crear_RN_Click);
            // 
            // B_Crear_Entidad
            // 
            this.B_Crear_Entidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_Crear_Entidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.B_Crear_Entidad.Location = new System.Drawing.Point(87, 35);
            this.B_Crear_Entidad.Name = "B_Crear_Entidad";
            this.B_Crear_Entidad.Size = new System.Drawing.Size(85, 23);
            this.B_Crear_Entidad.TabIndex = 6;
            this.B_Crear_Entidad.Text = "Crear Entidad";
            this.B_Crear_Entidad.UseVisualStyleBackColor = true;
            this.B_Crear_Entidad.Click += new System.EventHandler(this.B_Crear_Entity_Click);
            // 
            // T_Ruta_DB
            // 
            this.T_Ruta_DB.Enabled = false;
            this.T_Ruta_DB.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.T_Ruta_DB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.T_Ruta_DB.Location = new System.Drawing.Point(87, 8);
            this.T_Ruta_DB.Name = "T_Ruta_DB";
            this.T_Ruta_DB.Size = new System.Drawing.Size(707, 20);
            this.T_Ruta_DB.TabIndex = 5;
            this.T_Ruta_DB.Text = "RUTA:";
            // 
            // G_DB
            // 
            this.G_DB.AllowUserToAddRows = false;
            this.G_DB.AllowUserToDeleteRows = false;
            this.G_DB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.G_DB.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.G_DB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.G_DB.Location = new System.Drawing.Point(6, 63);
            this.G_DB.Name = "G_DB";
            this.G_DB.Size = new System.Drawing.Size(788, 238);
            this.G_DB.TabIndex = 4;
            // 
            // B_Crear_DB
            // 
            this.B_Crear_DB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_Crear_DB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.B_Crear_DB.Location = new System.Drawing.Point(6, 35);
            this.B_Crear_DB.Name = "B_Crear_DB";
            this.B_Crear_DB.Size = new System.Drawing.Size(75, 23);
            this.B_Crear_DB.TabIndex = 2;
            this.B_Crear_DB.Text = "Crear BD";
            this.B_Crear_DB.UseVisualStyleBackColor = true;
            this.B_Crear_DB.Click += new System.EventHandler(this.B_Crear_DAL_Click);
            // 
            // B_Cargar_DB
            // 
            this.B_Cargar_DB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_Cargar_DB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.B_Cargar_DB.Location = new System.Drawing.Point(6, 6);
            this.B_Cargar_DB.Name = "B_Cargar_DB";
            this.B_Cargar_DB.Size = new System.Drawing.Size(75, 23);
            this.B_Cargar_DB.TabIndex = 0;
            this.B_Cargar_DB.Text = "Cargar";
            this.B_Cargar_DB.UseVisualStyleBackColor = true;
            this.B_Cargar_DB.Click += new System.EventHandler(this.B_Cargar_DAL_Click);
            // 
            // F_Generador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(823, 349);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "F_Generador";
            this.Text = "Generador de Clases";
            this.Load += new System.EventHandler(this.F_Generador_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.G_DB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button B_Crear_DB;
        private System.Windows.Forms.Button B_Cargar_DB;
        private System.Windows.Forms.DataGridView G_DB;
        private System.Windows.Forms.TextBox T_Ruta_DB;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_DB;
        private System.Windows.Forms.Button B_Crear_RN;
        private System.Windows.Forms.Button B_Crear_Entidad;
    }
}

