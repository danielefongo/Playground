namespace Utility
{
    partial class MainForm
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
            this._menu = new System.Windows.Forms.MenuStrip();
            this._partitaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._nuovaPartitaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._joinPartitaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._opzioniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._impostazioniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._joinButton = new System.Windows.Forms.Button();
            this._descrittoriFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this._descrizioneText = new System.Windows.Forms.TextBox();
            this._menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menu
            // 
            this._menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._partitaToolStripMenuItem,
            this._opzioniToolStripMenuItem});
            this._menu.Location = new System.Drawing.Point(0, 0);
            this._menu.Name = "_menu";
            this._menu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this._menu.Size = new System.Drawing.Size(677, 28);
            this._menu.TabIndex = 0;
            this._menu.Text = "menuStrip1";
            // 
            // _partitaToolStripMenuItem
            // 
            this._partitaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._nuovaPartitaToolStripMenuItem,
            this._joinPartitaToolStripMenuItem});
            this._partitaToolStripMenuItem.Name = "_partitaToolStripMenuItem";
            this._partitaToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this._partitaToolStripMenuItem.Text = "Partita";
            // 
            // _nuovaPartitaToolStripMenuItem
            // 
            this._nuovaPartitaToolStripMenuItem.Name = "_nuovaPartitaToolStripMenuItem";
            this._nuovaPartitaToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this._nuovaPartitaToolStripMenuItem.Text = "Nuova Partita";
            // 
            // _joinPartitaToolStripMenuItem
            // 
            this._joinPartitaToolStripMenuItem.Enabled = false;
            this._joinPartitaToolStripMenuItem.Name = "_joinPartitaToolStripMenuItem";
            this._joinPartitaToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this._joinPartitaToolStripMenuItem.Text = "Join Partita";
            this._joinPartitaToolStripMenuItem.Click += new System.EventHandler(this.joinPartitaToolStripMenuItem_Click);
            // 
            // _opzioniToolStripMenuItem
            // 
            this._opzioniToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._impostazioniToolStripMenuItem});
            this._opzioniToolStripMenuItem.Name = "_opzioniToolStripMenuItem";
            this._opzioniToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this._opzioniToolStripMenuItem.Text = "Opzioni";
            // 
            // _impostazioniToolStripMenuItem
            // 
            this._impostazioniToolStripMenuItem.Name = "_impostazioniToolStripMenuItem";
            this._impostazioniToolStripMenuItem.Size = new System.Drawing.Size(164, 24);
            this._impostazioniToolStripMenuItem.Text = "Impostazioni";
            this._impostazioniToolStripMenuItem.Click += new System.EventHandler(this.impostazioniToolStripMenuItem_Click);
            // 
            // _joinButton
            // 
            this._joinButton.Dock = System.Windows.Forms.DockStyle.Right;
            this._joinButton.Enabled = false;
            this._joinButton.Location = new System.Drawing.Point(572, 0);
            this._joinButton.Margin = new System.Windows.Forms.Padding(4);
            this._joinButton.Name = "_joinButton";
            this._joinButton.Size = new System.Drawing.Size(105, 112);
            this._joinButton.TabIndex = 0;
            this._joinButton.Text = "Join";
            this._joinButton.UseVisualStyleBackColor = true;
            this._joinButton.Click += new System.EventHandler(this._joinButton_Click);
            // 
            // _descrittoriFlowLayoutPanel
            // 
            this._descrittoriFlowLayoutPanel.AutoScroll = true;
            this._descrittoriFlowLayoutPanel.BackColor = System.Drawing.Color.White;
            this._descrittoriFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._descrittoriFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._descrittoriFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._descrittoriFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this._descrittoriFlowLayoutPanel.Name = "_descrittoriFlowLayoutPanel";
            this._descrittoriFlowLayoutPanel.Size = new System.Drawing.Size(677, 395);
            this._descrittoriFlowLayoutPanel.TabIndex = 2;
            this._descrittoriFlowLayoutPanel.WrapContents = false;
            this._descrittoriFlowLayoutPanel.Resize += new System.EventHandler(this._descrittoriFlowLayoutPanel_Resize);
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.Location = new System.Drawing.Point(0, 28);
            this._splitContainer.Margin = new System.Windows.Forms.Padding(4);
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._descrittoriFlowLayoutPanel);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this.panel1);
            this._splitContainer.Panel2.Controls.Add(this._joinButton);
            this._splitContainer.Size = new System.Drawing.Size(677, 512);
            this._splitContainer.SplitterDistance = 395;
            this._splitContainer.SplitterWidth = 5;
            this._splitContainer.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._descrizioneText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(572, 112);
            this.panel1.TabIndex = 1;
            // 
            // _descrizioneText
            // 
            this._descrizioneText.Dock = System.Windows.Forms.DockStyle.Fill;
            this._descrizioneText.Location = new System.Drawing.Point(0, 0);
            this._descrizioneText.Margin = new System.Windows.Forms.Padding(4);
            this._descrizioneText.Multiline = true;
            this._descrizioneText.Name = "_descrizioneText";
            this._descrizioneText.ReadOnly = true;
            this._descrizioneText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._descrizioneText.Size = new System.Drawing.Size(572, 112);
            this._descrizioneText.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 540);
            this.Controls.Add(this._splitContainer);
            this.Controls.Add(this._menu);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "PlayGround";
            this.ResizeEnd += new System.EventHandler(this._descrittoriFlowLayoutPanel_Resize);
            this.Resize += new System.EventHandler(this._descrittoriFlowLayoutPanel_Resize);
            this._menu.ResumeLayout(false);
            this._menu.PerformLayout();
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.MenuStrip _menu;
        protected System.Windows.Forms.Button _joinButton;
        protected System.Windows.Forms.FlowLayoutPanel _descrittoriFlowLayoutPanel;
        protected System.Windows.Forms.TextBox _descrizioneText;
        protected System.Windows.Forms.SplitContainer _splitContainer;
        protected System.Windows.Forms.ToolStripMenuItem _partitaToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem _nuovaPartitaToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem _joinPartitaToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem _opzioniToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem _impostazioniToolStripMenuItem;

    }
}