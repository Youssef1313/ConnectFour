using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ConnectFour
{
    public partial class Form1 : Form
    {
        private  ConnectFour _board = new ConnectFour(new XInARowGameEngine());
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
            DrawBoard(_board);
            if (_board.Winner != Player.None)
            {
                MessageBox.Show($"GAME OVER!!\n{_board.Winner} Wins");
                _board = new ConnectFour(new XInARowGameEngine());
                DrawBoard(_board);
            }
        }

        private void DrawBoard(ConnectFour connectFour)
        {
            foreach (var pb in this.Controls.OfType<PictureBox>())
            {
                int rowIndex = pb.Name[2] - '0';
                int columnIndex = pb.Name[3] - '0';
                Player piece = connectFour.GetPieceAt(rowIndex, columnIndex);
                switch (piece)
                {
                    case Player.Red:
                        pb.Image = Properties.Resources.red;
                        break;
                    case Player.Yellow:
                        pb.Image = Properties.Resources.yellow;
                        break;
                    case Player.None:
                        pb.Image = Properties.Resources.empty;
                        break;
                }

            }
            label1.Text = $"{_board.WhoseTurn}'s Turn";
            label1.ForeColor = _board.WhoseTurn == Player.Red ? Color.Red : Color.Yellow;
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to create a new game ?\nThis action can NOT be undone",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) return;
            _board = new ConnectFour(new XInARowGameEngine());
            DrawBoard(_board);
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                _board.UndoLastMove();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            DrawBoard(_board);

        }
    }
}
