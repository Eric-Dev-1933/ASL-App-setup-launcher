using System;
using System.Collections.Generic;

[Serializable]
public class AppSetup
{
	public string name;
	public List<App> apps;
	
	public AppSetup(string name)
	{
		this.name = name;
		apps = new List<App>();
	}
}