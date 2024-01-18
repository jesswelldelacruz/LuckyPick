using System;
using System.Collections.Generic;
using System.Linq;
using LuckyPick.Repository;
using System.Threading.Tasks;
using LuckyPick.Models;
using System.Globalization;
using LuckyPick.Helper;
using LuckyPick.Enum;

namespace LuckyPick.Services
{
    public class LuckyPickService
    {
        public async static Task LuckyPick(int pcs, GameEnum game)
        {
            var data = new List<string>();

            var allSortData = await GetAllSortData().ConfigureAwait(true);
            var allConsecutiveData = await GetAllConsecutiveData().ConfigureAwait(true);
            var allSameData = await GetAllSameDigitData().ConfigureAwait(true);

            data.AddRange(allSortData);
            data.AddRange(allConsecutiveData);
            data.AddRange(allSameData);

            for (int i = 0; i < pcs; i++)
            {
                var luckyPick = Generate(6, (int)game);

                while (data.Contains(luckyPick))
                {
                    luckyPick = Generate(6, (int)game);
                }

                FileHelper.Export(EnumHelper.GetName<GameEnum>(game), luckyPick);
                Console.WriteLine(luckyPick);
            }
        }

        public async static Task LuckyPickSameLastDigit(int pcs, GameEnum game)
        {
            var data = new List<string>();

            var allSortData = await GetAllSortData().ConfigureAwait(true);
            var allConsecutiveData = await GetAllConsecutiveData().ConfigureAwait(true);
            var allSameData = await GetAllSameDigitData().ConfigureAwait(true);

            data.AddRange(allSortData);
            data.AddRange(allConsecutiveData);
            data.AddRange(allSameData);

            for (int i = 0; i < pcs; i++)
            {
                var luckyPick = Generate(6, (int)game);

                var digitArr = luckyPick.Replace(Environment.NewLine, string.Empty).Split('-');

                var sameLast = digitArr.GroupBy(i => Convert.ToInt32(i) % 10).Where(g => g.Count() >= 2);

                while (data.Contains(luckyPick) || !sameLast.Any())
                {
                    luckyPick = Generate(6, (int)game);
                    digitArr = luckyPick.Replace(Environment.NewLine, string.Empty).Split('-');
                    sameLast = digitArr.GroupBy(i => Convert.ToInt32(i) % 10).Where(g => g.Count() >= 2);
                }

                FileHelper.Export(EnumHelper.GetName<GameEnum>(game), luckyPick);
                Console.WriteLine(luckyPick);
            }
        }

        public async static Task Search(string combination)
        {
            var srtCombination = DataHelper.SortDigit(combination);

            var data = new List<string>();
            var allSortData = await GetAllSortData().ConfigureAwait(true);
            var allConsecutiveData = await GetAllConsecutiveData().ConfigureAwait(true);
            var allSameData = await GetAllSameDigitData().ConfigureAwait(true);

            data.AddRange(allSortData);
            data.AddRange(allConsecutiveData);
            data.AddRange(allSameData);

            if (data.Contains(srtCombination))
            {
                Console.WriteLine($"{srtCombination} is already exist");
            }
            else
            {
                Console.WriteLine($"{srtCombination} Good");
            }

        }

        public async static Task SearchWithGame(string combination)
        {
            var srtCombination = DataHelper.SortDigit(combination);

            var lotto642 = await DataRepository.GetAll<Lotto642>().ConfigureAwait(true);
            var megaLotto645 = await DataRepository.GetAll<MegaLotto645>().ConfigureAwait(true);
            var superLotto649 = await DataRepository.GetAll<SuperLotto649>().ConfigureAwait(true);
            var grandLotto655 = await DataRepository.GetAll<GrandLotto655>().ConfigureAwait(true);
            var ultraLotto658 = await DataRepository.GetAll<UltraLotto658>().ConfigureAwait(true);
            
            if (lotto642.Contains(srtCombination))
            {
                Console.WriteLine($"{srtCombination} is already exist in {EnumHelper.GetName<GameEnum>(GameEnum.Lotto642)}");
            }
            else if (megaLotto645.Contains(srtCombination))
            {
                Console.WriteLine($"{srtCombination} is already exist in {EnumHelper.GetName<GameEnum>(GameEnum.MegaLotto645)}");
            }
            else if (superLotto649.Contains(srtCombination))
            {
                Console.WriteLine($"{srtCombination} is already exist in {EnumHelper.GetName<GameEnum>(GameEnum.SuperLotto649)}");
            }
            else if (grandLotto655.Contains(srtCombination))
            {
                Console.WriteLine($"{srtCombination} is already exist in {EnumHelper.GetName<GameEnum>(GameEnum.GrandLotto655)}");
            }
            else if (ultraLotto658.Contains(srtCombination))
            {
                Console.WriteLine($"{srtCombination} is already exist in {EnumHelper.GetName<GameEnum>(GameEnum.UltraLotto658)}");
            }
            else
            {
                //Console.WriteLine($"{srtCombination} Good");
            }

        }
        public static string Draw(int max)
        {
            Random rnd = new Random();
            var num = rnd.Next(1, (max + 1));
            return num.ToString().PadLeft(2, '0');
        }

        public static string Generate(int num, int max)
        {
            var luckyDraw = new List<string>();

            for (int i = 0; i < num; i++)
            {
                var n = Draw(max);

                while (luckyDraw.Contains(n))
                {
                    n = Draw(max);
                }
                luckyDraw.Add(n);
            }

            return string.Join("-", luckyDraw.OrderBy(c => int.Parse(c)).ToList());
        }

        public async static Task<List<string>> GetAllSortData()
        {
            var data = new List<string>();

            var lotto642 = await DataRepository.GetAll<Lotto642>().ConfigureAwait(true);
            var megaLotto645 = await DataRepository.GetAll<MegaLotto645>().ConfigureAwait(true);
            var superLotto649 = await DataRepository.GetAll<SuperLotto649>().ConfigureAwait(true);
            var grandLotto655 = await DataRepository.GetAll<GrandLotto655>().ConfigureAwait(true);
            var ultraLotto658 = await DataRepository.GetAll<UltraLotto658>().ConfigureAwait(true);

            data.AddRange(lotto642);
            data.AddRange(megaLotto645);
            data.AddRange(superLotto649);
            data.AddRange(grandLotto655);
            data.AddRange(ultraLotto658);

            return data;
        }

