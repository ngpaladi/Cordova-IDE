﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Drawing;

namespace IDE_cordova
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private Project CurrentProject;
        public Session CurrentSession;
        private bool prevSave;
        FileStream fileStream;
        private bool justOpen;

        public MainWindow()
        {
            InitializeComponent();
            CurrentSession = new Session();
            CurrentProject = new Project();
            newProject.Click += NewProject_Click;
            openProject.Click += OpenProject_Click;
            buildWindows.Click += BuildWindows_Click;
            buildAndroid.Click += BuildAndroid_Click;
            updateCordova.Click += UpdateCordova_Click;
            openFile.Click += Open_Executed;
            saveFile.Click += Save_Executed;
            Editor.TextChanged += Editor_TextChanged;
            Editor.AcceptsTab = true;
            Editor.AcceptsReturn = true;
            prevSave = false;
            justOpen = false;
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            //txtUpdate();
        }
        /*private void txtUpdate() {
            if (justOpen)
            {
                return;
            }
            else
            {
                
                Editor.Foreground = System.Windows.Media.Brushes.White;
                Match m = Regex.Match(doc.ToString(), "</html>");
                m.NextMatch();
                if (m.Success)
                    try
                    {
                        foreach (string line in Editor.Lines())
                        {
                            if (line.Contains("<") && line.Contains(">"))
                            {
                                Editor.Select();
                                Editor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Yellow);
                            }
                            startIndex += line.Length + 1;
                        }
                    }
                    catch
                    { }
                else
                    return;
            }
        }*/

        public void submitCMD(string pgm, string args, string dir,  bool show)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            if (!show)
            {
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            else
            {
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            }
            startInfo.FileName = pgm;
            startInfo.WorkingDirectory = dir;
            startInfo.Arguments = args;
            process.StartInfo = startInfo;
            process.Start();
           // Process.Start(pgm, args);

        }

        public void submitCMD(string pgm, string args, bool show)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            if (!show)
            {
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            else
            {
                startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            }
            startInfo.FileName = pgm;
            startInfo.Arguments = args;
            process.StartInfo = startInfo;
            process.Start();
            // Process.Start(pgm, args);

        }

        private void UpdateCordova_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            submitCMD("npm", "install -g cordova@latest", true);
            //Process.Start("npm", "install -g cordova@latest");
        }

        private void BuildAndroid_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            submitCMD("cordova", "platform add android", CurrentProject.getBuildPath(), true);
            submitCMD("cordova", "build android", CurrentProject.getBuildPath(), true);
            CurrentProject.buildUpdate();
        }

        private void BuildWindows_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            submitCMD("cordova", "platform add windows", CurrentProject.getBuildPath(), true);
            submitCMD("cordova", "build windows", CurrentProject.getBuildPath(), true);
            CurrentProject.buildUpdate();
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            string path;
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "c:\\Users\\"+Environment.UserName+"\\Documents\\";
            file.Filter = "cdp files (*.cdp)|*.cdp|All files (*.*)|*.*";
            file.FilterIndex = 2;
            file.RestoreDirectory = true;

            DialogResult result = file.ShowDialog();
            
                path = file.FileName;
                string[] readText = File.ReadAllLines(path, Encoding.UTF8);
                CurrentProject.loadProject(readText[0], readText[1], readText[2], readText[3], path);
            justOpen = true;
            fileStream = new FileStream(CurrentProject.getBuildPath() + "\\www\\index.html", FileMode.Open);
            TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            range.Load(fileStream, System.Windows.Forms.DataFormats.Text);
            prevSave = true;
            CurrentSession.setSessionPath(CurrentProject.getBuildPath() + "\\www\\");
            justOpen = false;
            //txtUpdate();

        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            string name = "NewProject";
            string path = "c:\\Users\\" + Environment.UserName + "\\Documents\\";

            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = "c:\\Users\\" + Environment.UserName + "\\Documents\\";

            DialogResult result = folderDialog.ShowDialog();
            path = folderDialog.SelectedPath;

            //System.Windows.Forms.MessageBox.Show(path);

            name = Microsoft.VisualBasic.Interaction.InputBox("Enter project name:", "Cordova", "NewProject", 200, 200);
            name.Replace(' ', '_');
            //Project CurrentProject = new Project();
            CurrentProject.makeNewProject(name, path);
            submitCMD("cordova", CurrentProject.getCreateArgs(), CurrentProject.getPath(), true);
            System.Threading.Thread.Sleep(5000);
            string[] cdpText = { CurrentProject.getName(), CurrentProject.getPath(), CurrentProject.getCreateTimeStr(), CurrentProject.getBuildTimeStr()  };
            System.Threading.Thread.Sleep(5000);
            justOpen = true;
            File.WriteAllLines(CurrentProject.getCdpPath(), cdpText, Encoding.UTF8);
             fileStream = new FileStream(CurrentProject.getBuildPath() + "\\www\\index.html", FileMode.Open);
            TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            range.Load(fileStream, System.Windows.Forms.DataFormats.Text);
            prevSave = true;
            justOpen = false;
            //txtUpdate();


        }
        private void Open_Executed(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "HTML (*.html)|*.html|CSS(*.css) | *.css |Javascript(*.js) | *.js | All files (*.*)|*.*";
            dlg.InitialDirectory = CurrentSession.getSessionPath();
            DialogResult result = dlg.ShowDialog();
            justOpen = true;
             fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
                range.Load(fileStream, System.Windows.Forms.DataFormats.Text);
            prevSave = true;
            justOpen = false;
            //txtUpdate();
            
        }

        private void Save_Executed(object sender, RoutedEventArgs e)
        {
            if (!prevSave) { 
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "HTML (*.html)|*.html|CSS(*.css) | *.css |Javascript(*.js) | *.js | All files (*.*)|*.*";
                dlg.InitialDirectory = CurrentSession.getSessionPath();
                DialogResult result = dlg.ShowDialog();
                fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
                range.Save(fileStream, System.Windows.Forms.DataFormats.Text);
            }
            else
            {
                TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
                range.Save(fileStream, System.Windows.Forms.DataFormats.Text);
            }

        }
       /* public void syntaxHighlight()
        {
            Match m = Regex.Match(doc.ToString(), "<");
            Match n = Regex.Match(doc.ToString(), ">");

            while (n.Success)
            {
                TextPointer t = doc.ContentStart;
                t.GetPositionAtOffset(m.Index, LogicalDirection.Forward);
                n = n.NextMatch();
                TextPointer t2 = doc.ContentStart;
                t2.GetPositionAtOffset(n.Index, LogicalDirection.Forward);
                if (n.Success)
                {
                    t2 = t2.GetPositionAtOffset(n.Index, LogicalDirection.Forward);
                }
                else
                {
                    t2 = Editor.Document.ContentEnd;
                }
                doc.
                TextRange tokenTextRange = new TextRange(t, t2);
                tokenTextRange.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Yellow);
            }
            
        }*/
 
        /* private void Image_Click(object sender, RoutedEventArgs e)
         {
             //throw new NotImplementedException();
             Process.Start("https://cordova.apache.org/");
         }*/

    }
}

