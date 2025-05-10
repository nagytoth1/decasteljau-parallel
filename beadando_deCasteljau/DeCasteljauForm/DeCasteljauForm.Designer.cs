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
            this.elapsedTimeLbl = new System.Windows.Forms.Label();
            this.cbDecasteljau = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.Location = new System.Drawing.Point(10, 33);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(1170, 720);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.OnCanvasPainted);
            // 
            // executeBtn
            // 
            this.executeBtn.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.executeBtn.Location = new System.Drawing.Point(1070, 5);
            this.executeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.executeBtn.Name = "executeBtn";
            this.executeBtn.Padding = new System.Windows.Forms.Padding(2);
            this.executeBtn.Size = new System.Drawing.Size(100, 30);
            this.executeBtn.TabIndex = 3;
            this.executeBtn.Text = "Execute";
            this.executeBtn.UseVisualStyleBackColor = true;
            this.executeBtn.Click += new System.EventHandler(this.ExecuteButtonClicked);
            // 
            // elapsedTimeLbl
            // 
            this.elapsedTimeLbl.AutoSize = true;
            this.elapsedTimeLbl.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elapsedTimeLbl.Location = new System.Drawing.Point(12, 757);
            this.elapsedTimeLbl.Name = "elapsedTimeLbl";
            this.elapsedTimeLbl.Size = new System.Drawing.Size(54, 19);
            this.elapsedTimeLbl.TabIndex = 5;
            this.elapsedTimeLbl.Text = "Time:";
            // 
            // cbDecasteljau
            // 
            this.cbDecasteljau.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDecasteljau.Font = new System.Drawing.Font("Consolas", 12F);
            this.cbDecasteljau.FormattingEnabled = true;
            this.cbDecasteljau.Location = new System.Drawing.Point(10, 5);
            this.cbDecasteljau.Name = "cbDecasteljau";
            this.cbDecasteljau.Size = new System.Drawing.Size(400, 27);
            this.cbDecasteljau.TabIndex = 6;
            // 
            // DeCasteljauForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 811);
            this.Controls.Add(this.cbDecasteljau);
            this.Controls.Add(this.elapsedTimeLbl);
            this.Controls.Add(this.executeBtn);
            this.Controls.Add(this.canvas);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DeCasteljauForm";
            this.Text = "Graphics";
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button executeBtn;
        private System.Windows.Forms.Label elapsedTimeLbl;
        private System.Windows.Forms.ComboBox cbDecasteljau;
    }
}

