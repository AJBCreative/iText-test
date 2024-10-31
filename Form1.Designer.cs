namespace itext_project
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
            lbl_pdfInput = new Label();
            txt_pdfInputs = new TextBox();
            btn_pdfGeneration = new Button();
            btn_mtgCollectionAPI = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // lbl_pdfInput
            // 
            lbl_pdfInput.AutoSize = true;
            lbl_pdfInput.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lbl_pdfInput.Location = new Point(27, 455);
            lbl_pdfInput.Name = "lbl_pdfInput";
            lbl_pdfInput.Size = new Size(390, 15);
            lbl_pdfInput.TabIndex = 0;
            lbl_pdfInput.Text = "ENTER TEXT TO CONVERT TO PDF AND THEN CLICK \"GENERATE PDF\"";
            lbl_pdfInput.Click += lbl_pdfInput_Click;
            // 
            // txt_pdfInputs
            // 
            txt_pdfInputs.Location = new Point(27, 491);
            txt_pdfInputs.Multiline = true;
            txt_pdfInputs.Name = "txt_pdfInputs";
            txt_pdfInputs.ScrollBars = ScrollBars.Vertical;
            txt_pdfInputs.Size = new Size(571, 235);
            txt_pdfInputs.TabIndex = 1;
            // 
            // btn_pdfGeneration
            // 
            btn_pdfGeneration.Location = new Point(624, 491);
            btn_pdfGeneration.Name = "btn_pdfGeneration";
            btn_pdfGeneration.Size = new Size(142, 69);
            btn_pdfGeneration.TabIndex = 2;
            btn_pdfGeneration.Text = "Generate PDF";
            btn_pdfGeneration.UseVisualStyleBackColor = true;
            btn_pdfGeneration.Click += btn_pdfGeneration_Click;
            // 
            // btn_mtgCollectionAPI
            // 
            btn_mtgCollectionAPI.Location = new Point(27, 99);
            btn_mtgCollectionAPI.Name = "btn_mtgCollectionAPI";
            btn_mtgCollectionAPI.Size = new Size(142, 68);
            btn_mtgCollectionAPI.TabIndex = 3;
            btn_mtgCollectionAPI.Text = "GET mtg Collection API";
            btn_mtgCollectionAPI.UseVisualStyleBackColor = true;
            btn_mtgCollectionAPI.Click += btn_mtgCollectionAPI_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 39);
            label1.Name = "label1";
            label1.Size = new Size(433, 15);
            label1.TabIndex = 4;
            label1.Text = "These buttons will auto-generate Magic The Gathering Card sheets in PDF format";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(793, 743);
            Controls.Add(label1);
            Controls.Add(btn_mtgCollectionAPI);
            Controls.Add(btn_pdfGeneration);
            Controls.Add(txt_pdfInputs);
            Controls.Add(lbl_pdfInput);
            Name = "Form1";
            Text = "iTEXT PDF BUILDER";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_pdfInput;
        private TextBox txt_pdfInputs;
        private Button btn_pdfGeneration;
        private Button btn_mtgCollectionAPI;
        private Label label1;
    }
}