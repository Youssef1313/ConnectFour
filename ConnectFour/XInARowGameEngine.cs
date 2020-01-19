using System;
using System.Collections.Generic;
using System.Linq;

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
            if (rows <= 0)
            {
                throw new InvalidOperationException($"Argument '{nameof(rows)}' must be a positive value. The value '{rows}' not allowed.");
            }
            if (columns <= 0)
            {
                throw new InvalidOperationException($"Argument '{nameof(columns)}' must be a positive value. The value '{columns}' not allowed.");
            }

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

        public IEnumerable<Player> GetColumn(int columnIndex) => Enumerable.Range(0, Rows).Select(m => _board[m, columnIndex]);
        public IEnumerable<Player> GetRow(int rowIndex) => Enumerable.Range(0, Columns).Select(m => _board[rowIndex, m]);

        // Private methods.
        private Player GetWinner()
        {
            // Check rows.
            for (var i = 0; i < Rows; i++)
            {
                Player winner = HasXInARow(GetRow(i));
                if (winner != Player.None)
                {
                    return winner;
                }
            }

            // Check columns.
            for (var i = 0; i < Columns; i++)
            {
                Player winner = HasXInARow(GetColumn(i));
                if (winner != Player.None)
                {
                    return winner;
                }
            }

            // TODO: Performance ??

            var possibleIndices = new List<(int Row, int Column)>();
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    possibleIndices.Add((i, j));
                }
            }

            // Check BottomLeft diagonals.
            for (var i = X; i <= Rows + Columns; i++) // Start with X instead of 1 for performance. (Total number of diagonals is Rows + Columns, but we don't need diagonals containing less than X elements)
            {
                IEnumerable<(int Row, int Column)> diagonal = possibleIndices.Where(t => t.Row + t.Column == i);
                if (diagonal.Count() < X) break;
                Player winner = HasXInARow(diagonal.Select(t => _board[t.Row, t.Column]));
                if (winner != Player.None)
                {
                    return winner;
                }
            }

            // Check BottomRight diagonals. (Too bad performance - the general idea isn't too good)
            int max = Math.Max(Rows, Columns);
            for (var i = -max; i <= max; i++)
            {
                IEnumerable<(int Row, int Column)> diagonal = possibleIndices.Where(t => t.Row - t.Column == i);
                Player winner = HasXInARow(diagonal.Select(t => _board[t.Row, t.Column]));
                if (winner != Player.None)
                {
                    return winner;
                }
            }
            return Player.None;
        }

        private Player HasXInARow(IEnumerable<Player> elements)
        {
            Player player = Player.None;
            int count = 0;
            foreach (Player element in elements)
            {
                if (element == Player.None)
                {
                    player = element;
                    count = 0;
                }
                else if (element == player)
                {
                    if (++count == X)
                    {
                        return player;
                    }
                }
                else
                {
                    player = element;
                    count = 1;
                }
            }
            return Player.None;
        }
    }
}