        public async static Task<List<string>> GetAllConsecutiveData()
        {
            var data = new List<string>();

            var consecutive = await DataRepository.GetAll<_Consecutive>().ConfigureAwait(true);
            var consecutives_2 = await DataRepository.GetAll<_Consecutive_2>().ConfigureAwait(true);
            var consecutives_3 = await DataRepository.GetAll<_Consecutive_3>().ConfigureAwait(true);
            var consecutives_4 = await DataRepository.GetAll<_Consecutive_4>().ConfigureAwait(true);
            var consecutives_5 = await DataRepository.GetAll<_Consecutive_5>().ConfigureAwait(true);
            var consecutives_6 = await DataRepository.GetAll<_Consecutive_6>().ConfigureAwait(true);
            var consecutives_7 = await DataRepository.GetAll<_Consecutive_7>().ConfigureAwait(true);
            var consecutives_8 = await DataRepository.GetAll<_Consecutive_8>().ConfigureAwait(true);
            var consecutives_9 = await DataRepository.GetAll<_Consecutive_9>().ConfigureAwait(true);
            var consecutives_10 = await DataRepository.GetAll<_Consecutive_10>().ConfigureAwait(true);
            var consecutives_11 = await DataRepository.GetAll<_Consecutive_11>().ConfigureAwait(true);
            var consecutives_12 = await DataRepository.GetAll<_Consecutive_12>().ConfigureAwait(true);
            var consecutives_13 = await DataRepository.GetAll<_Consecutive_13>().ConfigureAwait(true);
            var consecutives_14 = await DataRepository.GetAll<_Consecutive_14>().ConfigureAwait(true);

            data.AddRange(consecutive);
            data.AddRange(consecutives_2);
            data.AddRange(consecutives_3);
            data.AddRange(consecutives_4);
            data.AddRange(consecutives_5);
            data.AddRange(consecutives_6);
            data.AddRange(consecutives_7);
            data.AddRange(consecutives_8);
            data.AddRange(consecutives_9);
            data.AddRange(consecutives_10);
            data.AddRange(consecutives_11);
            data.AddRange(consecutives_12);
            data.AddRange(consecutives_13);
            data.AddRange(consecutives_14);

            return data.Distinct().ToList();
        }

        public async static Task<List<string>> GetAllSameDigitData()
        {
            var data = new List<string>();

            var sameDigit = await DataRepository.GetAll<_SameDigit>().ConfigureAwait(true);
            var sameLastDigit = await DataRepository.GetAll<_SameLastDigit>().ConfigureAwait(true);

            data.AddRange(sameDigit);
            data.AddRange(sameLastDigit);

            return data.Distinct().ToList();
        }

        public static List<string> GetDuplicate(List<string> data)
        {
            var duplicates = new List<string>();

            var keyValuePairs = data.GroupBy(x => x)
                            .Where(g => g.Count() > 1)
                            .ToDictionary(x => x.Key, y => y.Count());

            foreach (var i in keyValuePairs)
            {
                var duplicate = $"{i.Key} = {i.Value}";
                duplicates.Add(duplicate);
            }

            return duplicates;
        }

        public static void FindConsecutives(string game, string digits, int div, int rep)
        {
            var digitArr = digits.Replace(Environment.NewLine, string.Empty).Split('-');

            int preVal = Convert.ToInt32(digitArr[0]);
            int count = 1;

            for (var i = 0; i < digitArr.Length; i++)
            {
                if (Convert.ToInt32(digitArr[i]) - div == preVal)
                {
                    count += 1;

                    if (count == rep)
                    {
                        Console.WriteLine(digits);
                        FileHelper.Export(game, digits);
                        break;
                    }
                }
                else
                {
                    count = 1;
                }
                preVal = Convert.ToInt32(digitArr[i]);
            }
        }

        public static void FindSameDigit(string game, string digits)
        {
            var digitArr = digits.Replace(Environment.NewLine, string.Empty).Split('-');

            var count = 0;

            foreach (var i in digitArr)
            {
                int a = Convert.ToInt32(i) / 10;
                int b = Convert.ToInt32(i) % 10;

                if (a == b)
                {
                    count += 1;
                }

                if (count >= 3)
                {
                    Console.WriteLine(digits);
                    FileHelper.Export(game, digits);
                }
            }
        }

        public static void FindSameLastDigit(string game, string digits)
        {
            var digitArr = digits.Replace(Environment.NewLine, string.Empty).Split('-');

            var sameLast = digitArr.GroupBy(i => Convert.ToInt32(i) % 10).SingleOrDefault(g => g.Count() >= 4);

            if (sameLast != null)
            {
                Console.WriteLine(digits);
                FileHelper.Export(game, digits);
            }
        }

