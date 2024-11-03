using System.IO;

namespace Syncronizer.Monitor
{
    public class Syncronizer
    {
        private FileSystemWatcher _watcher = null;
        private readonly string _sourceFolderPath = string.Empty;
        private readonly string _replicaFolderPath = string.Empty;

        public Syncronizer(string souceFolderPath, string replicaFolderPath)
        {
            _sourceFolderPath = souceFolderPath;
            _replicaFolderPath = replicaFolderPath;
            _watcher = new FileSystemWatcher(souceFolderPath);
            _watcher.NotifyFilter  = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName

            // Subscribe events
            _watcher.Created += OnCreatedFile;
            _watcher.Deleted += OnDeleteFile;
            _watcher.Changed += OnChangedFile;
            _watcher.Renamed += OnRenameFile;
            _watcher.Error += OnError;

            // Setting up events
            _watcher.IncludeSubdirectories = true; 
            _watcher.EnableRaisingEvents = true; 

        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            throw new System.NotImplementedException();

            // should error to the log

        }

        private void OnRenameFile(object sender, RenamedEventArgs e)
        {
            throw new System.NotImplementedException();

            // same operation should be done on the target folder
        }

        private void OnChangedFile(object sender, FileSystemEventArgs e)
        {
            throw new System.NotImplementedException();

            // same operation should be done on the target folder

        }

        private void OnDeleteFile(object sender, FileSystemEventArgs e)
        {
            throw new System.NotImplementedException();

            // should trigger delete to a path
        }

        private void OnCreatedFile(object sender, FileSystemEventArgs e)
        {
            throw new System.NotImplementedException();

            //should copy the file 
        }
    }
}
