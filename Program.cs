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


/* R-Way Trie Summary:
    -> A tree that represents a set of strings by their characters
    -> Each non-root node represents a single character 
    -> For an R-Way Trie for lowercase letters, each node has up to 26 possible children nodes (for each letter)
    -> A terminal node represents the end of a valid word in the Trie, using a boolean flag.
    -> Provides efficient storage and quick search for string and often used to implement auto-complete and predictive text input
*/

using System;
using System.Xml;

/* Node class for R-Way Trie 
    -> Uses an array of 26 child references (instead of left and right pointers like a BST)
        - Each index from 0 to 25 corresponds to characters a to z
    -> Uses a boolean flag IsEndOfWord to indicate if a complete word ends at this node */
class RWayTrieNode
{
    public RWayTrieNode[] Children;     //This creates an array of children nodes, where each child is an RWayTrieNode
    public bool IsEndOfWord;            //This creates a flag which indicates terminating characters, if true then it marks the end of a valid word

    //Constructor
    public RWayTrieNode()
    {
        Children = new RWayTrieNode[26];    //This creates space for 26 possible children (a to z), with initial null state
        IsEndOfWord = false;                //This initializes the boolean flag
    }
}


public class RWayTrie
{
    private RWayTrieNode root;

    public RWayTrie()
    {
        root = new RWayTrieNode();
    }

    /* Method 1: Insertion
        - Parameter:
            -> string: The word to be inserted into the Trie
        - Error Handling: 
            -> Ignores null/whitespace input
            -> Ignores numbers and special characters
        
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

        RWayTrieNode current = root;    //This starts the traversal from of the root
        word = word.ToLower();          //This normalizes input to lowercase characters

        //This iterates over every letter in the word
        foreach (char character in word)
        {
            //This maps the characters a to z to index 0 to 25 by subtracting the ASCII values of the characters
            int index = character - 'a';

            //This checks if the word contains only letters from a to z (which holds index locations 0 to 5)
            if (index < 0 || index >= 26)
            {
                Console.WriteLine($"Invalid word: Could not insert '{word}'");
                return;
            }

            //This checks if there is an existing child node for the letter, and creates one if it doesn't exist
            if (current.Children[index] == null)
            {
                current.Children[index] = new RWayTrieNode();
            }

            //This moves a level deeper in the trie for the following letters in the word
            current = current.Children[index];
        }

        current.IsEndOfWord = true;     //This marks the last node as the end of the word
    }


    /* Method 2: Search
        - Parameter:
            -> string: The word to be searched inside the Trie
        - Output:
            -> true if it exists, false if otherwise
        - Error Handling: 
            -> Ignores null/whitespace input
            -> Ignores numbers and special characters
        
        Pseudocode:
            -> Start at the root
            -> For each character in the word,
                1. Convert it to lowercase 
                2. Determine its respective numeric array index (0 to 25 corresponds to characters a to z)
                3. If the corresponding child at index is null, then it doesn't exist, so return false
                4. Otherwise, move down one level to the child node
            -> After the last character, return true if the node is marked as the end of the word */
    public bool Search(string word)
    {
        //Error handling for empty or invalid input
        if (string.IsNullOrWhiteSpace(word))
        {
            return false;
        }

        RWayTrieNode current = root;    //This starts the search from the children of the root
        word = word.ToLower();          //This normalizes input to lowercase characters

        //This iterates over every letter in the word
        foreach (char character in word)
        {
            //This determines the respective array index by subtracting the ASCII values of the characters
            int index = character - 'a';

            //This checks if the word contains only letters from a to z (which holds index locations 0 to 5)
            if (index < 0 || index >= 26)
            {
                return false;        //Invalid characters entered
            }

            //This checks if there is an existing child node for the letter
            if (current.Children[index] == null)
            {
                return false;        //Word doesn't exist
            }

            //This moves a level deeper in the trie for the following letters in the word
            current = current.Children[index];

        }

        //After the last character, the word exists if flag node is marked as true from insertion
        return current.IsEndOfWord;

    }

    /* Method 3: Deletion (Public)
        - Parameter:
            -> string: The word to be deleted from the Trie
        - Output:
            -> true if it was found and deleted, false if not found
        - Error Handling: 
            -> Ignores null/whitespace input
            -> Ignores numbers and special characters
        - Calls a private recursive helper method to handle traversal and node deletion */

    public bool Delete(string word)
    {
        //Error handling for empty or invalid input
        if (string.IsNullOrWhiteSpace(word))
        {
            return false;
        }

        word = word.ToLower();          //This normalizes input to lowercase characters
        return Delete(root, word, 0);   //This calls the private helper method
    }

    /* Method 4: Deletion (Private Helper Method)
        - Parameters:
            -> node: The current node being checked
            -> word: The word to be deleted
            -> charIndex: The index of current character in the word
        - Output:
            -> returns true if the target word was found and deleted
        
        Pseudocode:
            -> Traverses down the trie using each character of the word
            -> When we reach the end of the word (current index == word length)
                1. If IsEndOfWord is false, then the word doesn't exist, so return false
                2. Otherwise, 
                    - Unmark IsEndOfWord by setting to false
                    - Do not delete the node if it still has children
                    - Return true to indicate that the word is deleted
            -> Recursion
                - Determine the index for the current character
                - If the required child is missing, word doesn't exist, so return false
                - Recursively call Delete on the child with charIndex + 1
                    -> If returned false, the word was not deleted 
                    -> If returned true, 
                        - Check if child node has no remaining children to form other words and is not marked IsEndOfWord
                        - Sets node.Children[index] = null to remove child reference
                        - Return true to confirm that the target word was deleted */

