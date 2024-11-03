using Syncronizer.Common;
using System;
using System.IO;

namespace Syncronizer.Monitor
{
    public class Syncronizer
    {
        private FileSystemWatcher _watcher = null;
        LogInfomation _logInfomation = null;
        private readonly string _sourceFolderPath = string.Empty;
        private readonly string _replicaFolderPath = string.Empty;

        public Syncronizer(string souceFolderPath, string replicaFolderPath)
        {
            _sourceFolderPath = souceFolderPath;
            _replicaFolderPath = replicaFolderPath;

            _logInfomation = new LogInfomation();
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
            _watcher.EnableRaisingEvents = true;
            _logInfomation.Log($"Monitor start");
        }

        public void StopWatcher()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
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
