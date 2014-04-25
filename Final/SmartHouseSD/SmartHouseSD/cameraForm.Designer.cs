namespace SmartHouseSD
{
    partial class cameraForm
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
            this.components = new System.ComponentModel.Container();
            this.closeButton = new System.Windows.Forms.Button();
            this.recordButton = new System.Windows.Forms.Button();
            this.snapshotButton = new System.Windows.Forms.Button();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(219, 247);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 34);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Close Feed";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // recordButton
            // 
            this.recordButton.Image = global::SmartHouseSD.Properties.Resources.record_icon_resized;
            this.recordButton.Location = new System.Drawing.Point(138, 247);
            this.recordButton.Name = "recordButton";
            this.recordButton.Size = new System.Drawing.Size(75, 34);
            this.recordButton.TabIndex = 4;
            this.recordButton.Text = "Record";
            this.recordButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.recordButton.UseVisualStyleBackColor = true;
            this.recordButton.Click += new System.EventHandler(this.recordButton_Click);
            // 
            // snapshotButton
            // 
            this.snapshotButton.Image = global::SmartHouseSD.Properties.Resources.camera_icon_resized;
            this.snapshotButton.Location = new System.Drawing.Point(12, 247);
            this.snapshotButton.Name = "snapshotButton";
            this.snapshotButton.Size = new System.Drawing.Size(120, 34);
            this.snapshotButton.TabIndex = 3;
            this.snapshotButton.Text = "Take Snapshot";
            this.snapshotButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.snapshotButton.UseVisualStyleBackColor = true;
            this.snapshotButton.Click += new System.EventHandler(this.snapshotButton_Click);
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(12, 12);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(282, 229);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // cameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(306, 293);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.recordButton);
            this.Controls.Add(this.snapshotButton);
            this.Controls.Add(this.imageBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "cameraForm";
            this.Text = "View of ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.cameraForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Button recordButton;
        private System.Windows.Forms.Button closeButton;
        internal System.Windows.Forms.Button snapshotButton;
    }
}