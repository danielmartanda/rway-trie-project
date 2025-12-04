/* 
================================================================
Course: COIS 3020 - Data Structures and Algorithms II
Assignment 3: R-Way Trie Data Structure
Date: December 2025

Project Description: 
This program implements an R-Way trie data structure for storing and retrieving strings over the 
lowercase alphabet. The trie supports insertion and lookup operations, and includes functionality 
for generating all words that share a given prefix. The program loads words from an input file, 
constructs the trie, and allows the user to query prefixes to retrieve matching dictionary entries. 

Authors:
1. Ussanth Balasingam - Student ID: 0765174
2. Daniel Martanda - Student ID: 0813510

================================================================
*/

using System;
using System.Xml;
using System.Collections.Generic;

namespace RWayTrieProject
{

    /* Program Class - Main Method
        -> Responsible for:
            - Creating an instance of RWayTrie
            - Reading a file to populate the RWayTrie (through BuildFromTextFile)
            - Prompting the user for prefixes to search
            - Displaying all matching words returned (through PrefixMatch)
    */
    class Program
    {
        static void Main(string[] args)
        {
            //Creates an instance of trie data structure
            RWayTrie trie = new RWayTrie();

            Console.Clear();        //Clears the console for clean output

            //------INSERT AND SEARCH TESTING------
            //Inserting 3 valid words
            Console.WriteLine("Running valid insertion/search tests");

            Console.WriteLine("-> Inserting 'luffy', 'roronoa', 'lebron'");
            trie.Insert("luffy");
            trie.Insert("roronoa");
            trie.Insert("lebron");

            Console.WriteLine($"-> Searching for (\"luffy\"): {trie.Search("luffy")}");
            Console.WriteLine($"-> Searching for (\"roronoa\"): {trie.Search("roronoa")}");
            Console.WriteLine($"-> Searching for (\"lebron\"): {trie.Search("lebron")}");
            Console.WriteLine();

            //Inserting 3 invalid words
            Console.WriteLine("Running invalid insertion/search tests");

            Console.WriteLine("-> Inserting 'y2k', '21kid', '@gmail.com'");
            trie.Insert("y2k");
            trie.Insert("21kid");
            trie.Insert("@gmail.com");

            Console.WriteLine($"-> Searching for (\"y2k\"): {trie.Search("y2k")}");
            Console.WriteLine($"-> Searching for (\"21kid\"): {trie.Search("21kid")}");
            Console.WriteLine($"-> Searching for (\"@gmail.com\"): {trie.Search("@gmail.com")}");
            Console.WriteLine();
            //--------------------------------------

            //------EMPTY TRIE TESTING------
            Console.WriteLine("Running edge case tests");
            Console.WriteLine($"-> Searching for (\"AppleDevice\") in empty trie: {trie.Search("AppleDevices")}");
            Console.WriteLine();
            //--------------------------------------

            try
            {
                //Loads word from a text file
                trie.BuildFromTextFile(@"../../../words.txt");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Program cannot continue without the word text file. Press any key to exit.");
                Console.ReadKey();
                return;         //Exit from the main, do not enter the user input loop
            }
            
            //Loop to prompt user to search for prefixes
            while (true)
            {
                Console.WriteLine();
                Console.Write("\nEnter a prefix (or type 'exit' to quit): ");
                string prefix = Console.ReadLine();         //Stores the entered prefix

                //Checks if user wants to exit the program
                if (prefix.ToLower() == "exit")
                {
                    break;          //Terminates the loop
                }

                //Stores the matching words in a list returned from PrefixMatch
                List<string> results = trie.PrefixMatch(prefix);

                Console.WriteLine($"\nWords matching the prefix \"{prefix}\":");

                //Iterates through all matching words and prints them
                foreach (string word in results)
                {
                    Console.Write($" - {word}");
                }

                //If no words were found, display message to user
                if (results.Count == 0)
                {
                    Console.WriteLine("Sorry, no matches were found.");
                }

            }

            //Message to display when program ends
            Console.WriteLine("\nProgram complete. Exiting now...");

        }
    }

}
