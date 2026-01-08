using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0.0")]
[assembly: AssemblyTitle("ASL - App Setup Launcher")]
[assembly: AssemblyDescription("ASL serves as an asistant that opens all your apps for you with just one click.")]
[assembly: AssemblyCompany("E. Dev")]
[assembly: AssemblyProduct("ASL - App Setup Launcher")]
[assembly: AssemblyCopyright("Copyright © E. Dev")]

public class ASL
{
	public static Font titleFont = new Font("Aptos (Body)", 16, FontStyle.Regular); 
	public static Font textFont = new Font("Aptos (Body)", 10, FontStyle.Regular);
	public static Font textFont_bold = new Font("Aptos (Body)", 10, FontStyle.Bold);
	
	[STAThread]
	static void Main(string[] args)
	{	
		Serialization.DeserializeData();
	
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new LaunchForm());
	}
}