        public async static Task FindExisting()
        {
            //var data = new List<string>();
            ////var sortData = DataRepository.GetAll655();
            //var allSortData = await GetAllSortData().ConfigureAwait(true);
            //var allConsecutiveData = await GetAllConsecutiveData().ConfigureAwait(true);
            //var allSameData = await GetAllSameDigitData().ConfigureAwait(true);

            //data.AddRange(allSortData);
            //data.AddRange(allConsecutiveData);
            //data.AddRange(allSameData);

var digits = $@"06-10-14-24-37-42
15-18-27-38-39-42
03-12-23-24-27-38
01-09-23-26-28-39
06-11-25-27-32-41
11-19-22-25-27-31
05-09-14-29-32-33
02-08-20-36-37-40
01-07-16-18-28-34
07-12-21-30-32-40
04-22-33-35-40-42
01-04-15-22-25-36
07-11-15-17-29-40
03-07-24-31-36-42
07-23-29-32-33-38
14-17-18-26-32-34
04-18-20-33-34-39
05-23-26-37-39-42
05-11-21-32-39-40
01-14-16-21-23-32
07-13-19-20-27-32
07-16-20-23-36-38
06-15-27-32-35-41
02-15-18-25-31-36
12-18-21-29-38-41
04-14-17-26-37-42
02-05-13-25-26-31
07-10-19-27-33-34
09-15-17-26-34-40
13-18-26-32-35-40
03-05-17-21-26-37
02-12-16-24-28-31
03-07-13-21-29-35
04-12-18-19-26-31
10-15-20-28-37-39
05-13-16-23-34-39
06-12-14-25-30-34
04-13-17-27-28-35
18-19-21-28-35-36
04-08-22-27-34-41
13-15-26-37-39-40
14-16-20-29-32-41
05-18-19-26-30-40
09-11-20-26-40-41
10-18-22-27-30-39
06-09-21-22-30-35
11-14-16-20-26-33
07-08-16-23-26-34
08-10-16-17-25-39
15-17-19-30-43-45
09-27-28-32-41-42
01-16-22-23-38-48
08-11-17-37-39-40
11-13-16-20-41-44
07-15-21-27-38-43
04-16-19-28-33-49
09-22-27-34-46-49
12-18-33-38-39-42
06-14-25-28-45-49
13-14-21-29-32-44
05-20-34-36-41-48
15-20-23-31-43-48
11-25-30-32-45-48
02-08-12-27-36-39
10-14-18-28-36-47
05-22-31-35-36-40
01-04-21-33-34-47
02-04-06-27-29-44
14-31-34-43-48-49
06-11-15-25-30-49
06-09-14-24-33-37
16-19-34-36-39-43
01-16-19-27-30-47
01-09-24-26-30-39
19-22-23-27-30-49
05-10-25-34-42-47
14-17-19-21-30-41
03-14-19-27-33-41
10-12-28-31-42-46
08-12-30-34-40-42
05-15-26-33-47-48
05-09-16-38-46-48
01-17-19-29-30-47
05-21-25-28-39-43
12-13-18-26-33-41
06-10-11-32-42-45
04-12-17-25-35-39
07-12-15-25-44-49
04-17-21-31-39-46
11-13-29-32-36-42
09-15-20-29-38-44
15-24-30-36-38-48
08-13-19-43-44-48
03-21-32-41-44-47
03-05-19-27-33-39
08-11-25-31-40-49
01-15-20-24-29-41
10-16-29-38-39-41
01-05-26-37-43-45
12-15-34-38-40-48
01-23-28-33-39-45
03-07-29-36-43-44
07-11-23-28-41-42
02-15-20-24-42-48
05-13-19-28-43-47
18-29-30-38-40-44
03-05-17-22-24-27
11-18-19-21-25-39
19-25-29-30-33-36
01-04-24-36-42-47
03-04-13-20-41-42
24-25-31-42-44-47
06-28-37-43-44-48
03-21-25-27-51-55
04-21-26-33-48-51
05-17-25-49-50-53
05-18-20-24-30-41
07-26-27-43-45-54
06-19-24-31-40-50
06-13-15-22-46-49
02-10-16-31-32-43
04-08-20-39-40-53
05-19-20-43-51-53
06-17-22-26-40-44
05-22-29-37-48-55
10-13-37-46-52-53
07-10-24-43-51-54
07-25-46-49-50-55
10-12-21-37-52-55
09-27-44-47-51-55
05-14-22-34-36-48
10-14-19-25-32-55
01-06-20-32-42-48
08-16-20-28-34-51
09-16-26-42-44-50
06-22-34-43-47-50
01-20-25-27-29-55
07-13-27-43-51-55
06-17-24-30-34-50
06-20-30-35-47-54
08-12-29-48-51-55
01-25-34-37-38-51
10-28-39-43-51-53
07-12-24-30-48-54
10-14-29-47-49-55
07-16-17-29-34-55
09-19-38-44-45-53
04-28-33-46-47-54
07-27-29-43-45-53
05-26-30-43-52-55
02-19-24-31-41-47
02-19-25-38-45-52
10-26-27-38-44-51
06-09-18-43-52-53
08-14-30-32-38-47
05-19-24-31-41-47
08-23-26-37-47-49
10-29-30-45-48-52
06-17-26-44-49-53
06-17-25-45-54-55
02-16-23-39-48-51
10-20-25-37-46-53
09-16-29-38-44-52
05-11-28-35-49-53
08-23-34-41-50-54
06-13-21-37-39-51
07-25-26-39-42-44
06-25-38-39-48-52
02-15-27-30-33-47
04-17-26-27-40-48
07-20-35-39-48-51
03-06-27-37-49-50
04-21-25-29-38-48
10-24-38-41-43-47
06-25-29-34-46-52
02-20-33-39-43-54
05-11-28-32-51-55
09-25-32-36-38-48
08-17-23-25-44-52
10-21-36-41-49-52
07-15-28-31-40-50
09-21-28-35-52-54
10-25-28-37-50-54
06-10-26-32-38-48
05-08-23-32-49-55
06-16-28-34-47-54
08-26-39-47-50-51
09-25-39-41-44-54
09-16-32-37-40-54
05-12-27-35-46-48
04-21-29-39-40-46
04-28-42-47-49-54
06-18-31-45-49-50
08-11-22-27-34-46
06-17-29-38-45-48
07-12-29-30-37-42
08-13-26-31-47-49
16-25-34-51-53-54
16-22-29-31-43-45
14-19-20-29-44-51
07-12-13-29-44-50
12-21-28-34-44-49
13-28-31-39-45-55
15-25-42-48-50-53
24-31-37-39-46-52
20-31-43-45-50-54
13-27-31-36-40-52
23-26-34-38-49-54
12-20-26-30-31-47
11-18-21-37-50-52
13-28-30-38-45-54
14-23-31-40-47-50
19-27-30-31-48-50
14-22-26-29-32-48
18-24-30-33-42-51
12-23-31-38-51-55
19-24-32-36-45-51
16-25-27-40-50-54
18-20-36-37-47-52
19-24-25-37-47-52
15-20-33-41-46-53
15-29-31-41-47-49
14-21-36-37-42-55
11-15-25-39-41-53
12-23-26-42-48-50
12-15-25-37-42-49
16-24-27-28-38-49
15-26-37-38-51-52
19-24-28-39-45-54
16-27-28-43-48-55
13-27-28-32-42-45
20-33-34-37-51-53
14-16-19-28-49-51
13-38-39-45-51-53
02-03-19-29-31-37
04-13-21-24-35-37
02-05-09-17-23-27
05-11-16-37-39-45
01-18-21-22-39-40
05-08-11-29-32-42
06-08-17-18-30-36
02-12-16-23-41-42
11-19-24-29-31-38
12-30-32-33-43-44
03-12-16-21-33-35
07-08-11-23-28-38
13-26-33-38-43-45
16-19-22-31-39-45
09-24-26-37-39-41
07-09-33-36-37-39
04-16-18-31-36-45
04-15-18-26-30-41
04-06-09-32-37-42
03-05-27-31-43-45
04-08-18-35-40-41
02-04-11-29-31-43
02-04-25-35-39-41
07-10-15-38-41-45
06-20-21-27-34-40
08-13-17-25-31-42
04-06-23-30-35-39
02-20-24-26-39-41
02-05-07-38-43-45
08-12-15-17-21-36
01-06-23-24-35-42
01-04-12-30-35-44
06-07-15-29-36-43
06-12-16-20-31-32
02-10-19-21-34-44
08-09-16-30-37-42
01-07-17-23-28-41
15-25-32-33-38-40
01-05-07-21-35-41
12-20-28-34-36-42
01-03-17-18-32-42
06-13-15-20-28-31
09-19-33-35-39-40
11-14-20-26-38-40
04-11-14-22-32-41
11-19-21-26-29-32
18-21-24-35-38-39
05-06-17-36-37-41
05-10-11-14-23-31
03-05-08-18-19-23
04-05-15-21-29-39
01-03-13-30-32-37
02-11-14-28-33-40
05-10-17-18-33-35
08-14-22-26-29-32
02-06-21-23-35-40
20-22-32-33-36-39
01-16-23-24-31-34
05-16-19-27-31-35
01-04-10-16-28-30
10-18-21-27-28-32
11-15-18-22-29-30
14-16-24-25-34-41
02-05-15-18-21-38
16-26-31-33-35-39
08-09-14-16-18-35
07-11-13-15-38-41
10-15-17-24-32-37
01-06-15-30-37-40
03-11-13-16-18-41
02-21-27-30-39-41
05-12-22-30-31-34
08-14-29-38-40-42
01-04-05-10-31-33
02-15-25-26-29-30
05-14-29-34-36-42
01-07-10-12-32-41
04-13-19-30-35-41
07-15-16-26-30-42
13-14-24-25-37-39
25-26-29-31-35-37
05-10-14-33-34-37
13-15-19-22-24-26
10-24-25-27-29-34
04-10-17-19-24-35
20-21-32-35-38-39
01-14-33-38-41-42
06-12-13-26-28-31
03-05-32-33-39-41
05-18-33-35-37-41
06-11-14-18-22-25
22-31-35-36-38-39
04-09-15-17-32-40
01-11-13-16-18-22
10-11-13-16-23-38
07-12-15-19-22-42
04-08-12-32-36-41
04-11-16-30-34-38
01-05-07-16-18-30
06-14-17-29-37-41
09-15-16-22-29-38
02-04-10-12-29-30
02-20-26-30-37-38
04-06-08-23-32-41
01-27-28-36-38-40
06-09-17-21-33-35
19-20-30-32-38-40
01-10-16-28-31-40
13-15-17-20-31-42
01-04-19-34-38-41
22-24-27-28-33-39
05-24-29-34-36-39
19-20-22-32-37-41
12-16-31-33-39-41
05-16-19-24-27-33
03-15-16-22-24-33
08-20-31-35-37-42
03-09-19-23-29-36
03-11-19-30-33-42";
            //var digits = $@"";
            //var digits = $@"";
            var digitArr = digits.Replace("\r", string.Empty).Split('\n');

            foreach (var i in digitArr)
            {
                //if (data.Contains(SortDigit(i)))
                //{
                //    Console.WriteLine(i);
                //}
                await SearchWithGame(i.Trim());
            }
        }

