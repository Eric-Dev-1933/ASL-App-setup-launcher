using System;
using System.Drawing;
using System.Windows.Forms;

public class ExceptionForm : Form
{
	private readonly Exception exception;
	private string message;
	
	Label details;
	
	public ExceptionForm(string friendlyMessage, Exception exception)
	{
		this.exception = exception;
		this.message = friendlyMessage;
		
		InitializeComponent();
		InitializeControls();
	}
	
	private void InitializeComponent()
	{
		this.Text = "ASL - Launch exception";
		this.ClientSize = new Size(400, 196);
		this.StartPosition = FormStartPosition.CenterScreen;
		this.FormBorderStyle = FormBorderStyle.FixedSingle;
		this.Padding = new Padding(8);
		this.AutoSize = true;
		this.MinimizeBox = false;
		this.MaximizeBox = false;
	}
	
	private void InitializeControls()
	{
		Label userMessage = new Label();
		userMessage.Text = message;
		userMessage.AutoSize = true;
		userMessage.Dock = DockStyle.Top;
		
		Panel bottom = new Panel();
		bottom.Dock = DockStyle.Bottom;
		bottom.Height = 34;
		
		Button button_show = new Button();
		button_show.Text = "Show more details";
		button_show.Font = ASL.textFont;
		button_show.Dock = DockStyle.Right;
		button_show.Size = new Size(160, 32);
		button_show.Click += ShowDetails;
		
		Button button_ok = new Button();
		button_ok.Text = "Ok";
		button_ok.Font = ASL.textFont;
		button_ok.Dock = DockStyle.Right;
		button_ok.Height = 32;
		button_ok.Click += PressOk;
		
		Panel subpanel = new Panel();
		subpanel.Dock = DockStyle.Fill;
		subpanel.Padding = new Padding(0, 16, 0, 0);
		subpanel.AutoScroll = true;
		subpanel.AutoSize = true;
		subpanel.MaximumSize = new Size(0, 256);
		
		details = new Label();
		details.Text = "\nException details:\n" + exception.ToString();
		details.Visible = false;
		details.AutoSize = true;
		//details.Dock = DockStyle.Top;
		
		subpanel.Controls.Add(details);
		bottom.Controls.Add(button_show);
		bottom.Controls.Add(button_ok);
		
		this.Controls.Add(subpanel);
		this.Controls.Add(bottom);
		this.Controls.Add(userMessage);
	}
	
	private void PressOk (object sender, EventArgs e)
	{
		this.Close();
	}
	
	private void ShowDetails (object sender, EventArgs e)
	{
		Button button = sender as Button;
		
		if(details.Visible)
		{
			details.Visible = false;
			button.Text = "Show more details";
		}
		else
		{
			details.Visible = true;
			button.Text = "Show less";
		}
	}
}