using MalshinonApp.Models;
using MalshinonApp.Data;
using MalshinonApp.Services;

Console.WriteLine("=== PersonService Test ===");

// יצירת מחלקות בפשטות (בלי Dependency Injection כרגע)
var personRepo = new PersonRepository();
var personService = new PersonService(personRepo);

// ניסיון של יצירה/שליפה
var person = personService.GetOrCreateByName("David");
Console.WriteLine($"ID: {person.Id} | Name: {person.FullName} | Code: {person.SecretCode}");

// ניסיון לשלוף את הקוד שוב לפי שם
var code = personService.GetSecretCodeByName("David");
Console.WriteLine($"Secret code for David: {code}");