using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    class Board
    {
        private string _moves = "";
        private const byte RowsCount = 6;
        private const byte ColumnsCount = 7;

        public Piece PlayerTurn { get; private set; } = Piece.Red;

        public Piece[,] GameBoard { get; private set; } = new Piece[RowsCount, ColumnsCount];

        public void Play(byte columnIndex)
        {
            if (columnIndex >= ColumnsCount) throw new ArgumentOutOfRangeException(nameof(columnIndex));
            var column = GetColumn(columnIndex);
            if (column.Count(m => m == Piece.None) == 0) throw new InvalidOperationException("This column is already filled.");
            for (var i = column.Length - 1; i >= 0; i--)
            {
                if (column[i] != Piece.None) continue;
                GameBoard[i, columnIndex] = PlayerTurn;
                _moves += columnIndex.ToString();
                RevertPlayer();
                break;
            }
        }

        public void UndoLastMove()
        {
            if (_moves.Length == 0) throw new InvalidOperationException("There is nothing to undo.");
            var tempMoves = _moves.Substring(0, _moves.Length - 1);
            _moves = "";
            GameBoard = new Piece[RowsCount, ColumnsCount];
            PlayerTurn = Piece.Red;
            foreach (var move in tempMoves)
            {
                Play((byte) (move - '0'));
            }
        }

        public Piece CheckForWinner()
        {
            var winningPiece = Piece.None;
            for (byte i = 0; i < ColumnsCount; i++)
            {
                winningPiece = HasForInRow(GetColumn(i));
                if (winningPiece != Piece.None) return winningPiece;
            }

            for (byte i = 0; i < RowsCount; i++)
            {
                 winningPiece = HasForInRow(GetRow(i));
                if (winningPiece != Piece.None) return winningPiece;
            }
            // negative slope
            var diagonal1 = new[] {GameBoard[2, 0], GameBoard[3, 1], GameBoard[4, 2], GameBoard[5, 3]};
            var diagonal2 = new[] {GameBoard[1, 0], GameBoard[2, 1], GameBoard[3, 2], GameBoard[4, 3], GameBoard[5, 4]};
            var diagonal3 = new[] {GameBoard[0, 0], GameBoard[1, 1], GameBoard[2, 2], GameBoard[3, 3], GameBoard[4, 4], GameBoard[5, 5]};
            var diagonal4 = new[] {GameBoard[0, 1], GameBoard[1, 2], GameBoard[2, 3], GameBoard[3, 4], GameBoard[4, 5], GameBoard[5, 6]};
            var diagonal5 = new[] {GameBoard[0, 2], GameBoard[1, 3], GameBoard[2, 4], GameBoard[3, 5], GameBoard[4, 6]};
            var diagonal6 = new[] {GameBoard[0, 3], GameBoard[1, 4], GameBoard[2, 5], GameBoard[3, 6]};

            // positive slope
            var diagonal7 = new[] { GameBoard[3, 0], GameBoard[2, 1], GameBoard[1, 2], GameBoard[0, 3] };
            var diagonal8 = new[] { GameBoard[4, 0], GameBoard[3, 1], GameBoard[2, 2], GameBoard[1, 3], GameBoard[0, 4]};
            var diagonal9 = new[] { GameBoard[5, 0], GameBoard[4, 1], GameBoard[3, 2], GameBoard[2, 3], GameBoard[1, 4], GameBoard[0, 5]};
            var diagonal10 = new[] { GameBoard[5, 1], GameBoard[4, 2], GameBoard[3, 3], GameBoard[2, 4], GameBoard[1, 5], GameBoard[0, 6] };
            var diagonal11 = new[] { GameBoard[5, 2], GameBoard[4, 3], GameBoard[3, 4], GameBoard[2, 5], GameBoard[1, 6]};
            var diagonal12 = new[] { GameBoard[5, 3], GameBoard[4, 4], GameBoard[3, 5], GameBoard[2, 6]};

            winningPiece = HasForInRow(diagonal1);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal2);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal3);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal4);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal5);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal6);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal7);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal8);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal9);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal10);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal11);
            if (winningPiece != Piece.None) return winningPiece;

            winningPiece = HasForInRow(diagonal12);
            if (winningPiece != Piece.None) return winningPiece;
            return Piece.None;
        }

        private static Piece HasForInRow(Piece[] pieces)
        {
            byte connected = 0;
            var lastPiece = Piece.None;
            foreach (var piece in pieces)
            {
                if (piece == Piece.None)
                {
                    connected = 0;
                    continue;
                }

                if (piece != lastPiece)
                {
                    connected = 1;
                    lastPiece = piece;
                    continue;
                }

                connected++;
                if (connected == 4)
                {
                    return lastPiece;
                }
            }

            return Piece.None;
        }

        private void RevertPlayer()
        {
            PlayerTurn = PlayerTurn == Piece.Red ? Piece.Yellow : Piece.Red;
        }

        private Piece[] GetRow(byte rowIndex)
        {
            return Enumerable.Range(0, ColumnsCount).Select(m => GameBoard[rowIndex, m]).ToArray();
        }

        private Piece[] GetColumn(byte columnIndex)
        {
            return Enumerable.Range(0, RowsCount).Select(m => GameBoard[m, columnIndex]).ToArray();
        }

    }
}
