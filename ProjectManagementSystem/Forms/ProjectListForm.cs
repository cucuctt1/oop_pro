using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Forms;

public class ProjectListForm : Form
{
    private readonly ProjectController _projectController;
    private readonly bool _focusCreateSection;

    private readonly GroupBox _groupCreateProject;
    private readonly TextBox _txtProjectName;
    private readonly TextBox _txtProjectDescription;
    private readonly DateTimePicker _dtpProjectStartDate;
    private readonly DateTimePicker _dtpProjectEndDate;
    private readonly ComboBox _cboCreateStatus;
    private readonly ComboBox _cboCreateLeader;
    private readonly CheckedListBox _chkInvolvedEmployees;
    private readonly Button _btnCreateProject;
    private readonly Button _btnSelectAllEmployees;
    private readonly Button _btnClearEmployees;

    private readonly DataGridView _gridProjects;
    private readonly DataGridView _gridTasks;
    private readonly Button _btnDelete;
    private readonly Button _btnRefresh;
    private readonly Button _btnUpdateStatus;
    private readonly Button _btnCreateTask;
    private readonly ComboBox _cboProjectStatus;
    private readonly Label _lblProjectStatus;
    private readonly Label _lblTaskSection;

    public ProjectListForm(ProjectController projectController)
        : this(projectController, false)
    {
    }

    public ProjectListForm(ProjectController projectController, bool focusCreateSection)
    {
        _projectController = projectController;
        _focusCreateSection = focusCreateSection;

        _groupCreateProject = new GroupBox();
        _txtProjectName = new TextBox();
        _txtProjectDescription = new TextBox();
        _dtpProjectStartDate = new DateTimePicker();
        _dtpProjectEndDate = new DateTimePicker();
        _cboCreateStatus = new ComboBox();
        _cboCreateLeader = new ComboBox();
        _chkInvolvedEmployees = new CheckedListBox();
        _btnCreateProject = new Button();
        _btnSelectAllEmployees = new Button();
        _btnClearEmployees = new Button();

        _gridProjects = new DataGridView();
        _gridTasks = new DataGridView();
        _btnDelete = new Button();
        _btnRefresh = new Button();
        _btnUpdateStatus = new Button();
        _btnCreateTask = new Button();
        _cboProjectStatus = new ComboBox();
        _lblProjectStatus = new Label();
        _lblTaskSection = new Label();

        InitializeForm();
        LoadStatusOptions();
        LoadLeaderOptions();
        LoadEmployeeOptions();
        LoadProjectsToGrid();

        if (_focusCreateSection)
        {
            _txtProjectName.Focus();
            _groupCreateProject.BackColor = Color.FromArgb(234, 244, 252);
        }
    }

