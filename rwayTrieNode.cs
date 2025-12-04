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

namespace RWayTrieProject
{
    /* RWay Trie Node Class 
        -> Represents a single node in the RWay Trie
        -> Each node contains:
            - An array of 26 child references (for characters a to z)
            - A boolean flag IsEndOfWord to indicate if a complete word ends at this node 
            - A character field storing the character associated with this node 
        -> The root node does not represent a character, so it stores the null character '\0' as a placeholder.
            This prevents the root from being included in any words during prefix or traversal operations. 
    */
    public class RWayTrieNode
    {
        public RWayTrieNode[] Children;     //Array of 26 children nodes, where each child is an RWayTrieNode, indexed from 0-25 for letters a to z
        public bool IsEndOfWord;            //A flag which indicates terminating characters, if true then it marks the end of a valid word
        public char Value;                  //Character stored at this node ('\0' for the root)

        //Default constructor for root node
        public RWayTrieNode()
        {
            Children = new RWayTrieNode[26];    //This creates space for 26 possible children (a to z), with initial null state
            IsEndOfWord = false;                //This initializes the boolean flag
            Value = '\0';                       //This stores a placeholder null character for the root node
        }

        //Constructor for child nodes
        public RWayTrieNode(char value)
        {
            Children = new RWayTrieNode[26];    //This creates space for 26 possible children (a to z), with initial null state
            IsEndOfWord = false;                //This initializes the boolean flag
            Value = value;                      //This stores the character for child node
        }
    }
}