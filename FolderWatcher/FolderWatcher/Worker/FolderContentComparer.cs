using System.Collections.Generic;
using System.IO;
using System.Linq;
using FolderWatcher.Models;

namespace FolderWatcher.Worker
{
    public static class FolderContentComparer
    {
        private static string[] excludedFiles = { "thumbs.db", ".ppinfocache", "desktop.ini" };

        public static IEnumerable<FileDetails> GetNewFiles(FolderContentSnapshot newSnapshot, FolderContentSnapshot oldSnapshot)
        {
            var newFiles = new List<FileDetails>();            

            foreach(var file in newSnapshot.Files)
            {
                var fileNameOnly = file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1).ToLower();

                if (excludedFiles.Contains(fileNameOnly))
                {
                    continue;
                }

                if(oldSnapshot.Files.All(x => x.FileName.ToLower() != file.FileName.ToLower()))
                {                    
                    newFiles.Add(new FileDetails
                    {
                        FileName = file.FileName,
                        ChangeDate = file.ChangeDate,
                        FolderId = newSnapshot.FolderId
                    });
                }
            }

            return newFiles;
        }

        public static IEnumerable<FileDetails> GetDeletedFiles(FolderContentSnapshot newSnapshot, FolderContentSnapshot oldSnapshot)
        {
            var deletedFiles = new List<FileDetails>();

            foreach (var file in oldSnapshot.Files)
            {
                var fileNameOnly = file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1).ToLower();

                if (excludedFiles.Contains(fileNameOnly))
                {
                    continue;
                }

                if (newSnapshot.Files.All(x => x.FileName.ToLower() != file.FileName.ToLower()))
                {                    
                    deletedFiles.Add(new FileDetails
                    {
                        FileName = file.FileName,
                        ChangeDate = file.ChangeDate,
                        FolderId = newSnapshot.FolderId
                    });
                }
            }

            return deletedFiles;
        }
    }
}
