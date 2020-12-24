using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataConnection.TextConnect
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }

        public static List<string> LoadFile(this string file)
        {
            if (File.Exists(file) == false)
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        /*
            Convertion section.
            Contains methods that are related to [Class]Model convertion 
            from List<string> to List<[Class]Model>.
        */

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(columns[0]);
                p.PlaceNumber = int.Parse(columns[1]);
                p.PlaceName = columns[2];
                p.PrizeAmount = decimal.Parse(columns[3]);
                p.PrizePercentage = double.Parse(columns[4]);

                output.Add(p);
            }

            return output;
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                PersonModel p = new PersonModel();
                p.Id = int.Parse(columns[0]);
                p.FirstName = columns[1];
                p.LastName = columns[2];
                p.EmailAddress = columns[3];
                p.CellphoneNumber = columns[4];

                output.Add(p);
            }

            return output;
        }

        /*
            Output saving section.
            Contains methods that are related to [Class]Model file output processing.
        */

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add(
                    $"{ p.Id }," +
                    $"{ p.PlaceNumber }," +
                    $"{ p.PlaceName }," +
                    $"{ p.PrizeAmount }," +
                    $"{ p.PrizePercentage }"
                    );
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToPersonFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add(
                    $"{ p.Id }," +
                    $"{ p.FirstName }," +
                    $"{ p.LastName }," +
                    $"{ p.EmailAddress }," +
                    $"{ p.CellphoneNumber }"
                    );
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
    }
}
