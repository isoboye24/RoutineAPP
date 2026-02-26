using FontAwesome.Sharp;
using RoutineAPP.AllForms;
using RoutineAPP.BLL;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.DAL.DTO;
using RoutineAPP.HelperService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoutineAPP
{
    public partial class FormDashboard : Form
    {
        private readonly ICategoryService _categoryService;
        private readonly IMonthService _monthService;
        private readonly IDailyRoutineService _dailyService;
        private readonly ITaskService _taskService;
        private readonly IReportService _reportService;
        private readonly ICommentService _commentService;
        private readonly IGraphService _graphService;

        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;
        public FormDashboard(ICategoryService categoryService, IMonthService monthService, IDailyRoutineService dailyService, 
            ITaskService taskService, IReportService reportService, ICommentService commentService)
        {
            InitializeComponent();
            _categoryService = categoryService;
            _monthService = monthService;
            _dailyService = dailyService;
            _taskService = taskService;
            _reportService = reportService;
            _commentService = commentService;

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(5, 40);
            panelSidebar.Controls.Add(leftBorderBtn);
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;            
        }
        private struct RBGColors
        {
            public static Color color1 = Color.FromArgb(172, 126, 241);
            public static Color color2 = Color.YellowGreen;
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(245, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
            public static Color normal = Color.MidnightBlue;
        }
        // Button Methods
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                // Button
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.Brown;
                currentBtn.ForeColor = color;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                // Left Border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
            }
        }
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.Maroon;
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.IconColor = Color.Gainsboro; ;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }
        // Drag From
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int IParam);
        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;            
            labelTitleChildForm.Text = "Daily Productivity Analytics System";
        }
        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                // open only a form
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            labelTitleChildForm.Text = childForm.Text;
        }
        DashboardBLL bll = new DashboardBLL();
        int currentMonth = DateTime.Today.Month;
        int currentYear = DateTime.Today.Year;

        private void loadTopMonthlyGrid()
        {
            
        }

        private void loadTopYearlyGrid()
        {
            
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            reportDTO = reportsBLL.SelectMonthlyReports(currentMonth, currentYear);

            dataGridViewTop5Monthly.DataSource = reportDTO.MonthlyTop5Reports;
            dataGridViewTop5Monthly.Columns[0].Visible = false;
            dataGridViewTop5Monthly.Columns[1].Visible = false;
            dataGridViewTop5Monthly.Columns[2].HeaderText = "Cat.";
            dataGridViewTop5Monthly.Columns[3].HeaderText = "Time";
            dataGridViewTop5Monthly.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewTop5Monthly.Columns[4].HeaderText = "%";
            dataGridViewTop5Monthly.Columns[5].Visible = false;
            dataGridViewTop5Monthly.Columns[6].Visible = false;
            foreach (DataGridViewColumn column in dataGridViewTop5Monthly.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            General.StyleDataGridView(dataGridViewTop5Monthly);

            dataGridViewTop5Yearly.DataSource = reportDTO.YearlyTop5Reports;
            dataGridViewTop5Yearly.Columns[0].Visible = false;
            dataGridViewTop5Yearly.Columns[1].Visible = false;
            dataGridViewTop5Yearly.Columns[2].HeaderText = "Cat.";
            dataGridViewTop5Yearly.Columns[3].HeaderText = "Time";
            dataGridViewTop5Yearly.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewTop5Yearly.Columns[4].HeaderText = "%";
            dataGridViewTop5Yearly.Columns[5].Visible = false;
            dataGridViewTop5Yearly.Columns[6].Visible = false;
            foreach (DataGridViewColumn column in dataGridViewTop5Yearly.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            }
            General.StyleDataGridView(dataGridViewTop5Yearly);

            RefreshCards();
        }
        private void RefreshCards()
        {
            labelTimeOnExercise.Text = bll.SelectCategoryInMonth(currentMonth, currentYear, "Exercise").ToString();
            labelTimeOnFamilyInMonth.Text = bll.SelectCategoryInMonth(currentMonth, currentYear, "Family").ToString();
            labelTimeOnGermanInMonth.Text = bll.SelectCategoryInMonth(currentMonth, currentYear, "German").ToString();
            labelTimeOnGodInMonth.Text = bll.SelectCategoryInMonth(currentMonth, currentYear, "God T").ToString();
            labelTimeOnProgrammingInMonth.Text = bll.SelectCategoryInMonth(currentMonth, currentYear, "Programming").ToString();
            labelTimeOnBooksInMonth.Text = bll.SelectCategoryInMonth(currentMonth, currentYear, "Books").ToString();

            labelTimeOnRussianInYear.Text = bll.SelectCategoryInYear(DateTime.Today.Year, "Russian").ToString();
            labelTimeOnGermanInYear.Text = bll.SelectCategoryInYear(DateTime.Today.Year, "German").ToString();
            labelTimeOnProgrammingInYear.Text = bll.SelectCategoryInYear(DateTime.Today.Year, "Programming").ToString();

            label4.Text = DateTime.Today.Year.ToString();
            label8.Text = General.ConventIntToMonth(DateTime.Today.Month);

            labeltop5InYear.Text = "Top 5 in " + DateTime.Today.Year;
            labeltop5InMonth.Text = "Top 5 in " + General.ConventIntToMonth(DateTime.Today.Month);
        }

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private bool buttonWasClicked = false;

        private void btnCategory_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormCategoryList(_categoryService));
        }

        private void btnMonthlyReports_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormReportsBoard(_reportService, _monthService, _categoryService, _dailyService));
        }

        private void btnDeletedData_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormDeletedData());
        }

        private void labelLogo_Click(object sender, EventArgs e)
        {
            if (buttonWasClicked)
            {
                currentChildForm.Close();
                Reset();
                RefreshCards();
            }
        }

        private void iconMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void iconMinimize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void btnRoutine_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormDailyRoutineList(_monthService, _dailyService, _taskService, _categoryService));
        }        

        private void btnSummary_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormSummaryList(_commentService, _dailyService, _monthService));
        }

        private void btnGraphs_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormGraphs(_graphService));
        }
    }
}
