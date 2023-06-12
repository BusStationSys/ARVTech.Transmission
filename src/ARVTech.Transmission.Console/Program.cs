using ARVTech.Transmission.Engine.UniPayCheck;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    static class Program
    {
        static void Main(string[] args)
        {
            // var filePathSource = @"E:\SistemasWEB\ARVTech\ARVTech.Transmission\ARVTech.Transmission.Console\bin\Contracheque202111.txt";

            var filePathSource = @"E:\SistemasWEB\ARVTech\ARVTech.Transmission\ARVTech.Transmission.Console\bin\";

            var transmissionUniPayCheck = new TransmissionUniPayCheck(
                filePathSource);

            var demonstrativosPagamento = transmissionUniPayCheck.GetDemonstrativosPagamento();

            foreach (var dp in demonstrativosPagamento)
            {
                Console.WriteLine($"Competência: {dp.Competencia}; Matrícula: {dp.Matricula}; Nome: {dp.Nome}.");
            }

            Console.ReadLine();
        }

        //static void Main(string[] args)
        //{
        //    // var filePathSource = @"E:\SistemasWEB\ARVTech\ARVTech.Transmission\ARVTech.Transmission.Console\bin\Contracheque202111.txt";

        //    var filePathSource = @"E:\SistemasWEB\ARVTech\ARVTech.Transmission\ARVTech.Transmission.Console\bin\";

        //    var transmissionUniPayCheck = new TransmissionUniPayCheck(
        //        filePathSource);

        //    var x = transmissionUniPayCheck.GetDemonstrativosPagamento();

        //    //using (var fileStream = File.OpenRead(filePathSource))
        //    //{
        //    //    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
        //    //    {

        //    //    }
        //    //}

        //    //var logFile = File.ReadAllLines(filePathSource);    // OK
        //    ////var lines = new List<string>(logFile);

        //    //foreach (var line in lines)
        //    //{

        //    //}

        //    List<string> allLinesText = File.ReadAllLines(filePathSource).ToList();

        //    //string x = string.Empty;

        //    //using (var openFile = File.OpenText(@"E:\\SistemasWEB\\ARV.ImportAndExport\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\9509_7220_01_fdm.ploc"))
        //    //{
        //    //    if (openFile != null)
        //    //    {
        //    //        var y = openFile.ReadToEnd();

        //    //        Console.WriteLine(y);
        //    //    }

        //    //    openFile.Close();
        //    //}

        //    //List<Result> resultsNew = new List<Result>();

        //    //List<string> stringCsv = new List<string>();

        //    //var filePathSource = @"E:\\SistemasWEB\\ARV.ImportAndExport\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\9509_7220_01_fdm.ploc";
        //    //var linesSource = File.ReadAllLines(filePathSource);

        //    //if (linesSource != null && linesSource.Length > 0)
        //    //    foreach (var line in linesSource)
        //    //    {
        //    //        string newLine = line.Replace('\t', '|');

        //    //        stringCsv.Add(newLine);
        //    //    }

        //    //var filePathDest = @"E:\\SistemasWEB\\ARV.ImportAndExport\\ConsoleApp1\\ConsoleApp1\\bin\\Debug\\Output\\SavedLists.csv";

        //    //if (File.Exists(filePathDest))
        //    //    File.Delete(filePathDest);

        //    //File.WriteAllLines(
        //    //    filePathDest,
        //    //    stringCsv.ToArray());

        //    //var linesDest = File.ReadAllLines(filePathDest);

        //    //bool firstLine = true;

        //    //List<Result> resultsBase = new List<Result>();

        //    //if (linesDest != null && linesDest.Length > 0)
        //    //    foreach (var line in linesDest)
        //    //    {
        //    //        if (firstLine)
        //    //        {
        //    //            firstLine = false;

        //    //            continue;
        //    //        }

        //    //        resultsBase.Add(new Result(line));
        //    //    }

        //    //List<int> sps = resultsBase.OrderBy(rb => rb.CMP)
        //    //    .GroupBy(rb => rb.SP)
        //    //    .Where(x => x.Count() >= 3)
        //    //    .Select(grp => grp.Key)
        //    //    .ToList();

        //    //foreach (var sp in sps)
        //    //{
        //    //    var find = resultsBase.Where(rb => rb.SP == sp).ToList();

        //    //    if (find != null && find.Count > 0)
        //    //    {
        //    //        int count = 0;

        //    //        foreach (var item in find)
        //    //        {
        //    //            if (count == 3)
        //    //                break;
        //    //            else
        //    //            {
        //    //                count++;

        //    //                resultsNew.Add(new Result
        //    //                {
        //    //                    Cruisename = item.Cruisename,
        //    //                    Latitude = item.Latitude,
        //    //                    Longitude = item.Longitude,
        //    //                    CMP = item.CMP,
        //    //                    LineName = item.LineName,
        //    //                    SP = item.SP
        //    //                });
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    //if (File.Exists(filePathDest))
        //    //    File.Delete(filePathDest);

        //    //using (TextWriter tw = new StreamWriter(filePathDest))
        //    //{
        //    //    tw.WriteLine("Linename|SP|CMP|Latitude|Longitude|Cruisename");

        //    //    foreach (var resultNew in resultsNew)
        //    //    {
        //    //        tw.WriteLine(
        //    //            string.Format(
        //    //                CultureInfo.InvariantCulture,
        //    //                "{0}|{1}|{2}|{3}|{4}|{5}", 
        //    //                resultNew.LineName, 
        //    //                resultNew.SP,
        //    //                resultNew.CMP,
        //    //                resultNew.Latitude,
        //    //                resultNew.Longitude,
        //    //                resultNew.Cruisename));
        //    //    }
        //    //}
        //}
    }
}
