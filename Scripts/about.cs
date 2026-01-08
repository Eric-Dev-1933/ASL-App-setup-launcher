using System;
using System.Drawing;
using System.Windows.Forms;

public class AboutForm : Form
{
	LaunchForm launchForm;
	
	public AboutForm(LaunchForm launchForm)
	{
		this.launchForm = launchForm;
		
		InitializeComponent();
		InitializeControls();
	}
	
	private void InitializeComponent()
	{
		this.Text = "ASL - About";
		this.ClientSize = new Size(640, 546);
		this.StartPosition = FormStartPosition.CenterScreen;
		this.MinimizeBox = false;
		this.MaximizeBox = false;
		this.FormClosed += new FormClosedEventHandler(Close);
		this.Padding = new Padding(8);
		this.FormBorderStyle = FormBorderStyle.FixedSingle;
	}
	
	private void InitializeControls()
	{
		Label content = new Label();
		content.Text = "ASL – App setup launcher is a tool designed for those who use a considerably big amount of software apps while working. We all know how tedious it is to be preparing each app one at a time (specially those that takes forever to boot!), but that changes with ASL." +
			"\n\nYou only need to easily configure an app setup and click ‘Launch apps’ on the main window." +
			"\n\nBut what is an app setup?:\nAn app setup is a list with the apps you want to launch; you can create as many setups as needed." +
			"\n\nHow do I create my first app setup?:\nIts easy, click ‘Go back’ down below or just close this window, then in the main window click ‘View setup data’. There you can find a ‘Create setup’ button, click it and type a name you like for your new app setup. Once you are done, your newly created setup will show up on the setups list next to the create button, select it so you can start adding apps to your setup." +
			"\n\nHow do I add apps to my setup?\nClick the ‘Add’ button at the bottom of the app setup window, you will have to browse your app .exe file, once you find it a little form will show up to confirm your app information, you can edit any information before adding your app if needed." +
			"\n\nWhat is this arguments field?:\nWindows apps can be launched with an arguments string; these arguments are a chain of text that represent settings for the app to be launched. Most apps doesnt need arguments to run so you can leave it empty." +
			"\n\nRight now, ASL only support  windows executable files (.exe)";

		content.Font = ASL.textFont;
		content.Dock = DockStyle.Fill;
	
		Button button_back = new Button();
		button_back.Text = "Go back";
		button_back.Font = ASL.textFont;
		button_back.Height = 32;
		button_back.Dock = DockStyle.Bottom;
		button_back.Click += GoBack;
	
		this.Controls.Add(content);
		this.Controls.Add(button_back);
	}
	
	private void GoBack(object sender, EventArgs e)
	{
		this.Close();
		launchForm.Show();
	}
	
	private void Close(object sender, FormClosedEventArgs e)
	{
		launchForm.Show();
	}
}