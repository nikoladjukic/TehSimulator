namespace TehSimulator
{
    partial class Form1
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.bStartStatus = new System.Windows.Forms.Button();
            this.bStopStatus = new System.Windows.Forms.Button();
            this.chbVoziloUValjcima = new System.Windows.Forms.CheckBox();
            this.tmrStatus = new System.Windows.Forms.Timer(this.components);
            this.tmrMeasurements = new System.Windows.Forms.Timer(this.components);
            this.tmrDelay = new System.Windows.Forms.Timer(this.components);
            this.bClearLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(10, 11);
            this.txtLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(602, 556);
            this.txtLog.TabIndex = 0;
            // 
            // bStartStatus
            // 
            this.bStartStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.bStartStatus.Location = new System.Drawing.Point(616, 11);
            this.bStartStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bStartStatus.Name = "bStartStatus";
            this.bStartStatus.Size = new System.Drawing.Size(210, 36);
            this.bStartStatus.TabIndex = 1;
            this.bStartStatus.Text = "Start Sending Status";
            this.bStartStatus.UseVisualStyleBackColor = true;
            this.bStartStatus.Click += new System.EventHandler(this.bStartStatus_Click);
            // 
            // bStopStatus
            // 
            this.bStopStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.bStopStatus.Location = new System.Drawing.Point(616, 51);
            this.bStopStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bStopStatus.Name = "bStopStatus";
            this.bStopStatus.Size = new System.Drawing.Size(210, 36);
            this.bStopStatus.TabIndex = 2;
            this.bStopStatus.Text = "Stop Sending Status";
            this.bStopStatus.UseVisualStyleBackColor = true;
            this.bStopStatus.Click += new System.EventHandler(this.bStopStatus_Click);
            // 
            // chbVoziloUValjcima
            // 
            this.chbVoziloUValjcima.AutoSize = true;
            this.chbVoziloUValjcima.Location = new System.Drawing.Point(616, 93);
            this.chbVoziloUValjcima.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chbVoziloUValjcima.Name = "chbVoziloUValjcima";
            this.chbVoziloUValjcima.Size = new System.Drawing.Size(104, 17);
            this.chbVoziloUValjcima.TabIndex = 3;
            this.chbVoziloUValjcima.Text = "Vozilo u valjcima";
            this.chbVoziloUValjcima.UseVisualStyleBackColor = true;
            // 
            // tmrStatus
            // 
            this.tmrStatus.Tick += new System.EventHandler(this.tmrStatus_Tick);
            // 
            // tmrMeasurements
            // 
            this.tmrMeasurements.Tick += new System.EventHandler(this.tmrMeasurements_Tick);
            // 
            // tmrDelay
            // 
            this.tmrDelay.Tick += new System.EventHandler(this.tmrDelay_Tick);
            // 
            // bClearLog
            // 
            this.bClearLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.bClearLog.Location = new System.Drawing.Point(616, 528);
            this.bClearLog.Margin = new System.Windows.Forms.Padding(2);
            this.bClearLog.Name = "bClearLog";
            this.bClearLog.Size = new System.Drawing.Size(210, 36);
            this.bClearLog.TabIndex = 4;
            this.bClearLog.Text = "Clear Log";
            this.bClearLog.UseVisualStyleBackColor = true;
            this.bClearLog.Click += new System.EventHandler(this.bClearLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 575);
            this.Controls.Add(this.bClearLog);
            this.Controls.Add(this.chbVoziloUValjcima);
            this.Controls.Add(this.bStopStatus);
            this.Controls.Add(this.bStartStatus);
            this.Controls.Add(this.txtLog);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Tehnički Pregled - Simulator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button bStartStatus;
        private System.Windows.Forms.Button bStopStatus;
        private System.Windows.Forms.CheckBox chbVoziloUValjcima;
        private System.Windows.Forms.Timer tmrStatus;
        private System.Windows.Forms.Timer tmrMeasurements;
        private System.Windows.Forms.Timer tmrDelay;
        private System.Windows.Forms.Button bClearLog;
    }
}