    private void InitializeForm()
    {
        Text = "Project List";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimumSize = new Size(1120, 720);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        Width = 1260;
        Height = 720;
        BackColor = Color.FromArgb(245, 248, 252);

        Panel headerPanel = new Panel();
        headerPanel.BackColor = Color.FromArgb(31, 78, 121);
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = 72;

        Label lblHeader = new Label();
        lblHeader.Text = "Project Dashboard";
        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblHeader.ForeColor = Color.White;
        lblHeader.Location = new Point(20, 20);
        headerPanel.Controls.Add(lblHeader);

        _groupCreateProject.Text = "Create New Project";
        _groupCreateProject.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _groupCreateProject.Location = new Point(20, 90);
        _groupCreateProject.Width = 1200;
        _groupCreateProject.Height = 185;
        _groupCreateProject.BackColor = Color.FromArgb(241, 247, 252);
        _groupCreateProject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        Label lblProjectName = new Label();
        lblProjectName.Text = "Name:";
        lblProjectName.AutoSize = true;
        lblProjectName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblProjectName.Location = new Point(16, 33);

        _txtProjectName.Location = new Point(95, 29);
        _txtProjectName.Width = 300;
        _txtProjectName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        Label lblProjectDescription = new Label();
        lblProjectDescription.Text = "Description:";
        lblProjectDescription.AutoSize = true;
        lblProjectDescription.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblProjectDescription.Location = new Point(16, 71);

        _txtProjectDescription.Location = new Point(95, 67);
        _txtProjectDescription.Width = 300;
        _txtProjectDescription.Height = 72;
        _txtProjectDescription.Multiline = true;
        _txtProjectDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        Label lblStart = new Label();
        lblStart.Text = "Start:";
        lblStart.AutoSize = true;
        lblStart.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblStart.Location = new Point(16, 150);

        _dtpProjectStartDate.Location = new Point(62, 146);
        _dtpProjectStartDate.Width = 120;
        _dtpProjectStartDate.Format = DateTimePickerFormat.Short;

        Label lblEnd = new Label();
        lblEnd.Text = "End:";
        lblEnd.AutoSize = true;
        lblEnd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblEnd.Location = new Point(194, 150);

        _dtpProjectEndDate.Location = new Point(233, 146);
        _dtpProjectEndDate.Width = 120;
        _dtpProjectEndDate.Format = DateTimePickerFormat.Short;

        Label lblCreateStatus = new Label();
        lblCreateStatus.Text = "Status:";
        lblCreateStatus.AutoSize = true;
        lblCreateStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblCreateStatus.Location = new Point(430, 33);

        _cboCreateStatus.Location = new Point(492, 29);
        _cboCreateStatus.Width = 180;
        _cboCreateStatus.DropDownStyle = ComboBoxStyle.DropDownList;

        Label lblCreateLeader = new Label();
        lblCreateLeader.Text = "Leader:";
        lblCreateLeader.AutoSize = true;
        lblCreateLeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblCreateLeader.Location = new Point(430, 71);

        _cboCreateLeader.Location = new Point(492, 67);
        _cboCreateLeader.Width = 180;
        _cboCreateLeader.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboCreateLeader.SelectedIndexChanged += CboCreateLeader_SelectedIndexChanged;

        Label lblEmployees = new Label();
        lblEmployees.Text = "Involved Employees:";
        lblEmployees.AutoSize = true;
        lblEmployees.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblEmployees.Location = new Point(705, 29);

        _chkInvolvedEmployees.Location = new Point(705, 53);
        _chkInvolvedEmployees.Width = 330;
        _chkInvolvedEmployees.Height = 105;
        _chkInvolvedEmployees.CheckOnClick = true;
        _chkInvolvedEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        _btnSelectAllEmployees.Text = "Select All";
        _btnSelectAllEmployees.Width = 92;
        _btnSelectAllEmployees.Height = 28;
        _btnSelectAllEmployees.Location = new Point(1048, 53);
        _btnSelectAllEmployees.BackColor = Color.White;
        _btnSelectAllEmployees.FlatStyle = FlatStyle.Flat;
        _btnSelectAllEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnSelectAllEmployees.Click += BtnSelectAllEmployees_Click;

        _btnClearEmployees.Text = "Clear";
        _btnClearEmployees.Width = 92;
        _btnClearEmployees.Height = 28;
        _btnClearEmployees.Location = new Point(1048, 87);
        _btnClearEmployees.BackColor = Color.White;
        _btnClearEmployees.FlatStyle = FlatStyle.Flat;
        _btnClearEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnClearEmployees.Click += BtnClearEmployees_Click;

        _btnCreateProject.Text = "Create Project";
        _btnCreateProject.Width = 200;
        _btnCreateProject.Height = 32;
        _btnCreateProject.Location = new Point(940, 147);
        _btnCreateProject.BackColor = Color.FromArgb(46, 125, 50);
        _btnCreateProject.ForeColor = Color.White;
        _btnCreateProject.FlatStyle = FlatStyle.Flat;
        _btnCreateProject.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnCreateProject.Click += BtnCreateProject_Click;

        _groupCreateProject.Controls.Add(lblProjectName);
        _groupCreateProject.Controls.Add(_txtProjectName);
        _groupCreateProject.Controls.Add(lblProjectDescription);
        _groupCreateProject.Controls.Add(_txtProjectDescription);
        _groupCreateProject.Controls.Add(lblStart);
        _groupCreateProject.Controls.Add(_dtpProjectStartDate);
        _groupCreateProject.Controls.Add(lblEnd);
        _groupCreateProject.Controls.Add(_dtpProjectEndDate);
        _groupCreateProject.Controls.Add(lblCreateStatus);
        _groupCreateProject.Controls.Add(_cboCreateStatus);
        _groupCreateProject.Controls.Add(lblCreateLeader);
        _groupCreateProject.Controls.Add(_cboCreateLeader);
        _groupCreateProject.Controls.Add(lblEmployees);
        _groupCreateProject.Controls.Add(_chkInvolvedEmployees);
        _groupCreateProject.Controls.Add(_btnSelectAllEmployees);
        _groupCreateProject.Controls.Add(_btnClearEmployees);
        _groupCreateProject.Controls.Add(_btnCreateProject);

        _gridProjects.Location = new Point(20, 288);
        _gridProjects.Width = 1200;
        _gridProjects.Height = 190;
        _gridProjects.AllowUserToAddRows = false;
        _gridProjects.AllowUserToDeleteRows = false;
        _gridProjects.ReadOnly = true;
        _gridProjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _gridProjects.MultiSelect = false;
        _gridProjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridProjects.RowHeadersVisible = false;
        _gridProjects.BackgroundColor = Color.White;
        _gridProjects.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _gridProjects.SelectionChanged += GridProjects_SelectionChanged;

        DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
        colId.Name = "ProjectId";
        colId.HeaderText = "Project ID";
        _gridProjects.Columns.Add(colId);

        DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
        colName.Name = "ProjectName";
        colName.HeaderText = "Name";
        _gridProjects.Columns.Add(colName);

        DataGridViewTextBoxColumn colDescription = new DataGridViewTextBoxColumn();
        colDescription.Name = "Description";
        colDescription.HeaderText = "Description";
        _gridProjects.Columns.Add(colDescription);

        DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
        colStatus.Name = "Status";
        colStatus.HeaderText = "Status";
        _gridProjects.Columns.Add(colStatus);

        DataGridViewTextBoxColumn colStartDate = new DataGridViewTextBoxColumn();
        colStartDate.Name = "StartDate";
        colStartDate.HeaderText = "Start Date";
        _gridProjects.Columns.Add(colStartDate);

        DataGridViewTextBoxColumn colEndDate = new DataGridViewTextBoxColumn();
        colEndDate.Name = "EndDate";
        colEndDate.HeaderText = "End Date";
        _gridProjects.Columns.Add(colEndDate);

        DataGridViewTextBoxColumn colLeader = new DataGridViewTextBoxColumn();
        colLeader.Name = "Leader";
        colLeader.HeaderText = "Leader";
        _gridProjects.Columns.Add(colLeader);

        DataGridViewTextBoxColumn colTaskCount = new DataGridViewTextBoxColumn();
        colTaskCount.Name = "TaskCount";
        colTaskCount.HeaderText = "Tasks";
        _gridProjects.Columns.Add(colTaskCount);

        _lblProjectStatus.Text = "Update Status:";
        _lblProjectStatus.AutoSize = true;
        _lblProjectStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        _lblProjectStatus.Location = new Point(20, 490);
        _lblProjectStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left;

        _cboProjectStatus.Location = new Point(130, 486);
        _cboProjectStatus.Width = 190;
        _cboProjectStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboProjectStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left;

        _btnUpdateStatus.Text = "Apply Status";
        _btnUpdateStatus.Width = 120;
        _btnUpdateStatus.Height = 32;
        _btnUpdateStatus.Location = new Point(334, 483);
        _btnUpdateStatus.BackColor = Color.FromArgb(31, 78, 121);
        _btnUpdateStatus.ForeColor = Color.White;
        _btnUpdateStatus.FlatStyle = FlatStyle.Flat;
        _btnUpdateStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        _btnUpdateStatus.Click += BtnUpdateStatus_Click;

        _btnCreateTask.Text = "Create Task + Assign";
        _btnCreateTask.Width = 180;
        _btnCreateTask.Height = 32;
        _btnCreateTask.Location = new Point(470, 483);
        _btnCreateTask.BackColor = Color.FromArgb(46, 125, 50);
        _btnCreateTask.ForeColor = Color.White;
        _btnCreateTask.FlatStyle = FlatStyle.Flat;
        _btnCreateTask.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnCreateTask.Click += BtnCreateTask_Click;

        _btnRefresh.Text = "Refresh";
        _btnRefresh.Width = 120;
        _btnRefresh.Height = 32;
        _btnRefresh.Location = new Point(960, 483);
        _btnRefresh.BackColor = Color.White;
        _btnRefresh.FlatStyle = FlatStyle.Flat;
        _btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnRefresh.Click += BtnRefresh_Click;

        _btnDelete.Text = "Delete";
        _btnDelete.Width = 120;
        _btnDelete.Height = 32;
        _btnDelete.Location = new Point(1100, 483);
        _btnDelete.BackColor = Color.FromArgb(183, 28, 28);
        _btnDelete.ForeColor = Color.White;
        _btnDelete.FlatStyle = FlatStyle.Flat;
        _btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnDelete.Click += BtnDelete_Click;

        _lblTaskSection.Text = "Tasks (select a project to view tasks)";
        _lblTaskSection.AutoSize = true;
        _lblTaskSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _lblTaskSection.Location = new Point(20, 534);
        _lblTaskSection.Anchor = AnchorStyles.Top | AnchorStyles.Left;

        _gridTasks.Location = new Point(20, 562);
        _gridTasks.Width = 1200;
        _gridTasks.Height = 120;
        _gridTasks.AllowUserToAddRows = false;
        _gridTasks.AllowUserToDeleteRows = false;
        _gridTasks.ReadOnly = true;
        _gridTasks.MultiSelect = false;
        _gridTasks.RowHeadersVisible = false;
        _gridTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridTasks.BackgroundColor = Color.White;
        _gridTasks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        DataGridViewTextBoxColumn taskId = new DataGridViewTextBoxColumn();
        taskId.Name = "TaskId";
        taskId.HeaderText = "Task ID";
        _gridTasks.Columns.Add(taskId);

        DataGridViewTextBoxColumn taskTitle = new DataGridViewTextBoxColumn();
        taskTitle.Name = "TaskTitle";
        taskTitle.HeaderText = "Title";
        _gridTasks.Columns.Add(taskTitle);

        DataGridViewTextBoxColumn taskDescription = new DataGridViewTextBoxColumn();
        taskDescription.Name = "TaskDescription";
        taskDescription.HeaderText = "Description";
        _gridTasks.Columns.Add(taskDescription);

        DataGridViewTextBoxColumn taskStatus = new DataGridViewTextBoxColumn();
        taskStatus.Name = "TaskStatus";
        taskStatus.HeaderText = "Status";
        _gridTasks.Columns.Add(taskStatus);

        DataGridViewTextBoxColumn taskAssignee = new DataGridViewTextBoxColumn();
        taskAssignee.Name = "TaskAssignee";
        taskAssignee.HeaderText = "Assignee";
        _gridTasks.Columns.Add(taskAssignee);

        Controls.Add(headerPanel);
        Controls.Add(_groupCreateProject);
        Controls.Add(_gridProjects);
        Controls.Add(_lblProjectStatus);
        Controls.Add(_cboProjectStatus);
        Controls.Add(_btnUpdateStatus);
        Controls.Add(_btnCreateTask);
        Controls.Add(_btnRefresh);
        Controls.Add(_btnDelete);
        Controls.Add(_lblTaskSection);
        Controls.Add(_gridTasks);

        Resize += ProjectListForm_Resize;
        ApplyResponsiveLayout();
    }

