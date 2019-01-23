using Newtonsoft.Json;
using SabrijaGolicFamilyTree.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SabrijaGolicFamilyTree
{
    class Program
    {
        static void Main(string[] args)
        {

            Person person = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(File.ReadAllText(@"C:\Users\Ryker\source\repos\SabrijaGolicFamilyTree\SabrijaGolicFamilyTree\family.json"));
            ShowFamily(person);


            Console.ReadKey();
        }
        private static void CreateFamilyTree()
        {
            Person person = new Person();
            Console.Write("Family title: ");
            person.Title = Console.ReadLine();
            Console.Write("Name: ");
            person.Name = Console.ReadLine();
            Console.Write("Date of birth: ");
            person.BirthDate = Console.ReadLine();
            Console.Write("Alive? true/false: ");
            person.IsAlive = bool.Parse(Console.ReadLine());
            Console.WriteLine("Is " + person.Name + " married? true/false");
            if (bool.Parse(Console.ReadLine()))
            {
                Console.WriteLine("Spuse of " + person.Name);
                Spouse spouse = new Spouse();
                Console.Write("Family title: ");
                spouse.Title = Console.ReadLine();
                Console.Write("Name: ");
                spouse.Name = Console.ReadLine();
                Console.Write("Date of birth: ");
                spouse.BirthDate = Console.ReadLine();
                Console.Write("Alive? true/false: ");
                spouse.IsAlive = bool.Parse(Console.ReadLine());
                person.Spouse = spouse;
            }
            person.Children = AddChildern();           
            string json = JsonConvert.SerializeObject(person);
            System.IO.File.WriteAllText(@"C:\Users\Ryker\Desktop\family.json", json);
        }
        
        private static List<Person> AddChildern()
        {
            List<Person> children = new List<Person>();
            Console.Write("How many children: ");
            int numberOfChildren = int.Parse(Console.ReadLine());
            for (int i = 0; i < numberOfChildren ; i++)
            {                
                Console.WriteLine("Kid number - " + i+1);
                Person person = new Person();
                Console.Write("Family title: ");
                person.Title = Console.ReadLine();
                Console.Write("Name: ");
                person.Name = Console.ReadLine();
                Console.Write("Date of birth: ");
                person.BirthDate = Console.ReadLine();
                Console.Write("Alive? true/false: ");
                person.IsAlive = bool.Parse(Console.ReadLine());
                Console.WriteLine("Is " + person.Name + " married? true/false");
                if (bool.Parse(Console.ReadLine()))
                {
                    Console.WriteLine("Spouse of " + person.Name);
                    Spouse spouse = new Spouse();
                    Console.Write("Family title: ");
                    spouse.Title = Console.ReadLine();
                    Console.Write("Name: ");
                    spouse.Name = Console.ReadLine();
                    Console.Write("Date of birth: ");
                    spouse.BirthDate = Console.ReadLine();
                    Console.Write("Alive? true/false: ");
                    spouse.IsAlive = bool.Parse(Console.ReadLine());
                    person.Spouse = spouse;
                }
                person.Children = AddChildern();
                             
                
                children.Add(person);
            }
            return children;
            
        }

        private static void ShowFamily(Person a)
        {
            ShowFamily(a, 0);
        }

        private static void ShowFamily(Person a, int level)
        {
            if(a.Spouse != null) { 
            Console.WriteLine("".PadLeft(level * 4) + (a.Children.Any() ? "* " : "- ") + a.Title + ": " + a.Name + " (" + a.BirthDate + ")" + " and " + a.Spouse.Title + ": " + a.Spouse.Name + " (" + a.Spouse.BirthDate + ")");
            } else
            {
                Console.WriteLine("".PadLeft(level * 4) + (a.Children.Any() ? "* " : "- ") + a.Title + ": " + a.Name + " (" + a.BirthDate + ")");
            }
            foreach (var c in a.Children)
            {
                ShowFamily(c, level + 1);
            }
        }
    }
}
