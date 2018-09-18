using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FolderWatcher.Models;
using LiteDB;

namespace FolderWatcher.Services
{
    public class NoSqlSnapshotService : IFolderContentSnapshotService
    {
        public const string DatabaseName = @"FolderSnapshots.db";
        public const string CollectionName = @"FolderSnapshots";

        public FolderContentSnapshot CreateSnapshot(FolderDetails folder)
        {
            var snapshot = new FolderContentSnapshot();

            var existingFiles = Directory.GetFiles(folder.FolderName, "*", SearchOption.TopDirectoryOnly);

            foreach (var file in existingFiles)
            {                
                var fileInfo = new FileInfo(file);
                snapshot.Files.Add(new FileDetails
                {
                    FileName = file,
                    ChangeDate = fileInfo.LastWriteTime,
                    FolderId = folder.FolderId                    
                });    
            }

            snapshot.FolderId = folder.FolderId;
            snapshot.SnapshotDate = DateTime.Now;            

            return snapshot;
        }

        public FolderContentSnapshot ReadSnapshot(Guid folderId)
        {
            FolderContentSnapshot result = null;
                        
            using (var database = new LiteDatabase(DatabaseName))
            {
                // Open collection 
                var snapshots = database.GetCollection<FolderContentSnapshot>(CollectionName);
                                
                result = snapshots.Find(x => x.FolderId == folderId).SingleOrDefault();
                   
                // create empty snapshot of nothing has been found
                if (result == null)
                {
                    result = new FolderContentSnapshot()
                    {
                        FolderId = folderId,
                        SnapshotDate = DateTime.Now,
                        Files = new List<FileDetails>()
                    };

                    // add newly created snapshot to database to ensure its existance in future
                    snapshots.Insert(result);
                }
            }

            return result;
        }

        public void UpdateSnapshot(FolderContentSnapshot snapshot)
        {
            using (var database = new LiteDatabase(DatabaseName))
            {                
                var snapshots = database.GetCollection<FolderContentSnapshot>(CollectionName);

                snapshots.Update(snapshot);
            }
        }
    }
}
