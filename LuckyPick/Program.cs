using LuckyPick.Repository;
using LuckyPick.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LuckyPick
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await LuckyPickService.FindExisting();
            await LuckyPickService.AddAsync();

            //await LuckyPickService.LuckyPickSameLastDigit(50, Enum.GameEnum.SuperLotto649).ConfigureAwait(true);
            //await LuckyPickService.Search("42-21-44-10-51-25");

            //var duplicates = LuckyPickService.GetDuplicate(await LuckyPickService.GetAllSortData().ConfigureAwait(true));

            //Console.WriteLine(" ");
            //Console.WriteLine(" ");
            //Console.WriteLine(" ");
            //Console.WriteLine(" ");
            //Console.WriteLine(" - Duplicates - ");
            //foreach (var item in duplicates)
            //{
            //    Console.WriteLine(item);
            //}

            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("LuckyPick v1.3");
            Console.ReadKey();
        }
    }
}
