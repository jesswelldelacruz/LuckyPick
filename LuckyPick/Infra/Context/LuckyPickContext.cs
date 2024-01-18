using LuckyPick.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyPick.Infra.Context
{
    public class LuckyPickContext : DbContext
    {
        public DbSet<_Consecutive> _Consecutives { get; set; }
        public DbSet<_Consecutive_2> _Consecutives_2 { get; set; }
        public DbSet<_Consecutive_3> _Consecutives_3 { get; set; }
        public DbSet<_Consecutive_4> _Consecutives_4 { get; set; }
        public DbSet<_Consecutive_5> _Consecutives_5 { get; set; }
        public DbSet<_Consecutive_6> _Consecutives_6 { get; set; }
        public DbSet<_Consecutive_7> _Consecutives_7 { get; set; }
        public DbSet<_Consecutive_8> _Consecutives_8 { get; set; }
        public DbSet<_Consecutive_9> _Consecutives_9 { get; set; }
        public DbSet<_Consecutive_10> _Consecutives_10 { get; set; }
        public DbSet<_Consecutive_11> _Consecutives_11 { get; set; }
        public DbSet<_Consecutive_12> _Consecutives_12 { get; set; }
        public DbSet<_Consecutive_13> _Consecutives_13 { get; set; }
        public DbSet<_Consecutive_14> _Consecutives_14 { get; set; }
        public DbSet<_SameDigit> _SameDigit { get; set; }
        public DbSet<_SameLastDigit> _SameLastDigit { get; set; }
        public DbSet<Lotto642> Lotto642 { get; set; }
        public DbSet<MegaLotto645> MegaLotto645 { get; set; }
        public DbSet<SuperLotto649> SuperLotto649 { get; set; }
        public DbSet<UltraLotto658> UltraLotto658 { get; set; }
        public DbSet<GrandLotto655> GrandLotto655 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=LuckyPick;Integrated Security=True; TrustServerCertificate=True;Trusted_Connection=True;");
        }

    }
}
