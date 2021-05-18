using System;
using System.Windows.Forms;

namespace InfoBez_Elgamal
{
    public partial class Form1 : Form
    {
        Elgamal Elgamal = new Elgamal();

        public Form1()
        {
            InitializeComponent();
            textBox_PrimeNumbers_From.Text = Elgamal.PrimesFrom.ToString();
            textBox_PrimeNumbers_To.Text = Elgamal.PrimesTo.ToString();
        }

        private void button_GenerateP_Click(object sender, EventArgs e)
        {
            Elgamal.GenerateP();
            textBox_P.Text = Elgamal.P.ToString();

            Elgamal.GenerateG();
            textBox_G.Text = Elgamal.G.ToString();
        }

        private void button_GenerateE_Click(object sender, EventArgs e)
        {
            Elgamal.GenerateX();
            textBox_X.Text = Elgamal.X.ToString();            
        }

        private void button_CalculateY_Click(object sender, EventArgs e)
        {
            Elgamal.CalculateY();
            textBox_Y.Text = Elgamal.Y.ToString();
        }

        private void button_Encrypt_Click(object sender, EventArgs e)
        {
            if (textBox_P.Text == "" || textBox_G.Text == "" || textBox_X.Text == "" || textBox_Y.Text == "")
            {
                richTextBox_InputText.Text = "Generate all parameters";
                return;
            }
            if (richTextBox_InputText.Text == "")
            {
                richTextBox_InputText.Text = "Type some text here...";
                return;
            }
            Elgamal.InputText = richTextBox_InputText.Text;
            Elgamal.Encrypt();
            richTextBox_EncryptedText.Text = Elgamal.EncryptedText;
        }

        private void button_Decrypt_Click(object sender, EventArgs e)
        {
            Elgamal.EncryptedText = richTextBox_EncryptedText.Text;
            Elgamal.Decrypt();
            richTextBox_DecryptedText.Text = Elgamal.DecrypredText;
        }

        private void textBox_PrimeNumbers_From_TextChanged(object sender, EventArgs e)
        {
            if (textBox_PrimeNumbers_From.Text != "")
            {
                ulong from = (ulong)int.Parse(textBox_PrimeNumbers_From.Text);
                Elgamal.PrimesFrom = from;
                Elgamal.PrimeNumbers.Clear();
            }

            if (Elgamal.PrimesFrom > Elgamal.PrimesTo)
            {
                Elgamal.PrimesFrom = Elgamal.PrimesTo - 1;
                textBox_PrimeNumbers_From.Text = Elgamal.PrimesFrom.ToString();
            }
        }

        private void textBox_PrimeNumbers_To_TextChanged(object sender, EventArgs e)
        {
            if (textBox_PrimeNumbers_To.Text != "")
            {
                ulong to = (ulong)int.Parse(textBox_PrimeNumbers_To.Text);
                Elgamal.PrimesTo = to;
                Elgamal.PrimeNumbers.Clear();
            }

            if (Elgamal.PrimesTo < Elgamal.PrimesFrom)
            {
                Elgamal.PrimesTo = Elgamal.PrimesFrom + 1;
                textBox_PrimeNumbers_To.Text = Elgamal.PrimesTo.ToString();
            }
        }

        private void button_GenerateK_Click(object sender, EventArgs e)
        {
            Elgamal.GenerateK();
            textBox_k.Text = Elgamal.K.ToString();
        }
    }
}
