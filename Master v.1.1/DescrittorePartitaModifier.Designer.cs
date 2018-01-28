namespace Master
{
    partial class DescrittorePartitaModifier
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
            this._ok = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._nome = new System.Windows.Forms.TextBox();
            this._nomeMaster = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._descrizione = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._annulla = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _ok
            // 
            this._ok.Location = new System.Drawing.Point(265, 193);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(64, 24);
            this._ok.TabIndex = 0;
            this._ok.Text = "Ok";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nome Partita";
            // 
            // _nome
            // 
            this._nome.Location = new System.Drawing.Point(97, 10);
            this._nome.Name = "_nome";
            this._nome.Size = new System.Drawing.Size(302, 20);
            this._nome.TabIndex = 2;
            // 
            // _nomeMaster
            // 
            this._nomeMaster.Location = new System.Drawing.Point(97, 36);
            this._nomeMaster.Name = "_nomeMaster";
            this._nomeMaster.Size = new System.Drawing.Size(302, 20);
            this._nomeMaster.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Nome Master";
            // 
            // _password
            // 
            this._password.Location = new System.Drawing.Point(97, 62);
            this._password.Name = "_password";
            this._password.Size = new System.Drawing.Size(302, 20);
            this._password.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // _descrizione
            // 
            this._descrizione.Location = new System.Drawing.Point(97, 88);
            this._descrizione.Multiline = true;
            this._descrizione.Name = "_descrizione";
            this._descrizione.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._descrizione.Size = new System.Drawing.Size(302, 99);
            this._descrizione.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Descrizione";
            // 
            // _annulla
            // 
            this._annulla.Location = new System.Drawing.Point(335, 193);
            this._annulla.Name = "_annulla";
            this._annulla.Size = new System.Drawing.Size(64, 24);
            this._annulla.TabIndex = 9;
            this._annulla.Text = "Annulla";
            this._annulla.UseVisualStyleBackColor = true;
            // 
            // DescrittorePartitaModifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 229);
            this.Controls.Add(this._annulla);
            this.Controls.Add(this._descrizione);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._nomeMaster);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._nome);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._ok);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(427, 267);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(427, 267);
            this.Name = "DescrittorePartitaModifier";
            this.Text = "Modifica Partita";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _nome;
        private System.Windows.Forms.TextBox _nomeMaster;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _descrizione;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button _annulla;
    }
}