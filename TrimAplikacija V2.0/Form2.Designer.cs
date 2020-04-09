namespace TrimAplikacija_V2._0
{
    partial class Form2
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
            this.lblEmployeeName = new System.Windows.Forms.Label();
            this.lblEmployeeLastName = new System.Windows.Forms.Label();
            this.txtEmployeeName = new System.Windows.Forms.TextBox();
            this.txtEmployeeLastName = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblEmployeeName
            // 
            this.lblEmployeeName.AutoSize = true;
            this.lblEmployeeName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeName.Location = new System.Drawing.Point(77, 70);
            this.lblEmployeeName.Name = "lblEmployeeName";
            this.lblEmployeeName.Size = new System.Drawing.Size(36, 21);
            this.lblEmployeeName.TabIndex = 0;
            this.lblEmployeeName.Text = "Ime";
            // 
            // lblEmployeeLastName
            // 
            this.lblEmployeeLastName.AutoSize = true;
            this.lblEmployeeLastName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeLastName.Location = new System.Drawing.Point(77, 100);
            this.lblEmployeeLastName.Name = "lblEmployeeLastName";
            this.lblEmployeeLastName.Size = new System.Drawing.Size(69, 21);
            this.lblEmployeeLastName.TabIndex = 1;
            this.lblEmployeeLastName.Text = "Prezime:";
            // 
            // txtEmployeeName
            // 
            this.txtEmployeeName.Location = new System.Drawing.Point(220, 70);
            this.txtEmployeeName.Name = "txtEmployeeName";
            this.txtEmployeeName.Size = new System.Drawing.Size(227, 20);
            this.txtEmployeeName.TabIndex = 6;
            // 
            // txtEmployeeLastName
            // 
            this.txtEmployeeLastName.Location = new System.Drawing.Point(220, 100);
            this.txtEmployeeLastName.Name = "txtEmployeeLastName";
            this.txtEmployeeLastName.Size = new System.Drawing.Size(227, 20);
            this.txtEmployeeLastName.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(144, 156);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(190, 60);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Pretraži";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 252);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtEmployeeLastName);
            this.Controls.Add(this.txtEmployeeName);
            this.Controls.Add(this.lblEmployeeLastName);
            this.Controls.Add(this.lblEmployeeName);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEmployeeName;
        private System.Windows.Forms.Label lblEmployeeLastName;
        private System.Windows.Forms.TextBox txtEmployeeName;
        private System.Windows.Forms.TextBox txtEmployeeLastName;
        private System.Windows.Forms.Button btnSearch;
    }
}