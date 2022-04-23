
namespace PingPong
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.PBoxBall = new System.Windows.Forms.PictureBox();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.ballTimer = new System.Windows.Forms.Timer(this.components);
            this.networkPaddleText = new System.Windows.Forms.Label();
            this.localPaddleText = new System.Windows.Forms.Label();
            this.HeartBeatText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PBoxBall)).BeginInit();
            this.SuspendLayout();
            // 
            // PBoxBall
            // 
            this.PBoxBall.Image = ((System.Drawing.Image)(resources.GetObject("PBoxBall.Image")));
            this.PBoxBall.Location = new System.Drawing.Point(392, 238);
            this.PBoxBall.Name = "PBoxBall";
            this.PBoxBall.Size = new System.Drawing.Size(16, 16);
            this.PBoxBall.TabIndex = 1;
            this.PBoxBall.TabStop = false;
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 1;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // ballTimer
            // 
            this.ballTimer.Enabled = true;
            this.ballTimer.Interval = 10;
            this.ballTimer.Tick += new System.EventHandler(this.ballTimer_Tick);
            // 
            // networkPaddleText
            // 
            this.networkPaddleText.AutoSize = true;
            this.networkPaddleText.ForeColor = System.Drawing.SystemColors.Control;
            this.networkPaddleText.Location = new System.Drawing.Point(610, 9);
            this.networkPaddleText.Name = "networkPaddleText";
            this.networkPaddleText.Size = new System.Drawing.Size(107, 15);
            this.networkPaddleText.TabIndex = 3;
            this.networkPaddleText.Text = "networkPaddleText";
            // 
            // localPaddleText
            // 
            this.localPaddleText.AutoSize = true;
            this.localPaddleText.ForeColor = System.Drawing.SystemColors.Control;
            this.localPaddleText.Location = new System.Drawing.Point(610, 35);
            this.localPaddleText.Name = "localPaddleText";
            this.localPaddleText.Size = new System.Drawing.Size(89, 15);
            this.localPaddleText.TabIndex = 4;
            this.localPaddleText.Text = "localPaddleText";
            // 
            // HeartBeatText
            // 
            this.HeartBeatText.AutoSize = true;
            this.HeartBeatText.ForeColor = System.Drawing.SystemColors.Control;
            this.HeartBeatText.Location = new System.Drawing.Point(360, 9);
            this.HeartBeatText.Name = "HeartBeatText";
            this.HeartBeatText.Size = new System.Drawing.Size(80, 15);
            this.HeartBeatText.TabIndex = 5;
            this.HeartBeatText.Text = "HeartBeatText";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(784, 531);
            this.Controls.Add(this.HeartBeatText);
            this.Controls.Add(this.localPaddleText);
            this.Controls.Add(this.networkPaddleText);
            this.Controls.Add(this.PBoxBall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.Text = "PingPong";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.PBoxBall)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox PBoxBall;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Timer ballTimer;
        private System.Windows.Forms.Label networkPaddleText;
        private System.Windows.Forms.Label localPaddleText;
        private System.Windows.Forms.Label HeartBeatText;
    }
}