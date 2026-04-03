using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Forms;

public class CreateTaskForm : Form
{
    private readonly ProjectController _projectController;
    private readonly string _projectId;

    private readonly TextBox _txtTitle;
    private readonly TextBox _txtDescription;
    private readonly ComboBox _cboStatus;
    private readonly ComboBox _cboAssignee;
    private readonly Button _btnCreate;

    public CreateTaskForm(ProjectController projectController, string projectId)
    {
        _projectController = projectController;
        _projectId = projectId;

        _txtTitle = new TextBox();
        _txtDescription = new TextBox();
        _cboStatus = new ComboBox();
        _cboAssignee = new ComboBox();
        _btnCreate = new Button();

        InitializeForm();
        LoadStatusOptions();
        LoadAssigneeOptions();
    }

    private void InitializeForm()
    {
        Text = "Create Task";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimumSize = new Size(540, 420);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(245, 248, 252);
        Width = 540;
        Height = 420;

        Label lblTitle = new Label();
        lblTitle.Text = "Task Title:";
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblTitle.Location = new Point(30, 35);

        _txtTitle.Location = new Point(170, 30);
        _txtTitle.Width = 320;
        _txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        Label lblDescription = new Label();
        lblDescription.Text = "Description:";
        lblDescription.AutoSize = true;
        lblDescription.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblDescription.Location = new Point(30, 90);

        _txtDescription.Location = new Point(170, 85);
        _txtDescription.Width = 320;
        _txtDescription.Height = 90;
        _txtDescription.Multiline = true;
        _txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        Label lblStatus = new Label();
        lblStatus.Text = "Status:";
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblStatus.Location = new Point(30, 205);

        _cboStatus.Location = new Point(170, 200);
        _cboStatus.Width = 320;
        _cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        Label lblAssignee = new Label();
        lblAssignee.Text = "Assign Employee:";
        lblAssignee.AutoSize = true;
        lblAssignee.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblAssignee.Location = new Point(30, 260);

        _cboAssignee.Location = new Point(170, 255);
        _cboAssignee.Width = 320;
        _cboAssignee.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboAssignee.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        _btnCreate.Text = "Create Task";
        _btnCreate.Width = 140;
        _btnCreate.Height = 38;
        _btnCreate.Location = new Point(350, 315);
        _btnCreate.BackColor = Color.FromArgb(31, 78, 121);
        _btnCreate.ForeColor = Color.White;
        _btnCreate.FlatStyle = FlatStyle.Flat;
        _btnCreate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _btnCreate.Click += BtnCreate_Click;

        Controls.Add(lblTitle);
        Controls.Add(_txtTitle);
        Controls.Add(lblDescription);
        Controls.Add(_txtDescription);
        Controls.Add(lblStatus);
        Controls.Add(_cboStatus);
        Controls.Add(lblAssignee);
        Controls.Add(_cboAssignee);
        Controls.Add(_btnCreate);
    }

    private void LoadStatusOptions()
    {
        _cboStatus.Items.Clear();
        List<EnumStatus> statuses = _projectController.GetStatusOptions();

        for (int i = 0; i < statuses.Count; i++)
        {
            _cboStatus.Items.Add(statuses[i]);
        }

        if (_cboStatus.Items.Count > 0)
        {
            _cboStatus.SelectedIndex = 0;
        }
    }

    private void LoadAssigneeOptions()
    {
        _cboAssignee.Items.Clear();
        List<Employee> employees = _projectController.GetEmployees();

        for (int i = 0; i < employees.Count; i++)
        {
            _cboAssignee.Items.Add(employees[i]);
        }

        if (_cboAssignee.Items.Count > 0)
        {
            _cboAssignee.SelectedIndex = 0;
        }
    }

    private void BtnCreate_Click(object? sender, EventArgs e)
    {
        if (_cboStatus.SelectedItem == null)
        {
            MessageBox.Show("Please select a task status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cboAssignee.SelectedItem == null)
        {
            MessageBox.Show("Please select an employee.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        EnumStatus selectedStatus = (EnumStatus)_cboStatus.SelectedItem;
        Employee? selectedAssignee = _cboAssignee.SelectedItem as Employee;

        if (selectedAssignee == null)
        {
            MessageBox.Show("Invalid assignee selected.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string message;
        bool created = _projectController.CreateTask(
            _projectId,
            _txtTitle.Text,
            _txtDescription.Text,
            selectedStatus,
            selectedAssignee,
            out message);

        if (created)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