        public static void GetAllCombination(int num)
        {
            var arr = new List<string>();
            for (int i = 1; i < num; i++)
            {
                arr.Add(i.ToString().PadLeft(2, '0'));
            }

            arr.DifferentCombinations(6);
        }

        public async static Task AddAsync()
        {
            var lotto642 = new List<Lotto642>
            {
                new Lotto642{Combination="29-11-32-34-23-22",DrawDate=DateTime.ParseExact("09/02/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="32-21-27-25-13-40",DrawDate=DateTime.ParseExact("09/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="03-31-15-19-10-27",DrawDate=DateTime.ParseExact("09/07/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="19-11-09-03-41-35",DrawDate=DateTime.ParseExact("09/09/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="17-14-03-26-33-05",DrawDate=DateTime.ParseExact("09/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="06-27-03-13-35-04",DrawDate=DateTime.ParseExact("09/14/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="30-39-04-32-42-29",DrawDate=DateTime.ParseExact("09/16/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="33-34-24-04-11-05",DrawDate=DateTime.ParseExact("09/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="25-10-14-17-19-42",DrawDate=DateTime.ParseExact("09/21/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="13-36-27-38-15-22",DrawDate=DateTime.ParseExact("09/23/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="08-32-34-28-10-35",DrawDate=DateTime.ParseExact("09/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="42-11-20-28-22-05",DrawDate=DateTime.ParseExact("09/28/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="38-10-29-32-39-42",DrawDate=DateTime.ParseExact("09/30/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="05-10-42-08-13-20",DrawDate=DateTime.ParseExact("10/03/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="25-24-26-35-21-42",DrawDate=DateTime.ParseExact("10/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="42-28-21-38-08-17",DrawDate=DateTime.ParseExact("10/07/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="39-13-41-04-01-03",DrawDate=DateTime.ParseExact("10/10/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="13-41-06-25-34-42",DrawDate=DateTime.ParseExact("10/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="23-16-26-37-25-41",DrawDate=DateTime.ParseExact("10/14/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="38-14-22-21-40-30",DrawDate=DateTime.ParseExact("10/17/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="11-08-25-05-34-16",DrawDate=DateTime.ParseExact("10/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="03-22-02-17-14-39",DrawDate=DateTime.ParseExact("10/21/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="31-05-34-11-36-20",DrawDate=DateTime.ParseExact("10/24/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="33-04-37-27-16-35",DrawDate=DateTime.ParseExact("10/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="05-32-24-04-18-22",DrawDate=DateTime.ParseExact("10/28/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="03-09-36-40-29-41",DrawDate=DateTime.ParseExact("10/31/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="29-05-38-01-33-27",DrawDate=DateTime.ParseExact("11/02/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="07-34-11-26-05-06",DrawDate=DateTime.ParseExact("11/04/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="29-37-26-21-17-08",DrawDate=DateTime.ParseExact("11/07/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="39-07-38-16-05-03",DrawDate=DateTime.ParseExact("11/09/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="03-28-17-11-26-16",DrawDate=DateTime.ParseExact("11/11/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="01-12-21-28-08-18",DrawDate=DateTime.ParseExact("11/14/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="31-14-38-10-22-13",DrawDate=DateTime.ParseExact("11/16/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="18-21-20-36-35-07",DrawDate=DateTime.ParseExact("11/18/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="25-03-39-27-08-35",DrawDate=DateTime.ParseExact("11/21/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="06-18-32-31-26-14",DrawDate=DateTime.ParseExact("11/23/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="32-33-39-07-25-08",DrawDate=DateTime.ParseExact("11/25/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="37-35-41-05-23-07",DrawDate=DateTime.ParseExact("11/28/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="36-38-29-37-41-30",DrawDate=DateTime.ParseExact("11/30/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="29-26-30-22-31-15",DrawDate=DateTime.ParseExact("12/02/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="16-10-40-25-41-19",DrawDate=DateTime.ParseExact("12/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="33-16-18-12-11-02",DrawDate=DateTime.ParseExact("12/07/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="25-12-02-15-26-01",DrawDate=DateTime.ParseExact("12/09/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="04-17-05-02-22-42",DrawDate=DateTime.ParseExact("12/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="03-05-10-13-11-18",DrawDate=DateTime.ParseExact("12/14/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="38-42-14-34-27-31",DrawDate=DateTime.ParseExact("12/16/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="32-05-23-31-21-06",DrawDate=DateTime.ParseExact("12/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="41-07-23-30-13-20",DrawDate=DateTime.ParseExact("12/21/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="33-11-38-37-32-06",DrawDate=DateTime.ParseExact("12/23/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="07-32-17-01-08-09",DrawDate=DateTime.ParseExact("12/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="18-34-01-11-28-04",DrawDate=DateTime.ParseExact("12/28/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="12-09-18-11-05-35",DrawDate=DateTime.ParseExact("12/30/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="04-22-07-10-29-14",DrawDate=DateTime.ParseExact("01/02/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="22-16-09-01-42-10",DrawDate=DateTime.ParseExact("01/04/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="10-30-34-09-14-21",DrawDate=DateTime.ParseExact("01/06/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="04-39-23-19-17-36",DrawDate=DateTime.ParseExact("01/09/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="20-05-34-09-26-19",DrawDate=DateTime.ParseExact("01/11/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="42-30-17-04-15-14",DrawDate=DateTime.ParseExact("01/13/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="03-29-18-30-07-22",DrawDate=DateTime.ParseExact("01/16/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new Lotto642{Combination="41-08-06-03-10-04",DrawDate=DateTime.ParseExact("01/18/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},


            };

            var megaLotto645 = new List<MegaLotto645>
            {
                new MegaLotto645{Combination="30-42-39-12-35-18",DrawDate=DateTime.ParseExact("09/01/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="32-39-26-21-37-06",DrawDate=DateTime.ParseExact("09/04/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="12-33-32-08-42-23",DrawDate=DateTime.ParseExact("09/06/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="33-17-04-43-15-38",DrawDate=DateTime.ParseExact("09/08/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="23-01-05-10-20-06",DrawDate=DateTime.ParseExact("09/11/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="12-41-30-22-43-24",DrawDate=DateTime.ParseExact("09/13/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="25-44-18-41-11-31",DrawDate=DateTime.ParseExact("09/15/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="37-26-05-16-21-24",DrawDate=DateTime.ParseExact("09/18/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="44-41-29-18-02-13",DrawDate=DateTime.ParseExact("09/20/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="31-03-20-29-26-37",DrawDate=DateTime.ParseExact("09/22/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="03-31-07-16-33-29",DrawDate=DateTime.ParseExact("09/25/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="34-32-36-42-01-21",DrawDate=DateTime.ParseExact("09/27/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="23-38-11-27-06-36",DrawDate=DateTime.ParseExact("09/29/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="08-05-38-25-13-29",DrawDate=DateTime.ParseExact("10/02/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="25-02-43-30-32-42",DrawDate=DateTime.ParseExact("10/04/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="09-45-25-38-26-34",DrawDate=DateTime.ParseExact("10/06/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="34-41-11-01-10-07",DrawDate=DateTime.ParseExact("10/09/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="41-12-13-06-31-05",DrawDate=DateTime.ParseExact("10/11/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="18-11-32-19-39-33",DrawDate=DateTime.ParseExact("10/13/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="04-15-45-44-40-08",DrawDate=DateTime.ParseExact("10/16/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="36-29-04-26-34-15",DrawDate=DateTime.ParseExact("10/18/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="15-36-30-37-05-17",DrawDate=DateTime.ParseExact("10/20/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="22-08-42-03-12-27",DrawDate=DateTime.ParseExact("10/23/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="04-34-02-28-42-20",DrawDate=DateTime.ParseExact("10/25/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="20-13-05-03-19-29",DrawDate=DateTime.ParseExact("10/27/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="35-41-30-03-40-42",DrawDate=DateTime.ParseExact("10/30/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="17-14-24-38-15-01",DrawDate=DateTime.ParseExact("11/01/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="14-43-25-32-01-38",DrawDate=DateTime.ParseExact("11/03/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="13-31-16-01-25-10",DrawDate=DateTime.ParseExact("11/06/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="21-16-05-41-42-02",DrawDate=DateTime.ParseExact("11/08/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="06-41-13-37-34-08",DrawDate=DateTime.ParseExact("11/10/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="35-14-10-15-24-20",DrawDate=DateTime.ParseExact("11/13/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="34-36-05-45-15-32",DrawDate=DateTime.ParseExact("11/15/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="14-18-42-19-26-23",DrawDate=DateTime.ParseExact("11/17/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="06-31-32-27-44-08",DrawDate=DateTime.ParseExact("11/20/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="20-42-39-33-43-15",DrawDate=DateTime.ParseExact("11/22/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="43-36-20-23-31-01",DrawDate=DateTime.ParseExact("11/24/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="06-23-25-29-22-36",DrawDate=DateTime.ParseExact("11/27/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="20-18-37-11-33-05",DrawDate=DateTime.ParseExact("11/29/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="25-44-23-24-39-38",DrawDate=DateTime.ParseExact("12/01/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="23-40-06-07-27-28",DrawDate=DateTime.ParseExact("12/04/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="10-42-23-40-15-12",DrawDate=DateTime.ParseExact("12/06/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="01-03-22-19-30-44",DrawDate=DateTime.ParseExact("12/08/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="32-25-04-37-19-38",DrawDate=DateTime.ParseExact("12/11/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="17-40-41-18-01-19",DrawDate=DateTime.ParseExact("12/13/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="32-14-34-13-01-35",DrawDate=DateTime.ParseExact("12/15/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="20-33-01-29-06-09",DrawDate=DateTime.ParseExact("12/18/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="43-09-32-06-27-20",DrawDate=DateTime.ParseExact("12/20/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="41-01-13-33-06-05",DrawDate=DateTime.ParseExact("12/22/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="25-18-37-13-17-39",DrawDate=DateTime.ParseExact("12/27/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="06-12-28-04-16-17",DrawDate=DateTime.ParseExact("12/29/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="37-29-14-17-16-22",DrawDate=DateTime.ParseExact("01/03/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="13-18-41-29-33-37",DrawDate=DateTime.ParseExact("01/05/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="09-07-29-28-11-18",DrawDate=DateTime.ParseExact("01/08/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="25-27-12-33-36-02",DrawDate=DateTime.ParseExact("01/10/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="06-34-11-32-40-02",DrawDate=DateTime.ParseExact("01/12/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="07-22-04-23-27-40",DrawDate=DateTime.ParseExact("01/15/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new MegaLotto645{Combination="28-05-33-01-23-20",DrawDate=DateTime.ParseExact("01/17/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},


            };

            var superLotto649 = new List<SuperLotto649>
            {
                new SuperLotto649{Combination="32-29-10-45-17-30",DrawDate=DateTime.ParseExact("09/03/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="03-12-24-17-44-43",DrawDate=DateTime.ParseExact("09/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="06-15-20-38-43-07",DrawDate=DateTime.ParseExact("09/07/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="42-01-22-44-15-03",DrawDate=DateTime.ParseExact("09/10/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="36-23-31-09-12-29",DrawDate=DateTime.ParseExact("09/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="30-33-46-26-42-38",DrawDate=DateTime.ParseExact("09/14/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="26-03-46-34-15-20",DrawDate=DateTime.ParseExact("09/17/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="02-33-38-17-28-41",DrawDate=DateTime.ParseExact("09/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="25-24-17-23-03-31",DrawDate=DateTime.ParseExact("09/21/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="04-38-21-24-23-20",DrawDate=DateTime.ParseExact("09/24/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="33-42-21-38-19-24",DrawDate=DateTime.ParseExact("09/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="34-19-14-25-18-10",DrawDate=DateTime.ParseExact("09/28/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="06-02-46-38-45-39",DrawDate=DateTime.ParseExact("10/01/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="47-42-29-28-09-32",DrawDate=DateTime.ParseExact("10/03/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="37-20-41-05-46-35",DrawDate=DateTime.ParseExact("10/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="47-43-35-20-29-13",DrawDate=DateTime.ParseExact("10/08/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="03-14-26-13-17-39",DrawDate=DateTime.ParseExact("10/10/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="25-39-02-33-26-22",DrawDate=DateTime.ParseExact("10/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="47-35-02-13-38-17",DrawDate=DateTime.ParseExact("10/15/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="02-20-33-32-13-48",DrawDate=DateTime.ParseExact("10/17/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="26-31-41-14-05-10",DrawDate=DateTime.ParseExact("10/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="13-08-38-44-31-11",DrawDate=DateTime.ParseExact("10/22/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="26-32-16-28-07-02",DrawDate=DateTime.ParseExact("10/24/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="34-27-13-38-26-14",DrawDate=DateTime.ParseExact("10/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="30-31-13-42-02-33",DrawDate=DateTime.ParseExact("10/29/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="43-45-32-16-26-19",DrawDate=DateTime.ParseExact("10/31/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="37-30-33-35-05-02",DrawDate=DateTime.ParseExact("11/02/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="38-16-27-04-41-24",DrawDate=DateTime.ParseExact("11/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="10-39-22-20-41-42",DrawDate=DateTime.ParseExact("11/07/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="11-41-19-47-31-30",DrawDate=DateTime.ParseExact("11/09/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="23-20-07-35-27-37",DrawDate=DateTime.ParseExact("11/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="16-39-32-07-01-27",DrawDate=DateTime.ParseExact("11/14/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="12-49-16-45-25-03",DrawDate=DateTime.ParseExact("11/16/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="16-24-19-35-47-15",DrawDate=DateTime.ParseExact("11/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="13-16-18-15-37-29",DrawDate=DateTime.ParseExact("11/21/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="39-11-13-36-32-08",DrawDate=DateTime.ParseExact("11/23/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="30-35-17-26-06-24",DrawDate=DateTime.ParseExact("11/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="15-27-24-31-19-30",DrawDate=DateTime.ParseExact("11/28/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="26-22-34-44-03-29",DrawDate=DateTime.ParseExact("11/30/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="12-27-35-09-31-42",DrawDate=DateTime.ParseExact("12/03/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="41-29-33-31-16-26",DrawDate=DateTime.ParseExact("12/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="24-03-14-19-39-33",DrawDate=DateTime.ParseExact("12/07/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="21-18-24-22-35-16",DrawDate=DateTime.ParseExact("12/10/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="21-49-28-29-47-30",DrawDate=DateTime.ParseExact("12/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="15-07-31-23-03-45",DrawDate=DateTime.ParseExact("12/14/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="02-07-29-06-45-15",DrawDate=DateTime.ParseExact("12/17/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="30-16-33-12-29-25",DrawDate=DateTime.ParseExact("12/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="16-22-14-23-08-02",DrawDate=DateTime.ParseExact("12/21/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="08-12-23-29-24-44",DrawDate=DateTime.ParseExact("12/24/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="30-06-48-14-49-37",DrawDate=DateTime.ParseExact("12/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="06-19-07-40-43-25",DrawDate=DateTime.ParseExact("12/28/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="15-06-41-22-49-44",DrawDate=DateTime.ParseExact("12/31/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="07-33-42-16-32-05",DrawDate=DateTime.ParseExact("01/02/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="34-04-17-28-11-30",DrawDate=DateTime.ParseExact("01/04/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="03-47-13-12-07-01",DrawDate=DateTime.ParseExact("01/07/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="24-39-41-29-22-20",DrawDate=DateTime.ParseExact("01/09/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="37-45-47-12-22-08",DrawDate=DateTime.ParseExact("01/11/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="29-35-24-20-02-43",DrawDate=DateTime.ParseExact("01/14/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="26-33-14-48-06-42",DrawDate=DateTime.ParseExact("01/16/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new SuperLotto649{Combination="32-42-01-04-10-02",DrawDate=DateTime.ParseExact("01/18/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
            };

            var grandLotto655 = new List<GrandLotto655>
            {
                new GrandLotto655{Combination="10-02-17-04-35-51",DrawDate=DateTime.ParseExact("09/02/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="55-50-06-45-12-40",DrawDate=DateTime.ParseExact("09/04/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="50-48-02-55-46-09",DrawDate=DateTime.ParseExact("09/06/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="42-32-54-44-11-39",DrawDate=DateTime.ParseExact("09/09/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="31-54-29-52-24-12",DrawDate=DateTime.ParseExact("09/11/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="45-23-11-13-46-04",DrawDate=DateTime.ParseExact("09/13/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="09-27-44-49-11-24",DrawDate=DateTime.ParseExact("09/16/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="09-02-26-49-22-30",DrawDate=DateTime.ParseExact("09/18/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="03-31-28-37-55-25",DrawDate=DateTime.ParseExact("09/20/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="39-52-45-22-11-17",DrawDate=DateTime.ParseExact("09/23/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="30-17-06-12-04-29",DrawDate=DateTime.ParseExact("09/25/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="18-35-36-34-20-21",DrawDate=DateTime.ParseExact("09/27/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="11-40-03-53-44-38",DrawDate=DateTime.ParseExact("09/30/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="47-02-04-44-54-25",DrawDate=DateTime.ParseExact("10/02/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="50-39-30-40-33-18",DrawDate=DateTime.ParseExact("10/04/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="46-29-17-54-30-50",DrawDate=DateTime.ParseExact("10/07/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="03-30-05-06-02-14",DrawDate=DateTime.ParseExact("10/09/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="14-07-37-20-47-43",DrawDate=DateTime.ParseExact("10/11/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="55-37-09-26-10-43",DrawDate=DateTime.ParseExact("10/14/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="11-48-25-02-30-38",DrawDate=DateTime.ParseExact("10/16/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="12-34-03-29-15-40",DrawDate=DateTime.ParseExact("10/18/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="04-42-44-49-39-21",DrawDate=DateTime.ParseExact("10/21/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="54-49-42-39-19-35",DrawDate=DateTime.ParseExact("10/23/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="51-50-27-49-11-24",DrawDate=DateTime.ParseExact("10/25/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="44-17-07-49-35-14",DrawDate=DateTime.ParseExact("10/28/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="14-32-54-44-52-47",DrawDate=DateTime.ParseExact("10/30/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="46-43-06-30-15-01",DrawDate=DateTime.ParseExact("11/01/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="33-18-47-26-24-20",DrawDate=DateTime.ParseExact("11/04/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="33-25-10-14-18-51",DrawDate=DateTime.ParseExact("11/06/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="21-33-32-54-01-52",DrawDate=DateTime.ParseExact("11/08/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="52-08-41-11-27-31",DrawDate=DateTime.ParseExact("11/11/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="48-02-54-41-19-35",DrawDate=DateTime.ParseExact("11/13/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="52-05-06-38-27-53",DrawDate=DateTime.ParseExact("11/15/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="42-52-49-50-32-08",DrawDate=DateTime.ParseExact("11/18/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="38-17-27-33-42-48",DrawDate=DateTime.ParseExact("11/20/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="06-10-02-17-23-51",DrawDate=DateTime.ParseExact("11/22/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="11-26-46-05-38-50",DrawDate=DateTime.ParseExact("11/25/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="51-32-11-19-40-10",DrawDate=DateTime.ParseExact("11/27/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="38-18-45-20-31-51",DrawDate=DateTime.ParseExact("11/29/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="29-30-11-09-46-32",DrawDate=DateTime.ParseExact("12/02/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="46-26-37-01-27-08",DrawDate=DateTime.ParseExact("12/04/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="04-32-52-06-46-26",DrawDate=DateTime.ParseExact("12/06/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="24-32-05-07-09-44",DrawDate=DateTime.ParseExact("12/09/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="33-03-55-41-01-09",DrawDate=DateTime.ParseExact("12/11/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="55-48-05-16-40-24",DrawDate=DateTime.ParseExact("12/13/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="25-20-24-28-06-09",DrawDate=DateTime.ParseExact("12/16/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="48-54-37-06-53-16",DrawDate=DateTime.ParseExact("12/18/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="41-35-44-50-51-32",DrawDate=DateTime.ParseExact("12/20/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="25-21-07-49-32-22",DrawDate=DateTime.ParseExact("12/23/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="27-05-51-43-54-12",DrawDate=DateTime.ParseExact("12/27/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="22-05-51-48-01-25",DrawDate=DateTime.ParseExact("12/30/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="52-04-24-42-26-39",DrawDate=DateTime.ParseExact("01/03/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="16-47-23-40-54-06",DrawDate=DateTime.ParseExact("01/06/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="08-18-24-17-28-04",DrawDate=DateTime.ParseExact("01/08/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="50-53-22-32-07-42",DrawDate=DateTime.ParseExact("01/10/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="11-28-53-54-03-37",DrawDate=DateTime.ParseExact("01/13/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="45-35-55-47-37-42",DrawDate=DateTime.ParseExact("01/15/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new GrandLotto655{Combination="24-50-52-09-51-03",DrawDate=DateTime.ParseExact("01/17/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},


            };

            var ultraLotto658 = new List<UltraLotto658>
            {
                new UltraLotto658{Combination="02-44-55-51-04-30",DrawDate=DateTime.ParseExact("09/01/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="51-08-56-18-37-09",DrawDate=DateTime.ParseExact("09/03/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="57-08-24-22-01-26",DrawDate=DateTime.ParseExact("09/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="40-52-03-54-14-35",DrawDate=DateTime.ParseExact("09/08/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="58-02-15-35-28-12",DrawDate=DateTime.ParseExact("09/10/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="09-46-08-53-30-47",DrawDate=DateTime.ParseExact("09/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="28-41-25-29-54-37",DrawDate=DateTime.ParseExact("09/15/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="27-16-21-31-56-01",DrawDate=DateTime.ParseExact("09/17/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="07-10-09-13-21-19",DrawDate=DateTime.ParseExact("09/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="10-12-31-36-58-19",DrawDate=DateTime.ParseExact("09/22/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="30-20-27-28-38-42",DrawDate=DateTime.ParseExact("09/24/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="56-14-25-23-22-40",DrawDate=DateTime.ParseExact("09/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="04-42-22-10-21-16",DrawDate=DateTime.ParseExact("09/29/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="55-34-30-25-43-08",DrawDate=DateTime.ParseExact("10/01/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="45-32-11-19-35-34",DrawDate=DateTime.ParseExact("10/03/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="19-30-06-29-43-48",DrawDate=DateTime.ParseExact("10/06/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="19-16-33-13-10-47",DrawDate=DateTime.ParseExact("10/08/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="16-57-27-28-24-47",DrawDate=DateTime.ParseExact("10/10/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="34-51-20-55-23-02",DrawDate=DateTime.ParseExact("10/13/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="56-53-40-06-35-42",DrawDate=DateTime.ParseExact("10/15/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="51-48-44-05-14-12",DrawDate=DateTime.ParseExact("10/17/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="25-54-48-50-37-29",DrawDate=DateTime.ParseExact("10/20/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="11-37-44-33-13-28",DrawDate=DateTime.ParseExact("10/22/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="06-25-30-31-07-12",DrawDate=DateTime.ParseExact("10/24/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="33-22-45-11-16-02",DrawDate=DateTime.ParseExact("10/27/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="51-34-24-02-53-06",DrawDate=DateTime.ParseExact("10/29/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="39-37-08-47-51-21",DrawDate=DateTime.ParseExact("10/31/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="58-37-47-31-10-32",DrawDate=DateTime.ParseExact("11/03/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="11-17-45-38-09-48",DrawDate=DateTime.ParseExact("11/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="24-46-08-26-57-33",DrawDate=DateTime.ParseExact("11/07/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="45-55-40-04-08-44",DrawDate=DateTime.ParseExact("11/10/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="42-18-56-33-20-17",DrawDate=DateTime.ParseExact("11/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="55-27-56-39-44-57",DrawDate=DateTime.ParseExact("11/14/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="45-10-02-20-28-58",DrawDate=DateTime.ParseExact("11/17/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="44-55-43-01-23-26",DrawDate=DateTime.ParseExact("11/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="20-35-27-07-19-33",DrawDate=DateTime.ParseExact("11/21/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="20-14-55-22-04-58",DrawDate=DateTime.ParseExact("11/24/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="39-09-40-01-55-14",DrawDate=DateTime.ParseExact("11/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="24-45-42-05-53-28",DrawDate=DateTime.ParseExact("11/28/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="03-09-44-21-34-26",DrawDate=DateTime.ParseExact("12/01/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="03-11-36-16-44-49",DrawDate=DateTime.ParseExact("12/03/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="21-04-23-12-32-35",DrawDate=DateTime.ParseExact("12/05/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="34-38-20-43-15-28",DrawDate=DateTime.ParseExact("12/08/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="37-48-19-15-57-39",DrawDate=DateTime.ParseExact("12/10/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="03-36-44-01-14-11",DrawDate=DateTime.ParseExact("12/12/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="11-18-52-58-54-08",DrawDate=DateTime.ParseExact("12/15/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="52-07-09-29-37-38",DrawDate=DateTime.ParseExact("12/17/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="33-30-43-32-27-57",DrawDate=DateTime.ParseExact("12/19/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="46-43-44-16-50-21",DrawDate=DateTime.ParseExact("12/22/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="05-06-25-13-17-16",DrawDate=DateTime.ParseExact("12/24/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="42-49-16-09-06-47",DrawDate=DateTime.ParseExact("12/26/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="19-35-25-42-58-05",DrawDate=DateTime.ParseExact("12/29/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="44-24-30-31-01-32",DrawDate=DateTime.ParseExact("12/31/2023","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="58-07-40-09-56-29",DrawDate=DateTime.ParseExact("01/02/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="46-54-05-35-04-13",DrawDate=DateTime.ParseExact("01/05/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="37-28-08-18-40-38",DrawDate=DateTime.ParseExact("01/07/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="10-34-39-45-52-24",DrawDate=DateTime.ParseExact("01/09/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="06-44-20-22-37-25",DrawDate=DateTime.ParseExact("01/12/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="23-40-08-42-45-55",DrawDate=DateTime.ParseExact("01/14/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},
                new UltraLotto658{Combination="39-45-18-26-58-05",DrawDate=DateTime.ParseExact("01/16/2024","MM/dd/yyyy",CultureInfo.InvariantCulture)},


            };

            await DataRepository.AddAsync(lotto642).ConfigureAwait(false);
            await DataRepository.AddAsync(megaLotto645).ConfigureAwait(false);
            await DataRepository.AddAsync(superLotto649).ConfigureAwait(false);
            await DataRepository.AddAsync(grandLotto655).ConfigureAwait(false);
            await DataRepository.AddAsync(ultraLotto658).ConfigureAwait(false);
        }

    }
}
