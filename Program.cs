using System;
using System.Collections.Generic;

public class PuzzleState
{
    public int[] Board; // Bordi i lojes, perfaqeson gjendjen e enigmes
    public int EmptyTileIndex; // Indeksi i pllakes bosh ne bord

    // Konstruktori i klases PuzzleState qe inicializon bordi dhe gjen indeksin e pllakes bosh
    public PuzzleState(int[] board)
    {
        Board = board;
        EmptyTileIndex = Array.IndexOf(board, 0);
    }

    // Metoda qe kontrollon nese gjendja aktuale eshte zgjidhja e enigmes
    public bool IsGoal()
    {
        for (int i = 0; i < 8; i++)
        {
            if (Board[i] != i + 1)
                return false;
        }
        return true;
    }

    // Metoda qe gjeneron gjendjet e ardhshme te mundshme bazuar ne levizjet e lejuara te pllakes bosh
    public List<PuzzleState> GetNextStates()
    {
        List<PuzzleState> nextStates = new List<PuzzleState>();
        int row = EmptyTileIndex / 3;
        int col = EmptyTileIndex % 3;

        // Kontrollon levizjet e mundshme dhe shton gjendjet e reja ne listen e gjendjeve te ardhshme
        if (row > 0) nextStates.Add(SwapTiles(EmptyTileIndex, EmptyTileIndex - 3));
        if (row < 2) nextStates.Add(SwapTiles(EmptyTileIndex, EmptyTileIndex + 3));
        if (col > 0) nextStates.Add(SwapTiles(EmptyTileIndex, EmptyTileIndex - 1));
        if (col < 2) nextStates.Add(SwapTiles(EmptyTileIndex, EmptyTileIndex + 1));

        return nextStates;
    }

    // Metoda private qe nderron vendet e dy pllakave ne bord dhe kthen nje gjendje te re te enigmes
    private PuzzleState SwapTiles(int index1, int index2)
    {
        int[] newBoard = (int[])Board.Clone();
        int temp = newBoard[index1];
        newBoard[index1] = newBoard[index2];
        newBoard[index2] = temp;
        return new PuzzleState(newBoard);
    }
}

public class PuzzleSolver
{
    // Metoda statike qe implementon algoritmin e kthimit mbrapa (backtracking) per te gjetur zgjidhjen e enigmes
    public static PuzzleState Backtracking(PuzzleState currentState, HashSet<string> visitedStates)
    {
        if (currentState.IsGoal())
            return currentState;

        string stateKey = string.Join(",", currentState.Board);
        if (visitedStates.Contains(stateKey))
            return null;
        
        visitedStates.Add(stateKey);

        foreach (var nextState in currentState.GetNextStates())
        {
            var result = Backtracking(nextState, visitedStates);
            if (result != null)
                return result;
        }
        return null;
    }
}

class Program
{
    static void Main()
    {
        // Definon gjendjen fillestare te enigmes dhe krijon nje instance te klases PuzzleState
        int[] initialBoard = { 1, 2, 3, 4, 5, 6, 0, 7, 8 };
        PuzzleState initialState = new PuzzleState(initialBoard);

        // Perdor algoritmin e kthimit mbrapa per te gjetur zgjidhjen e enigmes.
        PuzzleState solution = PuzzleSolver.Backtracking(initialState, new HashSet<string>());

        // Afishon zgjidhjen ne konsole nese ajo gjendet
        if (solution != null)
        {
            Console.WriteLine("Zgjidhja u gjet:");
            for (int i = 0; i < solution.Board.Length; i++)
            {
                Console.Write(solution.Board[i] + " ");
                if ((i + 1) % 3 == 0)
                    Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("Nuk u gjet zgjidhje.");
        }
    }
}
