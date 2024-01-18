using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LuckyPick.Models
{
    public class LottoModel
    {
        public List<string> Lotto642 { get; set; }

        public List<string> MegaLotto645 { get; set; }

        public List<string> SuperLotto649 { get; set; }

        public List<string> UltraLotto658 { get; set; }

        public List<string> GrandLotto655 { get; set; }
    }
}
