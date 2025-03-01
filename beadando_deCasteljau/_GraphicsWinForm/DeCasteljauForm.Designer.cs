namespace _GraphicsWinForm
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
            this.canvas = new System.Windows.Forms.PictureBox();
            this.rbCasteljauIter = new System.Windows.Forms.RadioButton();
            this.rbCasteljauRec = new System.Windows.Forms.RadioButton();
            this.executeBtn = new System.Windows.Forms.Button();
            this.rbIterMultiThread = new System.Windows.Forms.RadioButton();
            this.elapsedTimeLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.Location = new System.Drawing.Point(12, 33);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(1163, 721);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            // 
            // rbCasteljauIter
            // 
            this.rbCasteljauIter.AutoSize = true;
            this.rbCasteljauIter.Checked = true;
            this.rbCasteljauIter.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCasteljauIter.Location = new System.Drawing.Point(12, 8);
            this.rbCasteljauIter.Margin = new System.Windows.Forms.Padding(2);
            this.rbCasteljauIter.Name = "rbCasteljauIter";
            this.rbCasteljauIter.Size = new System.Drawing.Size(98, 21);
            this.rbCasteljauIter.TabIndex = 2;
            this.rbCasteljauIter.TabStop = true;
            this.rbCasteljauIter.Text = "Iterative";
            this.rbCasteljauIter.UseVisualStyleBackColor = true;
            // 
            // rbCasteljauRec
            // 
            this.rbCasteljauRec.AutoSize = true;
            this.rbCasteljauRec.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCasteljauRec.Location = new System.Drawing.Point(227, 8);
            this.rbCasteljauRec.Margin = new System.Windows.Forms.Padding(2);
            this.rbCasteljauRec.Name = "rbCasteljauRec";
            this.rbCasteljauRec.Size = new System.Drawing.Size(218, 21);
            this.rbCasteljauRec.TabIndex = 2;
            this.rbCasteljauRec.Text = "Recursive Multi-threaded";
            this.rbCasteljauRec.UseVisualStyleBackColor = true;
            // 
            // executeBtn
            // 
            this.executeBtn.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.executeBtn.Location = new System.Drawing.Point(1093, 3);
            this.executeBtn.Margin = new System.Windows.Forms.Padding(2);
            this.executeBtn.Name = "executeBtn";
            this.executeBtn.Size = new System.Drawing.Size(80, 25);
            this.executeBtn.TabIndex = 3;
            this.executeBtn.Text = "Execute";
            this.executeBtn.UseVisualStyleBackColor = true;
            this.executeBtn.Click += new System.EventHandler(this.executeBtn_Click);
            // 
            // rbIterMultiThread
            // 
            this.rbIterMultiThread.AutoSize = true;
            this.rbIterMultiThread.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbIterMultiThread.Location = new System.Drawing.Point(528, 6);
            this.rbIterMultiThread.Margin = new System.Windows.Forms.Padding(2);
            this.rbIterMultiThread.Name = "rbIterMultiThread";
            this.rbIterMultiThread.Size = new System.Drawing.Size(218, 21);
            this.rbIterMultiThread.TabIndex = 4;
            this.rbIterMultiThread.Text = "Iterative Multi-threaded";
            this.rbIterMultiThread.UseVisualStyleBackColor = true;
            // 
            // elapsedTimeLbl
            // 
            this.elapsedTimeLbl.AutoSize = true;
            this.elapsedTimeLbl.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elapsedTimeLbl.Location = new System.Drawing.Point(862, 8);
            this.elapsedTimeLbl.Name = "elapsedTimeLbl";
            this.elapsedTimeLbl.Size = new System.Drawing.Size(54, 19);
            this.elapsedTimeLbl.TabIndex = 5;
            this.elapsedTimeLbl.Text = "Time:";
            // 
            // DeCasteljauForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.elapsedTimeLbl);
            this.Controls.Add(this.rbIterMultiThread);
            this.Controls.Add(this.executeBtn);
            this.Controls.Add(this.rbCasteljauRec);
            this.Controls.Add(this.rbCasteljauIter);
            this.Controls.Add(this.canvas);
            this.Name = "DeCasteljauForm";
            this.Text = "Graphics";
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.RadioButton rbCasteljauIter;
        private System.Windows.Forms.RadioButton rbCasteljauRec;
        private System.Windows.Forms.Button executeBtn;
        private System.Windows.Forms.RadioButton rbIterMultiThread;
        private System.Windows.Forms.Label elapsedTimeLbl;
    }
}