    private bool Delete(RWayTrieNode node, string word, int charIndex)
    {
        //If the current node is null, the word path does not exist
        if (node == null)
        {
            return false;
        }

        //When we reached the end of the word
        if (charIndex == word.Length)
        {
            //If this node is not marked as the end of a word, the word doesn't exist
            if (!node.IsEndOfWord)
            {
                return false;       //Then word not found
            }

            //Otherwise, the word does exist 
            node.IsEndOfWord = false;       //This unmarks this node as end of word
            return true;                    //Word was successfully deleted
        }

        //This gets the index of the current character
        char character = word[charIndex];       
        int index = character - 'a';

        //If character path doesn't exist, the word isn't present in the Trie
        if (index < 0 || index >= 26 || node.Children[index] == null)
        {
            return false;
        }

        //Recursive call for the next character in the word
        bool deleted = Delete(node.Children[index], word, charIndex + 1);

        //If the word wasn't found or deleted in the subtree, stop here
        if (!deleted)
        {
            return false;
        }

        //This checks if the child node can be deleted after successful deletion of word
        //  - Checks if child is not empty, not the end of a word and has no children to form other words
        RWayTrieNode child = node.Children[index];
        if (child != null && !child.IsEndOfWord && HasNoChildren(child))
        {
            node.Children[index] = null;        //Remove the unused child reference
        }

        return true;    //The word was deleted

    }
    
    /* Method 5: HasNoChildren  
        - Parameters:
            -> node: The current node being checked 
        - Output:
            -> true, if the given node has no non-null children
        - Used by delete helper to decide when a node can be deleted */
    private bool HasNoChildren(RWayTrieNode node)
    {
        for (int i = 0; i < 26; i++)
        {
            if (node.Children[i] != null)
            {
                return false;
            }
        }
        return true;
    }

}

class Program
{
    static void Main(string[] args)
    {
        //Creates an instance of class
        RWayTrie trie = new RWayTrie();

        Console.Clear();        //Clears the output

        //Inserts several valid words into the Trie
        Console.WriteLine("");
        Console.WriteLine("------Operation 1: Inserting 4 Valid Words------");
        Console.WriteLine("Inserting valid words: dog, cat, catan and baseball");
        trie.Insert("dog");
        trie.Insert("cat");
        trie.Insert("catan");
        trie.Insert("baseball");

        Console.WriteLine("");
        Console.WriteLine("------Operation 2: Inserting 4 Invalid Words------");
        Console.WriteLine("Inserting invalid words: '', ' ', h8teful, h@ppiness");
        trie.Insert("");
        trie.Insert(" ");
        trie.Insert("h8teful");
        trie.Insert("h@ppiness");

        //Searches for both existing and non-existing words in the Trie
        Console.WriteLine("");
        Console.WriteLine("------Operation 3: Searching for Existing Words------");
        Console.WriteLine($"Searching for 'dog': {trie.Search("dog")}");
        Console.WriteLine($"Searching for 'cat': {trie.Search("cat")}");
        Console.WriteLine($"Searching for 'catan': {trie.Search("catan")}");
        Console.WriteLine($"Searching for 'baseball': {trie.Search("baseball")}");

        Console.WriteLine("");
        Console.WriteLine("------Operation 4: Searching for Non-existing Words------");
        Console.WriteLine($"Searching for 'bat': {trie.Search("bat")}");
        Console.WriteLine($"Searching for 'h8teful': {trie.Search("h8teful")}");
        Console.WriteLine($"Searching for 'base': {trie.Search("base")}");
        Console.WriteLine($"Searching for 'baseballs': {trie.Search("baseballs")}");

        //Deletes some words from the Trie and verifies the changes
        Console.WriteLine("");
        Console.WriteLine("------Operation 5: Deleting Words------");
        Console.WriteLine($"Deleting 'dog': {trie.Delete("dog")}");
        Console.WriteLine($"Deleting 'cat': {trie.Delete("cat")}");
        Console.WriteLine($"Deleting 'spider': {trie.Delete("spider")}");      //Doesn't exist
        Console.WriteLine($"Deleting 'base': {trie.Delete("base")}");          //Doesn't exist

        //Verifies the changes after deletion
        Console.WriteLine("");
        Console.WriteLine("------Operation 6: Verifying Word Deletion------");
        Console.WriteLine($"Searching for 'dog': {trie.Search("dog")}");            //Should be false after deletion
        Console.WriteLine($"Searching for 'cat': {trie.Search("cat")}");            //Should be false after deletion
        Console.WriteLine($"Searching for 'catan': {trie.Search("catan")}");        //Should be true
        Console.WriteLine($"Searching for 'baseball': {trie.Search("baseball")}");  //Should be true
        Console.WriteLine("");
    }
}