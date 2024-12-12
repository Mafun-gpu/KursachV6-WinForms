﻿namespace KursachV6_WinForms
{
    partial class EditProductForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

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
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblStock = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(150, 20);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(200, 22);
            this.txtName.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(150, 60);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(200, 22);
            this.txtDescription.TabIndex = 1;
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(150, 100);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(200, 22);
            this.txtPrice.TabIndex = 2;
            // 
            // txtStock
            // 
            this.txtStock.Location = new System.Drawing.Point(150, 140);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(200, 22);
            this.txtStock.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 23);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(123, 16);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Название товара:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 63);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(84, 16);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Описание:";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(20, 103);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(46, 16);
            this.lblPrice.TabIndex = 6;
            this.lblPrice.Text = "Цена:";
            // 
            // lblStock
            // 
            this.lblStock.AutoSize = true;
            this.lblStock.Location = new System.Drawing.Point(20, 143);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(115, 16);
            this.lblStock.TabIndex = 7;
            this.lblStock.Text = "Количество на складе:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(150, 200);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(260, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // EditProductForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtStock);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtName);
            this.Name = "EditProductForm";
            this.Text = "Редактирование товара";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}