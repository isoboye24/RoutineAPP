using FontAwesome.Sharp;
using RoutineAPP.AllForms;
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
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;
        public FormDashboard()
        {
            InitializeComponent();
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
            labelTitleChildForm.Text = "Routine App";
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
        private void FormDashboard_Load(object sender, EventArgs e)
        {

        }

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void iconClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private bool buttonWasClicked = false;

        private void btnCategory_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormCategoryList());
        }

        private void btnMonthlyReports_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormMonthlyRoutineReportsList());
        }

        private void btnYearlyReports_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormYearlyReportsList());
        }

        private void btnTotalReports_Click(object sender, EventArgs e)
        {
            buttonWasClicked = true;
            ActivateButton(sender, RBGColors.color2);
            OpenChildForm(new FormTotalReportsList());
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
            OpenChildForm(new FormDailyRoutineList());
        }
    }
}
