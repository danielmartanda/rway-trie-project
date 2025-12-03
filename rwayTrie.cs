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

        // Search: return true if FULL word exists
        // Time: O(L)
        public bool Search(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return false;

            word = word.Trim().ToLower();

            var current = root;

            foreach (char ch in word)
            {
                if (!IsValidChar(ch))
                    return false;

                int idx = CharToIndex(ch);

                if (idx < 0 || idx >= RWayTrieNode.R)
                    return false;

                // Path does not exist -> found
                if (current.Children[idx] == null)
                    return false;

                // Move down the path
                current = current.Children[idx];
            }

            // Return true only if last node marks end of a word
            return current.IsEndOfWord;
        }

        /*
        Task 2 helper: Traverse down the trie following a string
        Returns the node corresponding to the last character,
        or null if the path doesn't exist
        */

        private RWayTrieNode TraverseToNode(string s)
        {
            var current = root;

            if (string.IsNullOrEmpty(s))
                return current; // empty prefix = root

            s = s.Trim().ToLower();

            foreach (char ch in s)
            {
                if (!IsValidChar(ch))
                    return null;

                int idx = CharToIndex(ch);

                if (idx < 0 || idx >= RWayTrieNode.R)
                    return null;

                if (current.Children[idx] == null)
                    return null;

                current = current.Children[idx];
            }
            return current;
        }

        /* Task 2: PrefixMatch
        Return all words in the trie that start with the given prefix.

        Algorithm:
        1. Traverse down to the node for the prefix
        2. From that node, DFS through its subtree, building from words

        Time analysis: O(P + K) where P = prefix length, K = total chars in all matches
        */
        public List<string> PrefixMatch(string prefix)
        {
            List<string> results = new List<string>();

            if (string.IsNullOrWhiteSpace(prefix))
                return results;

            prefix = prefix.Trim().ToLower();

            // Edge case: empty prefix -> return all words
            // (Optional: you can also decide to treat empty as "no results")
            RWayTrieNode startNode = TraverseToNode(prefix);

            if (startNode == null)
            {
                // No such path in the trie -> no matches
                return results;
            }

            var sb = new System.Text.StringBuilder(prefix);

            // If the prefix itself is a complete word, include it
            if (startNode.IsEndOfWord)
            {
                results.Add(prefix);
            }

            // DFS deeper to find longer words
            DFSCollect(startNode, sb, results);

            return results;
        }

        /*
        DFS helper for PrefixMatch

        Node: Current Trie Node
        sb: StringBuilder holding prefix + path so far
        results: list where we store complete words
        */
        private void DFSCollect(RWayTrieNode node, System.Text.StringBuilder sb, List<string> results)
        {
            // For each possible child (a-z)
            for (int i = 0; i < RWayTrieNode.R; i++)
            {
                var child = node.Children[i];
                if (child == null)
                    continue;

                // Add this characther to the current string
                char c = (char)('a' + i);
                sb.Append(c);

                // If child is end-of-word, record this word
                if (child.IsEndOfWord)
                {
                    results.Add(sb.ToString());
                }

                // Recurse to explore deeper children
                DFSCollect(child, sb, results);

                // Backtrack: remove last character before trying the next child 
                sb.Length--;
            }
        }

        /*
        Task 3: Build Trie from File
        Reads a text file with one English word per line and inserts them
        */

        public void BuildFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Dictionary not found," filePath);
            }

            foreach (string line in File.ReadLines(filePath))
            {
                string word = line.Trim();

                if (string.IsNullOrWhiteSpace(word))
                    continue;

                Insert(word);
            }
        }

        // Delete + helpers from lab 3
        public bool Delete(String word)
        {
            if (string.IsNullOrWhiteSpace)
                return false;

            return DeleteHelper(root, word.ToLower(), 0);
        }

        private bool DeleteHelper(RWayTrie node, string word, int depth)
        {
            if (node == null)
                return false;

            // reached end of the word
            if (depth == word.Length)
            {
                if (!node.IsEndOfWord)
                    return false; // word not found
            }

            int idx = CharToIndex(word[depth]);
            if (idx < 0 || idx >= RWayTrieNode.R)
                return false;

            // Recursivley go to the next node
            if (DeleteHelper(node.Children[idx], word, depth + 1))
            {
                // child became empty -> remove it 
                node.Children[idx] = null;

                // return true if this node isnnow emoty and not end of another word
                return !node.IsEndOfWord && IsEmpty(node);
            }

            return false;
        }

        // Helper to check if a node has no children
        private bool IsEmpty(RWayTrieNode node)
        {
            for (int i = 0; i < RWayTrieNode.R; i++)
            {
                if (node.Children[i] != null)
                    return false;
            }

            return true;
        }
    }
}