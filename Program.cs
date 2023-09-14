using System;
using System.Collections.Generic;

namespace TicTacToe
{
  class Program
  {
    static void Main()
    {
      start();

      Board.update();
      while (Board.winner == '\0')
      {
        int c = playersTurn();
        int evaluation = Evaluate.e(c, Board.player);
        if (evaluation >= 10)
        {
          Board.winner = Board.player;
          break;
        }

        computersTurn();
      }

      if (Board.winner != '%')
      {
        Console.WriteLine("The winner is: " + Board.winner);
      } else {
        Console.WriteLine("The game is a tie!");
      }
    }

    static void start()
    {
      Console.Clear();
      Console.WriteLine("Tic Tac Toe by Emil Poppler \n");

      Console.Write("Choose between x or o: ");

      while (true)
      {
        char key = Console.ReadKey(true).KeyChar;
        Console.WriteLine(key);

        if (key == 'x' || key == 'o')
        {
          Board.player = key;
          if (key == 'x') { Board.computer = 'o'; }
          if (key == 'o') { Board.computer = 'x'; }

          break;
        } else {
          Console.WriteLine("\nIt can only be lowercase x or o! ");
          Console.Write("Choose between x or o: ");
        }
      }
    }

    static int playersTurn()
    {
      int choice;
      while (true)
      {
        Console.Write("Choose position");
        choice = (int)Char.GetNumericValue(Console.ReadKey(true).KeyChar);

        if (choice >= 0 && choice < Board.width * Board.height )
        {
          if (Board.board[choice] != 'x' && Board.board[choice] != 'o')
          {
            break;
          } else {
            Console.Write("\nChoose another position!");
          }
        } else {
          Console.Write("\nChoose a number between 0-8");
        }
        Console.Write("\n");
      }

      Board.change(choice, Board.player);
      Board.update();
      return choice;
    }

    static void computersTurn()
    {
      List<int> emptyPositionList = new List<int>();

      for (int i = 0; i < Board.board.Length; i++)
      {
        if (Board.board[i] == ' ')
        {
          emptyPositionList.Add(i);
        }
      }
      int[] emptyPositions = emptyPositionList.ToArray();

      if (emptyPositions.Length == 0) { Board.winner = '%'; }

      for (int i = 0; i < emptyPositions.Length; i++)
      {
        int placement = Evaluate.e(emptyPositions[i], Board.computer);

        if (placement == 10)
        {
          Board.winner = Board.computer;
          Board.change(emptyPositions[i], Board.computer);
          Board.update();
          return;
        }
      }

      for (int i = 0; i < emptyPositions.Length; i++)
      {
        int placement = Evaluate.e(emptyPositions[i], Board.player);

        if (placement == 10)
        {
          Board.change(emptyPositions[i], Board.computer);
          Board.update();
          return;
        }
      }

      int highest = 0;
      int index = 0;
      for (int i = 0; i < emptyPositions.Length; i++)
      {
        int placement = Evaluate.e(emptyPositions[i], Board.computer);

        if (placement > highest)
        {
          highest = placement;
          index = emptyPositions[i];
        }
      }

      Board.change(index, Board.computer);
      Board.update();
    }
  }

  static class Board
  {
    public static char winner = '\0';
    public static int width = 3;
    public static int height = 3;
    public static char[] board = {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '};
    public static char player = 'o';
    public static char computer = 'x';

    public static void change(int position, char tail)
    {
      board[position] = tail;
    }

    public static void update()
    {
      Console.Clear();
      Console.WriteLine();
      Console.WriteLine("     |     |              |     |      ");
      Console.WriteLine("  {0}  |  {1}  |  {2}        0  |  1  |  2  ", board[0], board[1], board[2]);
      Console.WriteLine("_____|_____|_____    _____|_____|_____ ");
      Console.WriteLine("     |     |              |     |      ");
      Console.WriteLine("  {0}  |  {1}  |  {2}        3  |  4  |  5  ", board[3], board[4], board[5]);
      Console.WriteLine("_____|_____|_____    _____|_____|_____ ");
      Console.WriteLine("     |     |              |     |      ");
      Console.WriteLine("  {0}  |  {1}  |  {2}        6  |  7  |  8  ", board[6], board[7], board[8]);
      Console.WriteLine("     |     |              |     |      ");
    }
  }

  static class Evaluate
  {
    static char evaluationPlayer;
    static char evaluationOpponent;
    public static int e(int move, char piece)
    {
      var position = convertTo2d(move);
      int x = position.x;
      int y = position.y;

      evaluationPlayer = piece;
      if (piece == 'x') { evaluationOpponent = 'o'; }
      if (piece == 'o') { evaluationOpponent = 'x'; }

      int row = EvaluateRow(x, y);
      int column = EvaluateColumn(x, y);
      int diagonal = EvaluateDiagonal(x, y);

      if (row >= 3 || column >= 3 || diagonal >= 3) { return 10; }

      return row + column + diagonal;
    }

    public static int EvaluateRow(int x, int y)
    {
      int row = 1;
      for (int i = 0; i < Board.width; i++)
      {
        if (i != x)
        {
          int p = convertTo1d(i, y);
          char piece = Board.board[p];

          if (piece == evaluationPlayer)
          {
            row++;
          } else if (piece == evaluationOpponent)
          {
            row--;
          }
        }
      }
      return row;
    }
    public static int EvaluateColumn(int x, int y)
    {
      int column = 1;
      for (int i = 0; i < Board.height; i++)
      {
        if (i != y)
        {
          int p = convertTo1d(x, i);
          char piece = Board.board[p];

          if (piece == evaluationPlayer)
          {
            column++;
          } else if (piece == evaluationOpponent)
          {
            column--;
          }
        }
      }
      return column;
    }
    public static int EvaluateDiagonal(int x, int y)
    {
      int diagonal = 0;


      for (int i = 0; i < Board.height; i++)
      {
        int diagonalPosition1 = convertTo1d(i, Board.height - 1 - i);

        if (convertTo1d(x, y) == diagonalPosition1)
        {
          int diagonal1 = 1;
          for (int j = 0; j < Board.height; j++)
          {
            if (j != x)
            {
              int p = convertTo1d(j, Board.height - 1 - j);
              char piece = Board.board[p];

              if (piece == evaluationPlayer)
              {
                diagonal1++;
              } else if (piece == evaluationOpponent)
              {
                diagonal1--;
              }
            }
          }
          diagonal += diagonal1;
        }

        int diagonalPosition2 = convertTo1d(i, i);

        if (convertTo1d(x, y) == diagonalPosition2)
        {
          int diagonal2 = 1;
          for (int j = 0; j < Board.height; j++)
          {
            if (j != x)
            {
              int p = convertTo1d(j, j);
              char piece = Board.board[p];

              if (piece == evaluationPlayer)
              {
                diagonal2++;
              } else if (piece == evaluationOpponent)
              {
                diagonal2--;
              }
            }
          }
          diagonal += diagonal2;
        }
      }

      return diagonal;
    }
    public static int convertTo1d(int x, int y)
    {
      int position = (y * Board.width) + x;
      return position;
    }
    public static (int x, int y) convertTo2d(int position)
    {
      double x = position % Board.width;
      double y = position / Board.width;
      return (x: (int)Math.Floor(x), y: (int)Math.Floor(y));
    }
  }
}
