namespace DeCasteljauForm
{
    partial class DeCasteljauForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeCasteljauForm));
            this.canvas = new System.Windows.Forms.PictureBox();
            this.executeBtn = new System.Windows.Forms.Button();
            this.statusLbl = new System.Windows.Forms.Label();
            this.cbDecasteljau = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.Transparent;
            this.canvas.Cursor = System.Windows.Forms.Cursors.Default;
            this.canvas.Location = new System.Drawing.Point(10, 43);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(1163, 650);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.OnCanvasPainted);
            // 
            // executeBtn
            // 
            this.executeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(170)))), ((int)(((byte)(255)))));
            this.executeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.executeBtn.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.executeBtn.ForeColor = System.Drawing.Color.DarkBlue;
            this.executeBtn.Location = new System.Drawing.Point(1070, 9);
            this.executeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.executeBtn.Name = "executeBtn";
            this.executeBtn.Size = new System.Drawing.Size(103, 26);
            this.executeBtn.TabIndex = 2;
            this.executeBtn.Text = "Execute";
            this.executeBtn.UseVisualStyleBackColor = false;
            this.executeBtn.Click += new System.EventHandler(this.ExecuteButtonClicked);
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.BackColor = System.Drawing.Color.Transparent;
            this.statusLbl.Font = new System.Drawing.Font("Consolas", 13F, System.Drawing.FontStyle.Bold);
            this.statusLbl.Location = new System.Drawing.Point(12, 707);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(60, 22);
            this.statusLbl.TabIndex = 3;
            this.statusLbl.Text = "Time:";
            // 
            // cbDecasteljau
            // 
            this.cbDecasteljau.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDecasteljau.Font = new System.Drawing.Font("Consolas", 12F);
            this.cbDecasteljau.FormattingEnabled = true;
            this.cbDecasteljau.Location = new System.Drawing.Point(318, 7);
            this.cbDecasteljau.Name = "cbDecasteljau";
            this.cbDecasteljau.Size = new System.Drawing.Size(400, 27);
            this.cbDecasteljau.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Consolas", 13F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select execution mode:";
            // 
            // DeCasteljauForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDecasteljau);
            this.Controls.Add(this.statusLbl);
            this.Controls.Add(this.executeBtn);
            this.Controls.Add(this.canvas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "DeCasteljauForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DeCasteljau";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DeCasteljauForm_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button executeBtn;
        private System.Windows.Forms.Label statusLbl;
        private System.Windows.Forms.ComboBox cbDecasteljau;
        private System.Windows.Forms.Label label1;
    }
}