    private void ProjectListForm_Resize(object? sender, EventArgs e)
    {
        ApplyResponsiveLayout();
    }

    private void ApplyResponsiveLayout()
    {
        int sidePadding = 20;
        int spacing = 12;
        int contentWidth = ClientSize.Width - (sidePadding * 2);

        if (contentWidth < 900)
        {
            contentWidth = 900;
        }

        _groupCreateProject.Width = contentWidth;
        _gridProjects.Width = contentWidth;
        _gridTasks.Width = contentWidth;

        _btnCreateProject.Left = _groupCreateProject.ClientSize.Width - _btnCreateProject.Width - 16;
        _btnSelectAllEmployees.Left = _groupCreateProject.ClientSize.Width - _btnSelectAllEmployees.Width - 16;
        _btnClearEmployees.Left = _groupCreateProject.ClientSize.Width - _btnClearEmployees.Width - 16;

        _btnDelete.Left = ClientSize.Width - sidePadding - _btnDelete.Width;
        _btnRefresh.Left = _btnDelete.Left - spacing - _btnRefresh.Width;
        _btnCreateTask.Left = _btnRefresh.Left - spacing - _btnCreateTask.Width;

        int projectTop = _groupCreateProject.Bottom + spacing;
        _gridProjects.Top = projectTop;

        int remainingHeight = ClientSize.Height - projectTop - 220;
        if (remainingHeight < 170)
        {
            remainingHeight = 170;
        }

        _gridProjects.Height = remainingHeight;

        int actionTop = _gridProjects.Bottom + 8;
        _lblProjectStatus.Top = actionTop + 8;
        _lblProjectStatus.Left = sidePadding;

        _cboProjectStatus.Left = _lblProjectStatus.Right + 10;
        _cboProjectStatus.Top = actionTop + 3;
        _btnUpdateStatus.Top = actionTop;
        _btnUpdateStatus.Left = _cboProjectStatus.Right + 12;
        _btnCreateTask.Top = actionTop;
        _btnRefresh.Top = actionTop;
        _btnDelete.Top = actionTop;

        _lblTaskSection.Top = actionTop + 48;

        _gridTasks.Top = _lblTaskSection.Bottom + 6;
        _gridTasks.Height = ClientSize.Height - _gridTasks.Top - 20;

        if (_gridTasks.Height < 100)
        {
            _gridTasks.Height = 100;
        }
    }

