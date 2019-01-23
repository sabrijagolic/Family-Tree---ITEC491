using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabrijaGolicFamilyTree.Model
{
    public class Person : Spouse
    {
        
        public List<Person> Children { get; set; }
        public Spouse Spouse { get; set; }
        

    }
}
