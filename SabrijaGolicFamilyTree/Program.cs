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
            while (ApplicationRunning()); //Untill function returns false application will keep running         
            
        }

        private static bool ApplicationRunning()
        {            
            int choice = 0;
            do //repeats untill user enters a correct number
            {
                Console.Clear();
                Console.WriteLine("Do you want to crate new family tree or work with existing one ?");
                Console.WriteLine("1. Create new family tree");
                Console.WriteLine("2. Open existing one");
                Console.WriteLine("0. Exit");
                Console.Write("Input: ");
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch
                {
                    choice = 3;
                }

            } while (choice != 1 && choice != 2 && choice != 0);
            if (choice == 1)
            {
                CreateFamilyTree();
                return true; //returns true after we create tree so application keeps running
            }
            else if (choice == 2)
            {
                do //repeats untill user enters a correct number
                {
                    Console.Clear();
                    Console.WriteLine("1. Display a family tree");
                    Console.WriteLine("2. Edit a famly tree");
                    Console.WriteLine("0. Back");
                    Console.Write("Input: ");
                    try
                    {
                        choice = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        choice = 3;
                    }

                } while (choice != 1 && choice != 2 && choice != 0);
                if(choice == 1)
                {
                    DisplayFamilyTree("",false);  //calls for method to display tree
                    Console.WriteLine("Press ANY key to continue");
                    Console.ReadKey();
                } else if(choice == 2)
                {
                    SearchFamilyTree(); //calls for method to search node to be edited
                    Console.WriteLine("Press ANY key to continue");
                    Console.ReadKey();
                }              
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void CreateFamilyTree()
        {
            Person person = new Person();//we create first person in tree from whom we start
            Console.Write("Family title: ");
            person.Title = Console.ReadLine();
            Console.Write("Name: ");
            person.Name = Console.ReadLine();
            Console.Write("Date of birth: ");
            person.BirthDate = Console.ReadLine();
            Console.Write("Alive? true/false: ");
            person.IsAlive = bool.Parse(Console.ReadLine());
            Console.WriteLine("Is " + person.Name + " married? true/false");
            if (bool.Parse(Console.ReadLine()))  //if person is married we add same information about his wife
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
            person.Children = AddChildern();  //we call recursive method to add his children
            string json = JsonConvert.SerializeObject(person);
            string path = "";
            do {
                Console.Clear();
                Console.WriteLine("Enter path and filename where you want to save your tree");
                Console.Write("Path and filename: ");
                path = Console.ReadLine();
                try
                {
                    File.WriteAllText(path, json);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press ANY key to try again");
                    Console.ReadKey();
                }
            } while (File.Exists(path) == false); //we prompt user to enter enter path until correct so he doesnt loose his tree
        }

        private static List<Person> AddChildern()  //recurisve method to add children
        {
            List<Person> children = new List<Person>();
            Console.Write("How many children: ");
            int numberOfChildren = int.Parse(Console.ReadLine());
            for (int i = 0; i < numberOfChildren; i++)
            {
                Console.WriteLine("Kid number - " + (i + 1));
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
                person.Children = AddChildern(); //adding children using recursion
                children.Add(person);
            }
            return children;
        }

        private static void DisplayFamilyTree(string p, bool gotFromEdit)
        {
            AllFamilyMembers = new List<Person>();            
            Person person = null;
            string path;
            if (gotFromEdit)
            {
                path = p;
                person = JsonConvert.DeserializeObject<Person>(File.ReadAllText(path)); //we load json file into person object
            }
            else { 
            do
            {
                Console.Clear();
                Console.WriteLine("Enter the path and filename of family tree .json file");
                Console.Write("Path and filename: ");
                path = Console.ReadLine();
                try
                {
                    person = JsonConvert.DeserializeObject<Person>(File.ReadAllText(path)); //we load json file into person object

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press ANY key to try again");
                    Console.ReadKey();
                }
            } while (person == null);
            }
            Console.Clear();
            ShowFamily(person, 0); //we call recurisve method to print out each person in family tree
            int min;
            Person temp;
            for (int i = 0; i < AllFamilyMembers.Count - 1; i++) //selection sort to order family members from oldest to youngest
            {
                min = i;
                for (int j = i + 1; j < AllFamilyMembers.Count; j++)
                {//we comapre year of birth which is last 4 characters of birthdate
                    if (string.Compare(AllFamilyMembers[j].BirthDate.Substring(AllFamilyMembers[j].BirthDate.Length - 4), AllFamilyMembers[min].BirthDate.Substring(AllFamilyMembers[min].BirthDate.Length - 4)) == -1)
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

            foreach (var member in AllFamilyMembers) //prints out all family members sorted by age
            {
                Console.WriteLine(member.Name + " - " + member.BirthDate);
            }
        }

        private static void ShowFamily(Person person, int level) //recurisve method to print each member in tree
        {
            AllFamilyMembers.Add(person);
            if (person.Spouse != null)
            {   //different print method if person is married
                Console.WriteLine("".PadLeft(level * 4) + (person.Children.Any() ? "* " : "- ") + person.Title + ": " + person.Name + " (" + person.BirthDate + ")" + " and " + person.Spouse.Title + ": " + person.Spouse.Name + " (" + person.Spouse.BirthDate + ")");
            }
            else
            {   //and not married
                Console.WriteLine("".PadLeft(level * 4) + (person.Children.Any() ? "* " : "- ") + person.Title + ": " + person.Name + " (" + person.BirthDate + ")");
            }
            foreach (var c in person.Children)
            {
                ShowFamily(c, level + 1); //level is used to indent children
            }
        }

        private static void SearchFamilyTree()
        {            
            Person person = null;
            string path;
            do
            {
                Console.Clear();
                Console.WriteLine("Enter the path and filename of family tree .json file");
                Console.Write("Path and filename: ");
                path = Console.ReadLine();
                try
                {
                    person = JsonConvert.DeserializeObject<Person>(File.ReadAllText(path)); //we load json file into person object

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press ANY key to try again");
                    Console.ReadKey();
                }
            } while (person == null);
            Console.WriteLine("Enter search parameters");
            Console.Write("Family title: "); //we enter 3 search parameters, 3 are enough to make each person unique
            string t = Console.ReadLine();
            Console.Write("Name: ");
            string n = Console.ReadLine();
            Console.Write("Date of birth: ");
            string d = Console.ReadLine();
            EditFamilyNode(person, n, d, t); //we call reursive method which we begin search in family tree
            string json = JsonConvert.SerializeObject(person);  //after we made adjustments we just update the .json file with new data
            File.WriteAllText(path, json);
            DisplayFamilyTree(path, true);
        } 

        private static void EditFamilyNode(Person person, string name, string date, string title) //recurisve method to go thru family tree and match our search parameters
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
            
            foreach (Person children in person.Children)
            {
                EditFamilyNode(children, name, date, title); //using recurison we go thru family tree
            }
        }

        private static string AliveStatus(bool alive) //based on bool variable retruns string
        {
            if (alive)
                return "Alive";
            else
                return "Dead";
        }
    }
}
