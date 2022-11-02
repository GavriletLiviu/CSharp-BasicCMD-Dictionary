using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            string password;
            Console.WriteLine("Please input password");
            password = Console.ReadLine();
            string[,] DictionaryDatabase;
            ReadDictionaryDatabase(out DictionaryDatabase);
            if (!IsAdmin(password))
            {
                string searchedWord;
                do
                {
                    Console.WriteLine("Please input word you wish to search for ( type x to exit app)");
                    searchedWord = Console.ReadLine();
                    searchedWord = searchedWord.ToLower();
                    History(searchedWord);
                    string answer = SearchDictionaryDatabase(DictionaryDatabase, searchedWord);
                    if (answer != null)
                        Console.WriteLine(answer);
                    else
                        Console.WriteLine("Word could not be found!");
                } while (searchedWord != "x");
            }
            else
            {
                Console.WriteLine("Please input word you wish to add");
                string word = Console.ReadLine();
                Console.WriteLine("Please input definition");
                string definition = Console.ReadLine();
                if (AddWord(ref DictionaryDatabase, word, definition))
                {
                    Console.WriteLine("Word Added Successfully!");
                    Console.WriteLine("Do you wish to add word to file database as well ?(y/n)");
                    char YesNo = Convert.ToChar(Console.ReadLine());
                    if (YesNo == 'y')
                        AddToLibrary(word, definition);
                }
                else
                {
                    Console.WriteLine("Word already in database!");
                }
            }
        }
        static void History(string word)
        {
            File.AppendAllText("history.txt", $"{word}\n");
        }
        static void AddToLibrary(string word,string definition)
        {
            string addition = $"{word}={definition}";
            File.AppendAllText("Database.txt", addition); 
        }
        static bool AddWord(ref string[,]DictionaryDatabase,string word,string definition)
        {
            if (SearchDictionaryDatabase(DictionaryDatabase,word)==null)
            {
                string[] dictionaryDatabase = File.ReadAllLines("Database.txt");
                DictionaryDatabase = new string[(dictionaryDatabase.Length+1), 2];
                for (int i = 0; i < dictionaryDatabase.Length; i++)
                {
                    string[] dictionarySample = dictionaryDatabase[i].Split('=');
                    DictionaryDatabase[i, 0] = dictionarySample[0];
                    DictionaryDatabase[i, 1] = dictionarySample[1];
                }
                word = DictionaryDatabase[(DictionaryDatabase.GetLength(0)-1), 0];
                definition = DictionaryDatabase[(DictionaryDatabase.GetLength(0)-1), 1];
                return true;
            }
            return false;
        }
        static bool IsAdmin(string password)
        {
            if (password == "admin")
                return true;
            return false;
        }
        static void ReadDictionaryDatabase (out string[,] DictionaryDatabase)
        {
            string []dictionaryDatabase = File.ReadAllLines("Database.txt");
            DictionaryDatabase = new string[dictionaryDatabase.Length, 2];
            for (int i=0; i<dictionaryDatabase.Length;i++)
            {
                string[] dictionarySample = dictionaryDatabase[i].Split('=');
                DictionaryDatabase[i,0] = dictionarySample[0];
                DictionaryDatabase[i, 1] = dictionarySample[1];
            }           
        }
        static string SearchDictionaryDatabase (string[,] DictionaryDatabase,string searchItem)
        {
            string answer = null;
            for ( int i=0;i<DictionaryDatabase.GetLength(0);i++)
            {
                if (searchItem == DictionaryDatabase[i, 0])
                    answer = DictionaryDatabase[i, 1];              
            }
            if (answer == null && searchItem != "x")
                return null;
            else if (searchItem == "x")
                return " ";
            return answer;
        }
    }
}
