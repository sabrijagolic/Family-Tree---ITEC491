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
        static List<Person> AllFamilyMembers;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Family Tree creator");
            Console.WriteLine("Do you want to crate new family tree or work with existing one ?");
            int choice = 0;
            do
            {
                Console.WriteLine("1. Create new family tree");
                Console.WriteLine("2. Open existing one");
                Console.WriteLine("0. Exit");
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch
                {
                    choice = 3;
                }

            } while (choice != 1 && choice != 2 && choice != 0);


           
            Console.ReadKey();
        }
        private static void SearchFamilyTree()
        {
            Person person = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(File.ReadAllText(@"C:\Users\Ryker\source\repos\SabrijaGolicFamilyTree\SabrijaGolicFamilyTree\family.json"));
            int level = 0;
            Console.Write("Family title: ");
            string t = Console.ReadLine();
            Console.Write("Name: ");
            string n = Console.ReadLine();
            Console.Write("Date of birth: ");
            string d = Console.ReadLine();
            EditFamilyNode(person, level, n, d, t);
            string json = JsonConvert.SerializeObject(person);
            System.IO.File.WriteAllText(@"C:\Users\Ryker\source\repos\SabrijaGolicFamilyTree\SabrijaGolicFamilyTree\family.json", json);
            DisplayFamilyTree();



        }
        private static void DisplayFamilyTree()
        {
            AllFamilyMembers = new List<Person>();
            Person person = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(File.ReadAllText(@"C:\Users\Ryker\source\repos\SabrijaGolicFamilyTree\SabrijaGolicFamilyTree\family.json"));
            ShowFamily(person, 0);
            int min;
            Person temp;
            for (int i = 0; i < AllFamilyMembers.Count - 1; i++) //selection sort to order family members from oldest to youngest
            {
                min = i;
                for (int j = i + 1; j < AllFamilyMembers.Count; j++)
                {//we comapre year of birth which is last 4 characters of birthdate
                    if (String.Compare(AllFamilyMembers[j].BirthDate.Substring(AllFamilyMembers[j].BirthDate.Length - 4), AllFamilyMembers[min].BirthDate.Substring(AllFamilyMembers[min].BirthDate.Length - 4)) == -1)
                    {
                        min = j;
                    }
                }
                if (min != i)
                {
                    temp = AllFamilyMembers[i];
                    AllFamilyMembers[i] = AllFamilyMembers[min];
                    AllFamilyMembers[min] = temp;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            foreach (var member in AllFamilyMembers)
            {
                Console.WriteLine(member.Name + " - " + member.BirthDate);
            }
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
            for (int i = 0; i < numberOfChildren; i++)
            {
                Console.WriteLine("Kid number - " + i + 1);
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

        private static void ShowFamily(Person person, int level)
        {
            AllFamilyMembers.Add(person);
            if (person.Spouse != null)
            {
                Console.WriteLine("".PadLeft(level * 4) + (person.Children.Any() ? "* " : "- ") + person.Title + ": " + person.Name + " (" + person.BirthDate + ")" + " and " + person.Spouse.Title + ": " + person.Spouse.Name + " (" + person.Spouse.BirthDate + ")");
            }
            else
            {
                Console.WriteLine("".PadLeft(level * 4) + (person.Children.Any() ? "* " : "- ") + person.Title + ": " + person.Name + " (" + person.BirthDate + ")");
            }
            foreach (var c in person.Children)
            {
                ShowFamily(c, level + 1);
            }
        }

        private static void EditFamilyNode(Person person, int level, string name, string date, string title)
        {
            if (person.Name.Equals(name) && person.Title.Equals(title) && person.BirthDate.Equals(date))
            {
                Console.WriteLine("Person FOUND");
                Console.WriteLine("You are editing - " + person.Title + ": " + person.Name + " (" + person.BirthDate + ") - " + AliveStatus(person.IsAlive));
                Console.Write("Family title: ");
                person.Title = Console.ReadLine();
                Console.Write("Name: ");
                person.Name = Console.ReadLine();
                Console.Write("Date of birth: ");
                person.BirthDate = Console.ReadLine();
                Console.Write("Alive? true/false: ");
                person.IsAlive = bool.Parse(Console.ReadLine());
            }
            if (person.Spouse != null)
            {
                if (person.Spouse.Name.Equals(name) && person.Spouse.Title.Equals(title) && person.Spouse.BirthDate.Equals(date))
                {
                    Console.WriteLine("Person FOUND");
                    Console.WriteLine("Spouse of " + person.Name);
                    Console.WriteLine("You are editing - " + person.Spouse.Title + ": " + person.Spouse.Name + " (" + person.Spouse.BirthDate + ") - " + AliveStatus(person.Spouse.IsAlive));
                    Console.Write("Family title: ");
                    person.Spouse.Title = Console.ReadLine();
                    Console.Write("Name: ");
                    person.Spouse.Name = Console.ReadLine();
                    Console.Write("Date of birth: ");
                    person.Spouse.BirthDate = Console.ReadLine();
                    Console.Write("Alive? true/false: ");
                    person.Spouse.IsAlive = bool.Parse(Console.ReadLine());
                }
            }
            
            foreach (Person c in person.Children)
            {
                EditFamilyNode(c, level + 1, name, date, title);
            }
        }

        private static string AliveStatus(bool alive)
        {
            if (alive)
                return "Alive";
            else
                return "Dead";
        }
    }
}
