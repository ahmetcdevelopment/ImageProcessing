namespace GoruntuIsleme
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cbx1 = new ComboBox();
            cbx2 = new ComboBox();
            cbx3 = new ComboBox();
            cbx4 = new ComboBox();
            btnResimYukle = new Button();
            pictureBox1 = new PictureBox();
            openFileDialog1 = new OpenFileDialog();
            btnSave = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // cbx1
            // 
            cbx1.FormattingEnabled = true;
            cbx1.Items.AddRange(new object[] { "Gri Seviye Dönüştür", "Siyah Beyaz", "Zoom In", "Zoom Out", "Kesip Al" });
            cbx1.Location = new Point(21, 92);
            cbx1.Name = "cbx1";
            cbx1.Size = new Size(151, 28);
            cbx1.TabIndex = 0;
            cbx1.SelectedIndexChanged += cbx1_SelectedIndexChanged;
            // 
            // cbx2
            // 
            cbx2.FormattingEnabled = true;
            cbx2.Items.AddRange(new object[] { "Histogram Eşitle", "Nicemleme" });
            cbx2.Location = new Point(21, 153);
            cbx2.Name = "cbx2";
            cbx2.Size = new Size(151, 28);
            cbx2.TabIndex = 1;
            cbx2.SelectedIndexChanged += cbx2_SelectedIndexChanged;
            // 
            // cbx3
            // 
            cbx3.FormattingEnabled = true;
            cbx3.Items.AddRange(new object[] { "Gauss Bulanıklaştırma", "Keskinleştirme", "Kenar Bulma", "Mean Uygula", "Median Uygula", "Kontraharmonik Uygula" });
            cbx3.Location = new Point(21, 211);
            cbx3.Name = "cbx3";
            cbx3.Size = new Size(151, 28);
            cbx3.TabIndex = 2;
            cbx3.SelectedIndexChanged += cbx3_SelectedIndexChanged;
            // 
            // cbx4
            // 
            cbx4.FormattingEnabled = true;
            cbx4.Items.AddRange(new object[] { "Genişletme", "Erozyon", "İskelet Çıkart" });
            cbx4.Location = new Point(21, 273);
            cbx4.Name = "cbx4";
            cbx4.Size = new Size(151, 28);
            cbx4.TabIndex = 3;
            cbx4.SelectedIndexChanged += cbx4_SelectedIndexChanged;
            // 
            // btnResimYukle
            // 
            btnResimYukle.Location = new Point(646, 339);
            btnResimYukle.Name = "btnResimYukle";
            btnResimYukle.Size = new Size(129, 29);
            btnResimYukle.TabIndex = 4;
            btnResimYukle.Text = "Resim Yükle";
            btnResimYukle.UseVisualStyleBackColor = true;
            btnResimYukle.Click += btnResimYukle_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(225, 78);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(550, 255);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(646, 374);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(129, 29);
            btnSave.TabIndex = 6;
            btnSave.Text = "Kaydet";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSave);
            Controls.Add(pictureBox1);
            Controls.Add(btnResimYukle);
            Controls.Add(cbx4);
            Controls.Add(cbx3);
            Controls.Add(cbx2);
            Controls.Add(cbx1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ComboBox cbx1;
        private ComboBox cbx2;
        private ComboBox cbx3;
        private ComboBox cbx4;
        private Button btnResimYukle;
        private PictureBox pictureBox1;
        private OpenFileDialog openFileDialog1;
        private Button btnSave;
    }
}
