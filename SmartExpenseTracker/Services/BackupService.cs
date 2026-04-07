using SmartExpenseTracker.Models;
using SmartExpenseTracker.Repositories;

namespace SmartExpenseTracker.Services;

public class BackupService
{
    private readonly DatabaseService _db;
    private readonly IExpenseRepository _expenseRepo;

    public BackupService(DatabaseService db, IExpenseRepository expenseRepo)
    {
        _db = db;
        _expenseRepo = expenseRepo;
    }

    /// <summary>
    /// Copy the SQLite DB file to the device's Downloads / Documents folder.
    /// Returns the destination path.
    /// </summary>
    public async Task<string> BackupAsync()
    {
        var source = _db.GetDatabasePath();
        var dest = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            $"smart_expense_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db3");
        await Task.Run(() => File.Copy(source, dest, overwrite: true));
        return dest;
    }

    /// <summary>
    /// Replace the current DB with the selected backup file.
    /// </summary>
    public async Task RestoreAsync(string backupPath)
    {
        if (!File.Exists(backupPath))
            throw new FileNotFoundException("Backup file not found.", backupPath);

        var dest = _db.GetDatabasePath();
        await Task.Run(() => File.Copy(backupPath, dest, overwrite: true));
    }

    /// <summary>
    /// Export all expenses to an XLSX file using ClosedXML.
    /// </summary>
    public async Task<string> ExportToExcelAsync()
    {
        var expenses = await _expenseRepo.GetAllAsync();
        var filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            $"expenses_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");

        await Task.Run(() =>
        {
            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var ws = workbook.Worksheets.Add("Expenses");

            // Header row
            ws.Cell(1, 1).Value = "Id";
            ws.Cell(1, 2).Value = "Amount";
            ws.Cell(1, 3).Value = "Category";
            ws.Cell(1, 4).Value = "Type";
            ws.Cell(1, 5).Value = "Payment Mode";
            ws.Cell(1, 6).Value = "Date";
            ws.Cell(1, 7).Value = "Notes";

            // Style header
            var header = ws.Range(1, 1, 1, 7);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightBlue;

            // Data rows
            for (int i = 0; i < expenses.Count; i++)
            {
                var e = expenses[i];
                int row = i + 2;
                ws.Cell(row, 1).Value = e.Id;
                ws.Cell(row, 2).Value = (double)e.Amount;
                ws.Cell(row, 3).Value = e.Category;
                ws.Cell(row, 4).Value = e.Type;
                ws.Cell(row, 5).Value = e.PaymentMode;
                ws.Cell(row, 6).Value = e.Date.ToString("yyyy-MM-dd");
                ws.Cell(row, 7).Value = e.Notes;
            }

            ws.Columns().AdjustToContents();
            workbook.SaveAs(filePath);
        });

        return filePath;
    }
}
