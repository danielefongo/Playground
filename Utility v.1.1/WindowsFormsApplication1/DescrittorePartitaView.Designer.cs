namespace Utility
{
    partial class DescrittorePartitaView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._nomePartitaLabel = new System.Windows.Forms.Label();
            this._nomeMasterLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // _nomePartitaLabel
            // 
            this._nomePartitaLabel.AutoSize = true;
            this._nomePartitaLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._nomePartitaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._nomePartitaLabel.Location = new System.Drawing.Point(77, 0);
            this._nomePartitaLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._nomePartitaLabel.Name = "_nomePartitaLabel";
            this._nomePartitaLabel.Size = new System.Drawing.Size(432, 54);
            this._nomePartitaLabel.TabIndex = 1;
            this._nomePartitaLabel.Text = "_nomePartitaLabel";
            this._nomePartitaLabel.Click += new System.EventHandler(this._nomePartitaLabel_Click);
            // 
            // _nomeMasterLabel
            // 
            this._nomeMasterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._nomeMasterLabel.AutoSize = true;
            this._nomeMasterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._nomeMasterLabel.Location = new System.Drawing.Point(77, 71);
            this._nomeMasterLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._nomeMasterLabel.Name = "_nomeMasterLabel";
            this._nomeMasterLabel.Size = new System.Drawing.Size(360, 46);
            this._nomeMasterLabel.TabIndex = 2;
            this._nomeMasterLabel.Text = "_nomeMasterLabel";
            this._nomeMasterLabel.Click += new System.EventHandler(this._nomeMasterLabel_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::Utility.Properties.Resources.shield1;
            this.pictureBox1.InitialImage = global::Utility.Properties.Resources.shield1;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(77, 118);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // DescrittorePartitaView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._nomeMasterLabel);
            this.Controls.Add(this._nomePartitaLabel);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DescrittorePartitaView";
            this.Size = new System.Drawing.Size(727, 118);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label _nomePartitaLabel;
        private System.Windows.Forms.Label _nomeMasterLabel;
    }
}
