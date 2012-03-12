
using Gtk;
using System;
using System.Configuration;
using System.Collections.Generic;


public partial class MainWindow : Gtk.Window
{
	protected string AppHomeDirectory;
	protected System.Threading.Thread cThr;	
	protected string CmdShell = string.Empty;	
	protected string PartImage = string.Empty;
		
	
	#region Config
	protected void SaveConfiguration ()
	{				
		Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				
		if (config.AppSettings.Settings["ManualPort"] == null) config.AppSettings.Settings.Add("ManualPort", ManualPort.Active.ToString());
		else config.AppSettings.Settings["ManualPort"].Value = ManualPort.Active.ToString();
		
		if (config.AppSettings.Settings["HostPortNum"] == null) config.AppSettings.Settings.Add("HostPortNum", HostPortNum.Text);
        else config.AppSettings.Settings["HostPortNum"].Value = HostPortNum.Text;

		if (config.AppSettings.Settings["ImageFilePath"] == null) config.AppSettings.Settings.Add("ImageFilePath", ImageFilePath.Text);
        else config.AppSettings.Settings["ImageFilePath"].Value = ImageFilePath.Text;
		
		if (config.AppSettings.Settings["ManualDevName"] == null) config.AppSettings.Settings.Add("ManualDevName", ManualDevName.Active.ToString());
        else config.AppSettings.Settings["ManualDevName"].Value = ManualDevName.Active.ToString();
		
		if (config.AppSettings.Settings["DevWord"] == null) config.AppSettings.Settings.Add("DevWord", DevWord.Text);
        else config.AppSettings.Settings["DevWord"].Value = DevWord.Text;
			
		if (config.AppSettings.Settings["SaveImageFilePath"] == null) config.AppSettings.Settings.Add("SaveImageFilePath", SaveImageFilePath.Text);
        else config.AppSettings.Settings["SaveImageFilePath"].Value = SaveImageFilePath.Text;
		
		if (config.AppSettings.Settings["ManualSaveDevName"] == null) config.AppSettings.Settings.Add("ManualSaveDevName", ManualSaveDevName.Active.ToString());
        else config.AppSettings.Settings["ManualSaveDevName"].Value = ManualSaveDevName.Active.ToString();
		
		if (config.AppSettings.Settings["SaveDevWord"] == null) config.AppSettings.Settings.Add("SaveDevWord", SaveDevWord.Text);
        else config.AppSettings.Settings["SaveDevWord"].Value = SaveDevWord.Text;
		
		if (config.AppSettings.Settings["ManualDelDevName"] == null) config.AppSettings.Settings.Add("ManualDelDevName", ManualDelDevName.Active.ToString());
        else config.AppSettings.Settings["ManualDelDevName"].Value = ManualDelDevName.Active.ToString();
		
		if (config.AppSettings.Settings["DelDevWord"] == null) config.AppSettings.Settings.Add("DelDevWord", DelDevWord.Text);
        else config.AppSettings.Settings["DelDevWord"].Value = DelDevWord.Text;
		
		if (config.AppSettings.Settings["CmdShell"] == null) config.AppSettings.Settings.Add("CmdShell", CmdShell);
        else config.AppSettings.Settings["CmdShell"].Value = CmdShell;
		
		if (config.AppSettings.Settings["Partimage"] == null) config.AppSettings.Settings.Add("Partimage", PartImage);
        else config.AppSettings.Settings["Partimage"].Value = PartImage;
		
		
		config.SaveAs(this.AppHomeDirectory + "/gpartimage-ng.config", ConfigurationSaveMode.Full, true);
	}
	
