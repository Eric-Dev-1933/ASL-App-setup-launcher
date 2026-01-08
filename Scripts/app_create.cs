using System;
using System.Drawing;
using System.Windows.Forms;

public class CreateAppForm : Form
{
	// Pointer to current launch form at runtime:
	private AppSetupForm setupForm;
	
	// Form edition state: If true, it means the user is ediding an app entry, if false, it means the
	// user is creating a new app entry.
	private bool edit = false;
	
	private int currentAppIndex = -1;
	
	// Pointer to current app data:
	private App app;
	
	// Form controls:
	Panel panel_container;
	
	FlowLayoutPanel panel_instances;
	FlowLayoutPanel panel_buttons;
	
	Label label_desc;
	Label label_name;
	Label label_path;
	Label label_args;
	Label label_instances;
	
	TextBox text_name;
	TextBox text_path;
	TextBox text_args;
	TextBox text_instances;
	
	Button button_add;
	Button button_cancel;
	
	// Constructor:
	public CreateAppForm(AppSetupForm setupForm, App app, bool edit)
	{
		this.setupForm = setupForm;
		this.app = app;
		
		this.edit = edit;
		if(this.edit)
			currentAppIndex = Serialization.currentSetup.apps.IndexOf(this.app);
		
		InitializeComponent();
		InitializeElements();
		
		// Add tooltips:
		ToolTip tooltip = new ToolTip();
		tooltip.SetToolTip(text_name, "This is a visual name, its set to the default app name but it can be changed at any time.");
		tooltip.SetToolTip(text_path, "This is the actual app path, make sure you are using the right path, otherwise this app will not launch.");
		tooltip.SetToolTip(text_args, "This is where launch arguments are places,\narguments are used to run apps under specific settings but keep in\nmind that every app may use a specific argument format.\nFeel free to live this blank if you are not sure.");
		tooltip.SetToolTip(text_instances, "This represents the number of times you want this app to be launched. \nWe're not sure why but we think this may come in handy.");
	}
	
	// Form init:
	private void InitializeComponent()
	{
		this.Text = "ASL - Adding an app";
		this.ClientSize = new Size(768, 384);
		this.StartPosition = FormStartPosition.CenterScreen;
		this.FormClosed += new FormClosedEventHandler(Close);
	}

