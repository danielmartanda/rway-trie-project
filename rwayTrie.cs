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
    // RWayTrie Class
    public class RWayTrie
    {
        // Root node of the tree (represents empty prefix)
        private RWayTrieNode root;

        public RWayTrie()
        {
            root = new RWayTrieNode();
        }

        // Map a character to an array index (0-25)
        // 'a' -> 0, 'b' -> 1, ..., 'z' -> 25
        private int CharToIndex(char c)
        {
            c = char.ToLower(c);
            return c - 'a';
        }

        // Helper: check if a character is a vaild letter for the trie
        public bool IsValidChar(char c)
        {
            c = char.ToLower(c);
            return c >= 'a' && c <= 'z';
        }

        // Insert adds a word to the trie
        // Time Analysis: O(L) where L would be the length of the word
        public void Insert(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return;

            word = word.Trim().ToLower();

            // If word has anything other than letters, ignore it for this assignment
            foreach (char ch in word)
            {
                if (!IsValidChar(ch))
                    return;
            }

            var current = root;

            foreach (char ch in word)
            {
                int idx = CharToIndex(ch);

                // if path doesn't exist, create a new node 
                if (current.Children[idx] == null)
                    current.Children[idx] = new RWayTrieNode();

                // Move down to the next level
                current = current.Children[idx];
            }

            // Mark the last node as end of a word
            current.IsEndOfWord = true;
        }
    }
}