	protected void LoadConfiguration ()
	{
		if(!System.IO.File.Exists(this.AppHomeDirectory + "/gpartimage-ng.config")) return;
		
		
		var mapper = new ExeConfigurationFileMap { ExeConfigFilename = this.AppHomeDirectory + "/gpartimage-ng.config" };
        
		Configuration config = ConfigurationManager.OpenMappedExeConfiguration(mapper, ConfigurationUserLevel.None);

		if (config.AppSettings.Settings["ManualPort"] != null) 
		{
			ManualPort.Active = bool.Parse(config.AppSettings.Settings["ManualPort"].Value);
			PortFromList.Active = !(bool.Parse(config.AppSettings.Settings["ManualPort"].Value));
		}
		
		if (config.AppSettings.Settings["HostPortNum"] != null) HostPortNum.Text = config.AppSettings.Settings["HostPortNum"].Value;
		
		if (config.AppSettings.Settings["ImageFilePath"] != null) ImageFilePath.Text = config.AppSettings.Settings["ImageFilePath"].Value;
		
		if (config.AppSettings.Settings["ManualDevName"] != null) 
		{
			ManualDevName.Active = bool.Parse(config.AppSettings.Settings["ManualDevName"].Value);
			DevNameFromList.Active = !(bool.Parse(config.AppSettings.Settings["ManualDevName"].Value));
		}
		
		if (config.AppSettings.Settings["DevWord"] != null) DevWord.Text = config.AppSettings.Settings["DevWord"].Value;
		
		if (config.AppSettings.Settings["SaveImageFilePath"] != null) SaveImageFilePath.Text = config.AppSettings.Settings["SaveImageFilePath"].Value;
			
		if (config.AppSettings.Settings["ManualSaveDevName"] != null) 
		{
			ManualSaveDevName.Active = bool.Parse(config.AppSettings.Settings["ManualSaveDevName"].Value);
			SaveDevNameFromList.Active = !(bool.Parse(config.AppSettings.Settings["ManualSaveDevName"].Value));
		}
		
		if (config.AppSettings.Settings["SaveDevWord"] != null) SaveDevWord.Text = config.AppSettings.Settings["SaveDevWord"].Value;
		
		if (config.AppSettings.Settings["ManualDelDevName"] != null) 
		{
			ManualDelDevName.Active = bool.Parse(config.AppSettings.Settings["ManualDelDevName"].Value);
			DelDevNameFromList.Active = !(bool.Parse(config.AppSettings.Settings["ManualDelDevName"].Value));
		}
		
		if (config.AppSettings.Settings["DelDevWord"] != null) DelDevWord.Text = config.AppSettings.Settings["DelDevWord"].Value;
		
		if (config.AppSettings.Settings["CmdShell"] != null)
		{
			this.CmdShell = config.AppSettings.Settings["CmdShell"].Value;
			CfgCmdShell.Text = this.CmdShell;
		}
		
		if (config.AppSettings.Settings["Partimage"] != null)
		{
			this.PartImage = config.AppSettings.Settings["Partimage"].Value;
			CfgPartImage.Text = this.PartImage;
		}
	}
	#endregion
	
	#region Device
	protected void RunCommand (string cmd, Gtk.ProgressBar procProgressBar)
	{		
		string respData;
		
		ShowLogMessage("Исполняемая команда: " + cmd, LogView, null);
		 
		System.Diagnostics.Process proc;
		proc = new System.Diagnostics.Process();
		proc.StartInfo.UseShellExecute = false;    
        proc.StartInfo.RedirectStandardOutput = true; 
		proc.StartInfo.RedirectStandardError = false;
		proc.StartInfo.FileName = CmdShell;
		proc.StartInfo.Arguments = "-c \"" + cmd + "\"";
		try
		{
			proc.Start();
			while ((respData = proc.StandardOutput.ReadLine()) != null)
			{
				ShowLogMessage(respData, LogView, procProgressBar);
			}
			proc.WaitForExit();
			proc.Close();	
		}
		catch
		{
			ShowLogMessage("Ошибка выполнения команды", LogView, null);
		}
	}
	
	
	protected void ClearList (Gtk.ComboBox clrComboBox)
	{	
		int IterCount = 0;
		
		Gtk.TreeIter listIter;
		clrComboBox.Model.GetIterFirst(out listIter);
		do 
		{ 
			IterCount++; 
		}
		while(clrComboBox.Model.IterNext(ref listIter));

		for(int i=0; i<IterCount; i++) clrComboBox.RemoveText(0);	
	}
	
