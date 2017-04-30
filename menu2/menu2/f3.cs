using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Numerics;

namespace menu2
{
    public partial class f3 : Form
    {
        public f3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();

            byte[] gByte = Encoding.Default.GetBytes(textBox1.Text);
            BigInteger g = new BigInteger(gByte);
            BigInteger p = new BigInteger(Convert.ToInt64(textBox2.Text));

            cript server = new cript();
            Random rnd = new Random();

            RSAParameters BobPublicRsaPara = server.GetPublicRsaPara(cript.ClientName.Bob);
            BigInteger BobModulus = new BigInteger(BobPublicRsaPara.Modulus);
            BigInteger BobExponent = new BigInteger(BobPublicRsaPara.Exponent);
            string sBobPublicPara = BobExponent.ToString() + ", " + BobModulus.ToString();
            textBox3.Text = sBobPublicPara;

            RSAParameters AlicePublicRsaPara = server.GetPublicRsaPara(cript.ClientName.Alice);
            BigInteger AliceModulus = new BigInteger(AlicePublicRsaPara.Modulus);
            BigInteger AliceExponent = new BigInteger(AlicePublicRsaPara.Exponent);
            string sAlicePublicPara = AliceExponent.ToString() + ", " + AliceModulus.ToString();
            textBox4.Text = sAlicePublicPara;

            int x, y;

            x = rnd.Next(2, Convert.ToInt32(textBox2.Text));
            y = rnd.Next(2, Convert.ToInt32(textBox2.Text));

            BigInteger ma, mb, Ka, Kb;

            ma = BigInteger.ModPow(g, x, p);
            mb = BigInteger.ModPow(g, y, p);

            richTextBox1.AppendText("1) Сгенерированное X=" + x.ToString() + "\nОтправлено Бобу(g^xmodp):" + ma.ToString() + "\n");

            richTextBox2.AppendText("2) Получено от Алисы:" + ma.ToString() + "\n");
            richTextBox2.AppendText("3) Сгенерированное Y=" + y.ToString() + "\n");

            Ka = BigInteger.ModPow(mb, x, p);
            Kb = BigInteger.ModPow(ma, y, p);

            richTextBox2.AppendText("Созданный сеансовый ключ K=" + Kb.ToString() + "\n");

            String AIv = "", BIv = "";
            try
            {
                server.InitAesProvider(cript.ClientName.Alice, Ka.ToString(), AIv);
                server.InitAesProvider(cript.ClientName.Bob, Kb.ToString(), BIv);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
                return;
            }

            string MesForAlis = ma.ToString() + "," + mb.ToString();
            byte[] BobSign = server.RsaSign(cript.ClientName.Bob, Encoding.Default.GetBytes(MesForAlis));
            byte[] Ekb = server.Encrypt(cript.ClientName.Bob, Encoding.Default.GetString(BobSign));
            richTextBox2.AppendText("Отправлено Алисе(mb+Ek(Signb(ma,mb):\n" + mb.ToString() + "," + Encoding.Default.GetString(Ekb) + "\n");

            richTextBox1.AppendText("4) Получено от Боба:\n" + mb.ToString() + "," + Encoding.Default.GetString(Ekb));
            richTextBox1.AppendText("\nСозданный сеансовый ключ K=" + Ka.ToString() + "\n\n");

            string recMessageAlice = server.Decrypt(cript.ClientName.Bob, Ekb);

            richTextBox1.AppendText("Расшифрованное сообщение:\n" + recMessageAlice + "\n");

            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
            {
                byte[] hash = sha256.ComputeHash(Encoding.Default.GetBytes(MesForAlis));
                if (server.RsaVerify(cript.ClientName.Bob, hash, BobSign)) richTextBox1.AppendText("\nПодпись валидна\n");
                else richTextBox1.AppendText("\nПодпись не валидна\n");
            }

            string MesForBob = ma.ToString() + "," + mb.ToString();
            byte[] AlisSign = server.RsaSign(cript.ClientName.Alice, Encoding.Default.GetBytes(MesForBob));
            byte[] Eka = server.Encrypt(cript.ClientName.Alice, Encoding.Default.GetString(AlisSign));
            richTextBox1.AppendText("5) Сообщение для Боба(Ek(Signa(ma,mb))):\n" + Encoding.Default.GetString(Eka));

            richTextBox2.AppendText("\n6) Получено от Алисы:\n" + Encoding.Default.GetString(Eka));

            string recMessageBob = server.Decrypt(cript.ClientName.Alice, Eka);

            richTextBox2.AppendText("\n\nРасшифрованное сообщение:\n" + recMessageBob + "\n");

            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
            {
                byte[] hash = sha256.ComputeHash(Encoding.Default.GetBytes(MesForBob));
                if (server.RsaVerify(cript.ClientName.Alice, hash, AlisSign)) richTextBox2.AppendText("\nПодпись валидна\n");
                else richTextBox2.AppendText("\nПодпись не валидна\n");
            }
        }
    }
}
