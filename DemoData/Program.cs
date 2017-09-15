using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
            Hashtable Ids = new Hashtable();
            
            string input = ConfigurationManager.AppSettings["InputFiles"];
            string output = ConfigurationManager.AppSettings["OutputFiles"];
            string[] rxFiles = Directory.GetFiles(input);

            foreach (string file in rxFiles)
            {
                var info = new FileInfo(file);
                var fname = info.Name.Substring(0, 2) + "01" + info.Name.Substring(5);

                switch (info.Name.Substring(0, 2))
                {
                    case "PB":
                    {
                        int counter = 0;

                        using (StreamReader f = new StreamReader(file))
                        {
                            using (StreamWriter writer = new StreamWriter(output + @"\" + fname))
                            {
                                string line;
                                while ((line = f.ReadLine()) != null)
                                {
                                    if (counter > 0)
                                    {
                                        string[] split = line.Split('|');

                                        var patientid = split[0];
                                        var dob = Convert.ToDateTime(split[1]);
                                        var gender = split[2].Replace("\"", "");
                                        dob = dob.AddDays(RandomNumber(-2, 2));

                                        var birthday = dob.ToString(format: "M/dd/yyyy");

                                        var m = new Member();

                                        switch (gender)
                                        {
                                            case "M":
                                                var testUser = new Faker<Member>()
                                                    .RuleFor(u => u.NewId, x => x.Random.Replace("1##########"))
                                                    .RuleFor(u => u.FirstName, x => x.Name.FirstName())
                                                    .RuleFor(u => u.LastName, x => x.Name.LastName())
                                                    .RuleFor(u => u.Gender, x => Gender.Male);
                                                m = testUser.Generate();
                                                break;
                                            case "F":
                                                var femaleUser = new Faker<Member>()
                                                    .RuleFor(u => u.NewId, x => x.Random.Replace("1##########"))
                                                    .RuleFor(u => u.FirstName, x => x.Name.FirstName())
                                                    .RuleFor(u => u.LastName, x => x.Name.LastName())
                                                    .RuleFor(u => u.Gender, x => Gender.Female);
                                                m = femaleUser.Generate();
                                                break;
                                            default:
                                                var uUser = new Faker<Member>()
                                                    .RuleFor(u => u.NewId, x => x.Random.Replace("1##########"))
                                                    .RuleFor(u => u.FirstName, x => x.Name.FirstName())
                                                    .RuleFor(u => u.LastName, x => x.Name.LastName());
                                                m = uUser.Generate();
                                                break;
                                        }

                                        Ids[patientid] = m.NewId;

                                        //Replace the Id
                                        //ID_NUMBER|BIRTH_DATE|GENDER_CODE|NAME_FIRST|NAME_LAST|MEMBERSHIP_DATE|REGION|GROUP_ID|OFFICE|NEW_MEMBER|LOB
                                        line = m.NewId + "|" + birthday + "|" + gender + "|" + m.FirstName + "|" +
                                               m.LastName + "|" + split[5] + "|" + split[6]
                                               + split[7] + "|" + split[8] + split[9] + "|" + split[10];
                                    }
                                    counter++;

                                    writer.WriteLine(line);
                                }
                            }
                        }


                    }
                        break;
                    case "RX":
                    {
                        int counter = 0;
                        string line;

                        using (StreamReader f = new StreamReader(file))
                        {
                            using (StreamWriter writer = new StreamWriter(output + @"\" + fname))
                            {
                                while ((line = f.ReadLine()) != null)
                                {
                                    if (counter > 0)
                                    {
                                        string[] split = line.Split('|');

                                        var patientid = split[0];

                                        var startPos = line.IndexOfNth("|", 1);

                                        var aStringBuilder = new StringBuilder(line);
                                        aStringBuilder.Remove(0, startPos - 1);
                                        aStringBuilder.Insert(0, Ids[patientid]);
                                        line = aStringBuilder.ToString();
                                    }

                                    writer.WriteLine(line);

                                }
                            }
                        }
                    }
                        break;
                }               
            }
        }

        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
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
