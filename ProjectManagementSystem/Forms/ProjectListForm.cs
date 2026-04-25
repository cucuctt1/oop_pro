using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Forms;

public class ProjectListForm : Form
{
    private readonly ProjectController _projectController;
    private readonly bool _focusCreateSection;
    private readonly bool _viewOnlyMode;

    private readonly GroupBox _groupFilterEmployees;
    private readonly CheckBox _chkFilterAllEmployees;
    private readonly CheckedListBox _chkFilterEmployees;
    private readonly Label _lblFilterDescription;

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
    private readonly Button _btnDelete;
    private readonly Button _btnRefresh;
    private readonly Button _btnUpdateStatus;
    private readonly Button _btnCreateTask;
    private readonly ComboBox _cboProjectStatus;
    private readonly Label _lblProjectStatus;

    private readonly Label _lblProjectSummary;
    private readonly TextBox _txtProjectSummary;
    private readonly Label _lblEmployeeSection;
    private readonly DataGridView _gridProjectEmployees;

    private readonly Label _lblTaskSection;
    private readonly DataGridView _gridTasks;

    public ProjectListForm(ProjectController projectController)
        : this(projectController, false, false)
    {
    }

    public ProjectListForm(ProjectController projectController, bool focusCreateSection)
        : this(projectController, focusCreateSection, false)
    {
    }

    public ProjectListForm(ProjectController projectController, bool focusCreateSection, bool viewOnlyMode)
    {
        _projectController = projectController;
        _focusCreateSection = focusCreateSection;
        _viewOnlyMode = viewOnlyMode;

        _groupFilterEmployees = new GroupBox();
        _chkFilterAllEmployees = new CheckBox();
        _chkFilterEmployees = new CheckedListBox();
        _lblFilterDescription = new Label();

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
        _btnDelete = new Button();
        _btnRefresh = new Button();
        _btnUpdateStatus = new Button();
        _btnCreateTask = new Button();
        _cboProjectStatus = new ComboBox();
        _lblProjectStatus = new Label();

        _lblProjectSummary = new Label();
        _txtProjectSummary = new TextBox();
        _lblEmployeeSection = new Label();
        _gridProjectEmployees = new DataGridView();

        _lblTaskSection = new Label();
        _gridTasks = new DataGridView();

        InitializeForm();
        LoadStatusOptions();
        LoadLeaderOptions();
        LoadEmployeeOptions();
        LoadFilterEmployees();
        LoadProjectsToGrid();

        if (_focusCreateSection && !_viewOnlyMode)
        {
            _txtProjectName.Focus();
            _groupCreateProject.BackColor = Color.FromArgb(234, 244, 252);
        }
    }