    private void LoadStatusOptions()
    {
        _cboProjectStatus.Items.Clear();
        _cboCreateStatus.Items.Clear();

        List<EnumStatus> statuses = _projectController.GetStatusOptions();
        for (int i = 0; i < statuses.Count; i++)
        {
            _cboProjectStatus.Items.Add(statuses[i]);
            _cboCreateStatus.Items.Add(statuses[i]);
        }

        if (_cboProjectStatus.Items.Count > 0)
        {
            _cboProjectStatus.SelectedIndex = 0;
        }

        if (_cboCreateStatus.Items.Count > 0)
        {
            _cboCreateStatus.SelectedIndex = 0;
        }
    }

    private void LoadLeaderOptions()
    {
        _cboCreateLeader.Items.Clear();

        List<Employee> leaders = _projectController.GetLeaders();
        for (int i = 0; i < leaders.Count; i++)
        {
            _cboCreateLeader.Items.Add(leaders[i]);
        }

        if (_cboCreateLeader.Items.Count > 0)
        {
            _cboCreateLeader.SelectedIndex = 0;
        }
    }

    private void LoadEmployeeOptions()
    {
        _chkInvolvedEmployees.Items.Clear();

        List<Employee> employees = _projectController.GetEmployees();
        for (int i = 0; i < employees.Count; i++)
        {
            _chkInvolvedEmployees.Items.Add(employees[i], false);
        }

        EnsureLeaderCheckedInEmployeeList();
    }

