using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace menu2
{
    public partial class f1 : Form
    {
        public f1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            byte[] gByte = Encoding.Default.GetBytes(textBox1.Text);
            byte[] aByte, bByte;
            BigInteger g = new BigInteger(gByte);
            BigInteger p = new BigInteger(Convert.ToInt64(textBox4.Text));

            Random rnd = new Random();
            string aStr = "", bStr = "";

            for(int i=0;i<3;i++)
            {
                aStr += rnd.Next().ToString();
                bStr += rnd.Next().ToString();
            }

            aByte = Encoding.Default.GetBytes(aStr);
            bByte = Encoding.Default.GetBytes(bStr);
            BigInteger a = new BigInteger(aByte);
            BigInteger b = new BigInteger(bByte);

            textBox2.Text = a.ToString();
            textBox3.Text = b.ToString();

            BigInteger AlisMes, BobMes;

            AlisMes = BigInteger.ModPow(g, a, p);
            BobMes = BigInteger.ModPow(g, b, p);

            richTextBox1.AppendText("1) Отправлено Бобу:\n" + AlisMes.ToString() + "\n");
            richTextBox2.AppendText("2) Получено от Алисы:\n" + AlisMes.ToString() + "\n");
            richTextBox2.AppendText("Отпралено Алисе:\n" + BobMes.ToString() + "\n");

            richTextBox1.AppendText("3) Получено от Боба:\n" + BobMes.ToString() + "\n");
            BigInteger Ka= BigInteger.ModPow(BobMes, a, p);
            richTextBox1.AppendText("Ka=:" + Ka.ToString() + "\n");

            BigInteger Kb = BigInteger.ModPow(AlisMes, b, p);
            richTextBox2.AppendText("Kb=:" + Kb.ToString() + "\n");

            if (Ka == Kb) textBox5.Text = "Ключи совпали";
            else textBox5.Text = "Ключи не совпали";
        }
    }
}
