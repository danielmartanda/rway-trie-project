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

namespace RWayTrieProject
{
    // Task 1: RWayTrieNode 
    public class RWayTrieNode
    {
        // Alphabet size (a-z)
        public const int R = 26;
        // Children will be in the array list as 0 = 'a' and then 25 will be z
        public RWayTrieNode[] Children;
        // True if this node marks the end of a word 
        public bool IsEndOfWord;

        public RWayTrieNode()
        {
            Children = new RWayTrieNode[R];
            IsEndOfWord = false;
        }
    }
}