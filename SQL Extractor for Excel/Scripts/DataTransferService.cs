using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class DataTransferService : IDisposable
    {
        private readonly MessagePackSerializerOptions m_lz4Options;
        private bool m_disposed = false;

        public DataTransferService()
        {
            // TypelessContractlessStandardResolver allows us to serialize object[] 
            // while preserving the actual underlying types (int, date, etc.)
            var resolver = CompositeResolver.Create(
                TypelessContractlessStandardResolver.Instance,
                StandardResolver.Instance
            );

            // Enable LZ4 Compression (BlockArray is usually best for large data)
            m_lz4Options = MessagePackSerializerOptions.Standard
                .WithResolver(resolver)
                .WithCompression(MessagePackCompression.Lz4BlockArray);
        }

        public async Task SaveSE4EDTAsync(string filePath, SE4EDTData data)
        {
            // Force proprietary extension
            if (!filePath.EndsWith(".se4edt", System.StringComparison.OrdinalIgnoreCase))
            {
                filePath += ".se4edt";
            }

            // FileShare.None prevents other processes from touching it while writing
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            {
                await MessagePackSerializer.SerializeAsync(fs, data, m_lz4Options).ConfigureAwait(false);
            }
        }

        public async Task<SE4EDTData> LoadSE4EDTAsync(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            {
                return await MessagePackSerializer.DeserializeAsync<SE4EDTData>(fs, m_lz4Options).ConfigureAwait(false);
            }
        }

        public async Task SaveSE4EDTToDefaultPathAsync(string fileName, string dbName, string query, DataTable table)
        {
            var data = new SE4EDTData(dbName, query, table);

            // Clean filename
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            string fullPath = Path.Combine(FileManager.FileExportDefaultPath, fileName);
            
            if (!FileManager.EnsureDirectoryExists(FileManager.FileExportDefaultPath))
            {
                return;
            }

            await SaveSE4EDTAsync(fullPath, data);
        }

        public async Task SaveSE4EDTWithFileDialogAsync(string fileName, string dbName, string query, DataTable table, string initialPath = null)
        {
            var data = new SE4EDTData(dbName, query, table);

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            if (initialPath != null && !Directory.Exists(initialPath))
                initialPath = null;

            string fullPath = FileManager.GetPathByDialog(fileName, initialPath ?? FileManager.DownloadsPath, "SE4EDT Files | *.se4edt", ".se4edt");

            // Fix: Stop execution if user cancels the dialog
            if (string.IsNullOrEmpty(fullPath)) return;

            await SaveSE4EDTAsync(fullPath, data);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                }

                m_disposed = true;
            }
        }
    }
}