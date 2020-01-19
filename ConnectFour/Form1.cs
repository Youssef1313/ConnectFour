using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ConnectFour
{
    public partial class Form1 : Form
    {
        private  ConnectFour connectFour = new ConnectFour(new XInARowGameEngine());
        public Form1()
        {
            InitializeComponent();
            foreach (var pb in Controls.OfType<PictureBox>())
            {
                pb.Click += pb_Click;
            }

        }

        private void pb_Click(object sender, EventArgs e)
        {
            if (connectFour.IsBoardFull || connectFour.Winner != Player.None) return;
            try
            {
                var colIndex = Convert.ToByte(((PictureBox)sender).Tag);
                connectFour.Play(colIndex);
            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }
            DrawBoard(connectFour);
            if (connectFour.Winner != Player.None)
            {
                MessageBox.Show($"GAME OVER!!\n{connectFour.Winner} Wins");
            }
            else if (connectFour.IsBoardFull) // Don't remove the else! The player may win in last move and the two conditions will be true.
            {
                MessageBox.Show("The game ended in a draw.");
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
            label1.Text = $"{this.connectFour.WhoseTurn}'s Turn";
            label1.ForeColor = this.connectFour.WhoseTurn == Player.Red ? Color.Red : Color.Yellow;
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to create a new game ?\nThis action can NOT be undone",
                    Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) return;
            connectFour = new ConnectFour(new XInARowGameEngine());
            DrawBoard(connectFour);
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                connectFour.UndoLastMove();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            DrawBoard(connectFour);

        }
    }
}
