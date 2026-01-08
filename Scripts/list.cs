using System;
using System.Drawing;
using System.Windows.Forms;

public class AppSetupForm : Form
{
	private LaunchForm launchForm;
	
	// Form controls:
	Panel container;	// Main form container.
	Panel appsContainer; // This layout contains the apps controls and is a scrollable view (Vertical).
	FlowLayoutPanel bottom_panel; // Bottom panel of the form, this is where the "Add" and "Back" buttons are placed.
	Label label_title;
	Label label_description;
	Button button_add;
	Button button_back;
	
	// Constructor:
	public AppSetupForm(LaunchForm launchForm)
	{
		this.launchForm = launchForm;
		this.FormClosed += new FormClosedEventHandler(Close);
		InitializeComponent();
		InitializeControls();
		
		RefreshAppList();
	}
	
	// Form init:
	private void InitializeComponent()
	{
		this.Text = "ASL - Apps setup";
		this.ClientSize = new Size(768, 512);
		this.StartPosition = FormStartPosition.CenterScreen;
		this.Padding = new Padding(8);
	}
	
	// Form controls init:
	private void InitializeControls()
	{
		// Main layout:
		container = new Panel();
		container.Dock = DockStyle.Fill;
		
		// App list layout:
		appsContainer = new Panel();
		appsContainer.AutoScroll = true;
		appsContainer.Dock = DockStyle.Fill;
		appsContainer.BackColor = Color.White;
		
		// Form title:
		label_title = new Label();
		label_title.Text = "Apps setup";
		label_title.Font = ASL.titleFont;
		label_title.Dock = DockStyle.Top;
		label_title.Height = 32;
		
		// Form description:
		label_description = new Label();
		label_description.Text = "Use this window to build a list with the apps you want to launch.";
		label_description.Font = ASL.textFont;
		label_description.Dock = DockStyle.Top;
		label_description.Height = 32; 
		
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
		
		// Add controls to the form:
		bottom_panel.Controls.Add(button_back);
		bottom_panel.Controls.Add(button_add);
		container.Controls.Add(bottom_panel);
		container.Controls.Add(appsContainer);
		container.Controls.Add(label_description);
		container.Controls.Add(label_title);
		
		this.Controls.Add(container);
	}
	
	public void RefreshAppList()
	{
		// Remove visual controls from list:
		foreach(Control child in appsContainer.Controls)
			child.Dispose();
		appsContainer.Controls.Clear();
		
		// Fill with updated data:
		foreach(App a in Serialization.apps)
		{
			Panel appPanel = a.ToFormControl(this);
			appsContainer.Controls.Add(appPanel);
		}
	}
	
	private void Close(object sender, FormClosedEventArgs e)
	{
		// Go back to launch window:
		
		launchForm.Show();
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
		int currentAppIndex = Serialization.apps.IndexOf(app);
		if(currentAppIndex >= Serialization.apps.Count - 1)
			return;
		
		// Get the index of the app above:
		int aboveAppIndex = currentAppIndex + 1;
		
		// Get actual app data on both indexes:
		App currentApp = Serialization.apps[currentAppIndex];
		App appAbove = Serialization.apps[aboveAppIndex];
		
		// Swap data between indexes:
		Serialization.apps[currentAppIndex] = appAbove;
		Serialization.apps[aboveAppIndex] = currentApp;
		
		RefreshAppList();
	}
	
	public void MoveDown(App app)
	{
		int currentAppIndex = Serialization.apps.IndexOf(app);
		if(currentAppIndex <= 0)
			return;
		
		int belowAppIndex = currentAppIndex - 1;
		
		App currentApp = Serialization.apps[currentAppIndex];
		App appBelow = Serialization.apps[belowAppIndex];
		
		Serialization.apps[currentAppIndex] = appBelow;
		Serialization.apps[belowAppIndex] = currentApp;
		
		RefreshAppList();
	}
}