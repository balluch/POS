﻿using System;
using System.Collections.Generic;
using System.Linq;
using Grouping.Model;
using System.Text.Json;

namespace Grouping
{
    class Program
    {
        private static JsonSerializerOptions serializerOptions
            = new JsonSerializerOptions { WriteIndented = false };
        static void Main(string[] args)
        {

            // *************************************************************************************
            // Schreibe in den nachfolgenden Übungen statt der Zeile
            // List<object> result = null!;
            // die korrekte LINQ Abfrage. Verwende den entsprechenden Datentyp statt object.
            // Du kannst eine "schöne" (also eingerückte) Ausgabe der JSON Daten erreichen, indem
            // du die Variable WriteIndented in Zeile 12 auf true setzt.
            //
            // !!HINWEIS!!
            // Beende deine Abfrage immer mit ToList(), damit die Daten für die JSON Serialisierung
            // schon im Speicher sind. Ansonsten würde es auch einen Compilerfehler geben, da
            // WriteJson() eine Liste haben möchte.
            // *************************************************************************************

            ExamsDb db = ExamsDb.FromFiles("csv");

            {
                var result = db.Exams
                    .GroupBy(e => new { e.TeacherId, e.Teacher.Lastname })
                    .Select(g => new
                    {
                        g.Key.TeacherId,
                        g.Key.Lastname,
                        ExamsCount = g.Count(),
                        ExamDates = g.Select(x => x.Date.ToShortDateString())
                    })
                    .OrderBy(e => e.TeacherId)
                    .Take(3)
                    .ToList();
                Console.WriteLine("MUSTER: Anzahl der Prüfungen und Datumsangaben zu den Prüfungen pro Lehrer (erste 3 Lehrer).");
                WriteJson(result);
            }

            // *************************************************************************************
            // ÜBUNG 1: Wie viele Klassen gibt es pro Abteilung?
            //          Das Ergebnis ist absteigend nach der Anzahl der Klassen zu sortieren.
            //          Bei gleicher Klassenanzahl ist zusätzlich nach dem Abteilungsnamen
            //          aufsteigend zu sortieren. Verwende dafür OrderByDescending() und ThenBy()
            //          mit den entsprechenden Lambda Expressions.
            // *************************************************************************************
            {
                List<object> result = null!;
                Console.WriteLine("RESULT1");
                WriteJson(result);
            }

            // *************************************************************************************
            // ÜBUNG 2: Wie die vorige Übung, allerdings sind nur Abteilungen
            //          mit mehr als 10 Klassen auszugeben.
            //          Das Ergebnis ist absteigend nach der Anzahl der Klassen zu sortieren.
            //          Verwende dafür OrderByDescending() mit der entsprechenden Lambda Expression.
            //          Hinweis: Filtere mit Where nach dem Erstellen der Objekte mit Department
            //                   und Count.
            // *************************************************************************************
            {
                List<object> result = null!;
                Console.WriteLine("RESULT2");
                WriteJson(result);
            }

            // *************************************************************************************
            // ÜBUNG 3: Wann ist der letzte Test (Max von Exam.Date) pro Lehrer und Fach der 5AHIF
            //          in der Tabelle Exams?
            // *************************************************************************************
            {
                List<object> result = null!;
                Console.WriteLine("RESULT3");
                WriteJson(result);
            }

            // *************************************************************************************
            // ÜBUNG 4: Wie viele Klassen sind pro Tag und Stunde (PeriodNr) gleichzeitig im Haus?
            //          Hinweis: Gruppiere zuerst nach Tag und Stunde in Lesson. Für die Ermittlung
            //          der Klassenanzahl zähle die eindeutigen KlassenIDs, indem mit Distinct eine
            //          Liste dieser IDs (Id) erzeugt wird und dann mit Count() gezählt wird.
            //          Es sind mit OrderByDescending(g=>g.ClassCount).Take(6) nur die 6
            //          "stärksten" Stunden auszugeben.
            // *************************************************************************************
            {
                List<object> result = null!;
                Console.WriteLine("RESULT4");
                WriteJson(result);
            }

            // *************************************************************************************
            // ÜBUNG 5: Die 5AHIF möchte wissen, in welchem Monat sie wie viele Tests hat.
            //          Hinweis: Mit den Properties Month und Year kann auf das Monat bzw. Jahr
            //          eines DateTime Wertes zugegriffen werden. Die Ausgabe in DisplayMonth kann
            //          $"{mydate.Year:00}-{mydate.Month:00}" (mydate ist zu ersetzen)
            //          erzeugt werden
            // *************************************************************************************
            {
                List<object> result = null!;
                Console.WriteLine("RESULT5");
                WriteJson(result);
            }

            // *************************************************************************************
            // ÜBUNG 6: Erstelle für jeden Lehrer eine Liste der Fächer, die er unterrichtet. Es
            // sind nur die ersten 10 Datensätze auszugeben. Das kann mit
            // .OrderBy(t=>t.TeacherId).Take(10)
            // *************************************************************************************
            {
                List<object> result = null!;
                Console.WriteLine("RESULT6");
                WriteJson(result);
            }
        }

        public static void WriteJson<T>(List<T> result)
        {
            if (result is not null && typeof(T) == typeof(object))
            {
                Console.WriteLine("Warum erstellst du eine Liste von Elementen mit Typ object?");
                return;
            }
            Console.WriteLine(JsonSerializer.Serialize(result, serializerOptions));
        }
    }
}
