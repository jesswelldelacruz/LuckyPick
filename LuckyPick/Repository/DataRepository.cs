using LuckyPick.Infra.Context;
using System.Linq;
using LuckyPick.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LuckyPick.Enum;
using LuckyPick.Helper;

namespace LuckyPick.Repository
{
    public class DataRepository
    {
        public async static Task<Dictionary<string, string>> GetAllWithGame<T>(string game) where T : class
        {
            var data = new Dictionary<string, string>();
            using (var context = new LuckyPickContext())
            {
                var result = await context.Set<T>().ToListAsync().ConfigureAwait(true);

                foreach (var item in result)
                {
                    Type t = item.GetType();

                    PropertyInfo prop = t.GetProperty("Combination");

                    object combination = prop.GetValue(item);

                    data.Add(DataHelper.SortDigit((string)combination), game);
                }

                return data;
            }
        }


        public async static Task<List<string>> GetAll<T>() where T : class
        {
            var data = new List<string>();
            using (var context = new LuckyPickContext())
            {
                var result = await context.Set<T>().ToListAsync().ConfigureAwait(true);

                foreach (var item in result)
                {
                    Type t = item.GetType();

                    PropertyInfo prop = t.GetProperty("Combination");

                    object combination = prop.GetValue(item);

                    data.Add(DataHelper.SortDigit((string)combination));
                }

                return data;
            }
        }

        public async static Task AddAsync<T>(List<T> lottoList) where T: class
        {
            try
            {
                using (var context = new LuckyPickContext())
                {
                    var selected = new List<T>();
                    foreach (var item in lottoList)
                    {
                        Type t = item.GetType();

                        PropertyInfo prop = t.GetProperty("Combination");

                        object combination = prop.GetValue(item);

                        var res = await context.Set<T>().FindAsync(combination).ConfigureAwait(true);

                        if (res == null)
                        {
                            selected.Add(item);
                        }
                        else
                        {
                            Console.WriteLine($"{combination} is already exist");
                        }
                    }
                    if (selected.Count > 0)
                    {
                        await context.Set<T>().AddRangeAsync(selected);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
