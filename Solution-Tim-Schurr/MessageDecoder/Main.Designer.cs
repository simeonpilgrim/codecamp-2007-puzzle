namespace MessageDecoder
{
    partial class Main
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
            this.EncTransmissionTextBox = new System.Windows.Forms.TextBox();
            this.DecodeButton = new System.Windows.Forms.Button();
            this.DecodedMessageTextBox = new System.Windows.Forms.TextBox();
            this.InputLabel = new System.Windows.Forms.Label();
            this.DecodedLabel = new System.Windows.Forms.Label();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // EncTransmissionTextBox
            // 
            this.EncTransmissionTextBox.Location = new System.Drawing.Point(12, 25);
            this.EncTransmissionTextBox.Name = "EncTransmissionTextBox";
            this.EncTransmissionTextBox.ReadOnly = true;
            this.EncTransmissionTextBox.Size = new System.Drawing.Size(481, 20);
            this.EncTransmissionTextBox.TabIndex = 0;
            this.EncTransmissionTextBox.Text = ".-.-...-..-.-.------.--.....-...--.---.-.------...--------.-..---.--...-.---.-..-" +
                "-.-.-.....-.---.-..-----.-.--.-....-..-.........";
            // 
            // DecodeButton
            // 
            this.DecodeButton.Location = new System.Drawing.Point(499, 20);
            this.DecodeButton.Name = "DecodeButton";
            this.DecodeButton.Size = new System.Drawing.Size(85, 28);
            this.DecodeButton.TabIndex = 1;
            this.DecodeButton.Text = "Decode";
            this.DecodeButton.UseVisualStyleBackColor = true;
            this.DecodeButton.Click += new System.EventHandler(this.DecodeButton_Click);
            // 
            // DecodedMessageTextBox
            // 
            this.DecodedMessageTextBox.Location = new System.Drawing.Point(12, 78);
            this.DecodedMessageTextBox.Multiline = true;
            this.DecodedMessageTextBox.Name = "DecodedMessageTextBox";
            this.DecodedMessageTextBox.Size = new System.Drawing.Size(568, 453);
            this.DecodedMessageTextBox.TabIndex = 2;
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = true;
            this.InputLabel.Location = new System.Drawing.Point(12, 9);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(113, 13);
            this.InputLabel.TabIndex = 3;
            this.InputLabel.Text = "Encoded transmission:";
            // 
            // DecodedLabel
            // 
            this.DecodedLabel.AutoSize = true;
            this.DecodedLabel.Location = new System.Drawing.Point(12, 62);
            this.DecodedLabel.Name = "DecodedLabel";
            this.DecodedLabel.Size = new System.Drawing.Size(93, 13);
            this.DecodedLabel.TabIndex = 4;
            this.DecodedLabel.Text = "Possible solutions:";
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(12, 49);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(480, 10);
            this.Progress.TabIndex = 5;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 543);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.DecodedLabel);
            this.Controls.Add(this.InputLabel);
            this.Controls.Add(this.DecodedMessageTextBox);
            this.Controls.Add(this.DecodeButton);
            this.Controls.Add(this.EncTransmissionTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "Message Decoder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox EncTransmissionTextBox;
        private System.Windows.Forms.Button DecodeButton;
        private System.Windows.Forms.TextBox DecodedMessageTextBox;
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.Label DecodedLabel;
        private System.Windows.Forms.ProgressBar Progress;
    }
}

