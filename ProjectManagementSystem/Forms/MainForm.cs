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
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = Color.FromArgb(244, 247, 252);
        Width = 560;
        Height = 330;

        _headerPanel.BackColor = Color.FromArgb(31, 78, 121);
        _headerPanel.Location = new Point(0, 0);
        _headerPanel.Width = 560;
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
        _btnCreateProject.Width = 220;
        _btnCreateProject.Height = 52;
        _btnCreateProject.Location = new Point(36, 150);
        _btnCreateProject.BackColor = Color.FromArgb(46, 125, 50);
        _btnCreateProject.ForeColor = Color.White;
        _btnCreateProject.FlatStyle = FlatStyle.Flat;
        _btnCreateProject.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _btnCreateProject.Click += BtnCreateProject_Click;

        _btnViewProjects.Text = "View Projects";
        _btnViewProjects.Width = 220;
        _btnViewProjects.Height = 52;
        _btnViewProjects.Location = new Point(292, 150);
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
    }

    private void BtnCreateProject_Click(object? sender, EventArgs e)
    {
        using (ProjectListForm projectListForm = new ProjectListForm(_projectController, true))
        {
            projectListForm.ShowDialog(this);
        }
    }

    private void BtnViewProjects_Click(object? sender, EventArgs e)
    {
        using (ProjectListForm projectListForm = new ProjectListForm(_projectController))
        {
            projectListForm.ShowDialog(this);
        }
    }
}
