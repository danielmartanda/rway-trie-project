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
using System.Collections.Generic;
using System.IO;

namespace RWayTrieProject
{
    /* RWay Trie Class 
        -> Represents the R-way Trie data structure for lowercase a-z words
        -> Provides efficient storage and quick search for strings and often used to implement auto-complete and predictive text input
        -> Responsible for:
            - Inserting words into the trie (Insert)
            - Searching for words in the trie (Search)
            - Retrieving all words that share a given prefix (PrefixMatch) 
            - Building the trie from a text file (BuildFromTextFile)
    */

    public class RWayTrie
    {
        private RWayTrieNode root;          //Reference to the root node of the trie

        //Constructor
        public RWayTrie()
        {
            root = new RWayTrieNode();      //Root node uses a null placeholder character and doesnt represent a real character
        }

        /* Method 1: Insertion
            - Parameter:
                -> string: The word to be inserted into the Trie
            - Error Handling: 
                -> Ignores null/whitespace input
                -> Ignores words that contain characters not a-z, like numbers and special characters
            
            Pseudocode:
                -> Start at the root
                -> For each character in the word,
                    1. Convert it to lowercase 
                    2. Store in its respective numeric array index (0 to 25 corresponds to characters a to z)
                    3. If the corresponding child is null, then it doesn't exist, so create a new node for that character
                    4. Move down one level to the child node
                -> After the last character, mark the node as the end of the word */
        public void Insert(string word)
        {
            //Error handling for empty or invalid input
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.WriteLine($"Invalid word: Could not insert '{word}'");
                return;
            }

            RWayTrieNode current = root;    //Starts the traversal from the root
            word = word.ToLower();          //Normalizes input to lowercase characters

            //Iterates over every letter in the word
            foreach (char character in word)
            {
                //Maps the characters a to z to index 0 to 25 by subtracting the ASCII values of the characters
                int index = character - 'a';

                //Checks if the word contains only letters from a to z (which holds index locations 0 to 25)
                if (index < 0 || index >= 26)
                {
                    Console.WriteLine($"Invalid word: Could not insert '{word}'");
                    return;
                }

                //Checks if there is an existing child node for the letter, and creates one if it doesn't exist
                if (current.Children[index] == null)            
                {
                    current.Children[index] = new RWayTrieNode(character);      //Creates one with its character
                }

                //Moves a level deeper in the trie for the following letters in the word
                current = current.Children[index];
            }

            //Marks the last node as the end of the word
            current.IsEndOfWord = true;     
        }

        /* Method 2: Search
            - Parameter:
                -> string: The word to be searched inside the Trie
            - Output:
                -> true if it exists, false if otherwise
            - Error Handling: 
                -> Returns false for null/whitespace input
                -> Returns false if the word contain characters not a-z, like numbers and special characters
            
            Pseudocode:
                -> Start at the root
                -> For each character in the word,
                    1. Convert it to lowercase 
                    2. Determine its respective numeric array index (0 to 25 corresponds to characters a to z)
                    3. If the corresponding child is null, then the word doesn't exist, so return false
                    4. Otherwise, move down one level to the child node
                -> After the last character, return true if IsEndOfWord is true */
        public bool Search(string word)
        {
            //Error handling for empty or invalid input
            if (string.IsNullOrWhiteSpace(word))
            {
                return false;
            }

            RWayTrieNode current = root;    //Starts the traversal from the root
            word = word.ToLower();          //Normalizes input to lowercase characters

            //Iterates over every letter in the word
            foreach (char character in word)
            {
                //Maps the characters a to z to index 0 to 25 by subtracting the ASCII values of the characters
                int index = character - 'a';

                //Checks if the word contains only letters from a to z (which holds index locations 0 to 25)
                if (index < 0 || index >= 26)
                {
                    return false;        //Invalid characters entered
                }

                //Checks if there is an existing child node for the letter, null means the word doesnt exist
                if (current.Children[index] == null)            
                {
                    return false;        //Word doesn't exist
                }

                //Moves a level deeper in the trie for the following letters in the word
                current = current.Children[index];

            }

            //The word only exists if the final node is marked as IsEndOfWord
            return current.IsEndOfWord;

        }

        /* Method 3: PrefixMatch
            - Returns a list of words in the trie that match a given prefix
            - Parameter:
                -> string prefix: The prefix to be searched inside the Trie
            - Output:
                -> List<string> containing all valid words that match the prefix
            - Error Handling: 
                -> Returns an empty list for null/whitespace input
                -> Returns an empty list if the prefix contains characters not a-z, like numbers and special characters
            
            Pseudocode:
                -> Start at the root
                -> For each character in the prefix,
                    1. Convert it to lowercase 
                    2. Determine its respective numeric array index (0 to 25 corresponds to characters a to z)
                    3. If the corresponding child is null, then the word doesn't exist, so return an empty list
                    4. Otherwise, move down one level to the child node
                -> Once the prefix node is reached, perform a DFS:
                    1. Track the growing word using a StringBuilder or string path
                    2. Each time a node with IsEndOfWord == true is visited, add the word to the result list
                -> Return the list of matched words
        */






        /* Method 4: BuildFromTextFile
            - Builds the trie using all words found inside a given text file
            - Parameter:
                -> string filePath: The path to the text file containing one word per line
            - Output:
                -> No return value (void), but the trie will be filled with all valid words
            - Error Handling: 
                -> If the file cannot be opened, display an error message
                -> Ignores blank lines or lines containing invalid characters
            
            Pseudocode:
                -> Attempt to open the file
                -> While not at the end of the file:
                    1. Read a line
                    2. Trim the string and ensure it is not empty
                    3. Call Insert(word) to store it in the trie
                -> Close the file
        */



    }

}