    private void EnsureLeaderCheckedInEmployeeList()
    {
        if (_cboCreateLeader.SelectedItem == null)
        {
            return;
        }

        Employee? selectedLeader = _cboCreateLeader.SelectedItem as Employee;
        if (selectedLeader == null)
        {
            return;
        }

        for (int i = 0; i < _chkInvolvedEmployees.Items.Count; i++)
        {
            Employee? employee = _chkInvolvedEmployees.Items[i] as Employee;
            if (employee == null)
            {
                continue;
            }

            if (employee.Id == selectedLeader.Id)
            {
                _chkInvolvedEmployees.SetItemChecked(i, true);
                return;
            }
        }
    }

    private void LoadProjectsToGrid()
    {
        _gridProjects.Rows.Clear();

        List<Project> projects = _projectController.GetProjects();

        for (int i = 0; i < projects.Count; i++)
        {
            Project project = projects[i];

            string leaderInfo = string.Empty;
            if (project.Leader != null)
            {
                leaderInfo = project.Leader.Name + " - " + project.Leader.GetRole();
            }

            int taskCount = 0;
            if (project.Tasks != null)
            {
                taskCount = project.Tasks.Count;
            }

            _gridProjects.Rows.Add(
                project.ProjectId,
                project.ProjectName,
                project.Description,
                project.Status.ToString(),
                project.StartDate.ToShortDateString(),
                project.EndDate.ToShortDateString(),
                leaderInfo,
                taskCount);
        }

        if (_gridProjects.Rows.Count > 0)
        {
            _gridProjects.Rows[0].Selected = true;
            SyncStatusFromSelectedProject();
            LoadTasksForSelectedProject();
        }
        else
        {
            _gridTasks.Rows.Clear();
            _lblTaskSection.Text = "Tasks (select a project to view tasks)";
        }
    }

