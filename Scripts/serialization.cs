using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

public static class Serialization
{
	/*
		New logic should be implemented as follows:
		
		1. A new [serializable] class called 'AppSetup' should be created with the following properties:
			a. Name
			b. List<App>
			
		2. Serialization class should have a static list of app setups.
		
		3. Serialization class will have a static(global) pointer to the selected app setup.
		
		4. Deserialization process:
			Does the save file (user_data.ud) exist?
			Yes:
				Deserialize data.
				Selected app setup remains null until user selects it on the launch window.
			No:
				Create a new and empty list of AppSetup's.
				
		5. Serialization process:
			Does the save file (user_data.ud) exists?
			No:
				Create the file directory.
			Serialize data.
			
		6. 'Add' proccess:
			Has user selected an app setup?
			No:
				Return.
			Yes:
				Add new app to selected app setup.
				Serialize data.
				
		7. 'Update' process:
			Has user selected an app setup?
			No:
				return;
			Yes:
				Does current app exsist in the selected app setup?
				Yes:
					Update app properties.
					Serialize data.
				No:
					Show a 'the current app doesnt exsist in the selected app setup' message.
					Ask user if current app should be added as a new app.
					Yes:
						Add app data as a new app to the selected app setup.
						Serialize data.
					No:
						Show a 'app was not added' message.
						Return.
	*/
	
	// Current user data, this contains every app setup data user has created.
	public static List<AppSetup> user_data = new List<AppSetup>();
	
	// Current setup selection.
	public static AppSetup currentSetup;
	
	public static int lastSetupIndex = 0;
	
	// Default setup path:
	public static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/ASL/user_data.data";
	
	public static void AddSetup(AppSetup setup)
	{
		if(user_data == null)
		{
			user_data = new List<AppSetup>();
			user_data.Add(setup);
		}
		else
		{
			user_data.Add(setup);
		}
		
		currentSetup = setup;
		lastSetupIndex = user_data.IndexOf(setup);
		SerializeData();
	}
	
	public static void EditSetup(string name)
	{
		currentSetup.name = name;
		SerializeData();
	}
	
	public static void RemoveSetup()
	{		
		if(user_data.Contains(currentSetup))
		{
			user_data.Remove(currentSetup);
			currentSetup = null;
			lastSetupIndex = 0;
			
			SerializeData();
		}
	}
	
	public static void Add(App app)
	{
		if(currentSetup == null)
			return;
		
		currentSetup.apps.Add(app);
		SerializeData();
	}
	
	public static void Update(int index, App app)
	{
		// Return if no app setup was selected before.
		if(currentSetup == null)
			return;
		
		if(currentSetup.apps.Contains(app))
		{
			currentSetup.apps[index] = app;
			SerializeData();
		}
		else
		{
			string message = string.Format("The app entry you are currently editing is not part of the selected app setup '{0}'.\n\nWould you like to addit as a new app to the current app setup?", currentSetup.name);
			DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			
			if(result == DialogResult.OK)
				Add(app);
			else
			{
				MessageBox.Show("The app was not added.");
				return;
			}
		}
	}
	
	public static void Remove(App app)
	{
		if(currentSetup == null)
			return;
		
		currentSetup.apps.Remove(app);
		SerializeData();
	}
	
	public static void SerializeData()
	{
		// Create user data directory if doesn't exsist:
		if(!Directory.Exists(path))
			Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/ASL/");

		using (FileStream fs = new FileStream(path, FileMode.Create))
		{
			using (BinaryWriter  bw = new BinaryWriter (fs))
			{
				// Write curren user setups count.
				bw.Write(user_data.Count);
				
				// Write each setup data.
				foreach(AppSetup setup in user_data)
				{
					// Write current setup name.
					bw.Write(setup.name);
					
					// Write apps count in current setup.
					bw.Write(setup.apps.Count);
					
					// Write current setup apps data.
					foreach(App app in setup.apps)
					{
						bw.Write(app.name);
						bw.Write(app.path);
						bw.Write(app.args);
						bw.Write(app.instances);
					}
				}
				
				bw.Write(lastSetupIndex);
			}
		}
	}	
	
	public static void DeserializeData()
	{
		if(!File.Exists(path))
		{
			// App setup file doesnt exsist, set deserialization state to false;
			return;
		}
		
		using (FileStream fs = new FileStream(path, FileMode.Open))
		{
			using (BinaryReader reader = new BinaryReader(fs))
			{
				// Clear data in curren user data:
				user_data = new List<AppSetup>();
				
				// Fill user_data with updated data:
				
				// Get setup count:
				int setupCount = reader.ReadInt32();
				
				// Read setup data:
				for(int i = 0; i < setupCount; i++)
				{
					// Read setup name:
					AppSetup setup = new AppSetup(reader.ReadString());
					
					// Read apps data:
					int appsCount = reader.ReadInt32();
					for(int j = 0; j < appsCount; j++)
					{
						App app = new App();
						
						app.name = reader.ReadString();
						app.path = reader.ReadString();
						app.args = reader.ReadString();
						app.instances = reader.ReadInt32();
						
						setup.apps.Add(app);
					}
					
					user_data.Add(setup);
				}
				
				lastSetupIndex = reader.ReadInt32();
			}
		}
	}
	
	public static void DebugSetup()
	{
		
	}
}