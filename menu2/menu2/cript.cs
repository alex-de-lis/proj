using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;


namespace menu2
{
    class cript
    {
        private RSACryptoServiceProvider m_AliceRsa;
        private RSACryptoServiceProvider m_BobRsa;
        private AesCryptoServiceProvider m_AliceAes;
        private AesCryptoServiceProvider m_BobAes;

        public cript()
        {
            m_AliceAes = new AesCryptoServiceProvider();
            m_BobAes = new AesCryptoServiceProvider();
            m_AliceRsa = new RSACryptoServiceProvider();
            m_BobRsa = new RSACryptoServiceProvider();
        }

        public enum ClientName { Alice, Bob };

        public void InitAesProvider(ClientName client, String key, String IV)
        {
            AesCryptoServiceProvider prov;
            if (client == ClientName.Alice)
                prov = m_AliceAes;
            else if (client == ClientName.Bob)
                prov = m_BobAes;
            else throw new ArgumentException();
            while (key.Length < 16)
                key = key + "0";
            while (IV.Length < 16)
                IV = IV + "0";
            prov.Key = Encoding.UTF8.GetBytes(key);
            prov.IV = Encoding.UTF8.GetBytes(IV);
        }
        public byte[] Encrypt(ClientName client, String plainText)
        {
            AesCryptoServiceProvider prov;
            if (client == ClientName.Alice)
                prov = m_AliceAes;
            else if (client == ClientName.Bob)
                prov = m_BobAes;
            else throw new ArgumentException();

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (prov.Key == null || prov.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (prov.IV == null || prov.IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = prov.Key;
                aesAlg.IV = prov.IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        public string Decrypt(ClientName client, byte[] cipherText)
        {
            AesCryptoServiceProvider prov;
            if (client == ClientName.Alice)
                prov = m_AliceAes;
            else if (client == ClientName.Bob)
                prov = m_BobAes;
            else throw new ArgumentException();

            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (prov.Key == null || prov.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (prov.IV == null || prov.IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = prov.Key;
                aesAlg.IV = prov.IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }

        public byte[] RsaSign(ClientName client, byte[] plainText)
        {
            RSACryptoServiceProvider prov;
            if (client == ClientName.Alice)
                prov = m_AliceRsa;
            else if (client == ClientName.Bob)
                prov = m_BobRsa;
            else throw new ArgumentException();
            return prov.SignData(plainText, new SHA256CryptoServiceProvider());
        }
        public bool RsaVerify(ClientName client, byte[] hash, byte[] signature)
        {
            RSACryptoServiceProvider prov;
            if (client == ClientName.Alice)
                prov = m_AliceRsa;
            else if (client == ClientName.Bob)
                prov = m_BobRsa;
            else throw new ArgumentException();
            return prov.VerifyHash(hash, "SHA256", signature);
        }
        public RSAParameters GetPublicRsaPara(ClientName client)
        {
            RSACryptoServiceProvider prov;
            if (client == ClientName.Alice)
                prov = m_AliceRsa;
            else if (client == ClientName.Bob)
                prov = m_BobRsa;
            else throw new ArgumentException();
            RSAParameters rsap = prov.ExportParameters(false);
            return rsap;
        }
        public RSAParameters GetPrivateRsaPara(ClientName client)
        {
            RSACryptoServiceProvider prov;
            if (client == ClientName.Alice)
                prov = m_AliceRsa;
            else if (client == ClientName.Bob)
                prov = m_BobRsa;
            else throw new ArgumentException();
            RSAParameters rsap = prov.ExportParameters(true);
            return rsap;
        }
    }
}
