using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

public class AppsPanel : Panel
{
	public AppsPanel()
	{
		this.DoubleBuffered = true;
		this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
	}
	
	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams cp = base.CreateParams;
			cp.ExStyle |= 0x02000000;
			return cp;
		}
	}
}

public class AppSetupForm : Form
{
	[System.Runtime.InteropServices.DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
	private const int WM_SETREDRAW = 11;
	
	private LaunchForm launchForm;
	
	// Form controls:
	Panel container;	// Main form container.
	AppsPanel appsContainer; // This layout contains the apps controls and is a scrollable view (Vertical).
	FlowLayoutPanel bottom_panel; // Bottom panel of the form, this is where the "Add" and "Back" buttons are placed.
	Label label_title;
	Label label_description;
	Button button_add;
	Button button_back;
	Button button_editSetup;
	Button button_deleteSetup;
	ComboBox setupSelection;
	
	// Constructor:
	public AppSetupForm(LaunchForm launchForm)
	{
		this.launchForm = launchForm;
		this.FormClosed += new FormClosedEventHandler(Close);
		this.Activated += OnFocus;		
		this.DoubleBuffered = true;
		
		// Set form properties like text, size, etc.
		InitializeComponent();
		
		// Initialize and add all the visual components.
		InitializeControls();
		
		this.Shown += AfterShow;
	}
	
	// Form init:
	private void InitializeComponent()
	{
		this.Text = "ASL - Apps setup";
		this.ClientSize = new Size(768, 512);
		this.MinimumSize = new Size(768, 512);
		this.FormBorderStyle = FormBorderStyle.FixedSingle;
		this.StartPosition = FormStartPosition.CenterScreen;
		this.Padding = new Padding(8);
		this.MaximizeBox = false;
	}
	
	// Form controls init:
	private void InitializeControls()
	{
		// Main layout:
		container = new Panel();
		container.Dock = DockStyle.Fill;
		
		// App list layout:
		appsContainer = new AppsPanel();
		appsContainer.AutoScroll = true;
		appsContainer.Dock = DockStyle.Fill;
		appsContainer.BackColor = Color.White;
		appsContainer.MaximumSize = new Size(0, 350);
		
		// Form title:
		label_title = new Label();
		label_title.Text = "Apps setup";
		label_title.Font = ASL.titleFont;
		label_title.Dock = DockStyle.Top;
		label_title.Height = 32;
		
		// Form description:
		label_description = new Label();
		label_description.Text = "Any apps shown on the list bellow will be launched when you click 'Launch' on the main window.";
		label_description.Font = ASL.textFont;
		label_description.Dock = DockStyle.Top;
		label_description.Height = 48; 
		
		Panel setupPanel = new Panel();
		setupPanel.Dock = DockStyle.Top;
		setupPanel.Height = 48;
		
		Label selectionLabel = new Label();
		selectionLabel.Text = "App setup: ";
		selectionLabel.Font = ASL.textFont;
		selectionLabel.Top = 4;
		selectionLabel.Width = 80;
		selectionLabel.TextAlign = ContentAlignment.MiddleLeft;
		selectionLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		
		setupSelection = new ComboBox();
		setupSelection.DropDownStyle = ComboBoxStyle.DropDownList;
		setupSelection.Font = ASL.textFont;
		setupSelection.Width = 256;
		setupSelection.Top = 4;
		setupSelection.Left = selectionLabel.Width;
		setupSelection.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		setupSelection.Margin = new Padding(0);

		Button button_createSetup = new Button();
		button_createSetup.Text = "Create setup";
		button_createSetup.Font = ASL.textFont;
		button_createSetup.Size = new Size(128, 32);
		button_createSetup.MaximumSize = new Size(128, 32);
		button_createSetup.Dock = DockStyle.Right;
		button_createSetup.Click += CreateSetup;
		
		button_editSetup = new Button();
		button_editSetup.Text = "Edit setup";
		button_editSetup.Font = ASL.textFont;
		button_editSetup.Size = new Size(128, 32);
		button_editSetup.MaximumSize = new Size(128, 32);
		button_editSetup.Dock = DockStyle.Right;
		button_editSetup.Click += EditSetup;
		
		button_deleteSetup = new Button();
		button_deleteSetup.Text = "Delete setup";
		button_deleteSetup.Font = ASL.textFont;
		button_deleteSetup.Size = new Size(128, 32);
		button_deleteSetup.MaximumSize = new Size(128, 32);
		button_deleteSetup.Dock = DockStyle.Right;
		button_deleteSetup.Click += DeleteSetup;
		
		setupSelection.SelectedIndexChanged += SelectAppSetup;
		
		// Bottom panel: "Add" and "Go back" buttons are added here.
		bottom_panel = new FlowLayoutPanel();
		bottom_panel.FlowDirection = FlowDirection.RightToLeft;
		bottom_panel.Dock = DockStyle.Bottom;
		bottom_panel.Height = 34;
		
		// Add button:
		button_add = new Button();
		button_add.Text = "Add";
		button_add.Font = ASL.textFont;
		button_add.Height = 32;
		button_add.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		button_add.Click += AddApp;
		
		// Go back button:
		button_back = new Button();
		button_back.Text = "Go back";
		button_back.Font = ASL.textFont;
		button_back.Height = 32;
		button_back.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		button_back.Click += GoToLaunch;
		
		// Disable edit and create buttons if no setup is selected.
		if(Serialization.currentSetup == null)
		{
			button_add.Enabled = false;
			button_deleteSetup.Enabled = false;
			button_editSetup.Enabled = false;
		}
		
		// Add controls to the form:
		setupPanel.Controls.Add(selectionLabel);
		setupPanel.Controls.Add(setupSelection);
		setupPanel.Controls.Add(button_createSetup);
		setupPanel.Controls.Add(button_editSetup);
		setupPanel.Controls.Add(button_deleteSetup);
		bottom_panel.Controls.Add(button_back);
		bottom_panel.Controls.Add(button_add);
		container.Controls.Add(bottom_panel);
		container.Controls.Add(appsContainer);
		container.Controls.Add(setupPanel);
		container.Controls.Add(label_description);
		//container.Controls.Add(label_title);
		
		this.Controls.Add(container);
	}
	
	public void SuspendDrawing(Control parent)
	{
		SendMessage(parent.Handle, WM_SETREDRAW, 0, 0);
	}
	
	public void ResumeDrawing(Control parent)
	{
		SendMessage(parent.Handle, WM_SETREDRAW, 1, 0);
		parent.Invalidate(true);
		parent.Update();
	}
	
	public void RefreshAppList()
	{			
		appsContainer.Controls.Clear();
	
		if(Serialization.currentSetup == null)
		{
			Label empty = new Label();
			empty.Text = "Select an app setup above.";
			empty.Font = ASL.textFont;
			empty.TextAlign = ContentAlignment.MiddleCenter;
			empty.Dock = DockStyle.Fill;
			
			appsContainer.Controls.Add(empty);
			return;
		}
		
		if(Serialization.currentSetup.apps.Count <= 0)
		{
			Label empty = new Label();
			empty.Text = string.Format("The selected app setup '{0}' is empty, click 'Add' to add one.", Serialization.currentSetup.name);
			empty.Font = ASL.textFont;
			empty.TextAlign = ContentAlignment.MiddleCenter;
			empty.Dock = DockStyle.Fill;
			
			appsContainer.Controls.Add(empty);
			return;
		}

		// Fill with updated data:
		List<Control> newControls = new List<Control>();
		
		foreach(App a in Serialization.currentSetup.apps)
		{
			newControls.Add(a.ToFormControl(this));
		}
		appsContainer.Controls.AddRange(newControls.ToArray());
	}
	
	private void RefreshComboBox()
	{
		setupSelection.Items.Clear();
		setupSelection.Items.Add("None");
		
		if(Serialization.user_data != null)
		{
			foreach(AppSetup setup in Serialization.user_data)
			{
				setupSelection.Items.Add(setup.name);
			}
			
			if(Serialization.user_data.Count > 0)
				setupSelection.SelectedIndex = Serialization.lastSetupIndex + 1;
			else
				setupSelection.SelectedIndex = 0;
		}
		else
			setupSelection.SelectedIndex = 0;
	}
	
	private void Close(object sender, FormClosedEventArgs e)
	{
		// Go back to launch window:
		
		launchForm.Show();
	}
	
	private void AfterShow(object sender, EventArgs e)
	{		
		SuspendDrawing(appsContainer);
		
		try
		{
			RefreshAppList();
		}
		finally
		{
			ResumeDrawing(appsContainer);
			this.Focus();
		}
	}
	
	private void OnFocus(object sender, EventArgs e)
	{
		setupSelection.Items.Clear();
		setupSelection.Items.Add("None");
		
		if(Serialization.currentSetup == null)
		{
			button_add.Enabled = false;
			button_deleteSetup.Enabled = false;
			button_editSetup.Enabled = false;
		}
		else
		{
			button_add.Enabled = true;
			button_deleteSetup.Enabled = true;
			button_editSetup.Enabled = true;
		}
		
		RefreshComboBox();
		RefreshAppList();
	}
	
	private void GoToLaunch(object sender, EventArgs e)
	{
		// Go back to launch window:
		
		this.Close();
		launchForm.Show();
	}
	
	private void AddApp(object sender, EventArgs e)
	{
		// App creation logic:
		
		// 1. Browse app .exe file.
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		openFileDialog.Filter = "Windows executable (*.exe)|*.exe";
		
		if(openFileDialog.ShowDialog() == DialogResult.OK)
		{
			// 2. Read app info and show it in the "adding an app" form.
			App newApp = new App(openFileDialog.FileName);
			
			CreateAppForm form = new CreateAppForm(this, newApp, false);
			this.Hide();
			form.Show();
		}
	}
	
	public void RemoveApp(App app)
	{
		DialogResult result = MessageBox.Show(
			"You are about to delete an app entry and this cannot be undone, this does not delete the app itslef from you computer.\n\nWould you like to continue?",
			"Caution",
			MessageBoxButtons.YesNo,
			MessageBoxIcon.Question
		);
		
		if(result == DialogResult.Yes)
		{
			Serialization.Remove(app);
			RefreshAppList();
		}
	}
	
	public void MoveUp(App app)
	{
		// Get current app index:
		int currentAppIndex = Serialization.currentSetup.apps.IndexOf(app);
		if(currentAppIndex >= Serialization.currentSetup.apps.Count - 1)
			return;
		
		// Get the index of the app above:
		int aboveAppIndex = currentAppIndex + 1;
		
		// Get actual app data on both indexes:
		App currentApp = Serialization.currentSetup.apps[currentAppIndex];
		App appAbove = Serialization.currentSetup.apps[aboveAppIndex];
		
		// Swap data between indexes:
		Serialization.currentSetup.apps[currentAppIndex] = appAbove;
		Serialization.currentSetup.apps[aboveAppIndex] = currentApp;
		
		RefreshAppList();
	}
	
	public void MoveDown(App app)
	{
		int currentAppIndex = Serialization.currentSetup.apps.IndexOf(app);
		if(currentAppIndex <= 0)
			return;
		
		int belowAppIndex = currentAppIndex - 1;
		
		App currentApp = Serialization.currentSetup.apps[currentAppIndex];
		App appBelow = Serialization.currentSetup.apps[belowAppIndex];
		
		Serialization.currentSetup.apps[currentAppIndex] = appBelow;
		Serialization.currentSetup.apps[belowAppIndex] = currentApp;
		
		RefreshAppList();
	}
	
	public void CreateSetup(object sender, EventArgs e)
	{
		using (CreateSetupForm form = new CreateSetupForm(false))
		{
			DialogResult result = form.ShowDialog();
			return;
		}
	}
	
	public void EditSetup(object sender, EventArgs e)
	{
		using (CreateSetupForm form = new CreateSetupForm(true))
		{
				DialogResult result = form.ShowDialog();
				return;
		}
	}
	
	public void DeleteSetup(object sender, EventArgs e)
	{	
		DialogResult result = MessageBox.Show("You are about to delete an app setup and this cannot be undone.\n\nWould you like to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
		if(result == DialogResult.Yes)
		{
			// Delete app setup:
			Serialization.RemoveSetup();
			setupSelection.SelectedIndex = 0;
		}
		
		RefreshComboBox();
		RefreshAppList();
	}
	
	public void SelectAppSetup(object sender, EventArgs e)
	{
		ComboBox combobox = (ComboBox)sender;
		
		if(combobox.SelectedIndex == 0)
		{
			Serialization.currentSetup = null;
			Serialization.lastSetupIndex = -1;
		}
		else
		{
			if(Serialization.user_data != null)
			{
				Serialization.currentSetup = Serialization.user_data[combobox.SelectedIndex - 1];
				Serialization.lastSetupIndex = Serialization.user_data.IndexOf(Serialization.currentSetup);
			}
			else
				combobox.SelectedIndex = 0;
		}

		if(Serialization.currentSetup == null)
		{
			button_add.Enabled = false;
			button_deleteSetup.Enabled = false;
			button_editSetup.Enabled = false;
		}
		else
		{
			button_add.Enabled = true;
			button_deleteSetup.Enabled = true;
			button_editSetup.Enabled = true;
		}
		
		RefreshAppList();
	}
}