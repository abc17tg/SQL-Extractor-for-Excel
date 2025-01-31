using System;
using ScintillaNET;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class ScintillaPauseUpdatesBlock : IDisposable
    {
        private readonly Scintilla m_scintilla;
        private bool m_disposed = false;

        public ScintillaPauseUpdatesBlock(Scintilla scintilla)
        {
            m_scintilla = scintilla ?? throw new ArgumentNullException(nameof(scintilla));
            try
            {
                // Pause updates and tracking
                m_scintilla.BeginUndoAction();
                m_scintilla.SuspendLayout();  // Suspend layout updates
            }
            catch (Exception ex)
            {
                // If initialization fails, clean up and rethrow
                Dispose();
                throw new InvalidOperationException("Failed to pause Scintilla updates.", ex);
            }
        }

        public void Dispose()
        {
            if (m_disposed) return;

            try
            {
                // Resume updates and tracking
                m_scintilla.ResumeLayout();
                m_scintilla.EndUndoAction();
            }
            catch (Exception ex)
            {
                // Log or handle errors during cleanup
                // (You can replace this with actual logging if needed)
                Console.Error.WriteLine($"Error resuming Scintilla updates: {ex.Message}");
            }
            finally
            {
                m_disposed = true;
            }
        }
    }

}