	// Form controls init:
	private void InitializeElements()
	{
		// 1. Create controls:
		
		label_desc = new Label();
		label_desc.Text = "The following app will be added to the launch list:";
		label_desc.Font = ASL.textFont;
		label_desc.Dock = DockStyle.Top;
		
		label_name = new Label();
		label_name.Text = "App name:";
		label_name.Font = ASL.textFont_bold;
		label_name.Dock = DockStyle.Top;
		label_name.TextAlign = ContentAlignment.BottomLeft;
		
		label_path = new Label();
		label_path.Text = "Path:";
		label_path.Font = ASL.textFont_bold;
		label_path.Dock = DockStyle.Top;
		label_path.TextAlign = ContentAlignment.BottomLeft;
		label_path.Height = 40;
		
		label_args = new Label();
		label_args.Text = "Arguments:";
		label_args.Font = ASL.textFont_bold;
		label_args.Dock = DockStyle.Top;
		label_args.TextAlign = ContentAlignment.BottomLeft;
		label_args.Height = 40;
		
		label_instances = new Label();
		label_instances.Text = "Instances:";
		label_instances.Font = ASL.textFont_bold;
		label_instances.TextAlign = ContentAlignment.MiddleLeft;
		
		text_name = new TextBox();
		text_name.Text = app.name;
		text_name.Font = ASL.textFont;
		text_name.Dock = DockStyle.Top;
		
		text_path = new TextBox();
		text_path.Text = app.path;
		text_path.Font = ASL.textFont;
		text_path.Dock = DockStyle.Top;
		text_path.Multiline = true;
		text_path.WordWrap = true;
		text_path.Height = 64;
		
		text_args = new TextBox();
		text_args.Dock = DockStyle.Top;
		text_args.Multiline = true;
		text_args.WordWrap = true;
		text_args.Height = 64;
		text_args.Text = app.args;
		text_args.Font = ASL.textFont;
		
		text_instances = new TextBox();
		text_instances.Text = app.instances.ToString();
		text_instances.Width = 32;
		text_instances.Font = ASL.textFont;
		
		button_add = new Button();
		if(!edit)
			button_add.Text = "Add";
		else
			button_add.Text = "Apply";
		button_add.Click += AddApp;
		button_add.Font = ASL.textFont;
		button_add.Height = 32;
		
		button_cancel = new Button();
		button_cancel.Text = "Cancel";
		button_cancel.Font = ASL.textFont;
		button_cancel.Click += Cancel;
		button_cancel.Height = 32;
		
		// 2. Create panels:
		
		panel_instances = new FlowLayoutPanel();
		panel_instances.Dock = DockStyle.Top;
		panel_instances.FlowDirection = FlowDirection.LeftToRight;
		panel_instances.Height = 30;
		panel_instances.Controls.Add(label_instances);
		panel_instances.Controls.Add(text_instances);
		
		panel_buttons = new FlowLayoutPanel();
		panel_buttons.Dock = DockStyle.Bottom;
		panel_buttons.FlowDirection = FlowDirection.RightToLeft;
		panel_buttons.Height = 34;
		panel_buttons.Controls.Add(button_cancel);
		panel_buttons.Controls.Add(button_add);
		
		panel_container = new Panel();
		panel_container.Dock = DockStyle.Fill;
		panel_container.Padding = new Padding(8);
		panel_container.Controls.Add(panel_instances);
		panel_container.Controls.Add(panel_buttons);
		panel_container.Controls.Add(text_args);
		panel_container.Controls.Add(label_args);
		panel_container.Controls.Add(text_path);
		panel_container.Controls.Add(label_path);
		panel_container.Controls.Add(text_name);
		panel_container.Controls.Add(label_name);
		panel_container.Controls.Add(label_desc);
		
		this.Controls.Add(panel_container);
	}
	
	private void Close(object sender, FormClosedEventArgs e)
	{
		// Go back to launch window:
		
		setupForm.Show();
	}
	
	private void AddApp(object sender, EventArgs e)
	{
		if(edit)
		{
			// Ask for confirmation before making any changes:
			
			DialogResult result = MessageBox.Show(
				"You are about to update an app entry data and any changes you make cannot be undone.\n\nWould you like to continue?",
				"Caution",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);
			
			// Abort operation if no confirmation is granted:
			if(result == DialogResult.No)
				return;
		}
		
		int instances;
		if(int.TryParse(text_instances.Text, out instances))
		{
			// App instance count was validaded as an integer, proceed:
			app.name = text_name.Text;
			app.path = text_path.Text;
			app.args = text_args.Text;
			app.instances = instances;
			
			/*
				New logic should be:
					Serialization.Update and Serialization.Add will check for the selected app setup
					and will add the new app to it and will update the app values if current app belongs
					to the current app setup.
					
					Is user editing an existing app:
					Yes:
						Does the current app exsist in the selected app setup?
						Yes:
							Update app data.
						No:
							Show a 'This app does not belogn to the selected app setup message'.
							Does user want to add it as a new app to current app setup?
							Yes:
								Add the new app data.
							No:
								Close the window.
								Return.
					No:
						Add the new app data.
			*/
			
			if(edit)
			{
				// User is updating an app entry:
				Serialization.Update(currentAppIndex, app);
			}
			else
			{
				// The user is creating a new app entry:
				Serialization.Add(app);
			}
			
			// Return to setup form:
			this.Close();
			setupForm.Show();
			setupForm.RefreshAppList();
		}
		else
		{
			MessageBox.Show("The app instances should be represented in integer numbers, current value it not valid.\nPlease enter a valid numeric value.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}
	}
	
	private void Cancel(object sender, EventArgs e)
	{
		this.Close();
		setupForm.Show();
	}
}