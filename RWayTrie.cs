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
using System.Text;

namespace RWayTrieProject
{
    /* RWay Trie Class 
        -> Represents the R-way Trie data structure for lowercase a-z words
        -> Provides efficient storage and quick search for strings and is often used 
            to implement auto-complete and predictive text input
        -> Responsible for:
            - Inserting words into the trie (Insert)
            - Searching for words in the trie (Search)
            - Retrieving all words that share a given prefix (PrefixMatch) 
            - Building the trie from a text file (BuildFromTextFile)
    */

    public class RWayTrie
    {
        private RWayTrieNode root;          //Reference to the root node of the trie

        private int invalidWordCount = 0;   //Tracks how many invalid words were skipped during loading
        private int validWordCount = 0;     //Tracks how many valid words were skipped during loading

        //Constructor
        public RWayTrie()
        {
            root = new RWayTrieNode();      //Root node uses a null placeholder character and doesnt represent a real character
        }

        /* Method 1: Insert
            - Purpose: 
                -> Inserts a lowercase word into the R-Way trie
            - Parameter:
                -> string word: The word to be inserted into the trie
            - Error Handling: 
                -> Ignores null/whitespace input
                -> Silently skips words that contain characters not a-z, to avoid flooding the console
                    Ex. Numbers and special characters    
        */
        public void Insert(string word)
        {
            //Error handling for empty or invalid input
            if (string.IsNullOrWhiteSpace(word))
            {
                //Blank lines are ignored
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
                    //Skips words containing characters not a-z and tracks how many invalid words were skipped
                    invalidWordCount++;
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
            //Increases the valid word count
            validWordCount++; 
        }

        /* Method 2: Search
            - Purpose: 
                -> Checks if a given word is stored as a complete word in the trie
            - Parameter:
                -> string word: The word to be searched inside the trie
            - Output:
                -> true if it exists and ends at a terminal node
                -> false otherwise (including invalid or mixed character words)
            - Error Handling: 
                -> Returns false for null/whitespace input
                -> Returns false if the word contain characters not a-z            
         */
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
            - Purpose:
                -> Returns all words in the trie that match a given prefix
                -> Finds the node that represents the last character of the prefix
                    using TraverseToNode, then performs a DFS from that node using
                    DFSCollect to gather all full words that extend the prefix
            - Parameter:
                -> string prefix: The prefix to be searched inside the trie
            - Output:
                -> List<string> containing all valid words that match the prefix
            - Error Handling: 
                -> Returns an empty list for null/whitespace input
                -> Returns an empty list if the prefix contains characters not a-z, or if the prefix path does not exist 
        */

        public List<string> PrefixMatch(string prefix)
        {
            //Creates a list to store all matching words
            List<string> results = new List<string>();

            //Error handling for null or whitespace prefixes
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return results;         //Returns an empty list
            }

            //Trims and normalizes the prefix to lowercase characters
            prefix = prefix.Trim().ToLower();

            //Traverses the trie to find the node representing the final character of the prefix
            RWayTrieNode prefixNode = TraverseToNode(prefix);

            //If the path for the prefix does not exist, there are no matching words
            if (prefixNode == null)
            {
                return results;
            }

            //Uses a StringBuilder to build words as we traverse the trie
            StringBuilder currentWord = new StringBuilder(prefix);

            //If the prefix is a complete word, add it to the results
            if (prefixNode.IsEndOfWord)
            {
                results.Add(prefix);
            }

            //Performs a depth-first search (DFS) from the prefix node to find all longer words
            DFSCollect(prefixNode, currentWord, results);

            //Returns the completed list of words that match the prefix
            return results;
        }

        /* Method 4: TraverseToNode (Helper)
            - Purpose:
                -> Traverses the trie, following a word or prefix from the root and returns the node reached
            - Parameter:
                -> string text: A word or prefix to follow from the root
            - Output:
                -> The RWayTrieNode corresponding to the last character in the string
                -> Null if the path does not exist or contains invalid characters
        */

