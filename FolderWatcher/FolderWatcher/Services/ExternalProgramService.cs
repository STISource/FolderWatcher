using System.Diagnostics;
using System.IO;

namespace FolderWatcher.Services
{
    public class ExternalProgramService : IExternalProgramService
    {
        public void OpenWinExplorerWithFileSelected(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            
            filePath = Path.GetFullPath(filePath);
            Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));            
        }
    }
}