    private string GetSelectedProjectId()
    {
        if (_gridProjects.SelectedRows.Count == 0)
        {
            return string.Empty;
        }

        DataGridViewRow selectedRow = _gridProjects.SelectedRows[0];
        object? cellValue = selectedRow.Cells["ProjectId"].Value;

        if (cellValue == null)
        {
            return string.Empty;
        }

        return cellValue.ToString() ?? string.Empty;
    }

    private void SyncStatusFromSelectedProject()
    {
        string projectId = GetSelectedProjectId();
        if (string.IsNullOrWhiteSpace(projectId))
        {
            return;
        }

        List<Project> projects = _projectController.GetProjects();
        EnumStatus selectedStatus = EnumStatus.Pending;
        bool found = false;

        for (int i = 0; i < projects.Count; i++)
        {
            if (projects[i].ProjectId == projectId)
            {
                selectedStatus = projects[i].Status;
                found = true;
                break;
            }
        }

        if (!found)
        {
            return;
        }

        for (int i = 0; i < _cboProjectStatus.Items.Count; i++)
        {
            object? item = _cboProjectStatus.Items[i];
            if (item != null)
            {
                EnumStatus status = (EnumStatus)item;
                if (status == selectedStatus)
                {
                    _cboProjectStatus.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    private void LoadTasksForSelectedProject()
    {
        _gridTasks.Rows.Clear();

        string projectId = GetSelectedProjectId();
        if (string.IsNullOrWhiteSpace(projectId))
        {
            _lblTaskSection.Text = "Tasks (select a project to view tasks)";
            return;
        }

        _lblTaskSection.Text = "Tasks for Project: " + projectId;

        List<TaskItem> tasks = _projectController.GetTasksByProjectId(projectId);
        for (int i = 0; i < tasks.Count; i++)
        {
            TaskItem task = tasks[i];
            string assigneeName = string.Empty;

            if (task.Assignee != null)
            {
                assigneeName = task.Assignee.Name + " - " + task.Assignee.GetRole();
            }

            _gridTasks.Rows.Add(
                task.TaskId,
                task.Title,
                task.Description,
                task.Status.ToString(),
                assigneeName);
        }
    }

    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        LoadProjectsToGrid();
    }

    private void BtnCreateProject_Click(object? sender, EventArgs e)
    {
        if (_cboCreateStatus.SelectedItem == null)
        {
            MessageBox.Show("Please select project status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cboCreateLeader.SelectedItem == null)
        {
            MessageBox.Show("Please select project leader.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        EnumStatus selectedStatus = (EnumStatus)_cboCreateStatus.SelectedItem;
        Employee? selectedLeader = _cboCreateLeader.SelectedItem as Employee;

        if (selectedLeader == null)
        {
            MessageBox.Show("Invalid leader selected.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        List<Employee> involvedEmployees = new List<Employee>();
        for (int i = 0; i < _chkInvolvedEmployees.CheckedItems.Count; i++)
        {
            Employee? employee = _chkInvolvedEmployees.CheckedItems[i] as Employee;
            if (employee != null)
            {
                involvedEmployees.Add(employee);
            }
        }

        string message;
        bool created = _projectController.CreateProject(
            _txtProjectName.Text,
            _txtProjectDescription.Text,
            _dtpProjectStartDate.Value.Date,
            _dtpProjectEndDate.Value.Date,
            selectedStatus,
            selectedLeader,
            involvedEmployees,
            out message);

        if (created)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetCreateProjectForm();
            LoadProjectsToGrid();
        }
        else
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSelectAllEmployees_Click(object? sender, EventArgs e)
    {
        for (int i = 0; i < _chkInvolvedEmployees.Items.Count; i++)
        {
            _chkInvolvedEmployees.SetItemChecked(i, true);
        }
    }

    private void BtnClearEmployees_Click(object? sender, EventArgs e)
    {
        for (int i = 0; i < _chkInvolvedEmployees.Items.Count; i++)
        {
            _chkInvolvedEmployees.SetItemChecked(i, false);
        }

        EnsureLeaderCheckedInEmployeeList();
    }

    private void CboCreateLeader_SelectedIndexChanged(object? sender, EventArgs e)
    {
        EnsureLeaderCheckedInEmployeeList();
    }

    private void ResetCreateProjectForm()
    {
        _txtProjectName.Text = string.Empty;
        _txtProjectDescription.Text = string.Empty;
        _dtpProjectStartDate.Value = DateTime.Today;
        _dtpProjectEndDate.Value = DateTime.Today;

        if (_cboCreateStatus.Items.Count > 0)
        {
            _cboCreateStatus.SelectedIndex = 0;
        }

        if (_cboCreateLeader.Items.Count > 0)
        {
            _cboCreateLeader.SelectedIndex = 0;
        }

        for (int i = 0; i < _chkInvolvedEmployees.Items.Count; i++)
        {
            _chkInvolvedEmployees.SetItemChecked(i, false);
        }

        EnsureLeaderCheckedInEmployeeList();
    }

    private void BtnUpdateStatus_Click(object? sender, EventArgs e)
    {
        string projectId = GetSelectedProjectId();
        if (string.IsNullOrWhiteSpace(projectId))
        {
            MessageBox.Show("Please select a project.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cboProjectStatus.SelectedItem == null)
        {
            MessageBox.Show("Please select a status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        EnumStatus selectedStatus = (EnumStatus)_cboProjectStatus.SelectedItem;
        string message;
        bool updated = _projectController.UpdateProjectStatus(projectId, selectedStatus, out message);

        if (updated)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadProjectsToGrid();
        }
        else
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCreateTask_Click(object? sender, EventArgs e)
    {
        string projectId = GetSelectedProjectId();
        if (string.IsNullOrWhiteSpace(projectId))
        {
            MessageBox.Show("Please select a project first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using (CreateTaskForm createTaskForm = new CreateTaskForm(_projectController, projectId))
        {
            DialogResult result = createTaskForm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                LoadProjectsToGrid();
            }
        }
    }

    private void GridProjects_SelectionChanged(object? sender, EventArgs e)
    {
        SyncStatusFromSelectedProject();
        LoadTasksForSelectedProject();
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        string projectId = GetSelectedProjectId();
        if (string.IsNullOrWhiteSpace(projectId))
        {
            MessageBox.Show("Please select a project to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DialogResult confirm = MessageBox.Show(
            "Are you sure you want to delete this project?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        string message;
        bool deleted = _projectController.DeleteProject(projectId, out message);

        if (deleted)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadProjectsToGrid();
        }
        else
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
