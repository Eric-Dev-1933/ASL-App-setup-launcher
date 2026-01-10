# ASL-App-setup-launcher
- Desktop application used to launch predefined sets of applications with support for launch arguments with a single action.
- Focused on improving workflow efficiency by reducing repetitive manual app launches.
- Implemented application grouping and basic UI logic.

How to build
---
Although you can download last build, you can also build youy own code. Go to the "scripts" folder and open windows console there and run the following command:
```
C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe /t:winexe /out:AppSetupLauncher.exe main.cs launch.cs apps_setup.cs app_create.cs app.cs serialization.cs about.cs exception.cs app_setup.cs setup_create.cs
```
The output executable file should show up in the same folder.

How it works
---
<img width="496" height="300" alt="image" src="https://github.com/user-attachments/assets/76cc3042-1004-46b3-9280-4fb14158af40" />

ASL let you build you own list of apps to be launched, we call thess lists 'setups'.
Each app on these list can have:
- A custom name
- Windows based launch parameters (For advanced users)
- Instances count: How many instances should be executed.

Click "View app setup" and the App setup window will show up, there you can create, edit or delete app setups. There is no count limits so you can have as many setups and can be as large as a computer can handle, just dont blow you computer trying to launch thousands of apps.

![asl_0](https://github.com/user-attachments/assets/8437a9f0-a5d4-40a8-96b5-0c51fd9b3e83)
![asl_1](https://github.com/user-attachments/assets/8461c496-cdc5-4c96-b75c-5cee235c5cb1)


