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
            label1.Text = _board.PlayerTurn + " turn";
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
        }
       
    }
}
