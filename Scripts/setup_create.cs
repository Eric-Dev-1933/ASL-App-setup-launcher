using System;
using System.Drawing;
using System.Windows.Forms;

public class CreateSetupForm : Form
{
	private bool edit = false;
	
	Label label_title;
	TextBox text_name;
	Button button_ok;
	Button button_cancel;
	
	public CreateSetupForm(bool edit)
	{
		this.edit = edit;
		
		InitializeComponent();
		InitializeControls();
	}
	
	public void InitializeComponent()
	{
		if(!edit)
			this.Text = "ASL - Creating app setup";
		else
			this.Text = "ASL - Editing app setup";
		
		this.ClientSize = new Size(400, 196);
		this.StartPosition = FormStartPosition.CenterScreen;
		this.FormBorderStyle = FormBorderStyle.FixedSingle;
		this.Padding = new Padding(8);
		this.AutoSize = true;
		this.MinimizeBox = false;
		this.MaximizeBox = false;
	}
	
	public void InitializeControls()
	{
		label_title = new Label();
		label_title.Text = "New app setup name:";
		label_title.Font = ASL.textFont;
		label_title.Dock = DockStyle.Top;
		
		text_name = new TextBox();
		text_name.Font = ASL.textFont;
		
		if(!edit)
		{
			if(Serialization.user_data != null)
				text_name.Text = "New app setup " + Serialization.user_data.Count;
			else
				text_name.Text = "New app setup";
		}
		else
		{
			text_name.Text = Serialization.currentSetup.name;
		}
		
		text_name.Dock = DockStyle.Top;
		
		FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
		buttonPanel.FlowDirection = FlowDirection.RightToLeft;
		buttonPanel.Dock = DockStyle.Bottom;
		buttonPanel.Height = 34;
		
		button_ok = new Button();
		if(!edit)
			button_ok.Text = "Ok";
		else
			button_ok.Text = "Apply changes";
		button_ok.Font = ASL.textFont;
		button_ok.Height = 32;
		button_ok.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		button_ok.Click += Create;
		
		button_cancel = new Button();
		button_cancel.Text = "Cancel";
		button_cancel.Font = ASL.textFont;
		button_cancel.Height = 32;
		button_cancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
		button_cancel.Click += Cancel;
		
		buttonPanel.Controls.Add(button_ok);
		buttonPanel.Controls.Add(button_cancel);
		this.Controls.Add(buttonPanel);
		this.Controls.Add(text_name);
		this.Controls.Add(label_title);
	}
	
	public void Create(object sender, EventArgs e)
	{
		if(text_name.Text == "")
		{
			MessageBox.Show("App setup name cannot be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}
		
		if(!edit)
		{
			AppSetup newSetup = new AppSetup(text_name.Text);
			Serialization.AddSetup(newSetup);
		}
		else
		{
			Serialization.EditSetup(text_name.Text);
		}
		
		this.Close();
	}
	
	public void Cancel(object sender, EventArgs e)
	{
		this.Close();
	}
}