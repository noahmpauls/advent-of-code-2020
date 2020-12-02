# Day 1

Finding a set of k numbers out of n possible values that sum to 2020 is a dynamic programming problem. My initial approach was the most straightforward:

```
def findComboRecursive:
    if we're at the kth recursion:
        check if the numbers in soFar sum to the target
        if so, return them
        otherwise, return the empty list
    else:
        for each number in the n possible values:
            add number to soFar
            recurse, and return if values worked
        if nothing worked, return empty set
```

This approach requires memoization to run efficiently, and ultimately repeats several values. The runtime is O(n^k), as there are a total of n^k combinations to check.

## Better Approach

There is a better approach that requires no memoization but still includes recursive fun! The idea is to test each value only once by limiting the possible values each layer of recursion can test.

Let's say we have an array of length 5, and we want a combination of 3 numbers. Suppose we start by testing the first 3 values in the list:

```
+---+---+---+---+---+
| 1 | 2 | 3 |   |   |
+---+---+---+---+---+
```

Now let's say we move check #3 to the right:

```
+---+---+---+---+---+
| 1 | 2 |   | 3 |   |
+---+---+---+---+---+
```

There are 3 possible combos that you can generate from here:

```
+---+---+---+---+---+
| 1 | 2 |   | 3 |   |
+---+---+---+---+---+

+---+---+---+---+---+
| 1 |   | 2 | 3 |   |
+---+---+---+---+---+

+---+---+---+---+---+
|   | 1 | 2 | 3 |   |
+---+---+---+---+---+

```

What is key to note is that indices 1, 2, and 3 stay in the same position relative to one another. They never occupy the same space, nor does 1 come after 2, 2 after 3, etc. The position of each index bounds the position of adjacent indices.

Following this pattern, we can build a recursive algorithm that checks every combination without repeats:

```
def findComboRecursive(target, comboSize, vals, chosenSoFar, endIndex):
    if chosenSoFar.size == comboSize:
        if chosenSoFar.sum == target:
            return chosenSoFar
        else
            return empty set
    else: 
        # the start index is the first value that won't overlap with deeper
        # recursion indices
        startIndex = comboSize - chosenSoFar.size - 1
        for (i in range(startIndex, endIndex)):
            result = findComboRecursive(target, comboSize, chosenSoFar+vals[i], i)
            if result:
                return result
        return empty set
```