using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class ExcelExecutionBlock : IDisposable
    {
        private Excel.Application m_app;
        private Excel.XlCalculation m_xlCalculation;
        private bool m_screenUpdating;
        private bool m_events;
        private bool m_interactive;
        public ExcelExecutionBlock(Excel.Application app)
        {
            m_app = app;
            m_screenUpdating = app.ScreenUpdating;
            m_events = app.EnableEvents;
            m_xlCalculation = app.Calculation;
            m_interactive = app.Interactive;

            m_app.ScreenUpdating = false;
            m_app.EnableEvents = false;
            m_app.Calculation = Excel.XlCalculation.xlCalculationManual;
            m_interactive = false;
        }

        public void Dispose()
        {
            m_app.ScreenUpdating = m_screenUpdating;
            m_app.EnableEvents = m_events;
            m_app.Calculation = m_xlCalculation;  
            m_app.Interactive = true;
        }
    }

}
