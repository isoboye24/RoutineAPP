using FontAwesome.Sharp;
using Microsoft.Extensions.DependencyInjection;
using RoutineAPP.AllForms;
using RoutineAPP.Core.Entities;
using RoutineAPP.Core.Interfaces;
using RoutineAPP.HelperService;
using RoutineAPP.UI.ViewModel;
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
using static RoutineAPP.Helper.ReportHelper;

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
        private readonly IDashboardService _dashboardService;
        private readonly IDateProvider _dateProvider;
        private readonly IServiceProvider _serviceProvider;

        private int currentMonth;
        private int currentYear;

        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;

        List<Top5ReportViewModel> _top5MonthlyReportVM;
        List<Top5ReportViewModel> _top5AnnualReportVM;

        public FormDashboard(ICategoryService categoryService, IMonthService monthService, IDailyRoutineService dailyService, 
            ITaskService taskService, IReportService reportService, ICommentService commentService, IGraphService graphService, IDashboardService dashboardService, 
            IDateProvider dateProvider, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(5, 40);
            panelSidebar.Controls.Add(leftBorderBtn);
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;

            _categoryService = categoryService;
            _monthService = monthService;
            _dailyService = dailyService;
            _taskService = taskService;
            _reportService = reportService;
            _commentService = commentService;
            _graphService = graphService;
            _dashboardService = dashboardService;
            _dateProvider = dateProvider;
            _serviceProvider = serviceProvider;

            currentMonth = _dateProvider.Today.Month;
             currentYear = _dateProvider.Today.Year;
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

        private void loadTop5MonthlyReport(int month, int year)
        {
            var domainList = _reportService.GetFormattedTop5MonthlyReport(month, year);

            _top5MonthlyReportVM  = domainList
                .Select(x => new Top5ReportViewModel
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    TotalMinutes = x.TotalMinutes,
                    FormattedTotalMinutes = x.FormattedTotalMinutes,
                    Percentage = x.Percentage 
                })
                .ToList();

            dataGridViewTop5Monthly.DataSource = _top5MonthlyReportVM;
            ConfigureReportDetailsGrid(dataGridViewTop5Monthly, ReportGridType.Top5ReportDetails);
        }

        private void loadTop5AnnualReport(int year)
        {
            var domainList = _reportService.GetFormattedTop5AnnualReport(year);

            _top5AnnualReportVM  = domainList
                .Select(x => new Top5ReportViewModel
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    TotalMinutes = x.TotalMinutes,
                    FormattedTotalMinutes = x.FormattedTotalMinutes,
                    Percentage = x.Percentage 
                })
                .ToList();

            dataGridViewTop5Yearly.DataSource = _top5AnnualReportVM;
            ConfigureReportDetailsGrid(dataGridViewTop5Yearly, ReportGridType.Top5ReportDetails);
        }


        private void FormDashboard_Load(object sender, EventArgs e)
        {
            loadTop5MonthlyReport(currentMonth, currentYear);
            loadTop5AnnualReport(currentYear);

            RefreshCards();
        }

        private void RefreshCards()
        {
            labelTimeOnExercise.Text = _dashboardService.GetCategoryTimeMonthly(currentMonth, currentYear, "Exercise");
            labelTimeOnFamilyInMonth.Text = _dashboardService.GetCategoryTimeMonthly(currentMonth, currentYear, "Family");
            labelTimeOnGermanInMonth.Text = _dashboardService.GetCategoryTimeMonthly(currentMonth, currentYear, "German");
            labelTimeOnGodInMonth.Text = _dashboardService.GetCategoryTimeMonthly(currentMonth, currentYear, "God T");
            labelTimeOnProgrammingInMonth.Text = _dashboardService.GetCategoryTimeMonthly(currentMonth, currentYear, "Programming");
            labelTimeOnBooksInMonth.Text = _dashboardService.GetCategoryTimeMonthly(currentMonth, currentYear, "Books");

            labelTimeOnRussianInYear.Text = _dashboardService.GetCategoryTimeAnually(currentYear, "Russian");
            labelTimeOnGermanInYear.Text = _dashboardService.GetCategoryTimeAnually(currentYear, "German");
            labelTimeOnProgrammingInYear.Text = _dashboardService.GetCategoryTimeAnually(currentYear, "Programming");

            label4.Text = currentYear.ToString();
            label8.Text = GeneralHelper.ConventIntToMonth(currentMonth) + " " + currentYear;

            labeltop5InYear.Text = "Top 5 in " + currentYear;
            labeltop5InMonth.Text = "Top 5 in " + GeneralHelper.ConventIntToMonth(currentMonth) + " " + currentYear;
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
            var form = _serviceProvider.GetRequiredService<FormCategoryList>();
            OpenChildForm(form);
        }

        private void btnMonthlyReports_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            var form = _serviceProvider.GetRequiredService<FormReportsBoard>();
            OpenChildForm(form);
        }

        private void btnDeletedData_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            var form = _serviceProvider.GetRequiredService<FormDeletedData>();
            OpenChildForm(form);
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
            var form = _serviceProvider.GetRequiredService<FormDailyRoutineList>();
            OpenChildForm(form);
        }        

        private void btnSummary_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            var form = _serviceProvider.GetRequiredService<FormCommentList>();
            OpenChildForm(form);
        }

        private void btnGraphs_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            var form = _serviceProvider.GetRequiredService<FormGraphs>();
            OpenChildForm(form);
        }
    }
}
