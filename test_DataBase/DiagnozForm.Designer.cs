namespace test_DataBase
{
    partial class DiagnozForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagnozForm));
            this.reg_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // reg_button
            // 
            this.reg_button.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.reg_button.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.reg_button.Location = new System.Drawing.Point(65, 117);
            this.reg_button.Name = "reg_button";
            this.reg_button.Size = new System.Drawing.Size(232, 30);
            this.reg_button.TabIndex = 46;
            this.reg_button.Text = "Добавить";
            this.reg_button.UseVisualStyleBackColor = true;
            this.reg_button.Click += new System.EventHandler(this.reg_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(65, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 17);
            this.label3.TabIndex = 45;
            this.label3.Text = "Введите наименование диагноза";
            // 
            // name_TextBox
            // 
            this.name_TextBox.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.name_TextBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.name_TextBox.Location = new System.Drawing.Point(65, 64);
            this.name_TextBox.Margin = new System.Windows.Forms.Padding(20);
            this.name_TextBox.Multiline = true;
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(232, 30);
            this.name_TextBox.TabIndex = 44;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.reg_button);
            this.panel1.Controls.Add(this.name_TextBox);
            this.panel1.Location = new System.Drawing.Point(26, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(371, 198);
            this.panel1.TabIndex = 47;
            // 
            // DiagnozForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 282);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DiagnozForm";
            this.Text = "Добавить диагноз";
            this.Load += new System.EventHandler(this.DiagnozForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button reg_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox name_TextBox;
        private System.Windows.Forms.Panel panel1;
    }
}