        private RWayTrieNode TraverseToNode(string text)
        {
            RWayTrieNode current = root;        //Starts the traversal from the root

            //Error handling for empty strings
            if (string.IsNullOrEmpty(text))
            {
                return current;     //Returns the root node
            }

            //Trims and normalizes the string to lowercase 
            text = text.Trim().ToLower();

            //Iterates over every character in the string
            foreach (char character in text)
            {
                //Maps the characters a to z to index 0 to 25 by subtracting the ASCII values of the characters
                int index = character - 'a';

                //Checks if the word contains only letters from a to z (which holds index locations 0 to 25)
                if (index < 0 || index >= 26)
                {
                    return null;        //Invalid characters, path does not exist
                }

                //Checks if there is an existing child node for the letter 
                if (current.Children[index] == null)            
                {
                    return null;        //No child exists at this index, so path doesn't exist
                }

                //Moves a level deeper in the trie for the following letters in the word
                current = current.Children[index];

            }

            //Returns the node reached at the end of the string
            return current;
        }

        /* Method 5: DFSCollect (Helper)
            - Purpose:
                -> Performs a depth-first search (DFS) from the given node 
                    to collect all complete words in its subtree. Uses backtracking on 
                    currentWord so that each recursive call sees the correct prefix. 
            - Parameters:
                -> RWayTrieNode node: The current node in the trie
                -> StringBuilder currentWord: Holds the prefix plus the path built
                -> List<string> results: A list where all complete words will be stored
        */
        private void DFSCollect(RWayTrieNode node, StringBuilder currentWord, List<string> results)
        {
            //Iterates over all possible children
            for (int i = 0; i < 26; i++)
            {
                RWayTrieNode child = node.Children[i];

                //If there is no child node, skip this index
                if (child == null)
                {
                    continue;
                }

                //Determines the character represented by this child based on its index
                char character = (char)('a' + i);   //Need to explain this

                //Appends the character to the current word path
                currentWord.Append(character);

                //If the child node represents the end of a valid word, add it to the results list
                if (child.IsEndOfWord)
                {
                    results.Add(currentWord.ToString());
                }

                //Recursively explores deeper levels of the trie
                DFSCollect(child, currentWord, results);

                //Removes the last character before moving the next child
                currentWord.Length--;
            }

        }


        /* Method 6: BuildFromTextFile
            - Purpose:
                -> Populates the trie with all valid words found inside a given text file
                -> Uses a StreamReader to read the file line by line
                    - Trimming each line and inserting non-empty words into the trie
                -> Displays the count of valid and invalid words entered to show the size of the trie
            - Parameter:
                -> string filePath: The path to the text file containing one word per line
            - Output:
                -> No return value (void), but the trie will be filled with all valid words
            - Error Handling: 
                -> If the file path is null, empty or whitespace, displays an error message and doesnt open file
                -> If the file does not exist, displays an error message
                -> Ignores blank lines; invalid characters are handled inside Insert

            - Reference:
                -> Documentation for StreamReader used in this method:
                    https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=net-10.0
        */

        public void BuildFromTextFile(string filePath)
        {
            //Two checks done before attempting to open the file
            //Checks if the file path is null, empty or whitespace
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Error: File path is empty.");
                return;     //Doesn't attempt to open file
            }

            //Checks if the file actually exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File '{filePath}' not found.");
                return;     //Doesn't attempt to open file
            }

            try
            {
                //Opens the file for reading inside a using block to ensure it is closed
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string? line;       //Nullable reference variable as ReadLine() returns null at end of the file

                    //Reads the file line by line until the end is reached
                    while ((line = reader.ReadLine()) != null)
                    {
                        //Removes leading and trailing whitespace from the line
                        string word = line.Trim();

                        //Ignores empty or whitespace lines
                        if (string.IsNullOrWhiteSpace(word))
                        {
                            continue;
                        }

                        //Inserts the word into the trie (validation is handled inside method)
                        Insert(word);
                    }
                }
                Console.WriteLine($"Successfully built trie from '{filePath}'.");
                Console.WriteLine($"Inserted {validWordCount} valid words");
                Console.WriteLine($"Skipped {invalidWordCount} invalid words");
            }
            catch (Exception ex)
            {
                //Displays a helpful error message if something goes wrong while reading
                Console.WriteLine($"Error while reading file '{filePath}': {ex.Message}");

            }

        }


    }

}
