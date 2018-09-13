using System;
using System.Collections.Generic;

namespace FolderWatcher.Models
{
    public class FolderContentSnapshot
    {
        public int Id { get; set; }

        public DateTime SnapshotDate { get; set; }

        public Guid FolderId { get; set; }

        public IList<FileDetails> Files { get; set; }
    }
}
