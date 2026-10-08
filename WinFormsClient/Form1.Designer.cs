using WinFormsClient.Controls;

namespace WinFormsClient
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
            panelHeader = new Panel();
            statusPill = new CardPanel();
            lblStatusText = new Label();
            btnConnect = new RoundedButton();
            txtAddress = new TextBox();
            lblSubtitle = new Label();
            lblTitle = new Label();
            panelSide = new Panel();
            accentPanel = new Panel();
            lblHint = new Label();
            btnDelete = new RoundedButton();
            btnGetById = new RoundedButton();
            btnUpdate = new RoundedButton();
            btnAdd = new RoundedButton();
            nudCount = new NumericUpDown();
            lblCount = new Label();
            txtPrice = new TextBox();
            lblPrice = new Label();
            txtName = new TextBox();
            lblName = new Label();
            txtId = new TextBox();
            lblId = new Label();
            lblSideTitle = new Label();
            panelMain = new Panel();
            gridCard = new CardPanel();
            dataGridView1 = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            panelToolbar = new Panel();
            lblTotal = new Label();
            txtPageSize = new TextBox();
            lblPageSize = new Label();
            txtPage = new TextBox();
            lblPage = new Label();
            btnPaged = new RoundedButton();
            btnStreamAll = new RoundedButton();
            panelLog = new Panel();
            txtLog = new RichTextBox();
            lblLogTitle = new Label();
            panelHeader.SuspendLayout();
            statusPill.SuspendLayout();
            panelSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudCount).BeginInit();
            panelMain.SuspendLayout();
            gridCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panelToolbar.SuspendLayout();
            panelLog.SuspendLayout();
            SuspendLayout();
            //
            // panelHeader
            //
            panelHeader.BackColor = Color.FromArgb(30, 41, 59);
            panelHeader.Controls.Add(statusPill);
            panelHeader.Controls.Add(btnConnect);
            panelHeader.Controls.Add(txtAddress);
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1120, 76);
            panelHeader.TabIndex = 0;
            //
            // statusPill
            //
            statusPill.BorderWidth = 0;
            statusPill.CardColor = Color.FromArgb(22, 163, 74);
            statusPill.Controls.Add(lblStatusText);
            statusPill.CornerRadius = 17;
            statusPill.Location = new Point(16, 21);
            statusPill.Name = "statusPill";
            statusPill.Size = new Size(110, 34);
            statusPill.TabIndex = 4;
            //
            // lblStatusText
            //
            lblStatusText.Dock = DockStyle.Fill;
            lblStatusText.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatusText.ForeColor = Color.White;
            lblStatusText.Location = new Point(0, 0);
            lblStatusText.Name = "lblStatusText";
            lblStatusText.Size = new Size(110, 34);
            lblStatusText.TabIndex = 0;
            lblStatusText.Text = "● متصل";
            lblStatusText.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnConnect
            //
            btnConnect.BackColor = Color.FromArgb(59, 130, 246);
            btnConnect.CornerRadius = 8;
            btnConnect.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(136, 21);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(76, 34);
            btnConnect.TabIndex = 3;
            btnConnect.Text = "اتصال";
            btnConnect.Click += btnConnect_Click;
            //
            // txtAddress
            //
            txtAddress.BackColor = Color.FromArgb(15, 23, 42);
            txtAddress.BorderStyle = BorderStyle.None;
            txtAddress.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtAddress.ForeColor = Color.FromArgb(226, 232, 240);
            txtAddress.Location = new Point(222, 24);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(250, 22);
            txtAddress.TabIndex = 2;
            txtAddress.Text = "https://localhost:7164/";
            //
            // lblSubtitle
            //
            lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSubtitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtitle.Location = new Point(560, 44);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(530, 20);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "ProductService • Unary / Client Stream / Server Stream / Bidirectional";
            lblSubtitle.TextAlign = ContentAlignment.MiddleRight;
            //
            // lblTitle
            //
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(740, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(350, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "مدیریت محصولات gRPC";
            lblTitle.TextAlign = ContentAlignment.MiddleRight;
            //
            // panelSide
            //
            panelSide.BackColor = Color.White;
            panelSide.Controls.Add(accentPanel);
            panelSide.Controls.Add(lblHint);
            panelSide.Controls.Add(btnDelete);
            panelSide.Controls.Add(btnGetById);
            panelSide.Controls.Add(btnUpdate);
            panelSide.Controls.Add(btnAdd);
            panelSide.Controls.Add(nudCount);
            panelSide.Controls.Add(lblCount);
            panelSide.Controls.Add(txtPrice);
            panelSide.Controls.Add(lblPrice);
            panelSide.Controls.Add(txtName);
            panelSide.Controls.Add(lblName);
            panelSide.Controls.Add(txtId);
            panelSide.Controls.Add(lblId);
            panelSide.Controls.Add(lblSideTitle);
            panelSide.Dock = DockStyle.Right;
            panelSide.Location = new Point(790, 76);
            panelSide.Name = "panelSide";
            panelSide.Size = new Size(330, 554);
            panelSide.TabIndex = 2;
            //
            // accentPanel
            //
            accentPanel.BackColor = Color.FromArgb(37, 99, 235);
            accentPanel.Dock = DockStyle.Left;
            accentPanel.Location = new Point(0, 0);
            accentPanel.Name = "accentPanel";
            accentPanel.Size = new Size(4, 554);
            accentPanel.TabIndex = 14;
            //
            // lblHint
            //
            lblHint.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            lblHint.ForeColor = Color.FromArgb(148, 163, 184);
            lblHint.Location = new Point(20, 476);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(290, 64);
            lblHint.TabIndex = 13;
            lblHint.Text = "برای حذف، ردیف‌های جدول را انتخاب کنید؛ در غیر این صورت از شناسه فیلد اول استفاده می‌شود.\r\nبا «تعداد» چند محصول را همزمان ارسال کنید.";
            //
            // btnDelete
            //
            btnDelete.BackColor = Color.FromArgb(220, 38, 38);
            btnDelete.CornerRadius = 10;
            btnDelete.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(20, 428);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(290, 40);
            btnDelete.TabIndex = 12;
            btnDelete.Text = "حذف انتخاب‌ها (Client Stream)";
            btnDelete.Click += btnDelete_Click;
            //
            // btnGetById
            //
            btnGetById.BackColor = Color.FromArgb(71, 85, 105);
            btnGetById.CornerRadius = 10;
            btnGetById.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnGetById.ForeColor = Color.White;
            btnGetById.Location = new Point(20, 380);
            btnGetById.Name = "btnGetById";
            btnGetById.Size = new Size(290, 40);
            btnGetById.TabIndex = 11;
            btnGetById.Text = "دریافت با شناسه (Unary)";
            btnGetById.Click += btnGetById_Click;
            //
            // btnUpdate
            //
            btnUpdate.BackColor = Color.FromArgb(245, 158, 11);
            btnUpdate.CornerRadius = 10;
            btnUpdate.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Location = new Point(20, 332);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(290, 40);
            btnUpdate.TabIndex = 10;
            btnUpdate.Text = "ویرایش (Unary)";
            btnUpdate.Click += btnUpdate_Click;
            //
            // btnAdd
            //
            btnAdd.BackColor = Color.FromArgb(37, 99, 235);
            btnAdd.CornerRadius = 10;
            btnAdd.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(20, 284);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(290, 40);
            btnAdd.TabIndex = 9;
            btnAdd.Text = "افزودن گروهی (Bidirectional)";
            btnAdd.Click += btnAdd_Click;
            //
            // nudCount
            //
            nudCount.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            nudCount.Location = new Point(20, 242);
            nudCount.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            nudCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCount.Name = "nudCount";
            nudCount.Size = new Size(290, 30);
            nudCount.TabIndex = 8;
            nudCount.TextAlign = HorizontalAlignment.Right;
            nudCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblCount
            //
            lblCount.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblCount.ForeColor = Color.FromArgb(100, 116, 139);
            lblCount.Location = new Point(20, 222);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(290, 18);
            lblCount.TabIndex = 7;
            lblCount.Text = "تعداد برای افزودن گروهی";
            lblCount.TextAlign = ContentAlignment.MiddleRight;
            //
            // txtPrice
            //
            txtPrice.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtPrice.Location = new Point(20, 184);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(290, 30);
            txtPrice.TabIndex = 6;
            //
            // lblPrice
            //
            lblPrice.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblPrice.ForeColor = Color.FromArgb(100, 116, 139);
            lblPrice.Location = new Point(20, 164);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(290, 18);
            lblPrice.TabIndex = 5;
            lblPrice.Text = "قیمت (Price)";
            lblPrice.TextAlign = ContentAlignment.MiddleRight;
            //
            // txtName
            //
            txtName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtName.Location = new Point(20, 126);
            txtName.Name = "txtName";
            txtName.Size = new Size(290, 30);
            txtName.TabIndex = 4;
            //
            // lblName
            //
            lblName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblName.ForeColor = Color.FromArgb(100, 116, 139);
            lblName.Location = new Point(20, 106);
            lblName.Name = "lblName";
            lblName.Size = new Size(290, 18);
            lblName.TabIndex = 3;
            lblName.Text = "نام (Name)";
            lblName.TextAlign = ContentAlignment.MiddleRight;
            //
            // txtId
            //
            txtId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtId.Location = new Point(20, 68);
            txtId.Name = "txtId";
            txtId.Size = new Size(290, 30);
            txtId.TabIndex = 2;
            //
            // lblId
            //
            lblId.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblId.ForeColor = Color.FromArgb(100, 116, 139);
            lblId.Location = new Point(20, 48);
            lblId.Name = "lblId";
            lblId.Size = new Size(290, 18);
            lblId.TabIndex = 1;
            lblId.Text = "شناسه (Id)";
            lblId.TextAlign = ContentAlignment.MiddleRight;
            //
            // lblSideTitle
            //
            lblSideTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            lblSideTitle.ForeColor = Color.FromArgb(30, 41, 59);
            lblSideTitle.Location = new Point(20, 14);
            lblSideTitle.Name = "lblSideTitle";
            lblSideTitle.Size = new Size(290, 26);
            lblSideTitle.TabIndex = 0;
            lblSideTitle.Text = "عملیات روی محصول";
            lblSideTitle.TextAlign = ContentAlignment.MiddleRight;
            //
            // panelMain
            //
            panelMain.BackColor = Color.FromArgb(244, 246, 250);
            panelMain.Controls.Add(gridCard);
            panelMain.Controls.Add(panelToolbar);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 76);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(790, 554);
            panelMain.TabIndex = 1;
            //
            // gridCard
            //
            gridCard.BorderWidth = 1;
            gridCard.CardColor = Color.White;
            gridCard.Controls.Add(dataGridView1);
            gridCard.CornerRadius = 14;
            gridCard.Dock = DockStyle.Fill;
            gridCard.Location = new Point(0, 60);
            gridCard.Name = "gridCard";
            gridCard.Padding = new Padding(12);
            gridCard.Size = new Size(790, 494);
            gridCard.TabIndex = 1;
            //
            // dataGridView1
            //
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.ColumnHeadersHeight = 42;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colPrice });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.GridColor = Color.FromArgb(226, 232, 240);
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.MultiSelect = true;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowTemplate.Height = 36;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(766, 470);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            //
            // colId
            //
            colId.FillWeight = 30F;
            colId.HeaderText = "شناسه";
            colId.Name = "colId";
            colId.ReadOnly = true;
            //
            // colName
            //
            colName.FillWeight = 45F;
            colName.HeaderText = "نام";
            colName.Name = "colName";
            colName.ReadOnly = true;
            //
            // colPrice
            //
            colPrice.FillWeight = 25F;
            colPrice.HeaderText = "قیمت";
            colPrice.Name = "colPrice";
            colPrice.ReadOnly = true;
            //
            // panelToolbar
            //
            panelToolbar.Controls.Add(lblTotal);
            panelToolbar.Controls.Add(txtPageSize);
            panelToolbar.Controls.Add(lblPageSize);
            panelToolbar.Controls.Add(txtPage);
            panelToolbar.Controls.Add(lblPage);
            panelToolbar.Controls.Add(btnPaged);
            panelToolbar.Controls.Add(btnStreamAll);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Location = new Point(0, 0);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new Size(790, 60);
            panelToolbar.TabIndex = 0;
            //
            // lblTotal
            //
            lblTotal.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotal.ForeColor = Color.FromArgb(71, 85, 105);
            lblTotal.Location = new Point(16, 11);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(96, 38);
            lblTotal.TabIndex = 6;
            lblTotal.Text = "تعداد: 0";
            lblTotal.TextAlign = ContentAlignment.MiddleCenter;
            //
            // txtPageSize
            //
            txtPageSize.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtPageSize.Location = new Point(120, 16);
            txtPageSize.Name = "txtPageSize";
            txtPageSize.Size = new Size(42, 30);
            txtPageSize.TabIndex = 5;
            txtPageSize.Text = "20";
            //
            // lblPageSize
            //
            lblPageSize.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblPageSize.ForeColor = Color.FromArgb(71, 85, 105);
            lblPageSize.Location = new Point(166, 11);
            lblPageSize.Name = "lblPageSize";
            lblPageSize.Size = new Size(46, 38);
            lblPageSize.TabIndex = 4;
            lblPageSize.Text = "حجم";
            lblPageSize.TextAlign = ContentAlignment.MiddleCenter;
            //
            // txtPage
            //
            txtPage.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            txtPage.Location = new Point(216, 16);
            txtPage.Name = "txtPage";
            txtPage.Size = new Size(36, 30);
            txtPage.TabIndex = 3;
            txtPage.Text = "1";
            //
            // lblPage
            //
            lblPage.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblPage.ForeColor = Color.FromArgb(71, 85, 105);
            lblPage.Location = new Point(256, 11);
            lblPage.Name = "lblPage";
            lblPage.Size = new Size(44, 38);
            lblPage.TabIndex = 2;
            lblPage.Text = "صفحه";
            lblPage.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnPaged
            //
            btnPaged.BackColor = Color.FromArgb(37, 99, 235);
            btnPaged.CornerRadius = 10;
            btnPaged.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnPaged.ForeColor = Color.White;
            btnPaged.Location = new Point(306, 10);
            btnPaged.Name = "btnPaged";
            btnPaged.Size = new Size(228, 40);
            btnPaged.TabIndex = 1;
            btnPaged.Text = "دریافت صفحه‌ای (Unary)";
            btnPaged.Click += btnPaged_Click;
            //
            // btnStreamAll
            //
            btnStreamAll.BackColor = Color.FromArgb(22, 163, 74);
            btnStreamAll.CornerRadius = 10;
            btnStreamAll.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            btnStreamAll.ForeColor = Color.White;
            btnStreamAll.Location = new Point(544, 10);
            btnStreamAll.Name = "btnStreamAll";
            btnStreamAll.Size = new Size(230, 40);
            btnStreamAll.TabIndex = 0;
            btnStreamAll.Text = "دریافت همه (Server Stream)";
            btnStreamAll.Click += btnStreamAll_Click;
            //
            // panelLog
            //
            panelLog.BackColor = Color.FromArgb(15, 23, 42);
            panelLog.Controls.Add(txtLog);
            panelLog.Controls.Add(lblLogTitle);
            panelLog.Dock = DockStyle.Bottom;
            panelLog.Location = new Point(0, 630);
            panelLog.Name = "panelLog";
            panelLog.Size = new Size(1120, 150);
            panelLog.TabIndex = 3;
            //
            // txtLog
            //
            txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLog.BackColor = Color.FromArgb(15, 23, 42);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
            txtLog.ForeColor = Color.FromArgb(148, 163, 184);
            txtLog.Location = new Point(12, 34);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = RichTextBoxScrollBars.Vertical;
            txtLog.Size = new Size(1096, 104);
            txtLog.TabIndex = 1;
            txtLog.WordWrap = false;
            //
            // lblLogTitle
            //
            lblLogTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLogTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
            lblLogTitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblLogTitle.Location = new Point(998, 8);
            lblLogTitle.Name = "lblLogTitle";
            lblLogTitle.Size = new Size(110, 22);
            lblLogTitle.TabIndex = 0;
            lblLogTitle.Text = "گزارش عملیات";
            lblLogTitle.TextAlign = ContentAlignment.MiddleRight;
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1120, 780);
            Controls.Add(panelMain);
            Controls.Add(panelSide);
            Controls.Add(panelLog);
            Controls.Add(panelHeader);
            MinimumSize = new Size(1120, 780);
            Name = "Form1";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "مدیریت محصولات gRPC";
            Load += Form1_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            statusPill.ResumeLayout(false);
            panelSide.ResumeLayout(false);
            panelSide.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudCount).EndInit();
            panelMain.ResumeLayout(false);
            gridCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panelToolbar.ResumeLayout(false);
            panelToolbar.PerformLayout();
            panelLog.ResumeLayout(false);
            panelLog.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private CardPanel statusPill;
        private Label lblStatusText;
        private RoundedButton btnConnect;
        private TextBox txtAddress;
        private Label lblSubtitle;
        private Label lblTitle;
        private Panel panelSide;
        private Panel accentPanel;
        private Label lblHint;
        private RoundedButton btnDelete;
        private RoundedButton btnGetById;
        private RoundedButton btnUpdate;
        private RoundedButton btnAdd;
        private NumericUpDown nudCount;
        private Label lblCount;
        private TextBox txtPrice;
        private Label lblPrice;
        private TextBox txtName;
        private Label lblName;
        private TextBox txtId;
        private Label lblId;
        private Label lblSideTitle;
        private Panel panelMain;
        private CardPanel gridCard;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colPrice;
        private Panel panelToolbar;
        private Label lblTotal;
        private TextBox txtPageSize;
        private Label lblPageSize;
        private TextBox txtPage;
        private Label lblPage;
        private RoundedButton btnPaged;
        private RoundedButton btnStreamAll;
        private Panel panelLog;
        private RichTextBox txtLog;
        private Label lblLogTitle;
    }
}
