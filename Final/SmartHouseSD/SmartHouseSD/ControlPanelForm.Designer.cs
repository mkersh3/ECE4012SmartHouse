namespace SmartHouseSD
{
    partial class ControlPanelForm
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
            this.lampButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.updateEmailButton = new System.Windows.Forms.Button();
            this.cam1Button = new System.Windows.Forms.Button();
            this.cam2Button = new System.Windows.Forms.Button();
            this.getFullStatButton = new System.Windows.Forms.Button();
            this.DoorLockButton = new System.Windows.Forms.Button();
            this.EmailButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lampButton
            // 
            this.lampButton.Location = new System.Drawing.Point(12, 28);
            this.lampButton.Name = "lampButton";
            this.lampButton.Size = new System.Drawing.Size(107, 23);
            this.lampButton.TabIndex = 0;
            this.lampButton.Text = "Turn Lamp On";
            this.lampButton.UseVisualStyleBackColor = true;
            this.lampButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Current Temperature: ";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "System Off",
            "Low Activity",
            "Medium Activity",
            "High Activity"});
            this.comboBox1.Location = new System.Drawing.Point(125, 57);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(142, 21);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.Text = "Choose an Activity Level";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(273, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(223, 182);
            this.textBox1.TabIndex = 16;
            // 
            // updateEmailButton
            // 
            this.updateEmailButton.Location = new System.Drawing.Point(125, 28);
            this.updateEmailButton.Name = "updateEmailButton";
            this.updateEmailButton.Size = new System.Drawing.Size(142, 23);
            this.updateEmailButton.TabIndex = 17;
            this.updateEmailButton.Text = "Update Email Addresses";
            this.updateEmailButton.UseVisualStyleBackColor = true;
            this.updateEmailButton.Click += new System.EventHandler(this.button9_Click);
            // 
            // cam1Button
            // 
            this.cam1Button.Location = new System.Drawing.Point(12, 57);
            this.cam1Button.Name = "cam1Button";
            this.cam1Button.Size = new System.Drawing.Size(107, 23);
            this.cam1Button.TabIndex = 18;
            this.cam1Button.Text = "View Camera 1";
            this.cam1Button.UseVisualStyleBackColor = true;
            this.cam1Button.Click += new System.EventHandler(this.button10_Click);
            // 
            // cam2Button
            // 
            this.cam2Button.Location = new System.Drawing.Point(12, 86);
            this.cam2Button.Name = "cam2Button";
            this.cam2Button.Size = new System.Drawing.Size(107, 23);
            this.cam2Button.TabIndex = 19;
            this.cam2Button.Text = "View Camera 2";
            this.cam2Button.UseVisualStyleBackColor = true;
            this.cam2Button.Click += new System.EventHandler(this.button11_Click);
            // 
            // getFullStatButton
            // 
            this.getFullStatButton.Location = new System.Drawing.Point(125, 86);
            this.getFullStatButton.Name = "getFullStatButton";
            this.getFullStatButton.Size = new System.Drawing.Size(142, 23);
            this.getFullStatButton.TabIndex = 21;
            this.getFullStatButton.Text = "Get Full Status";
            this.getFullStatButton.UseVisualStyleBackColor = true;
            this.getFullStatButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // DoorLockButton
            // 
            this.DoorLockButton.Location = new System.Drawing.Point(13, 116);
            this.DoorLockButton.Name = "DoorLockButton";
            this.DoorLockButton.Size = new System.Drawing.Size(106, 23);
            this.DoorLockButton.TabIndex = 22;
            this.DoorLockButton.Text = "Get Door Lock";
            this.DoorLockButton.UseVisualStyleBackColor = true;
            this.DoorLockButton.Click += new System.EventHandler(this.DoorLockButton_Click);
            // 
            // EmailButton
            // 
            this.EmailButton.Location = new System.Drawing.Point(126, 116);
            this.EmailButton.Name = "EmailButton";
            this.EmailButton.Size = new System.Drawing.Size(141, 23);
            this.EmailButton.TabIndex = 23;
            this.EmailButton.Text = "Send Update Emails";
            this.EmailButton.UseVisualStyleBackColor = true;
            this.EmailButton.Click += new System.EventHandler(this.EmailButton_Click);
            // 
            // ControlPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 203);
            this.Controls.Add(this.EmailButton);
            this.Controls.Add(this.DoorLockButton);
            this.Controls.Add(this.getFullStatButton);
            this.Controls.Add(this.cam2Button);
            this.Controls.Add(this.cam1Button);
            this.Controls.Add(this.updateEmailButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lampButton);
            this.KeyPreview = true;
            this.Name = "ControlPanelForm";
            this.Text = "Smart House Control Panel";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SmartHouseControlPanel_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button lampButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button updateEmailButton;
        private System.Windows.Forms.Button cam1Button;
        private System.Windows.Forms.Button cam2Button;
        private System.Windows.Forms.Button getFullStatButton;
        private System.Windows.Forms.Button DoorLockButton;
        private System.Windows.Forms.Button EmailButton;
    }
}