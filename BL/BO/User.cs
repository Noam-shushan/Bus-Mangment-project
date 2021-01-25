using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class User
    {
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public bool Admin { get; set; }
        public bool IsDeleted { get; set; }
    }
}
