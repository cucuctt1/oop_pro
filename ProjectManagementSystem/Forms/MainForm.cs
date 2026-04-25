using ProjectManagementSystem.Controllers;

namespace ProjectManagementSystem.Forms;

public class MainForm : Form
{
    private readonly ProjectController _projectController;

    private readonly Panel _headerPanel;
    private readonly Label _lblSubtitle;
    private readonly Button _btnCreateProject;
    private readonly Button _btnViewProjects;
    private readonly Label _lblTitle;

    public MainForm()
    {
        _projectController = new ProjectController();

        _headerPanel = new Panel();
        _lblSubtitle = new Label();
        _btnCreateProject = new Button();
        _btnViewProjects = new Button();
        _lblTitle = new Label();

        InitializeForm();
    }

    private void InitializeForm()
    {
        Text = "Project Management System";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimumSize = new Size(560, 330);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(244, 247, 252);
        Width = 560;
        Height = 330;

        _headerPanel.BackColor = Color.FromArgb(31, 78, 121);
        _headerPanel.Dock = DockStyle.Top;
        _headerPanel.Height = 105;

        _lblTitle.Text = "Project Management System";
        _lblTitle.AutoSize = true;
        _lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
        _lblTitle.ForeColor = Color.White;
        _lblTitle.Location = new Point(22, 24);

        _lblSubtitle.Text = "Track projects, update status, and assign tasks to employees";
        _lblSubtitle.AutoSize = true;
        _lblSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        _lblSubtitle.ForeColor = Color.FromArgb(220, 230, 240);
        _lblSubtitle.Location = new Point(24, 62);

        _btnCreateProject.Text = "Create Project";
        _btnCreateProject.Height = 52;
        _btnCreateProject.BackColor = Color.FromArgb(46, 125, 50);
        _btnCreateProject.ForeColor = Color.White;
        _btnCreateProject.FlatStyle = FlatStyle.Flat;
        _btnCreateProject.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _btnCreateProject.Click += BtnCreateProject_Click;

        _btnViewProjects.Text = "View Projects";
        _btnViewProjects.Height = 52;
        _btnViewProjects.BackColor = Color.FromArgb(31, 78, 121);
        _btnViewProjects.ForeColor = Color.White;
        _btnViewProjects.FlatStyle = FlatStyle.Flat;
        _btnViewProjects.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _btnViewProjects.Click += BtnViewProjects_Click;

        _headerPanel.Controls.Add(_lblTitle);
        _headerPanel.Controls.Add(_lblSubtitle);

        Controls.Add(_headerPanel);
        Controls.Add(_btnCreateProject);
        Controls.Add(_btnViewProjects);

        _headerPanel.BringToFront();

        Resize += MainForm_Resize;
        ApplyResponsiveLayout();
    }

    private void MainForm_Resize(object? sender, EventArgs e)
    {
        ApplyResponsiveLayout();
    }

    private void ApplyResponsiveLayout()
    {
        int sidePadding = 24;
        int gap = 18;
        int top = _headerPanel.Bottom + 40;

        int availableWidth = ClientSize.Width - (sidePadding * 2) - gap;
        int buttonWidth = availableWidth / 2;

        if (buttonWidth < 180)
        {
            buttonWidth = 180;
        }

        int totalButtonsWidth = (buttonWidth * 2) + gap;
        int left = (ClientSize.Width - totalButtonsWidth) / 2;
        if (left < sidePadding)
        {
            left = sidePadding;
        }

        _btnCreateProject.Width = buttonWidth;
        _btnCreateProject.Location = new Point(left, top);

        _btnViewProjects.Width = buttonWidth;
        _btnViewProjects.Location = new Point(_btnCreateProject.Right + gap, top);
    }

    private void BtnCreateProject_Click(object? sender, EventArgs e)
    {
        using (ProjectListForm projectListForm = new ProjectListForm(_projectController, true, false))
        {
            projectListForm.ShowDialog(this);
        }
    }

    private void BtnViewProjects_Click(object? sender, EventArgs e)
    {
        using (ProjectListForm projectListForm = new ProjectListForm(_projectController, false, true))
        {
            projectListForm.ShowDialog(this);
        }
    }
}
