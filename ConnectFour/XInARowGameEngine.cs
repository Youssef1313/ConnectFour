using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    class XInARowGameEngine : IXInARowGameEngine
    {

        // Private fields.
        private Player[,] _board;
        private Stack<(int Row, int Column)> _moves = new Stack<(int Row, int Column)>();
        private int _filledSpots = 0;

        // Public properties.
        public int X { get; set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public Player WhoseTurn { get; private set; } = Player.Red;
        public Player Winner { get; private set; } = Player.None;
        public bool IsBoardFull { get; private set; } = false;

        // Public methods.
        public void SetDimensions(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            WhoseTurn = Player.Red;
            _board = new Player[rows, columns];
            _filledSpots = 0;
            _moves = new Stack<(int Row, int Column)>();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    _board[i, j] = Player.None;
                }
            }
        }
        public void Play(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || rowIndex > Rows - 1)
            {
                throw new InvalidOperationException($"The index {rowIndex} isn't valid for {nameof(rowIndex)}. It should be between 0 and {Rows - 1} inclusively.");
            }

            if (columnIndex < 0 || columnIndex > Columns - 1)
            {
                throw new InvalidOperationException($"The index {columnIndex} isn't valid for {nameof(columnIndex)}. It should be between 0 and {Columns - 1} inclusively.");
            }

            if (_board[rowIndex, columnIndex] != Player.None)
            {
                throw new InvalidOperationException($"You can't play in a position that's already occupied by {_board[rowIndex, columnIndex]}.");
            }

            _moves.Push((rowIndex, columnIndex));
            IsBoardFull = (++_filledSpots == Rows * Columns);
            _board[rowIndex, columnIndex] = WhoseTurn;
            WhoseTurn = (WhoseTurn == Player.Red) ? Player.Yellow : Player.Red;
            Winner = GetWinner();
        }

        public void UndoLastMove()
        {
            if (!_moves.Any())
            {
                throw new InvalidOperationException("There are no moves to undo.");
            }

            (int LastRow, int LastColumn) = _moves.Pop();
            
            if (_board[LastRow, LastColumn] == Player.None)
            {
                throw new InvalidCastException("Program shouldn't be in this state. This indicates an internal bug."); // Should never happen.
            }

            _board[LastRow, LastColumn] = Player.None;
            IsBoardFull = (--_filledSpots == Rows * Columns);
            WhoseTurn = (WhoseTurn == Player.Red) ? Player.Yellow : Player.Red;
            Winner = GetWinner();
        }

        public Player GetPieceAt(int row, int column) => _board[row, column];

        public Player[] GetColumn(int columnIndex) => Enumerable.Range(0, Rows).Select(m => _board[m, columnIndex]).ToArray();

        // Private methods.
        private Player GetWinner()
        {
            throw new NotImplementedException(); // TODO: Implement it!
        }
    }
}