    private void InitializeForm()
    {
        Text = _viewOnlyMode ? "Project Viewer" : "Project Dashboard";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimumSize = new Size(1120, 720);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScroll = true;
        Width = 1260;
        Height = 820;
        BackColor = Color.FromArgb(245, 248, 252);

        Panel headerPanel = new Panel();
        headerPanel.BackColor = Color.FromArgb(31, 78, 121);
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = 72;

        Label lblHeader = new Label();
        lblHeader.Text = _viewOnlyMode ? "Project Viewer Dashboard" : "Project Management Dashboard";
        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblHeader.ForeColor = Color.White;
        lblHeader.Location = new Point(20, 20);
        headerPanel.Controls.Add(lblHeader);

        _groupFilterEmployees.Text = "Project Filter by Employee";
        _groupFilterEmployees.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _groupFilterEmployees.BackColor = Color.FromArgb(241, 247, 252);
        _groupFilterEmployees.Location = new Point(20, 90);
        _groupFilterEmployees.Width = 1200;
        _groupFilterEmployees.Height = 110;
        _groupFilterEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        _chkFilterAllEmployees.Text = "All Employees";
        _chkFilterAllEmployees.AutoSize = true;
        _chkFilterAllEmployees.Location = new Point(16, 30);
        _chkFilterAllEmployees.CheckedChanged += ChkFilterAllEmployees_CheckedChanged;

        _lblFilterDescription.Text = "Uncheck All Employees and tick one or many employees to filter related projects.";
        _lblFilterDescription.AutoSize = true;
        _lblFilterDescription.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        _lblFilterDescription.Location = new Point(16, 65);

        _chkFilterEmployees.Location = new Point(220, 22);
        _chkFilterEmployees.Width = 950;
        _chkFilterEmployees.Height = 72;
        _chkFilterEmployees.CheckOnClick = true;
        _chkFilterEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _chkFilterEmployees.ItemCheck += ChkFilterEmployees_ItemCheck;

        _groupFilterEmployees.Controls.Add(_chkFilterAllEmployees);
        _groupFilterEmployees.Controls.Add(_lblFilterDescription);
        _groupFilterEmployees.Controls.Add(_chkFilterEmployees);

        _groupCreateProject.Text = "Create New Project";
        _groupCreateProject.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        _groupCreateProject.BackColor = Color.FromArgb(241, 247, 252);
        _groupCreateProject.Location = new Point(20, 210);
        _groupCreateProject.Width = 1200;
        _groupCreateProject.Height = 185;
        _groupCreateProject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        Label lblProjectName = new Label();
        lblProjectName.Text = "Name:";
        lblProjectName.AutoSize = true;
        lblProjectName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblProjectName.Location = new Point(16, 33);

        _txtProjectName.Location = new Point(95, 29);
        _txtProjectName.Width = 300;
        _txtProjectName.Anchor = AnchorStyles.Top | AnchorStyles.Left;

        Label lblProjectDescription = new Label();
        lblProjectDescription.Text = "Description:";
        lblProjectDescription.AutoSize = true;
        lblProjectDescription.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblProjectDescription.Location = new Point(16, 71);

        _txtProjectDescription.Location = new Point(95, 67);
        _txtProjectDescription.Width = 300;
        _txtProjectDescription.Height = 72;
        _txtProjectDescription.Multiline = true;
        _txtProjectDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left;

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

        _gridProjects.Location = new Point(20, 405);
        _gridProjects.Width = 1200;
        _gridProjects.Height = 180;
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
        _lblProjectStatus.Location = new Point(20, 600);

        _cboProjectStatus.Location = new Point(130, 596);
        _cboProjectStatus.Width = 190;
        _cboProjectStatus.DropDownStyle = ComboBoxStyle.DropDownList;

        _btnUpdateStatus.Text = "Apply Status";
        _btnUpdateStatus.Width = 120;
        _btnUpdateStatus.Height = 32;
        _btnUpdateStatus.Location = new Point(334, 593);
        _btnUpdateStatus.BackColor = Color.FromArgb(31, 78, 121);
        _btnUpdateStatus.ForeColor = Color.White;
        _btnUpdateStatus.FlatStyle = FlatStyle.Flat;
        _btnUpdateStatus.Click += BtnUpdateStatus_Click;

        _btnCreateTask.Text = "Create Task + Assign";
        _btnCreateTask.Width = 180;
        _btnCreateTask.Height = 32;
        _btnCreateTask.Location = new Point(470, 593);
        _btnCreateTask.BackColor = Color.FromArgb(46, 125, 50);
        _btnCreateTask.ForeColor = Color.White;
        _btnCreateTask.FlatStyle = FlatStyle.Flat;
        _btnCreateTask.Click += BtnCreateTask_Click;

        _btnRefresh.Text = "Refresh";
        _btnRefresh.Width = 120;
        _btnRefresh.Height = 32;
        _btnRefresh.Location = new Point(960, 593);
        _btnRefresh.BackColor = Color.White;
        _btnRefresh.FlatStyle = FlatStyle.Flat;
        _btnRefresh.Click += BtnRefresh_Click;

        _btnDelete.Text = "Delete";
        _btnDelete.Width = 120;
        _btnDelete.Height = 32;
        _btnDelete.Location = new Point(1100, 593);
        _btnDelete.BackColor = Color.FromArgb(183, 28, 28);
        _btnDelete.ForeColor = Color.White;
        _btnDelete.FlatStyle = FlatStyle.Flat;
        _btnDelete.Click += BtnDelete_Click;

        _lblProjectSummary.Text = "Selected Project Details";
        _lblProjectSummary.AutoSize = true;
        _lblProjectSummary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _lblProjectSummary.Location = new Point(20, 640);

        _txtProjectSummary.Location = new Point(20, 666);
        _txtProjectSummary.Width = 1200;
        _txtProjectSummary.Height = 72;
        _txtProjectSummary.Multiline = true;
        _txtProjectSummary.ReadOnly = true;
        _txtProjectSummary.ScrollBars = ScrollBars.Vertical;
        _txtProjectSummary.BackColor = Color.White;
        _txtProjectSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        _lblEmployeeSection.Text = "Employees in Selected Project";
        _lblEmployeeSection.AutoSize = true;
        _lblEmployeeSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _lblEmployeeSection.Location = new Point(20, 746);

        _gridProjectEmployees.Location = new Point(20, 772);
        _gridProjectEmployees.Width = 1200;
        _gridProjectEmployees.Height = 90;
        _gridProjectEmployees.AllowUserToAddRows = false;
        _gridProjectEmployees.AllowUserToDeleteRows = false;
        _gridProjectEmployees.ReadOnly = true;
        _gridProjectEmployees.MultiSelect = false;
        _gridProjectEmployees.RowHeadersVisible = false;
        _gridProjectEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gridProjectEmployees.BackgroundColor = Color.White;
        _gridProjectEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        DataGridViewTextBoxColumn empId = new DataGridViewTextBoxColumn();
        empId.Name = "EmployeeId";
        empId.HeaderText = "Employee ID";
        _gridProjectEmployees.Columns.Add(empId);

        DataGridViewTextBoxColumn empName = new DataGridViewTextBoxColumn();
        empName.Name = "EmployeeName";
        empName.HeaderText = "Name";
        _gridProjectEmployees.Columns.Add(empName);

        DataGridViewTextBoxColumn empRole = new DataGridViewTextBoxColumn();
        empRole.Name = "EmployeeRole";
        empRole.HeaderText = "Role";
        _gridProjectEmployees.Columns.Add(empRole);

        _lblTaskSection.Text = "Tasks (select a project to view tasks)";
        _lblTaskSection.AutoSize = true;
        _lblTaskSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _lblTaskSection.Location = new Point(20, 872);

        _gridTasks.Location = new Point(20, 898);
        _gridTasks.Width = 1200;
        _gridTasks.Height = 130;
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
        Controls.Add(_groupFilterEmployees);
        Controls.Add(_groupCreateProject);
        Controls.Add(_gridProjects);
        Controls.Add(_lblProjectStatus);
        Controls.Add(_cboProjectStatus);
        Controls.Add(_btnUpdateStatus);
        Controls.Add(_btnCreateTask);
        Controls.Add(_btnRefresh);
        Controls.Add(_btnDelete);
        Controls.Add(_lblProjectSummary);
        Controls.Add(_txtProjectSummary);
        Controls.Add(_lblEmployeeSection);
        Controls.Add(_gridProjectEmployees);
        Controls.Add(_lblTaskSection);
        Controls.Add(_gridTasks);

        if (_viewOnlyMode)
        {
            _groupCreateProject.Visible = false;
            _lblProjectStatus.Visible = false;
            _cboProjectStatus.Visible = false;
            _btnUpdateStatus.Visible = false;
            _btnCreateTask.Visible = false;
            _btnDelete.Visible = false;
        }

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
        int spacing = 10;
        int contentWidth = ClientSize.Width - (sidePadding * 2);
        if (contentWidth < 760)
        {
            contentWidth = 760;
        }
        int contentRight = sidePadding + contentWidth;

        bool compactVertical = ClientSize.Height < 760;
        int summaryHeight = compactVertical ? 60 : 72;
        int projectEmployeesHeight = compactVertical ? 72 : 90;
        int minTaskHeight = compactVertical ? 90 : 110;
        int minProjectGridHeight = compactVertical ? 100 : 150;

        _groupFilterEmployees.Left = sidePadding;
        _groupFilterEmployees.Top = 90;
        _groupFilterEmployees.Width = contentWidth;

        _chkFilterAllEmployees.Left = 16;
        _chkFilterAllEmployees.Top = 30;

        int filterListTop = 24;
        int filterListHeight = Math.Max(60, (_chkFilterEmployees.ItemHeight * 2) + 12);
        int filterListLeft = 220;
        int filterListWidth = _groupFilterEmployees.ClientSize.Width - filterListLeft - 16;
        if (filterListWidth < 260)
        {
            filterListWidth = 260;
            filterListLeft = _groupFilterEmployees.ClientSize.Width - filterListWidth - 16;
            if (filterListLeft < 16)
            {
                filterListLeft = 16;
            }
        }

        _chkFilterEmployees.Left = filterListLeft;
        _chkFilterEmployees.Top = filterListTop;
        _chkFilterEmployees.Width = filterListWidth;
        _chkFilterEmployees.Height = filterListHeight;

        _lblFilterDescription.Left = 16;
        _lblFilterDescription.Top = _chkFilterEmployees.Bottom + 8;
        _lblFilterDescription.MaximumSize = new Size(_groupFilterEmployees.ClientSize.Width - 32, 0);

        _groupFilterEmployees.Height = _lblFilterDescription.Bottom + 12;

        int contentTopAfterCreate;

        if (_viewOnlyMode)
        {
            _groupCreateProject.Visible = false;
            contentTopAfterCreate = _groupFilterEmployees.Bottom + spacing;
        }
        else
        {
            _groupCreateProject.Visible = true;
            _groupCreateProject.Left = sidePadding;
            _groupCreateProject.Top = _groupFilterEmployees.Bottom + spacing;
            _groupCreateProject.Width = contentWidth;

            int createGroupHeight = compactVertical ? 205 : 220;
            _groupCreateProject.Height = createGroupHeight;

            int createRightPadding = 16;
            int createButtonGap = 10;
            int employeeListLeft = 705;
            int employeeListTop = 53;
            int employeeButtonsWidth = _btnSelectAllEmployees.Width;
            int employeeListWidth = _groupCreateProject.ClientSize.Width - employeeListLeft - createRightPadding - createButtonGap - employeeButtonsWidth;
            int minEmployeeListLeft = _cboCreateLeader.Right + 14;

            if (employeeListWidth < 180)
            {
                employeeListWidth = 180;
                employeeListLeft = _groupCreateProject.ClientSize.Width - createRightPadding - createButtonGap - employeeButtonsWidth - employeeListWidth;
                if (employeeListLeft < minEmployeeListLeft)
                {
                    employeeListLeft = minEmployeeListLeft;
                    employeeListWidth = _groupCreateProject.ClientSize.Width - employeeListLeft - createRightPadding - createButtonGap - employeeButtonsWidth;
                }
            }

            if (employeeListWidth < 120)
            {
                employeeListWidth = 120;
            }

            if (employeeListWidth > 420)
            {
                employeeListWidth = 420;
                employeeListLeft = _groupCreateProject.ClientSize.Width - createRightPadding - createButtonGap - employeeButtonsWidth - employeeListWidth;
            }

            _chkInvolvedEmployees.Left = employeeListLeft;
            _chkInvolvedEmployees.Top = employeeListTop;
            _chkInvolvedEmployees.Width = employeeListWidth;

            int buttonsLeft = _chkInvolvedEmployees.Right + createButtonGap;
            int maxButtonsLeft = _groupCreateProject.ClientSize.Width - createRightPadding - employeeButtonsWidth;
            if (buttonsLeft > maxButtonsLeft)
            {
                int overflow = buttonsLeft - maxButtonsLeft;
                employeeListWidth = _chkInvolvedEmployees.Width - overflow;
                if (employeeListWidth < 120)
                {
                    employeeListWidth = 120;
                }

                _chkInvolvedEmployees.Width = employeeListWidth;
                buttonsLeft = _chkInvolvedEmployees.Right + createButtonGap;
            }

            _btnSelectAllEmployees.Left = buttonsLeft;
            _btnSelectAllEmployees.Top = employeeListTop;

            _btnClearEmployees.Left = _btnSelectAllEmployees.Left;
            _btnClearEmployees.Top = _btnSelectAllEmployees.Bottom + 6;

            _btnCreateProject.Top = _groupCreateProject.ClientSize.Height - _btnCreateProject.Height - 10;
            _btnCreateProject.Left = _groupCreateProject.ClientSize.Width - _btnCreateProject.Width - createRightPadding;

            int employeeListHeight = _btnCreateProject.Top - employeeListTop - 8;
            if (employeeListHeight < 70)
            {
                employeeListHeight = 70;
            }
            _chkInvolvedEmployees.Height = employeeListHeight;

            contentTopAfterCreate = _groupCreateProject.Bottom + spacing;
        }

        _gridProjects.Left = sidePadding;
        _gridProjects.Top = contentTopAfterCreate;
        _gridProjects.Width = contentWidth;

        int detailTopOffsetFromAction = _viewOnlyMode ? 40 : 46;
        int reservedBelowProjectGrid = 8
            + detailTopOffsetFromAction
            + _lblProjectSummary.Height
            + 6
            + summaryHeight
            + 8
            + _lblEmployeeSection.Height
            + 6
            + projectEmployeesHeight
            + 8
            + _lblTaskSection.Height
            + 6
            + minTaskHeight
            + 20;

        int gridProjectHeight = ClientSize.Height - _gridProjects.Top - reservedBelowProjectGrid;
        if (gridProjectHeight < minProjectGridHeight)
        {
            gridProjectHeight = minProjectGridHeight;
        }
        _gridProjects.Height = gridProjectHeight;

        int actionTop = _gridProjects.Bottom + 8;

        if (!_viewOnlyMode)
        {
            _lblProjectStatus.Left = sidePadding;
            _lblProjectStatus.Top = actionTop + 8;

            _cboProjectStatus.Left = _lblProjectStatus.Right + 10;
            _cboProjectStatus.Top = actionTop + 4;

            _btnUpdateStatus.Left = _cboProjectStatus.Right + 12;
            _btnUpdateStatus.Top = actionTop + 2;
        }

        int right = contentRight;

        if (_btnDelete.Visible)
        {
            _btnDelete.Left = right - _btnDelete.Width;
            _btnDelete.Top = actionTop + 2;
            right = _btnDelete.Left - spacing;
        }

        if (_btnRefresh.Visible)
        {
            _btnRefresh.Left = right - _btnRefresh.Width;
            _btnRefresh.Top = actionTop + 2;
            right = _btnRefresh.Left - spacing;
        }

        if (_btnCreateTask.Visible)
        {
            _btnCreateTask.Left = right - _btnCreateTask.Width;
            _btnCreateTask.Top = actionTop + 2;
            right = _btnCreateTask.Left - spacing;
        }

        int detailTop = actionTop + detailTopOffsetFromAction;

        _lblProjectSummary.Left = sidePadding;
        _lblProjectSummary.Top = detailTop;

        _txtProjectSummary.Left = sidePadding;
        _txtProjectSummary.Top = _lblProjectSummary.Bottom + 6;
        _txtProjectSummary.Width = contentWidth;
        _txtProjectSummary.Height = summaryHeight;

        _lblEmployeeSection.Left = sidePadding;
        _lblEmployeeSection.Top = _txtProjectSummary.Bottom + 8;

        _gridProjectEmployees.Left = sidePadding;
        _gridProjectEmployees.Top = _lblEmployeeSection.Bottom + 6;
        _gridProjectEmployees.Width = contentWidth;
        _gridProjectEmployees.Height = projectEmployeesHeight;

        _lblTaskSection.Left = sidePadding;
        _lblTaskSection.Top = _gridProjectEmployees.Bottom + 8;

        _gridTasks.Left = sidePadding;
        _gridTasks.Top = _lblTaskSection.Bottom + 6;
        _gridTasks.Width = contentWidth;
        _gridTasks.Height = ClientSize.Height - _gridTasks.Top - 20;

        if (_gridTasks.Height < minTaskHeight)
        {
            _gridTasks.Height = minTaskHeight;
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

    private void LoadFilterEmployees()
    {
        _chkFilterEmployees.Items.Clear();

        List<Employee> employees = _projectController.GetEmployees();
        for (int i = 0; i < employees.Count; i++)
        {
            _chkFilterEmployees.Items.Add(employees[i], false);
        }

        _chkFilterAllEmployees.Checked = true;
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

    private List<Project> GetFilteredProjects(List<Project> allProjects)
    {
        List<Project> filtered = new List<Project>();

        if (_chkFilterAllEmployees.Checked)
        {
            for (int i = 0; i < allProjects.Count; i++)
            {
                filtered.Add(allProjects[i]);
            }

            return filtered;
        }

        List<string> selectedEmployeeIds = new List<string>();
        for (int i = 0; i < _chkFilterEmployees.CheckedItems.Count; i++)
        {
            Employee? employee = _chkFilterEmployees.CheckedItems[i] as Employee;
            if (employee != null)
            {
                selectedEmployeeIds.Add(employee.Id);
            }
        }

        if (selectedEmployeeIds.Count == 0)
        {
            return filtered;
        }

        for (int i = 0; i < allProjects.Count; i++)
        {
            Project project = allProjects[i];
            bool match = false;

            if (project.Employees != null)
            {
                for (int j = 0; j < project.Employees.Count; j++)
                {
                    Employee employee = project.Employees[j];
                    for (int k = 0; k < selectedEmployeeIds.Count; k++)
                    {
                        if (employee.Id == selectedEmployeeIds[k])
                        {
                            match = true;
                            break;
                        }
                    }

                    if (match)
                    {
                        break;
                    }
                }
            }

            if (match)
            {
                filtered.Add(project);
            }
        }

        return filtered;
    }

    private void LoadProjectsToGrid()
    {
        _gridProjects.Rows.Clear();

        List<Project> projects = _projectController.GetProjects();
        List<Project> filteredProjects = GetFilteredProjects(projects);

        for (int i = 0; i < filteredProjects.Count; i++)
        {
            Project project = filteredProjects[i];

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
            LoadProjectDetailForSelectedProject();
            LoadTasksForSelectedProject();
        }
        else
        {
            ClearProjectDetail();
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

    private Project? GetProjectById(string projectId)
    {
        List<Project> projects = _projectController.GetProjects();

        for (int i = 0; i < projects.Count; i++)
        {
            if (projects[i].ProjectId == projectId)
            {
                return projects[i];
            }
        }

        return null;
    }

    private void SyncStatusFromSelectedProject()
    {
        if (_viewOnlyMode)
        {
            return;
        }

        string projectId = GetSelectedProjectId();
        if (string.IsNullOrWhiteSpace(projectId))
        {
            return;
        }

        Project? project = GetProjectById(projectId);
        if (project == null)
        {
            return;
        }

        for (int i = 0; i < _cboProjectStatus.Items.Count; i++)
        {
            object? item = _cboProjectStatus.Items[i];
            if (item != null)
            {
                EnumStatus status = (EnumStatus)item;
                if (status == project.Status)
                {
                    _cboProjectStatus.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    private void LoadProjectDetailForSelectedProject()
    {
        _gridProjectEmployees.Rows.Clear();

        string projectId = GetSelectedProjectId();
        if (string.IsNullOrWhiteSpace(projectId))
        {
            ClearProjectDetail();
            return;
        }

        Project? project = GetProjectById(projectId);
        if (project == null)
        {
            ClearProjectDetail();
            return;
        }

        string leaderInfo = "N/A";
        if (project.Leader != null)
        {
            leaderInfo = project.Leader.Name + " (" + project.Leader.GetRole() + ")";
        }

        string summary = "Project ID: " + project.ProjectId + Environment.NewLine
            + "Project Name: " + project.ProjectName + Environment.NewLine
            + "Status: " + project.Status + " | Leader: " + leaderInfo + Environment.NewLine
            + "Timeline: " + project.StartDate.ToShortDateString() + " - " + project.EndDate.ToShortDateString() + Environment.NewLine
            + "Description: " + project.Description;

        _txtProjectSummary.Text = summary;

        if (project.Employees != null)
        {
            for (int i = 0; i < project.Employees.Count; i++)
            {
                Employee employee = project.Employees[i];
                _gridProjectEmployees.Rows.Add(employee.Id, employee.Name, employee.GetRole());
            }
        }
    }

    private void ClearProjectDetail()
    {
        _txtProjectSummary.Text = "No project selected.";
        _gridProjectEmployees.Rows.Clear();
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

    private void ChkFilterAllEmployees_CheckedChanged(object? sender, EventArgs e)
    {
        bool isAll = _chkFilterAllEmployees.Checked;

        _chkFilterEmployees.Enabled = !isAll;

        if (isAll)
        {
            for (int i = 0; i < _chkFilterEmployees.Items.Count; i++)
            {
                _chkFilterEmployees.SetItemChecked(i, false);
            }
        }

        LoadProjectsToGrid();
    }

    private void ChkFilterEmployees_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        if (_chkFilterAllEmployees.Checked)
        {
            return;
        }

        BeginInvoke(new MethodInvoker(TriggerFilterRefresh));
    }

    private void TriggerFilterRefresh()
    {
        LoadProjectsToGrid();
    }

    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        LoadProjectsToGrid();
    }

    private void BtnCreateProject_Click(object? sender, EventArgs e)
    {
        if (_viewOnlyMode)
        {
            MessageBox.Show("View mode does not allow create project.", "Permission", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

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
        if (_viewOnlyMode)
        {
            MessageBox.Show("View mode does not allow status update.", "Permission", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

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
        if (_viewOnlyMode)
        {
            MessageBox.Show("View mode does not allow creating task.", "Permission", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

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
        LoadProjectDetailForSelectedProject();
        LoadTasksForSelectedProject();
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (_viewOnlyMode)
        {
            MessageBox.Show("View mode does not allow deleting project.", "Permission", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

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
