using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

namespace DemoData
{
    class Program
    {
        static void Main(string[] args)
        {

            
            string input = ConfigurationManager.AppSettings["InputFiles"];
            string[] rxFiles = Directory.GetFiles(input);

            foreach (string file in rxFiles)
            {
                var info = new FileInfo(file);
                if (info.Name.Substring(0, 2) == "PB")
                {
                    var testUser = new Faker<Member>()
                        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                        .RuleFor(u => u.LastName, f => f.Name.LastName())
                        .RuleFor(u => u.Gender, f => Gender.Male);

                    var user = testUser.Generate();

                    int counter = 0;
                    string line;

                    using (StreamReader f = new StreamReader(file))
                    {
                        while ((line = f.ReadLine()) != null)
                        {
                            string[] split = line.Split('|');

                            string patientid = split[0];
                            string dob = split[1];
                            string gender = split[2];


                            var startPos = line.IndexOfNth("|", 1);
                            var endPos = line.IndexOfNth("|", 2);


                        }
                    }


                }
                else if (info.Name.Substring(0, 2) == "RX")
                {
                    int counter = 0;
                    string line;

                    using (StreamReader f = new StreamReader(file))
                    {
                        while ((line = f.ReadLine()) != null)
                        {
                            var startPos = line.IndexOfNth("|", 1);
                            var endPos = line.IndexOfNth("|", 2);


                        }
                    }
                }


               
            }
        }
    }

    public static class extensions
    {
        public static int IndexOfNth(this string str, string value, int nth = 1)
        {
            if (nth <= 0)
                throw new ArgumentException("Can not find the zeroth index of substring in string. Must start with 1");
            int offset = str.IndexOf(value);
            for (int i = 1; i < nth; i++)
            {
                if (offset == -1) return -1;
                offset = str.IndexOf(value, offset + 1);
            }
            return offset;
        }
    }
}
