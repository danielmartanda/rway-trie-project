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

            //Loads word from a text file
            trie.BuildFromTextFile(@"../../../words.txt");

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
