﻿using System.IO;

namespace CodeElements.UpdateSystem.Windows.Patcher.Reversion
{
    internal class RevertMoveFile : IRevertable
    {
        public RevertMoveFile(string sourceFile, string destinationFile)
        {
            SourceFile = sourceFile;
            DestinationFile = destinationFile;
        }

        public RevertMoveFile()
        {
        }

        public string SourceFile { get; set; }
        public string DestinationFile { get; set; }

        public void Revert()
        {
            if (!File.Exists(SourceFile))
                return;
            if (File.Exists(DestinationFile))
                File.Delete(DestinationFile);

            Directory.CreateDirectory(Path.GetDirectoryName(DestinationFile));
            File.Move(SourceFile, DestinationFile);
        }

        public RevertableType Type { get; } = RevertableType.MoveFile;
    }
}