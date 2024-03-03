using System;
using System.IO;
using System.Linq;

namespace Game
{
    internal class Program
    {
        static Random generator = new Random();
        const int MinDiceRoll = 1;
        const int MaxDiceRoll = 7;
        const string Score = "Score.txt";
        const string GameGrid = "GameGrid.txt";
        const string GameStats = "GameStats.txt";
        static string ReadString(string prompt, string a, string b)
        {
            string result;
            do
            {
                Console.Write(prompt);
                result = Console.ReadLine();
            } while (result != a && result != b);
            return result;
        }
        static string ReadUsername(string prompt, string a, string b)
        {
            string result;
            do
            {
                Console.Write(prompt);
                result = Console.ReadLine();
            } while (result == a || result == b);
            return result;
        }
        static int RollDice(int i)
        {
            int Roll = generator.Next(MinDiceRoll, MaxDiceRoll);
            if (Roll % 2 == 0)
            {
                Console.WriteLine("The rolled number is: " + Roll + ".");
                return 1;
            }
            else
            {
                Console.WriteLine("The rolled number is: " + Roll + ".");
                return 2;
            }
        }
        static void Rules()
        {
            Console.WriteLine("\nHere are the rules:");
            Console.WriteLine("- Players will take turns putting their assigned mark in available spaces.");
            Console.WriteLine("- Choose any available number from the grid to replace it with your mark");
            Console.WriteLine("- First player to get 3 of their marks in a row (horizontally, vertically, or diagonally) wins.");
            Console.WriteLine("- When there are no remaining numbers, the game is over");
            Console.WriteLine("- If no player has 3 marks in a row, the game ends in a tie.");
            Console.WriteLine("Enjoy!");
        }
        static void Main(string[] args)
        {
            string Starter = "";
            string Second = "";
            bool StarterWon = false;
            bool SecondWon = false;
            int ScorePlayer1 = 0;
            int ScorePlayer2 = 0;
            string Player1 = "";
            string Player2 = "";
            string[] Stats = { };
            string[] Scores = { };
            string Resume = "n";     

            Console.WriteLine("Tic Tac Toe");

            Rules();

            if (File.Exists(Score))
            {
                Resume = ReadString("\nDo you want to resume from last score (y/n)? ", "y", "n");
                if (Resume == "y")
                {
                    StreamReader ScoreInput = new StreamReader(Score);
                    Scores = File.ReadAllLines(Score);
                    Player1 = Scores[0];
                    Player2 = Scores[1];
                    ScorePlayer1 = int.Parse(Scores[2]);
                    ScorePlayer2 = int.Parse(Scores[3]);
                    ScoreInput.Close();

                    Console.WriteLine("Player 1: " + Player1 + " - " + ScorePlayer1);
                    Console.WriteLine("Player 2: " + Player2 + " - " + ScorePlayer2);
                }
                else if (Resume == "n")
                {
                    File.Delete(Score);
                    File.Delete(GameGrid);
                    File.Delete(GameStats);
                }
            }

            if (File.Exists(GameGrid) == false && File.Exists(GameStats) == false && Resume == "n")
            {
                Console.WriteLine();
                Player1 = ReadUsername("Player 1, choose an username: ", "", " ");
                Player2 = ReadUsername("Player 2, choose an username: ", "", " ");
            }

            string PlayAgain = "no";
            do
            {
                if (File.Exists(GameGrid) == false && File.Exists(GameStats) == false)
                {
                    string Ans = ReadString("\nDo you want to roll a dice to decide which player starts (y/n)? ", "y", "n");
                    if (Ans == "y")
                    {
                        if (RollDice(1) == 1)
                        {
                            Starter = Player1;
                            Second = Player2;
                            Console.WriteLine("As the number rolled is even, " + Starter + " starts.");
                        }
                        else
                        {
                            Starter = Player2;
                            Second = Player1;
                            Console.WriteLine("As the number rolled is odd, " + Starter + " starts.");
                        }
                    }
                    else if (Ans == "n")
                    {
                        string StarterChoice = ReadString("\nPress 1 if you want " + Player1 + " to start, or 2 if you want " + Player2 + " to start: ", "1", "2");

                        if (StarterChoice == "1")
                        {
                            Starter = Player1;
                            Second = Player2;
                        }
                        else if (StarterChoice == "2")
                        {
                            Starter = Player2;
                            Second = Player1;                         
                        }
                        Console.WriteLine(Starter + " starts.");
                    }
                }

                string[] GridNumbers = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                int MaxTurns = GridNumbers.Length;
                int TurnNumber = 1;

                for (int i = 0; i < MaxTurns; ++i)
                {
                    if (File.Exists(GameGrid))
                    {
                        StreamReader OngoingGameGridInput = new StreamReader(GameGrid);
                        GridNumbers = File.ReadAllLines(GameGrid);
                        OngoingGameGridInput.Close();
                    }

                    if (File.Exists(GameStats))
                    {
                        StreamReader OngoingGameStatsInput = new StreamReader(GameStats);
                        Stats = File.ReadAllLines(GameStats);
                        TurnNumber = int.Parse(Stats[0]);
                        i = (int.Parse(Stats[1]) + 1);
                        MaxTurns = int.Parse(Stats[2]);
                        Starter = Stats[3];
                        Second = Stats[4];
                        ScorePlayer1 = int.Parse(Stats[5]);
                        ScorePlayer2 = int.Parse(Stats[6]);
                        Player1 = Stats[7];
                        Player2 = Stats[8];
                        OngoingGameStatsInput.Close();
                    }

                    Console.WriteLine("\n " + GridNumbers[0] + " | " + GridNumbers[1] + " | " + GridNumbers[2]);
                    Console.WriteLine("---|---|---");
                    Console.WriteLine(" " + GridNumbers[3] + " | " + GridNumbers[4] + " | " + GridNumbers[5]);
                    Console.WriteLine("---|---|---");
                    Console.WriteLine(" " + GridNumbers[6] + " | " + GridNumbers[7] + " | " + GridNumbers[8]);

                    ++TurnNumber;

                    if (TurnNumber % 2 == 0)
                    {
                        Console.Write("\n" + Starter + "'s turn, choose a number: ");
                    }
                    else if (TurnNumber % 2 != 0)
                    {
                        Console.Write("\n" + Second + "'s turn, choose a number: ");
                    }

                    string input = Console.ReadLine();
                    bool ContainsNumber = GridNumbers.Contains(input);

                    if (ContainsNumber == true && TurnNumber % 2 == 0)
                    {
                        int Position = int.Parse(input);
                        GridNumbers[(Position - 1)] = "X";
                    }
                    else if (ContainsNumber == true && TurnNumber % 2 != 0)
                    {
                        int Position = int.Parse(input);
                        GridNumbers[(Position - 1)] = "O";
                    }
                    else
                    {
                        ++MaxTurns;
                        ++TurnNumber;
                        Console.WriteLine("Invalid number. Choose a number from 1 to 9, which hasn't been selected yet.");
                    }

                    if (
                        (GridNumbers[0] == "X" && GridNumbers[1] == "X" && GridNumbers[2] == "X") ||
                        (GridNumbers[3] == "X" && GridNumbers[4] == "X" && GridNumbers[5] == "X") ||
                        (GridNumbers[6] == "X" && GridNumbers[7] == "X" && GridNumbers[8] == "X") ||
                        (GridNumbers[0] == "X" && GridNumbers[3] == "X" && GridNumbers[6] == "X") ||
                        (GridNumbers[1] == "X" && GridNumbers[4] == "X" && GridNumbers[7] == "X") ||
                        (GridNumbers[2] == "X" && GridNumbers[5] == "X" && GridNumbers[8] == "X") ||
                        (GridNumbers[0] == "X" && GridNumbers[4] == "X" && GridNumbers[8] == "X") ||
                        (GridNumbers[2] == "X" && GridNumbers[4] == "X" && GridNumbers[6] == "X"))
                    {
                        StarterWon = true;

                        Console.WriteLine("\n " + GridNumbers[0] + " | " + GridNumbers[1] + " | " + GridNumbers[2]);
                        Console.WriteLine("---|---|---");
                        Console.WriteLine(" " + GridNumbers[3] + " | " + GridNumbers[4] + " | " + GridNumbers[5]);
                        Console.WriteLine("---|---|---");
                        Console.WriteLine(" " + GridNumbers[6] + " | " + GridNumbers[7] + " | " + GridNumbers[8]);

                        Console.WriteLine("\nCongratulations " + Starter + ", you have won!!");
                        break;
                    }
                    else if (
                        (GridNumbers[0] == "O" && GridNumbers[1] == "O" && GridNumbers[2] == "O") ||
                        (GridNumbers[3] == "O" && GridNumbers[4] == "O" && GridNumbers[5] == "O") ||
                        (GridNumbers[6] == "O" && GridNumbers[7] == "O" && GridNumbers[8] == "O") ||
                        (GridNumbers[0] == "O" && GridNumbers[3] == "O" && GridNumbers[6] == "O") ||
                        (GridNumbers[1] == "O" && GridNumbers[4] == "O" && GridNumbers[7] == "O") ||
                        (GridNumbers[2] == "O" && GridNumbers[5] == "O" && GridNumbers[8] == "O") ||
                        (GridNumbers[0] == "O" && GridNumbers[4] == "O" && GridNumbers[8] == "O") ||
                        (GridNumbers[2] == "O" && GridNumbers[4] == "O" && GridNumbers[6] == "O"))
                    {
                        SecondWon = true;

                        Console.WriteLine("\n " + GridNumbers[0] + " | " + GridNumbers[1] + " | " + GridNumbers[2]);
                        Console.WriteLine("---|---|---");
                        Console.WriteLine(" " + GridNumbers[3] + " | " + GridNumbers[4] + " | " + GridNumbers[5]);
                        Console.WriteLine("---|---|---");
                        Console.WriteLine(" " + GridNumbers[6] + " | " + GridNumbers[7] + " | " + GridNumbers[8]);

                        Console.WriteLine("\nCongratulations " + Second + ", you have won!!");
                        break;
                    }
                    else if (
                        (GridNumbers[0] != "1" && GridNumbers[1] != "2" && GridNumbers[2] != "3") &&
                        (GridNumbers[3] != "4" && GridNumbers[4] != "5" && GridNumbers[5] != "6") &&
                        (GridNumbers[6] != "7" && GridNumbers[7] != "8" && GridNumbers[8] != "9"))
                    {
                        Console.WriteLine("\n " + GridNumbers[0] + " | " + GridNumbers[1] + " | " + GridNumbers[2]);
                        Console.WriteLine("---|---|---");
                        Console.WriteLine(" " + GridNumbers[3] + " | " + GridNumbers[4] + " | " + GridNumbers[5]);
                        Console.WriteLine("---|---|---");
                        Console.WriteLine(" " + GridNumbers[6] + " | " + GridNumbers[7] + " | " + GridNumbers[8]);

                        Console.WriteLine("\nIt's a draw!!");
                    }

                    StreamWriter OngoingGameGridOutput = new StreamWriter(GameGrid);
                    foreach (string Number in GridNumbers)
                    {
                        OngoingGameGridOutput.WriteLine(Number);
                    }
                    OngoingGameGridOutput.Close();

                    StreamWriter OngoingGameStatsOutput = new StreamWriter(GameStats);
                    OngoingGameStatsOutput.WriteLine(TurnNumber);
                    OngoingGameStatsOutput.WriteLine(i);
                    OngoingGameStatsOutput.WriteLine(MaxTurns);
                    OngoingGameStatsOutput.WriteLine(Starter);
                    OngoingGameStatsOutput.WriteLine(Second);
                    OngoingGameStatsOutput.WriteLine(ScorePlayer1);
                    OngoingGameStatsOutput.WriteLine(ScorePlayer2);
                    OngoingGameStatsOutput.WriteLine(Player1);
                    OngoingGameStatsOutput.WriteLine(Player2);
                    OngoingGameStatsOutput.Close();
                }

                if (Player1 == Starter && StarterWon == true || Player1 == Second && SecondWon == true)
                {
                    ++ScorePlayer1;
                }
                else if (Player2 == Starter && StarterWon == true || Player2 == Second && SecondWon == true)
                {
                    ++ScorePlayer2;
                }
                Console.WriteLine("\n" + Player1 + " " + ScorePlayer1 + " - " + Player2 + " " + ScorePlayer2);

                StreamWriter ScoreOutput = new StreamWriter("Score.txt");
                ScoreOutput.WriteLine(Player1);
                ScoreOutput.WriteLine(Player2);
                ScoreOutput.WriteLine(ScorePlayer1);
                ScoreOutput.WriteLine(ScorePlayer2);
                ScoreOutput.Close();

                File.Delete(GameGrid);
                File.Delete(GameStats);

                string Answer = ReadString("\nDo you want to play again (y/n)? ", "y", "n");
                if (Answer == "y")
                {
                    PlayAgain = "yes";
                }
                else if (Answer == "n")
                {
                    PlayAgain = "no";
                }
            } while (PlayAgain == "yes");

            Console.Write("\nThanks for playing!!");

            Console.ReadKey();
        }
    }
}
