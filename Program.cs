using MalshinonApp.Models;
using MalshinonApp.Data;
using MalshinonApp.Services;

Console.WriteLine("=== Submit a Report ===");

// שלב 1: יצירת מופעים (במקום DI)
var personRepo = new PersonRepository();
var reportRepo = new ReportRepository();
var alertRepo = new AlertRepository();

var personService = new PersonService(personRepo);
var alertService = new AlertService(alertRepo, reportRepo);
var reportService = new ReportService(personService, reportRepo, alertService);
var analysisService = new AnalysisService(reportRepo, personRepo);

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

Console.WriteLine("✅ Report submitted successfully!");

// === שלב 4: ניתוח ואנליטיקה ===
Console.WriteLine("\n=== Analysis Dashboard ===");

// 1. מגויסים פוטנציאליים
Console.WriteLine("\n🧑‍💼 Potential Recruits:");
var recruits = analysisService.GetPotentialRecruits();
foreach (var p in recruits)
{
    Console.WriteLine($"- {p.FullName} ({p.SecretCode})");
}

// 2. יעדים בסיכון
Console.WriteLine("\n🎯 High-Risk Targets:");
var threats = analysisService.GetHighRiskTargets();
foreach (var p in threats)
{
    Console.WriteLine($"- {p.FullName} ({p.SecretCode})");
}

// 3. התראות קיימות
Console.WriteLine("\n🚨 Alerts:");
var alerts = alertService.GetAllAlerts();
foreach (var a in alerts)
{
    Console.WriteLine($"- Target #{a.TargetId} | {a.Reason} | {a.AlertTimeWindowStart}–{a.AlertTimeWindowEnd}");
}