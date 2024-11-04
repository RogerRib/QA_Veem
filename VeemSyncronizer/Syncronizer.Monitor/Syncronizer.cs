using Syncronizer.Common;
using System;
using System.IO;
using System.Timers;

namespace Syncronizer.Monitor
{
    public class Syncronizer
    {
        private FileSystemWatcher _watcher = null;
        private LogInfomation _logInfomation = null;
        private Timer _monitoringTimer;
        private readonly string _sourceFolderPath = string.Empty;
        private readonly string _replicaFolderPath = string.Empty;
        private int _monitoringInterval = 5000;

        public Syncronizer(string souceFolderPath, string replicaFolderPath, LogInfomation logInfomation = null, int monitoringInterval = 5000)
        {
            _logInfomation = logInfomation ?? new LogInfomation();
            _monitoringInterval = monitoringInterval;

            if (!Directory.Exists(souceFolderPath))
            {
                _logInfomation.Log($"Directory {souceFolderPath} not exists", LogInfomation.LogLevel.Warning);
                _logInfomation.Log($"Creating derectory {souceFolderPath}");

                try
                {
                    Directory.CreateDirectory(souceFolderPath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (!Directory.Exists(replicaFolderPath))
            {
                _logInfomation.Log($"Directory {replicaFolderPath} not exists", LogInfomation.LogLevel.Warning);
                _logInfomation.Log($"Creating derectory {replicaFolderPath}");

                try
                {
                    Directory.CreateDirectory(replicaFolderPath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            _sourceFolderPath = souceFolderPath;
            _replicaFolderPath = replicaFolderPath;

            _logInfomation.Log($"Source folder path setted to:{_sourceFolderPath}");
            _logInfomation.Log($"Replica folder path setted to:{_replicaFolderPath}");

            _watcher = new FileSystemWatcher(souceFolderPath);
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName;

            // Subscribe events
            _watcher.Created += OnCreatedFile;
            _watcher.Deleted += OnDeleteFile;
            _watcher.Changed += OnChangedFile;
            _watcher.Renamed += OnRenameFile;
            _watcher.Error += OnError;

            // Setting up events
            _watcher.IncludeSubdirectories = true;             

            _monitoringTimer = new Timer(_monitoringInterval);
            _monitoringTimer.Elapsed += OnTimerElapsed;
            _monitoringTimer.AutoReset = true;

            _logInfomation.Log($"Monitor start");
            _monitoringTimer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Temporarily stop monitoring
            _watcher.EnableRaisingEvents = false;

            // Re-enable after a delay
            System.Threading.Thread.Sleep(500); 
            _watcher.EnableRaisingEvents = true;

        }

        public void StopWatcher()
        {
            _logInfomation.Log($"Syncronization will be stopped between source {_sourceFolderPath} and replicata folder {_replicaFolderPath}");
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            _logInfomation.Log($"Syncronization stop between source {_sourceFolderPath} and replicata folder {_replicaFolderPath}");

        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            _logInfomation.Log($"Error message exception:\n{e.GetException()}", LogInfomation.LogLevel.Error);
        }

        private void OnRenameFile(object sender, RenamedEventArgs e)
        {
            string oldTargetPath = e.OldFullPath.Replace(_sourceFolderPath, _replicaFolderPath);
            string newTargetPath = e.FullPath.Replace(_sourceFolderPath, _replicaFolderPath);
            try
            {
                if (Directory.Exists(oldTargetPath))
                {
                    string logMessage = $"Rename folder from {oldTargetPath} to {newTargetPath}";
                    _logInfomation.Log(logMessage);

                    Directory.Move(oldTargetPath, newTargetPath);
                }
                else if (File.Exists(oldTargetPath))
                {
                    string logMessage = $"Rename file from {oldTargetPath} to {newTargetPath}";
                    _logInfomation.Log(logMessage);

                    File.Move(oldTargetPath, newTargetPath);
                }
                else 
                {
                    throw new Exception($"Path not found {oldTargetPath}");
                }
            }
            catch (Exception ex)
            {
                _logInfomation.Log($"Error renaming, message exception \n{ex.Message}", LogInfomation.LogLevel.Error);
            }
        }

        private void OnChangedFile(object sender, FileSystemEventArgs e)
        {
            string targetPath = e.FullPath.Replace(_sourceFolderPath, _replicaFolderPath);
            try
            {
                string logMessage = $"Copy file at {e.FullPath}. Replicate at {targetPath}";
                _logInfomation.Log(logMessage);
                if (File.Exists(e.FullPath))
                {
                    File.Copy(e.FullPath, targetPath, true);
                }
                else
                {
                    throw new Exception($"File not exists on {e.FullPath}");
                }
            }
            catch (Exception ex)
            {
                _logInfomation.Log($"Error chaging {targetPath}, message \n{ex.Message}", LogInfomation.LogLevel.Error);
            }
        }

        private void OnDeleteFile(object sender, FileSystemEventArgs e)
        {
            string targetPath = e.FullPath.Replace(_sourceFolderPath, _replicaFolderPath);
            try
            {
                if (Directory.Exists(targetPath))
                {
                    string logMessage = $"Delete folder at {e.FullPath}. Replicate at {targetPath}";
                    _logInfomation.Log(logMessage);
                    Directory.Delete(targetPath, true); 
                }
                else if (File.Exists(targetPath))
                {
                    string logMessage = $"Delete file at {e.FullPath}. Replicate at {targetPath}";
                    _logInfomation.Log(logMessage);
                    File.Delete(targetPath);
                }
                else
                {
                    throw new Exception($"Path not found {targetPath}");
                }
            }
            catch (Exception ex)
            {
                _logInfomation.Log($"Error deleting file {targetPath}, message \n{ex.Message}", LogInfomation.LogLevel.Error);
            }
        }

        private void OnCreatedFile(object sender, FileSystemEventArgs e)
        {
            string targetPath = e.FullPath.Replace(_sourceFolderPath, _replicaFolderPath);
            try
            {
                if (Directory.Exists(e.FullPath))
                {
                    string logMessage = $"Created folder at {e.FullPath}. Replicate at {targetPath}";
                    _logInfomation.Log(logMessage);
                    Directory.CreateDirectory(targetPath); 
                }
                else if (File.Exists(e.FullPath))
                {
                    string logMessage = $"Copy file at {e.FullPath}. Replicate at {targetPath}";
                    _logInfomation.Log(logMessage);
                    File.Copy(e.FullPath, targetPath, true); 
                }
                else
                {
                    throw new Exception($"Path not found {e.FullPath}");
                }
            }
            catch (Exception ex)
            {
                _logInfomation.Log($"Error creating {targetPath}, message \n{ex.Message}", LogInfomation.LogLevel.Error);
            }

        }
    }
}
