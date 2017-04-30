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
    public partial class f2 : Form
    {
        public f2()
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
            BigInteger p = new BigInteger(Convert.ToInt64(textBox2.Text));

            Random rnd = new Random();
            string aStr = "", bStr = "";

            for (int i = 0; i < 3; i++)
            {
                aStr += rnd.Next().ToString();
                bStr += rnd.Next().ToString();
            }

            aByte = Encoding.Default.GetBytes(aStr);
            bByte = Encoding.Default.GetBytes(bStr);
            BigInteger a = new BigInteger(aByte);
            BigInteger b = new BigInteger(bByte);

            textBox3.Text = a.ToString();
            textBox4.Text = b.ToString();

            BigInteger PKa, PKb;

            PKa = BigInteger.ModPow(g, a, p);
            PKb = BigInteger.ModPow(g, b, p);

            textBox6.Text = PKa.ToString();
            textBox7.Text = PKb.ToString();

            int x, y;

            x = rnd.Next(2, Convert.ToInt32(textBox2.Text) - 1);
            y = rnd.Next(2, Convert.ToInt32(textBox2.Text) - 1);

            BigInteger AlisMes, BobMes;

            AlisMes = BigInteger.ModPow(g, x, p);
            BobMes = BigInteger.ModPow(g, y, p);
            richTextBox1.AppendText("1) Сгенерированное X=" + x.ToString() + "\nСообщение для Боба(g^xmodp):" + AlisMes.ToString() + "\n");
            richTextBox2.AppendText("2) Получено от Алисы:" + AlisMes.ToString() + "\nСгенерированное Y=" + y.ToString() + "\nСообщение для Алисы(g^xmodp):" + BobMes.ToString() + "\n");

            richTextBox1.AppendText("3) Получено от Боба:" + BobMes.ToString() + "\n");

            BigInteger Ka, Kb;

            Ka = BigInteger.ModPow(g, BigInteger.Add(BigInteger.Multiply(b, x), BigInteger.Multiply(y, a)), p);
            richTextBox1.AppendText("4) Создание сеансового ключа Ka=(g^b)^x*(g^y)^amodp=" + Ka.ToString() + "\n");

            Kb= BigInteger.ModPow(g, BigInteger.Add(BigInteger.Multiply(x, b), BigInteger.Multiply(a, y)), p);
            richTextBox2.AppendText("5) Создание сеансового ключа Kb=(g^x)^b*(g^a)^bmodp=" + Kb.ToString());

            if (Ka == Kb) textBox5.Text = "Ключи совпали";
            else textBox5.Text = "Ключи не совпали";
        }
    }
}
