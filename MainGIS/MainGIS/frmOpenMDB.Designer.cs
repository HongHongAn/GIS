namespace MainGIS
{
    partial class frmOpenMDB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FeatureClassBox = new System.Windows.Forms.ComboBox();
            this.btOpenMdbFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FeatureClassBox
            // 
            this.FeatureClassBox.FormattingEnabled = true;
            this.FeatureClassBox.Location = new System.Drawing.Point(125, 23);
            this.FeatureClassBox.Name = "FeatureClassBox";
            this.FeatureClassBox.Size = new System.Drawing.Size(152, 20);
            this.FeatureClassBox.TabIndex = 0;
            // 
            // btOpenMdbFile
            // 
            this.btOpenMdbFile.Location = new System.Drawing.Point(6, 20);
            this.btOpenMdbFile.Name = "btOpenMdbFile";
            this.btOpenMdbFile.Size = new System.Drawing.Size(96, 23);
            this.btOpenMdbFile.TabIndex = 1;
            this.btOpenMdbFile.Text = "Open mdb File";
            this.btOpenMdbFile.UseVisualStyleBackColor = true;
            this.btOpenMdbFile.Click += new System.EventHandler(this.btGetFeature_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btOpenMdbFile);
            this.groupBox1.Controls.Add(this.FeatureClassBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 397);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "mdb";
            // 
            // frmOpenMDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 421);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmOpenMDB";
            this.Text = "frmOpenMDB";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox FeatureClassBox;
        private System.Windows.Forms.Button btOpenMdbFile;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}