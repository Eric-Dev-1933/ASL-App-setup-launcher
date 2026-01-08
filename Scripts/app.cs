using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

public class CustomButton : Button
{
	public CustomButton()
	{
		this.DoubleBuffered = true;
		this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
		this.UpdateStyles();
	}
}

public class CustomLabel : Label
{
	public CustomLabel()
	{
		this.DoubleBuffered = true;
		this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
		this.UpdateStyles();
	}
}

[Serializable]
public class App
{
	public string name;
	public string path;
	public string args = "";
	public int instances = 1;
	
	public App() {}
	
	public App(string path)
	{
		this.path = path;
		name = Path.GetFileNameWithoutExtension(path);
		
		Console.WriteLine(string.Format("[App created]: name: '{0}', path: '{1}'.", name, path));
	}
	
	public AppsPanel ToFormControl(AppSetupForm form)
	{
		// Main app control panel.
		AppsPanel root = new AppsPanel();
		root.Dock = DockStyle.Top;
		root.Height = 48;
		
		if(Serialization.currentSetup.apps.IndexOf(this) % 2 != 0)
			root.BackColor = Color.FromArgb(255, 255, 255);
		else
			root.BackColor = Color.FromArgb(245, 245, 245);
		root.Padding = new Padding(8);
		
		// Left side of root:
		AppsPanel left = new AppsPanel();
		left.Dock = DockStyle.Fill;
		left.AutoSize = true;
		left.MinimumSize = new Size(0, 32);
		left.BackColor = root.BackColor;
		
		// Right side of root:
		AppsPanel right = new AppsPanel();
		right.Dock = DockStyle.Right;
		right.AutoSize = true;
		right.MinimumSize = new Size(0, 32);
		right.BackColor = root.BackColor;
		
		// Controls:
		
		CustomLabel label_name = new CustomLabel();
		label_name.Text = name;
		label_name.Font = ASL.textFont;
		label_name.Dock = DockStyle.Fill;
		label_name.TextAlign = ContentAlignment.MiddleLeft;
		label_name.BackColor = root.BackColor;
		
		CustomButton button_up = new CustomButton();
		button_up.Text = "▲";
		button_up.Size = new Size(32, 32);
		button_up.Dock = DockStyle.Right;
		button_up.Click += (sender, e) => { form.MoveUp(this); };
		button_up.BackColor = Color.FromArgb(255, 255, 255);;
		
		CustomButton button_down = new CustomButton();
		button_down.Text = "▼";
		button_down.Size = new Size(32, 32);
		button_down.Dock = DockStyle.Right;
		button_down.Click += (sender, e) => { form.MoveDown(this); };
		button_down.BackColor = Color.FromArgb(255, 255, 255);;
		
		CustomButton button_edit = new CustomButton();
		button_edit.Text = "Edit";
		button_edit.Font = ASL.textFont;
		button_edit.Height = 32;
		button_edit.Dock = DockStyle.Right;
		button_edit.Click += (sender, e) => { Edit(form, this); };
		button_edit.BackColor = Color.FromArgb(255, 255, 255);;
		
		CustomButton button_remove = new CustomButton();
		button_remove.Text = "Remove";
		button_remove.Font = ASL.textFont;
		button_remove.Height = 32;
		button_remove.Dock = DockStyle.Right;
		button_remove.Click += (sender, e) => { form.RemoveApp(this); };
		button_remove.BackColor = Color.FromArgb(255, 255, 255);;
				
		left.Controls.Add(label_name);
		
		right.Controls.Add(button_up);
		right.Controls.Add(button_down);
		right.Controls.Add(button_edit);
		right.Controls.Add(button_remove);
		
		root.Controls.Add(left);
		root.Controls.Add(right);
		
		return root;
	}
	
	private void Edit(AppSetupForm setupForm, App app)
	{
		CreateAppForm form = new CreateAppForm(setupForm, app, true);
		form.Show();
		setupForm.Hide();
	}
	
	public void Launch()
	{
		for(int i = 0; i < instances; i++)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.FileName = path;
			startInfo.Arguments = args;
			startInfo.UseShellExecute = false;
			
			Process process = new Process();
			process.StartInfo = startInfo;
			process.Start();
		}
	}
}