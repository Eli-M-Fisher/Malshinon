using MalshinonApp.Models;
using MalshinonApp.Data;
using MalshinonApp.Services;

Console.WriteLine("=== Submit a Report ===");

// שלב 1: יצירת מופעים (במקום DI)
var personRepo = new PersonRepository();
var personService = new PersonService(personRepo);

var reportRepo = new ReportRepository();
var reportService = new ReportService(personService, reportRepo);

// שלב 2: קלט מהמשתמש
Console.Write("Enter your identifier (name or secret code): ");
string reporterInput = Console.ReadLine()?.Trim() ?? "";

Console.Write("Enter target identifier (name or secret code): ");
string targetInput = Console.ReadLine()?.Trim() ?? "";

Console.Write("Enter report text: ");
string text = Console.ReadLine()?.Trim() ?? "";

DateTime now = DateTime.Now;

// שלב 3: שליחת הדיווח
reportService.SubmitReport(reporterInput, targetInput, text, now);

Console.WriteLine("Report submitted successfully!");