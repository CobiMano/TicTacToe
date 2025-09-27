using Additional_Homework_Tic_Tac_Toe_.Interfaces;
using TicTacToe.UI;

namespace Additional_Homework_Tic_Tac_Toe_.Opponents;

public class Player : IPlayer
{
    public string Name => nameof(Player);

    private UI _ui;
    private List<string> _field;
    public Player(UI ui)
    {
        _ui = ui;
        _field = _ui.GetField();
    }

    public void MakeMove()
    {
        int userInput;
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out userInput) && userInput > 0 && userInput < 10 && _field[userInput - 1] == " ")
            {
                break;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            _ui.DrawField();
            Thread.Sleep(200);
            Console.ResetColor();
            _ui.DrawField();
        }

        _field[userInput - 1] = _ui.GetNextMove();
        _ui.SetNameWinner(Name);
    }
}