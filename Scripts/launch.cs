using System;
using System.Windows.Forms;
using System.Drawing;

public class LaunchForm : Form
{
	// Main form controls:
	private Label label_title;
	private Button button_launch;
	private Button button_view_list;
	private Button button_about;
	private ComboBox setupSelection;
	
	// Constructor:
	public LaunchForm()
	{
		try
		{
			InitializeComponent();
			InitializeControls();
		} catch(Exception e)
		{
			MessageBox.Show(e.ToString());
		}
	}
	
	// Form init:
	private void InitializeComponent()
	{
		this.Text = "ASL - Launcher";
		this.ClientSize = new Size(480, 256);
		this.StartPosition = FormStartPosition.CenterScreen;
		this.FormBorderStyle = FormBorderStyle.FixedSingle;
		this.Activated += Focus;
	}
	
	// Form controls init:
	private void InitializeControls()
	{
		// Launch button:
		button_launch = new Button();
		button_launch.Text = "Launch apps";
		button_launch.Font = ASL.textFont;
		button_launch.Size = new Size(128, 48);
		button_launch.Location = new Point((this.ClientSize.Width / 2) - (button_launch.Width / 2), (this.ClientSize.Height / 2) - (button_launch.Height / 2));
		button_launch.Click += Launch;
		
		// Title label:
		label_title = new Label();
		label_title.Text = "ASL: App setup launcher";
		label_title.Font = ASL.titleFont;
		label_title.TextAlign = ContentAlignment.MiddleCenter;
		label_title.Size = new Size(this.ClientSize.Width, 48);
		label_title.Location = new Point((this.ClientSize.Width / 2) - (label_title.Width / 2), button_launch.Top - label_title.Height - 16);
		
		// About button:
		button_about = new Button();
		button_about.Text = "About";
		button_about.Font = ASL.textFont;
		button_about.Height = 32;
		button_about.Location = new Point(this.ClientSize.Width - button_about.Width - 8, this.ClientSize.Height - button_about.Height - 8);
		button_about.Click += About;
		
		// View list button:
		button_view_list = new Button();
		button_view_list.Text = "View apps setup";
		button_view_list.Font = ASL.textFont;
		button_view_list.Size = new Size(128, 32);
		button_view_list.Location = new Point(button_about.Left - button_view_list.Width - 8, button_about.Top);
		button_view_list.Click += ViewAppsSetup;
		
		// Setup selection combo box:
		setupSelection = new ComboBox();
		setupSelection.DropDownStyle = ComboBoxStyle.DropDownList;
		setupSelection.Items.Add("None");
		setupSelection.Font = ASL.textFont;
		setupSelection.Size = new Size(196, 32);
		setupSelection.Location = new Point(8, button_about.Top + 4);
		setupSelection.SelectedIndexChanged += SelectSetup;
		
		// Fill combo box options:
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

		// Add controls.
		this.Controls.Add(button_launch);
		this.Controls.Add(label_title);
		this.Controls.Add(button_about);
		this.Controls.Add(button_view_list);
		this.Controls.Add(setupSelection);
	}

	private void Launch(object sender, EventArgs e)
	{
		if(Serialization.user_data == null || Serialization.user_data.Count <= 0)
		{
			DialogResult result = MessageBox.Show("No app setup data has been created yet. Would you like to create one?", "No setup data found.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if(result == DialogResult.Yes)
			{
				// Open app setup creation window.
				ViewAppsSetup(null, null);
			}
			else
				return;
		}
		else
		{
			// Is a setup selected?
			if(Serialization.currentSetup != null)
			{
				// Is current setup empty?
				if(Serialization.currentSetup.apps.Count <= 0)
				{
					MessageBox.Show("The selected app setup is empty, please click on 'View app setup' and add some apps.", "Empty app setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				
				// Start launch sequence.
				foreach(App app in Serialization.currentSetup.apps)
				{
					try 
					{
						app.Launch();
					}
					catch (Exception ex)
					{
						string message = app.name + " failed to launch.\n\nPlease make sure that both path and arguments (if used) are valid and try again.";
						using (ExceptionForm form = new ExceptionForm(message, ex))
						{
							DialogResult result = form.ShowDialog();
							return;
						}
					}
					
					Application.Exit();
				}
			}
			else
			{
				MessageBox.Show("Please select an app setup to launch.", "No app setup selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
		}		
	}
	
	private void ViewAppsSetup(object sender, EventArgs e)
	{
		// Switch to apps view window:

		AppSetupForm form = new AppSetupForm(this);
		form.Show();
		this.Hide();
	}
	
	private void About(object sender, EventArgs e)
	{
		// Show the 'about' section:
		
		AboutForm form = new AboutForm(this);
		form.Show();
		this.Hide();
	}
	
	private void SelectSetup(object sender, EventArgs e)
	{
		// Select an app setup based on selected index.
		
		ComboBox comboBox = (ComboBox)sender;
		
		if(comboBox.SelectedIndex <= 0)
		{
			Serialization.currentSetup = null;
			Serialization.lastSetupIndex = -1;
		}
		else
		{
			Serialization.currentSetup = Serialization.user_data[comboBox.SelectedIndex - 1];
			Serialization.lastSetupIndex = Serialization.user_data.IndexOf(Serialization.currentSetup);
		}
		
		Serialization.SerializeData();
	}
	
	private void Focus(object sender, EventArgs e)
	{
		// Fill combo box options:
		setupSelection.Items.Clear();
		setupSelection.Items.Add("None");
		
		if(Serialization.user_data != null)
		{
			foreach(AppSetup setup in Serialization.user_data)
			{
				setupSelection.Items.Add(setup.name);
			}
			setupSelection.SelectedIndex = Serialization.lastSetupIndex + 1;
		}
		else	
			setupSelection.SelectedIndex = 0;
	}
}