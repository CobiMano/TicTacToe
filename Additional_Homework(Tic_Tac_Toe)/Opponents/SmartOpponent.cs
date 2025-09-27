using Additional_Homework_Tic_Tac_Toe_.Interfaces;
using TicTacToe.UI;

namespace Additional_Homework_Tic_Tac_Toe_.Opponents;

public class SmartOpponent : IPlayer
{
    public string Name => nameof(SmartOpponent);

    private UI _ui;
    private List<string> _field;
    public SmartOpponent(UI ui)
    {
        _ui = ui;
        _field = _ui.GetField();
    }

    public void MakeMove()
    {
        int bestMove = FindBestMove(new List<string>(_field), _ui.GetNextMove(), GetOpponentSymbol());
        _field[bestMove] = _ui.GetNextMove();

        if (_ui.gameMode != 3) _ui.DrawField();

        _ui.SetNameWinner(Name);
    } // Логика умного оппонента

    private int FindBestMove(List<string> currentField, string currentSymbol, string opponentSymbol)
    {
        List<Task> tasks = new List<Task>();
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

    private int Minimax(List<string> board, int depth, bool isComputerTurn, string aiSymbol, string playerSymbol)
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

    private int CheckGameResult(List<string> board, string aiSymbol, string playerSymbol)
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

    private bool IsBoardFull(List<string> board)
    {
        foreach (string cell in board)
        {
            if (cell == " ") return false;
        }
        return true;
    } // Проверка заполнена ли доска

    private string GetOpponentSymbol()
    {
        return _ui.GetNextMove() == "+" ? "0" : "+";
    } // Вернуть символ опоонента
}