	protected void LoadHostList (Gtk.ComboBox HostList)
	{	
		ClearList(HostList);
		
		try
		{
			string[] hostsArr = System.IO.Directory.GetDirectories("/sys/class/scsi_host");
			foreach(string hostName in hostsArr)
			{
				if(!System.IO.File.Exists(hostName + "/scan")) continue;
				HostList.AppendText(System.IO.Path.GetFileName(hostName));
			}
			HostList.Active = 0;
		}
		catch { }	
	}
	
	protected void LoadDevList (Gtk.ComboBox DeviceList)
	{	
		ClearList(DeviceList);
		
		try
		{
			string[] devArr = System.IO.Directory.GetFiles("/dev");
			foreach(string devPath in devArr)
			{
				string devName = System.IO.Path.GetFileName(devPath);
				if(devName.Length < 3) continue;
				if(devName.Substring(0,2) != "sd") continue;
				DeviceList.AppendText(devName);
			}
			DeviceList.Active = 0;
		}
		catch { }	
	}
	
	protected void LoadDelDevList (Gtk.ComboBox DelDevList)
	{	
		ClearList(DelDevList);
		
		try
		{
			string[] devArr = System.IO.Directory.GetDirectories("/sys/block");
			foreach(string devPath in devArr)
			{
				string devName = System.IO.Path.GetFileName(devPath);
				if(devName.Length < 3) continue;
				if(devName.Substring(0,2) != "sd") continue;
				if(!System.IO.File.Exists(devPath + "/device/delete")) continue;
				DelDevList.AppendText(devName);
			}
			DelDevList.Active = 0;
		}
		catch { }
	}	

	protected void ConnectDevice ()
	{	
		Gdk.Threads.Enter ();
        try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ManualPort.Sensitive = false;		
				HostPortNum.Sensitive = false;		
				PortFromList.Sensitive = false;		
				HostList.Sensitive = false;		
				RefreshHostList.Sensitive = false;
				
				StartScan.Sensitive = false;
				UpackImage.Sensitive = false;
				SaveImageBtn.Sensitive = false;
				DisconnectDeviceBtn.Sensitive = false;
				ApplyConfig.Sensitive = false;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();
		}
		
		string selectedHostName;
		
		if(ManualPort.Active) selectedHostName = "host" + HostPortNum.Text;
		else selectedHostName = HostList.ActiveText;
		
		ShowLogMessage("Начато сканирование [" + selectedHostName + "]", LogView, null);
		
		this.RunCommand("echo 0 0 0 > /sys/class/scsi_host/" + selectedHostName + "/scan", null);
		
		ShowLogMessage("Сканирование завершено [" + selectedHostName + "]", LogView, null);
		
		Gdk.Threads.Enter ();
		try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ManualPort.Sensitive = true;		
				HostPortNum.Sensitive = true;		
				PortFromList.Sensitive = true;		
				HostList.Sensitive = true;		
				RefreshHostList.Sensitive = true;
				
