using Additional_Homework_Tic_Tac_Toe_.Interfaces;
using TicTacToe.UI;

namespace Additional_Homework_Tic_Tac_Toe_.Opponents;

public class RandomOpponent : IPlayer
{
    public string Name => nameof(RandomOpponent);

    private UI _ui;
    private List<string> _field;
    public RandomOpponent(UI ui)
    {
        _ui = ui;
        _field = _ui.GetField();
    }

    public void MakeMove()
    {
        while (true)
        {
            int randomCell = Random.Shared.Next(0, 9);
            if (_field[randomCell] == " ")
            {
                _field[randomCell] = _ui.GetNextMove();
                break;
            }
        }

        if (_ui.gameMode != 3) _ui.DrawField();

        _ui.SetNameWinner(Name);
    } // Логика рандомного оппонента
}