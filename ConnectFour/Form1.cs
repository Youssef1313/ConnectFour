using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectFour
{
    public partial class Form1 : Form
    {
        private  Board _board = new Board();
        public Form1()
        {
            InitializeComponent();
            foreach (var pb in this.Controls.OfType<PictureBox>())
            {
                pb.Click += pb_Click;
            }

        }

        private void pb_Click(object sender, EventArgs e)
        {
            var clickedPictureBox = (PictureBox)sender;
            try
            {
                var colIndex = Convert.ToByte(clickedPictureBox.Tag);
                _board.Play(colIndex);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            DrawBoard(_board.GameBoard);
            var winnerPiece = _board.CheckForWinner();
            if (winnerPiece != Piece.None)
            {
                MessageBox.Show($"GAME OVER!!\n{winnerPiece} Wins");
                _board = new Board();
                DrawBoard(_board.GameBoard);
            }
        }

        private void DrawBoard(Piece[,] board)
        {
            foreach (var pb in this.Controls.OfType<PictureBox>())
            {
                var rowIndex = pb.Name[2] - '0';
                var columnIndex = pb.Name[3] - '0';
                var piece = board[rowIndex, columnIndex];
                switch (piece)
                {
                    case Piece.Red:
                        pb.Image = Properties.Resources.red;
                        break;
                    case Piece.Yellow:
                        pb.Image = Properties.Resources.yellow;
                        break;
                    case Piece.None:
                        pb.Image = Properties.Resources.empty;
                        break;
                }

            }
            label1.Text = _board.PlayerTurn + "'s Turn";
            label1.ForeColor = _board.PlayerTurn == Piece.Red ? Color.Red : Color.Yellow;
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to create a new game ?\nThis action can NOT be undone",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) return;
            _board = new Board();
            DrawBoard(_board.GameBoard);
        }
    }
}
