using System.Collections.Generic;
using System.IO;
using System.Linq;
using FolderWatcher.Models;

namespace FolderWatcher.Worker
{
    public static class FolderContentComparer
    {
        public const string excludeFile = "thumbs.db";

        public static IEnumerable<FileDetails> GetNewFiles(FolderDetails folderDetails, FolderContentSnapshot snapshot)
        {
            var newFiles = new List<FileDetails>();

            var existingFiles = Directory.GetFiles(folderDetails.FolderName, "*", SearchOption.TopDirectoryOnly);

            foreach(var file in existingFiles)
            {
                if(file.ToLower().EndsWith(excludeFile))
                {
                    continue;
                }

                if(snapshot.Files.All(x => x.FileName.ToLower() != file.ToLower()))
                {
                    var fileInfo = new FileInfo(file);
                    newFiles.Add(new FileDetails
                    {
                        FileName = file,
                        ChangeDate = fileInfo.LastWriteTime,
                        FolderId = folderDetails.FolderId
                    });
                }
            }

            return newFiles;
        }
    }
}
