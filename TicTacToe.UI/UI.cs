namespace TicTacToe.UI;
public class UI
{
    private List<string> _field;
    private string _nextMove;
    private string _nameWinner;

    public int gameMode;
    public UI(List<string>? field,  string nextMove, string nameWinner)
    {
        _field = field;
        _nextMove = nextMove;
        _nameWinner = nameWinner;
    }

    public void DrawMenu()
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
    } // Отрисовка меню

    public void DrawField()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        for (int i = 0; i < 9; i++)
        {
            if (i == 2 || i == 5 || i == 8)
            {
                Console.Write($"{_field[i]}");
            }
            else
            {
                Console.Write($"{_field[i]}|");
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

    public string GetNextMove()
    {
        return _nextMove;
    }

    public void SetNextMove(string nextMove)
    {
        _nextMove = nextMove;
    }

    public List<string> GetField()
    {
        return _field;
    }

    public string GetNameWinner()
    {
        return _nameWinner;
    }

    public void SetNameWinner(string nameWinner)
    {
        _nameWinner = nameWinner;
    }
}
