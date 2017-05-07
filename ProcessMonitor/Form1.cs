using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace ProcessMonitor
{
	public partial class Form1 : Form
	{
		public Form1()
		{ 
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			timer1.Enabled = true;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			String[] names = { "cmd", "explorer", "notepad" };

			for (int i = 0; i < names.Length; i++)
			{
				Label statuslabel = new Label();
				statuslabel.Location = new Point(100, i * 15);
				statuslabel.AutoSize = true;
				statuslabel.Name = names[i];
				this.Controls.Add(statuslabel);
			}

			Process[] processInfo = Process.GetProcesses();
			string[] processNames = new string[processInfo.Length];

			for (int i = 0; i < processInfo.Length; i++)
			{
				processNames[i] = processInfo[i].ProcessName;
			}

			for (int i = 0; i < names.Length; i++)
			{
				int status = System.Array.IndexOf(processNames, names[i]);
				if (status < 0)
				{
					var statuslabel = this.Controls[names[i]];
					statuslabel.Text = "Процесс " + names[i] + " не запущен";
				}
				else
				{
					var statuslabel = this.Controls[names[i]];
					statuslabel.Text = "Процесс " + names[i] + " запущен";
				}

				ProcessStartInfo psiOpt = new ProcessStartInfo(@"cmd.exe", @"/C tasklist /fi ""status eq not responding"" /fi ""imagename eq "+names[i]+".exe ");
				psiOpt.WindowStyle = ProcessWindowStyle.Hidden;
				psiOpt.StandardOutputEncoding = Encoding.GetEncoding(866);
				psiOpt.RedirectStandardOutput = true;
				psiOpt.UseShellExecute = false;
				psiOpt.CreateNoWindow = true;
				Process procCommand = Process.Start(psiOpt);
				StreamReader srIncoming = procCommand.StandardOutput;
				bool b = srIncoming.ReadToEnd().ToString().Contains("Информация: Задачи, отвечающие заданным критериям, отсутствуют.");
				if (b!=true)
				{
					var statuslabel = this.Controls[names[i]];
					statuslabel.Text = "Процесс " + names[i] + " завис";
				}
				
				procCommand.Close();
								
			}
		}		
	}
}