				StartScan.Sensitive = true;
				UpackImage.Sensitive = true;
				SaveImageBtn.Sensitive = true;
				DisconnectDeviceBtn.Sensitive = true;
				ApplyConfig.Sensitive = true;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();
		}
	}
	
	protected void SaveImage ()
	{
		Gdk.Threads.Enter ();
        try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				SaveImageFilePath.Sensitive = false;		
				SaveImageButton.Sensitive = false;		
				ManualSaveDevName.Sensitive = false;		
				SaveDevWord.Sensitive = false;		
				SaveDevNameFromList.Sensitive = false;
				SaveDevList.Sensitive = false;
				RefreshSaveDevList.Sensitive = false;
				
				StartScan.Sensitive = false;
				UpackImage.Sensitive = false;
				SaveImageBtn.Sensitive = false;
				DisconnectDeviceBtn.Sensitive = false;
				ApplyConfig.Sensitive = false;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();
		}
		
		string selDevice;
		
		if(ManualSaveDevName.Active) selDevice = "sd" + SaveDevWord.Text;
		else selDevice = SaveDevList.ActiveText;
		
		ShowLogMessage("Начато создание образа [" + selDevice + "]", LogView, null);
		
		this.RunCommand("partimage-ng save /dev/" + selDevice + " stdout | gzip -c > \"" + SaveImageFilePath.Text + "\"", null); //CreateImageProgress
		
		ShowLogMessage("Создание образа завершено [" + selDevice + "]", LogView, null);
		
		Gdk.Threads.Enter ();
		try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				SaveImageFilePath.Sensitive = true;		
				SaveImageButton.Sensitive = true;		
				ManualSaveDevName.Sensitive = true;		
				SaveDevWord.Sensitive = true;		
				SaveDevNameFromList.Sensitive = true;
				SaveDevList.Sensitive = true;
				RefreshSaveDevList.Sensitive = true;
				
				StartScan.Sensitive = true;
				UpackImage.Sensitive = true;
				SaveImageBtn.Sensitive = true;
				DisconnectDeviceBtn.Sensitive = true;
				ApplyConfig.Sensitive = true;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();
		}
	}
	
	protected void UnpackImage ()
	{	
		Gdk.Threads.Enter ();
        try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ImageFilePath.Sensitive = false;		
				ImageFileOpenButton.Sensitive = false;		
				ManualDevName.Sensitive = false;		
				DevWord.Sensitive = false;		
				DevNameFromList.Sensitive = false;
				DevList.Sensitive = false;
				RefreshDevList.Sensitive = false;
				
				StartScan.Sensitive = false;
				UpackImage.Sensitive = false;
				SaveImageBtn.Sensitive = false;
				DisconnectDeviceBtn.Sensitive = false;
				ApplyConfig.Sensitive = false;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();
		}
		
		string selDevice;
		
		if(ManualDevName.Active) selDevice = "sd" + DevWord.Text;
		else selDevice = DevList.ActiveText;
		
		ShowLogMessage("Начата распаковка образа [" + selDevice + "]", LogView, null);
		
		this.RunCommand("zcat \"" + ImageFilePath.Text + "\" | partimage-ng -u 1 restore stdin /dev/" + selDevice, UnpackImageProgress);
		
		ShowLogMessage("Разпаковка образа завершена [" + selDevice + "]", LogView, null);
		
		Gdk.Threads.Enter ();
		try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ImageFilePath.Sensitive = true;		
				ImageFileOpenButton.Sensitive = true;		
				ManualDevName.Sensitive = true;		
				DevWord.Sensitive = true;		
				DevNameFromList.Sensitive = true;
				DevList.Sensitive = true;
				RefreshDevList.Sensitive = true;
				
				StartScan.Sensitive = true;
				UpackImage.Sensitive = true;
				SaveImageBtn.Sensitive = true;
				DisconnectDeviceBtn.Sensitive = true;
				ApplyConfig.Sensitive = true;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();
		}
	}
	
	protected void DisconnectDevice ()
	{	
		Gdk.Threads.Enter ();
		try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ManualDelDevName.Sensitive = false;		
				DelDevWord.Sensitive = false;		
				DelDevNameFromList.Sensitive = false;		
				DelDevList.Sensitive = false;		
				RefreshDelDevWord.Sensitive = false;
		
				StartScan.Sensitive = false;
				UpackImage.Sensitive = false;
				SaveImageBtn.Sensitive = false;
				DisconnectDeviceBtn.Sensitive = false;
				ApplyConfig.Sensitive = false;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();	
		}

		string selDevice;
		
		if(ManualDelDevName.Active) selDevice = "sd" + DelDevWord.Text;
		else selDevice = DelDevList.ActiveText;
		
		ShowLogMessage("Отключение устройства [" + selDevice + "]", LogView, null);
		
		this.RunCommand("echo 1 > /sys/block/" + selDevice + "/device/delete", null);
		
		ShowLogMessage("Отключение завершено [" + selDevice + "]", LogView, null);
		
		Gdk.Threads.Enter ();
		try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ManualDelDevName.Sensitive = true;		
				DelDevWord.Sensitive = true;		
				DelDevNameFromList.Sensitive = true;		
				DelDevList.Sensitive = true;		
				RefreshDelDevWord.Sensitive = true;
		
				StartScan.Sensitive = true;
				UpackImage.Sensitive = true;
				SaveImageBtn.Sensitive = true;
				DisconnectDeviceBtn.Sensitive = true;
				ApplyConfig.Sensitive = true;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();	
		}		
	}
	
	
	protected void StartAutoSaveOperations ()
	{
		Gdk.Threads.Enter ();
		try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ManualPort.Sensitive = false;		
				HostPortNum.Sensitive = false;		
				PortFromList.Sensitive = false;		
				HostList.Sensitive = false;		
				RefreshHostList.Sensitive = false;
				
				SaveImageFilePath.Sensitive = false;		
				SaveImageButton.Sensitive = false;		
				ManualSaveDevName.Sensitive = false;		
				SaveDevWord.Sensitive = false;		
				SaveDevNameFromList.Sensitive = false;
				SaveDevList.Sensitive = false;
				RefreshSaveDevList.Sensitive = false;
				
				ImageFilePath.Sensitive = false;		
				ImageFileOpenButton.Sensitive = false;		
				ManualDevName.Sensitive = false;		
				DevWord.Sensitive = false;		
				DevNameFromList.Sensitive = false;
				DevList.Sensitive = false;
				RefreshDevList.Sensitive = false;

				ManualDelDevName.Sensitive = false;		
				DelDevWord.Sensitive = false;		
				DelDevNameFromList.Sensitive = false;		
				DelDevList.Sensitive = false;		
				RefreshDelDevWord.Sensitive = false;
				
				StartScan.Sensitive = false;
				UpackImage.Sensitive = false;
				SaveImageBtn.Sensitive = false;
				DisconnectDeviceBtn.Sensitive = false;
				ApplyConfig.Sensitive = false;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();	
		}
		
		ConnectDevice();
		SaveImage();
		DisconnectDevice();
		
		Gdk.Threads.Enter ();
		try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ManualPort.Sensitive = true;		
				HostPortNum.Sensitive = true;		
				PortFromList.Sensitive = true;		
				HostList.Sensitive = true;		
				RefreshHostList.Sensitive = true;
				
				SaveImageFilePath.Sensitive = true;		
				SaveImageButton.Sensitive = true;		
				ManualSaveDevName.Sensitive = true;		
				SaveDevWord.Sensitive = true;		
				SaveDevNameFromList.Sensitive = true;
				SaveDevList.Sensitive = true;
				RefreshSaveDevList.Sensitive = true;
				
				ImageFilePath.Sensitive = true;		
				ImageFileOpenButton.Sensitive = true;		
				ManualDevName.Sensitive = true;		
				DevWord.Sensitive = true;		
				DevNameFromList.Sensitive = true;
				DevList.Sensitive = true;
				RefreshDevList.Sensitive = true;

				ManualDelDevName.Sensitive = true;		
				DelDevWord.Sensitive = true;		
				DelDevNameFromList.Sensitive = true;		
				DelDevList.Sensitive = true;		
				RefreshDelDevWord.Sensitive = true;
				
				StartScan.Sensitive = true;
				UpackImage.Sensitive = true;
				SaveImageBtn.Sensitive = true;
				DisconnectDeviceBtn.Sensitive = true;
				ApplyConfig.Sensitive = true;
				AutoSaveImage.Sensitive = true;
				AutoUnpackImage.Sensitive = true;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();	
		}				
	}	

	protected void StartAutoUnpackOperations ()
	{
		Gdk.Threads.Enter ();
		try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ManualPort.Sensitive = false;		
				HostPortNum.Sensitive = false;		
				PortFromList.Sensitive = false;		
				HostList.Sensitive = false;		
				RefreshHostList.Sensitive = false;
				
				SaveImageFilePath.Sensitive = false;		
				SaveImageButton.Sensitive = false;		
				ManualSaveDevName.Sensitive = false;		
				SaveDevWord.Sensitive = false;		
				SaveDevNameFromList.Sensitive = false;
				SaveDevList.Sensitive = false;
				RefreshSaveDevList.Sensitive = false;
				
				ImageFilePath.Sensitive = false;		
				ImageFileOpenButton.Sensitive = false;		
				ManualDevName.Sensitive = false;		
				DevWord.Sensitive = false;		
				DevNameFromList.Sensitive = false;
				DevList.Sensitive = false;
				RefreshDevList.Sensitive = false;

				ManualDelDevName.Sensitive = false;		
				DelDevWord.Sensitive = false;		
				DelDevNameFromList.Sensitive = false;		
				DelDevList.Sensitive = false;		
				RefreshDelDevWord.Sensitive = false;
				
				StartScan.Sensitive = false;
				UpackImage.Sensitive = false;
				SaveImageBtn.Sensitive = false;
				DisconnectDeviceBtn.Sensitive = false;
				ApplyConfig.Sensitive = false;
				AutoSaveImage.Sensitive = false;
				AutoUnpackImage.Sensitive = false;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();	
		}
		
		ConnectDevice();
		UnpackImage();
		DisconnectDevice();
		
		Gdk.Threads.Enter ();
		try 
		{
        	Gtk.Application.Invoke (delegate 
			{
				ManualPort.Sensitive = true;		
				HostPortNum.Sensitive = true;		
				PortFromList.Sensitive = true;		
				HostList.Sensitive = true;		
				RefreshHostList.Sensitive = true;
				
				SaveImageFilePath.Sensitive = true;		
				SaveImageButton.Sensitive = true;		
				ManualSaveDevName.Sensitive = true;		
				SaveDevWord.Sensitive = true;		
				SaveDevNameFromList.Sensitive = true;
				SaveDevList.Sensitive = true;
				RefreshSaveDevList.Sensitive = true;
				
				ImageFilePath.Sensitive = true;		
				ImageFileOpenButton.Sensitive = true;		
				ManualDevName.Sensitive = true;		
				DevWord.Sensitive = true;		
				DevNameFromList.Sensitive = true;
				DevList.Sensitive = true;
				RefreshDevList.Sensitive = true;

				ManualDelDevName.Sensitive = true;		
				DelDevWord.Sensitive = true;		
				DelDevNameFromList.Sensitive = true;		
				DelDevList.Sensitive = true;		
				RefreshDelDevWord.Sensitive = true;
				
				StartScan.Sensitive = true;
				UpackImage.Sensitive = true;
				SaveImageBtn.Sensitive = true;
				DisconnectDeviceBtn.Sensitive = true;
				ApplyConfig.Sensitive = true;
				AutoSaveImage.Sensitive = true;
				AutoUnpackImage.Sensitive = true;
			});
        } 
		finally
		{
			Gdk.Threads.Leave ();	
		}				
	}
	#endregion
	
	#region Gui
	[Gtk.TreeNode (ListOnly=true)]
    protected class LogTreeNode : Gtk.TreeNode 
	{
	    private string eventDate;		
		private string eventDescr;
			
		[Gtk.TreeNodeValue (Column=0)]
	    public string EventDate { get { return this.eventDate; } }	
	    [Gtk.TreeNodeValue (Column=1)]
	    public string EventDescription { get { return this.eventDescr; } }
			
	    public LogTreeNode (string eDate, string eDescription)
	    {
	   		this.eventDate = eDate;
			this.eventDescr = eDescription;
	    }
    }
	
	protected double GetProgress (string dataLine)
	{	
		string[] dataLineArr; 
		
		dataLineArr = dataLine.Split(' ');		
		if(dataLineArr.Length < 6) 
		{
			dataLineArr = dataLine.Split('	');
			if(dataLineArr.Length < 6) return -1;
		}
		
		try
		{
			return (double.Parse(dataLineArr[2].Replace('.', ','))/100);
		}
		catch
		{
			return -1;
		}
	}
	
	protected void ShowLogMessage (string UserLogMessage, Gtk.NodeView UserLogView, Gtk.ProgressBar PrgBar)
	{
		double logProgress = GetProgress(UserLogMessage);
		if(logProgress != -1 && PrgBar != null)
		{
			Gdk.Threads.Enter ();
			try 
			{
	        	Gtk.Application.Invoke (delegate 
				{
					PrgBar.Fraction = logProgress;
					PrgBar.Text = logProgress.ToString() + "%";
				});
	        } 
			finally
			{
				Gdk.Threads.Leave ();
			}
		}
		else
		{
			Gdk.Threads.Enter ();
			try 
			{
	        	Gtk.Application.Invoke (delegate 
				{
					UserLogView.NodeStore.AddNode(new LogTreeNode(DateTime.Now.ToString(), UserLogMessage));
				});
	        } 
			finally
			{
				Gdk.Threads.Leave ();
			}
		}
	}	
	#endregion
	
	
	public MainWindow () : base(WindowType.Toplevel)
	{	
		this.AppHomeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/.config/GPartimage-ng";
		
		Build ();
		
		LogView.NodeStore = new NodeStore ( typeof(LogTreeNode) );			
		LogView.AppendColumn ("Время", new CellRendererText (), "text", 0);
       	LogView.AppendColumn ("Событие", new CellRendererText (), "text", 1);
		
		CmdShell = "/bin/bash";		
		CfgCmdShell.Text = CmdShell;	
		PartImage = "/usr/sbin/partimage-ng";
		CfgPartImage.Text = PartImage;
			
		LoadHostList(HostList);
		
		if(System.IO.Directory.Exists("/media")) ImageFileOpenButton.SetCurrentFolder("/media");
		
		LoadDevList(DevList);
		LoadDevList(SaveDevList);
		LoadDelDevList(DelDevList);
		
		FileFilter fileFilter;
		fileFilter = new FileFilter();
		fileFilter.Name = "Файл образа .img.gz";
		fileFilter.AddPattern("*.img.gz");
		ImageFileOpenButton.AddFilter(fileFilter);
		fileFilter = new FileFilter();
		fileFilter.Name = "Все файлы";
		fileFilter.AddPattern("*.*");
		ImageFileOpenButton.AddFilter(fileFilter);
		
		LoadConfiguration();
	}
	
	protected void OnWindowDeleteEvent (object sender, DeleteEventArgs a)
	{
		SaveConfiguration();
		
		Application.Quit ();
		a.RetVal = true;
	}
	
	
	protected virtual void OnShowAboutClicked (object sender, System.EventArgs e)
	{
		gpartimageng.AboutWindow awnd = new gpartimageng.AboutWindow();
		awnd.ShowAll();
	}
	
	// Обнаружение НЖМД	
	protected void OnRefreshHostListClicked (object sender, System.EventArgs e)
	{
		LoadHostList(HostList);
	}
	
	protected void OnStartScanClicked (object sender, System.EventArgs e)
	{
		cThr = new System.Threading.Thread( new System.Threading.ThreadStart(ConnectDevice) );
		cThr.IsBackground = true;
		cThr.Start();
	}
	
	// Распаковка образа	
	protected void OnImageFileOpenButtonSelectionChanged (object sender, System.EventArgs e)
	{
		ImageFilePath.Text = ImageFileOpenButton.Filename;	
	}
	
	protected void OnRefreshDevListClicked (object sender, System.EventArgs e)
	{
		LoadDevList(DevList);
	}
	
	protected virtual void UnpackImageClick (object sender, System.EventArgs e)
	{
		cThr = new System.Threading.Thread( new System.Threading.ThreadStart(UnpackImage) );
		cThr.IsBackground = true;
		cThr.Start();
	}
	
	// Создание образа	
	protected void OnSaveImageButtonClicked (object sender, System.EventArgs e)
	{
		FileChooserDialog dlg = new FileChooserDialog (
	    		"Выберите файл для сохранения" , null, FileChooserAction.Save,
	        	"gtk-cancel", ResponseType.Cancel,
	        	"gtk-save", ResponseType.Accept
	    	);
      
      	dlg.SelectMultiple = false;
      	dlg.LocalOnly = true;
      	dlg.Modal = true;
		
		FileFilter fileFilter;
		fileFilter = new FileFilter();
		fileFilter.Name = "Файл образа .img.gz";
		fileFilter.AddPattern("*.img.gz");
		dlg.AddFilter(fileFilter);
		
		if (dlg.Run () == (int)ResponseType.Accept) SaveImageFilePath.Text = dlg.Filename + ".img.gz";
		
      	dlg.Destroy ();
	}
	
	protected void OnRefreshSaveDevListClicked (object sender, System.EventArgs e)
	{
		LoadDevList(SaveDevList);
	}
	
	protected void OnSaveImageBtnClicked (object sender, System.EventArgs e)
	{
		cThr = new System.Threading.Thread( new System.Threading.ThreadStart(SaveImage) );
		cThr.IsBackground = true;
		cThr.Start();
	}
	
	// Отключение НЖМД
	protected void OnRefreshDelDevWordClicked (object sender, System.EventArgs e)
	{
		LoadDelDevList(DelDevList);
	}
	
	protected void OnDisconnectDeviceBtnClicked (object sender, System.EventArgs e)
	{
		cThr = new System.Threading.Thread( new System.Threading.ThreadStart(DisconnectDevice) );
		cThr.IsBackground = true;
		cThr.Start();
	}
	
	// Конфигурация	
	protected virtual void OnApplyConfigClicked (object sender, System.EventArgs e)
	{			
		CmdShell = CfgCmdShell.Text;	
		PartImage = CfgPartImage.Text;	
		
		LogView.NodeStore.AddNode(new LogTreeNode(DateTime.Now.ToString(), "Изменены настройки"));		
	}
	
	// Авто-сохранение
	protected void OnAutoSaveImageClicked (object sender, System.EventArgs e)
	{
		cThr = new System.Threading.Thread( new System.Threading.ThreadStart(StartAutoSaveOperations) );
		cThr.IsBackground = true;
		cThr.Start();	
	}
	
	// Авто-распаковка
	protected void OnAutoUnpackImageClicked (object sender, System.EventArgs e)
	{
		cThr = new System.Threading.Thread( new System.Threading.ThreadStart(StartAutoUnpackOperations) );
		cThr.IsBackground = true;
		cThr.Start();
	}

	// Пользовательские лог-данные	
	protected virtual void OnClearLogClicked (object sender, System.EventArgs e)
	{
		LogView.NodeStore.Clear();
	}

	protected void OnSaveLogClicked (object sender, System.EventArgs e)
	{
		FileChooserDialog dlg = new FileChooserDialog (
	    		"Выберите файл для сохранения" , null, FileChooserAction.Save,
	        	"gtk-cancel", ResponseType.Cancel,
	        	"gtk-save", ResponseType.Accept
	    	);
      
      	dlg.SelectMultiple = false;
      	dlg.LocalOnly = true;
      	dlg.Modal = true;
		
		FileFilter fileFilter;
		fileFilter = new FileFilter();
		fileFilter.Name = "Лог-файл .log";
		fileFilter.AddPattern("*.log");
		dlg.AddFilter(fileFilter);
		fileFilter = new FileFilter();
		fileFilter.Name = "Текстовый документ .txt";
		fileFilter.AddPattern("*.txt");
		dlg.AddFilter(fileFilter);
		
		if (dlg.Run () == (int)ResponseType.Accept) 
		{
			string logText = string.Empty;
			foreach(LogTreeNode logNode in LogView.NodeStore)
			{
				logText += "> " + logNode.EventDate + ": " + logNode.EventDescription + "\n";
			}
			try
			{
				System.IO.File.WriteAllText(dlg.Filename, logText);
			}
			catch
			{
				
			}
      	}
      	dlg.Destroy ();	
	}
}