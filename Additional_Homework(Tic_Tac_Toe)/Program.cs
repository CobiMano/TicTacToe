using Additional_Homework_Tic_Tac_Toe_;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class Program
{
    static List<string> field = new List<string>();
    static string nextMove = "+";
    static bool isWin;
    static bool isDraw;
    static int gameMode;
    static string nameWinner;
    static string fileInformationGame;
    static List<string> infoGames;

    static void Main()
    {
        GameManager gameManager = new GameManager();
        gameManager.StartGame();
    }

    static void InitializeGameFile()
    {
        if (!File.Exists(fileInformationGame))
        {
            File.Create(fileInformationGame).Close();
        }
    } // Инициализация игрового файла

    static void ResetGame()
    {
        isWin = false;
        isDraw = false;
        nameWinner = "";
        field.Clear();
        for (int i = 0; i < 9; i++)
        {
            field.Add(" ");
        }
    } // Сброс игры

    static void DrawField()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        for (int i = 0; i < 9; i++)
        {
            if (i == 2 || i == 5 || i == 8)
            {
                Console.Write($"{field[i]}");
            }
            else
            {
                Console.Write($"{field[i]}|");
            }

            if (i == 2 || i == 5)
            {
                Console.WriteLine();
                for (int j = 0; j < 5; j++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
            }
        }
        Console.WriteLine("\n\nВведите номер клетки куда хотите сходить:");
        Console.WriteLine("1|2|3\n-----\n4|5|6\n-----\n7|8|9\n");
    } // Отрисовка игрового поля

    static void Menu()
    {
        int userInput;
        do
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Выберите режим игры в крестики-нолики:");
            Console.WriteLine("1 - Против рандомного оппонента");
            Console.WriteLine("2 - Против умного оппонента");
            Console.WriteLine("3 - Умный оппонент против рандомного");
        } while (!(int.TryParse(Console.ReadLine(), out userInput) && userInput > 0 && userInput < 4));

        gameMode = userInput;

        if (gameMode != 3)
        {
            DrawField();
        }
    } // Логика меню

    static void TicTacToeGame()
    {
        switch (gameMode)
        {
            case 1: GameModePlayerAndRandomOpponent(); break;
            case 2: GameModePlayerAndSmartOpponent(); break;
            case 3: GameModeSmartOpponentAndRandomOpponent(); break;
        }
        WinOrDrawMessage();
    } // Логика игры в зависимости от выбранного режима

    static void Player()
    {
        int userInput;
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out userInput) && userInput > 0 && userInput < 10 && field[userInput - 1] == " ")
            {
                break;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            DrawField();
            Thread.Sleep(200);
            Console.ResetColor();
            DrawField();
        }

        field[userInput - 1] = nextMove;
        nameWinner = "Player";

        DrawField();
        CheckWinAndDraw();
        ReverseSign();
    } // Логика игрока

    static void SmartOpponent()
    {
        int bestMove = FindBestMove(new List<string>(field), nextMove, GetOpponentSymbol());
        field[bestMove] = nextMove;
        nameWinner = "SmartOpponent";

        if (gameMode != 3) DrawField();
        CheckWinAndDraw();
        ReverseSign();
    } // Логика умного оппонента

    static void RandomOpponent()
    {
        while (true)
        {
            int randomCell = Random.Shared.Next(0, 9);
            if (field[randomCell] == " ")
            {
                field[randomCell] = nextMove;
                break;
            }
        }

        nameWinner = "RandomOpponent";

        if (gameMode != 3) DrawField();
        CheckWinAndDraw();
        ReverseSign();
    } // Логика рандомного оппонента

    static void ReverseSign()
    {
        nextMove = nextMove == "+" ? "0" : "+";
    } // Меняем символ на противоположный

    static string GetOpponentSymbol()
    {
        return nextMove == "+" ? "0" : "+";
    } // Вернуть символ опоонента

    static void GameModePlayerAndSmartOpponent()
    {
        int rand = Random.Shared.Next(0, 2);

        if (rand == 0)
        {
            while (!isWin && !isDraw)
            {
                SmartOpponent();
                if (isWin || isDraw) break;
                Player();
                if (isWin || isDraw) break;
            }
        }
        else
        {
            while (!isWin && !isDraw)
            {
                Player();
                if (isWin || isDraw) break;
                SmartOpponent();
                if (isWin || isDraw) break;
            }
        }
    }

    static void GameModePlayerAndRandomOpponent()
    {
        int rand = Random.Shared.Next(0, 2);

        if (rand == 0)
        {
            while (!isWin && !isDraw)
            {
                RandomOpponent();
                if (isWin || isDraw) break;
                Player();
                if (isWin || isDraw) break;
            }
        }
        else
        {
            while (!isWin && !isDraw)
            {
                Player();
                if (isWin || isDraw) break;
                RandomOpponent();
                if (isWin || isDraw) break;
            }
        }
    }

    static void GameModeSmartOpponentAndRandomOpponent()
    {
        // Счетчики очков оппонентов
        int randomOpponentWin = 0;
        int smartOpponentWin = 0;
        int drawGame = 0;

        int countParties; // Количество партий между опоонентами
        do
        {
            Console.WriteLine("Введите количество игр, которое сыграют оппоненты:");
        } while (!(int.TryParse(Console.ReadLine(), out countParties) && countParties > 0));

        for (int i = 0; i < countParties; i++)
        {
            ResetGame(); // 
            int rand = Random.Shared.Next(0, 2);

            if (rand == 0)
            {
                while (!isWin && !isDraw)
                {
                    SmartOpponent();
                    if (isWin) { smartOpponentWin++; break; }
                    if (isDraw) { drawGame++; break; }

                    RandomOpponent();
                    if (isWin) { randomOpponentWin++; break; }
                    if (isDraw) { drawGame++; break; }
                }
            }
            else
            {
                while (!isWin && !isDraw)
                {
                    RandomOpponent();
                    if (isWin) { randomOpponentWin++; break; }
                    if (isDraw) { drawGame++; break; }

                    SmartOpponent();
                    if (isWin) { smartOpponentWin++; break; }
                    if (isDraw) { drawGame++; break; }
                }
            }

            isWin = false;
            isDraw = false;
        }

        string newTask = $"SmartOpponent {smartOpponentWin} | {drawGame} | {randomOpponentWin} RandomOpponent";
        infoGames.Add(newTask);
        File.AppendAllText(fileInformationGame, newTask + Environment.NewLine);
        Console.WriteLine("Информация о новых играх добавлена!\n");
    }

    static void CheckWinAndDraw()
    {
        int[,] winningLines = {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8},
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8},
            {0, 4, 8}, {2, 4, 6}
        };

        for (int i = 0; i < 8; i++)
        {
            int a = winningLines[i, 0];
            int b = winningLines[i, 1];
            int c = winningLines[i, 2];

            if (field[a] != " " && field[a] == field[b] && field[a] == field[c])
            {
                isWin = true;
                break;
            }
        }

        // Проверка на ничью
        isDraw = true;
        foreach (string cell in field)
        {
            if (cell == " ")
            {
                isDraw = false;
                break;
            }
        }
    } // Проверка результата игры

    static void WinOrDrawMessage()
    {
        if (isWin)
        {
            if (gameMode != 3) Console.WriteLine($"\nПобедитель: {nameWinner}!");
        }
        else
        {
            if (gameMode != 3) Console.WriteLine("\nНичья!");
        }
        Thread.Sleep(3000);
    } // Сообщение о победе/ничьей (при игре игрока с кем-либо)

    static int FindBestMove(List<string> currentField, string currentSymbol, string opponentSymbol)
    {
        int bestScore = int.MinValue;
        int bestMove = -1;

        // Перебираем все возможные ходы
        for (int i = 0; i < 9; i++)
        {
            if (currentField[i] == " ") // Если клетка пустая
            {
                // Пробуем сделать ход в эту клетку
                currentField[i] = currentSymbol;

                // Оцениваем этот ход с помощью алгоритма Minimax (алгоритма поиска лучшего хода)
                int score = Minimax(currentField, 0, false, currentSymbol, opponentSymbol);

                // Отменяем пробный ход
                currentField[i] = " ";

                // Если этот ход лучше предыдущих лучших
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        return bestMove;
    } // Поиск лучшего хода из всех возможных на доске

    static int Minimax(List<string> board, int depth, bool isComputerTurn, string aiSymbol, string playerSymbol)
    {
        // Проверяем, не закончилась ли игра
        int gameResult = CheckGameResult(board, aiSymbol, playerSymbol);
        if (gameResult != 0) return gameResult;

        // Проверяем ничью
        if (IsBoardFull(board)) return 0;

        if (isComputerTurn) // Ход ИИ
        {
            int bestScore = int.MinValue;

            for (int i = 0; i < 9; i++)
            {
                if (board[i] == " ")
                {
                    // ИИ делает ход
                    board[i] = aiSymbol;

                    // Рекурсивно оцениваем ответ игрока
                    int score = Minimax(board, depth + 1, false, aiSymbol, playerSymbol);

                    // Отменяем ход
                    board[i] = " ";

                    // Выбираем лучшую оценку (для ИИ)
                    bestScore = Math.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else // Ход игрока
        {
            int bestScore = int.MaxValue;

            for (int i = 0; i < 9; i++)
            {
                if (board[i] == " ")
                {
                    // Игрок делает ход
                    board[i] = playerSymbol;

                    // Рекурсивно оцениваем ответ ИИ
                    int score = Minimax(board, depth + 1, true, aiSymbol, playerSymbol);

                    // Отменяем ход
                    board[i] = " ";

                    // Выбираем худшую оценку (для ИИ) (Лучшую для игрока)
                    bestScore = Math.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    } // Алгоритм поиска лучшего хода (рекурсивный)

    static int CheckGameResult(List<string> board, string aiSymbol, string playerSymbol)
    {
        int[,] winningLines = {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8},
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8},
            {0, 4, 8}, {2, 4, 6}
        };

        // Проверяем все выигрышные комбинации
        for (int i = 0; i < 8; i++)
        {
            int a = winningLines[i, 0];
            int b = winningLines[i, 1];
            int c = winningLines[i, 2];

            // Если найдена выигрышная комбинация
            if (board[a] != " " && board[a] == board[b] && board[a] == board[c])
            {
                // +10 если победил ИИ, -10 если победил игрок
                return board[a] == aiSymbol ? 10 : -10;
            }
        }

        return 0; // Игра продолжается
    } // Проверка результата игры для алгоритма Minimax

    static bool IsBoardFull(List<string> board)
    {
        foreach (string cell in board)
        {
            if (cell == " ") return false;
        }
        return true;
    } // Проверка заполнена ли доска
}