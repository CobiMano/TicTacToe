using Additional_Homework_Tic_Tac_Toe_.Opponents;
using TicTacToe.UI;

namespace Additional_Homework_Tic_Tac_Toe_;

public class GameManager
{
    private bool isWin;
    private bool isDraw;
    private string nextMove = "+";
    private string nameWinner = "";

    private string fileInformationGame;
    private List<string> infoGames;

    private List<string> _field;
    private UI _ui;
    private Player _player;
    private SmartOpponent _smartOpponent;
    private RandomOpponent _randomOpponent;
    public GameManager()
    {
        _field = new List<string>();

        _ui = new UI(_field, nextMove, nameWinner);
        _player = new Player(_ui);
        _smartOpponent = new SmartOpponent(_ui);
        _randomOpponent = new RandomOpponent(_ui);
    }

    public void StartGame()
    {
        InitializeGameFile();

        while (true)
        {
            ResetGame();
            _ui.DrawMenu();
            LaunchGame();
        }
    }

    private void ResetGame()
    {
        isWin = false;
        isDraw = false;
        _field.Clear();
        for (int i = 0; i < 9; i++)
        {
                _field.Add(" ");
        }
    } // Сброс игры

    private void InitializeGameFile()
    {
        fileInformationGame = "../../../../TicTacToeGame.txt";

        if (!File.Exists(fileInformationGame))
        {
            File.Create(fileInformationGame).Close();
        }

        infoGames = File.ReadAllLines(fileInformationGame).ToList();
    } // Инициализация игрового файла

    private void LaunchGame()
    {
        switch (_ui.gameMode)
        {
            case 1: GameModePlayerAndRandomOpponent(); break;
            case 2: GameModePlayerAndSmartOpponent(); break;
            case 3: GameModeSmartOpponentAndRandomOpponent(); break;
        }
        WinOrDrawMessage();
    } // Запуск логики игры

    private void GameModePlayerAndSmartOpponent()
    {
        int rand = Random.Shared.Next(0, 2);

        if (rand == 0)
        {
            while (!isWin && !isDraw)
            {
                _smartOpponent.MakeMove();
                if (CheckCompletionGame()) break;

                _player.MakeMove();
                if (CheckCompletionGame()) break;
            }
        }
        else
        {
            while (!isWin && !isDraw)
            {
                _player.MakeMove();
                if (CheckCompletionGame()) break;

                _smartOpponent.MakeMove();
                if (CheckCompletionGame()) break;
            }
        }
    }

    private void GameModePlayerAndRandomOpponent()
    {
        int rand = Random.Shared.Next(0, 2);

        if (rand == 0)
        {
            while (!isWin && !isDraw)
            {
                _randomOpponent.MakeMove();
                if (CheckCompletionGame()) break;

                _player.MakeMove();
                if (CheckCompletionGame()) break;
            }
        }
        else
        {
            while (!isWin && !isDraw)
            {
                _player.MakeMove();
                if (CheckCompletionGame()) break;

                _randomOpponent.MakeMove();
                if (CheckCompletionGame()) break;
            }
        }
    }

    private void GameModeSmartOpponentAndRandomOpponent()
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
            ResetGame();
            int rand = Random.Shared.Next(0, 2);

            if (rand == 0)
            {
                while (!isWin && !isDraw)
                {
                    _smartOpponent.MakeMove();
                    CheckCompletionGame();
                    if (isWin) { smartOpponentWin++; break; }
                    if (isDraw) { drawGame++; break; }

                    _randomOpponent.MakeMove();
                    CheckCompletionGame();
                    if (isWin) { randomOpponentWin++; break; }
                    if (isDraw) { drawGame++; break; }
                }
            }
            else
            {
                while (!isWin && !isDraw)
                {
                    _randomOpponent.MakeMove();
                    CheckCompletionGame();
                    if (isWin) { randomOpponentWin++; break; }
                    if (isDraw) { drawGame++; break; }

                    _smartOpponent.MakeMove();
                    CheckCompletionGame();
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

    private bool CheckCompletionGame()
    {
        ReverseSign();

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

            if (_field[a] != " " && _field[a] == _field[b] && _field[a] == _field[c])
            {
                isWin = true;
                return true;
            }
        }

        // Проверка на ничью
        isDraw = true; // Изначально предполагаем, что ничья
        foreach (string cell in _field)
        {
            if (cell == " ") // Если хоть одна клетка не заполнена, то значит не ничья
            {
                isDraw = false;
                return false;
            }
        }

        return true; // Ничья, если дошло до этой строчки
    } // Проверка завершения игры

    void WinOrDrawMessage()
    {
        if (isWin)
        {
            if (_ui.gameMode != 3) Console.WriteLine($"\nПобедитель: {_ui.GetNameWinner()}!");
        }
        else
        {
            if (_ui.gameMode != 3) Console.WriteLine("\nНичья!");
        }
        Thread.Sleep(3000);
    } // Сообщение о победе/ничьей (при игре игрока с кем-либо)

    void ReverseSign()
    {
        nextMove = nextMove == "+" ? "0" : "+";
        _ui.SetNextMove(nextMove);
    } // Меняем символ на противоположный
}
