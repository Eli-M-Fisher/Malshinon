using MalshinonApp.Models;

namespace MalshinonApp.Services
{
    public interface ICSVImportService
    {
        /// <summary>
        /// Imports person and report data from a CSV file.
        /// </summary>
        /// <param name="filePath">The path to the CSV file to import.</param>
        /// <returns>A list of imported reports.</returns>
        List<Report> ImportReportsFromCsv(string filePath);
    }
}