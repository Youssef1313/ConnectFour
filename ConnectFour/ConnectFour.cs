using System;
using System.Linq;

namespace ConnectFour
{
    class ConnectFour
    {

        private readonly IXInARowGameEngine _engine;

        public Player Winner { get => _engine.Winner; }
        public Player WhoseTurn { get => _engine.WhoseTurn; }
        public bool IsBoardFull { get => _engine.IsBoardFull; }

        public ConnectFour(IXInARowGameEngine engine)
        {
            _engine = engine;
            _engine.SetDimensions(rows: 6, columns: 7);
            _engine.X = 4;
        }

        public void Play(byte columnIndex)
        {

            Player[] column = _engine.GetColumn(columnIndex).ToArray();
            if (column.Count(m => m == Player.None) == 0) throw new InvalidOperationException("This column is already filled.");
            int rowIndex = Array.LastIndexOf(column, Player.None);
            _engine.Play(rowIndex, columnIndex);
        }

        public void UndoLastMove()
        {
            _engine.UndoLastMove();
        }

        public Player GetPieceAt(int row, int column)
        {
            return _engine.GetPieceAt(row, column);
        }


    }
}
