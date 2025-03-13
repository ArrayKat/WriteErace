using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteErace.Models
{
    public partial class User
    {
        public string FIO => Surname==null || Name==null || Patronymic== null ? "Гость" : Surname + " " + Name + " " + Patronymic ;
    }
}
