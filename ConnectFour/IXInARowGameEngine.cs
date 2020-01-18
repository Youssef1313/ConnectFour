namespace ConnectFour
{
    interface IXInARowGameEngine
    {
        int X { get; set;  } // 3 for a TicTacToe, 4 for a connect four, and X for a connect X game.
        int Rows { get; }
        int Columns { get; }
        Player Winner { get; }
        Player WhoseTurn { get; }
        bool IsBoardFull { get; }
        void Play(int rowIndex, int columnIndex);
        void UndoLastMove();
        void SetDimensions(int rows, int columns);
        Player[] GetColumn(int columnIndex);
        Player GetPieceAt(int row, int column);
    }
}
