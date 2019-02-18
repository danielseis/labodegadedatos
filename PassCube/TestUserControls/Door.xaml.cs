using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Drawing;
using System.Collections;

namespace PassCube.TestUserControls
{
    /// <summary>
    /// Interaction logic for Door.xaml
    /// </summary>
    public partial class Door : Window
    {

        //string rutaOriginal = @"C:\Users\dseisdedos\Pictures\Saved Pictures\";
        string rutaOriginal = @"C:\xTrela\Data\WorkFiles\ArchivosEscaneados\Contactos\19806801267\";
        string Mensaje = "";

        public Door()
        {
            InitializeComponent();
        }

        private void btnCryptoImage_Click(object sender, RoutedEventArgs e)
        {
            // Controller.Helper.EncryptFile(rutaOriginal + "a5i8HNc.jpg", rutaOriginal + "a5i8HNc(Cript).jpg", Key);

            //Controller.Helper.EncryptFileIMG(rutaOriginal + "C100006036_6_20170307093714-_ID ESP-77738410C001.jpg", rutaOriginal + "C100006036_6_20170307093714-_ID ESP-77738410C001(Cript).jpg", "-#{t0Vs}", out Mensaje);

            //Console.WriteLine(Mensaje);


            EncryptSfile(rutaOriginal + "C1000060078_6_20160611110246-_ID ESP-39339868R000.jpg", rutaOriginal + "C1000060078_6_20160611110246-_ID ESP-39339868R000(cript).jpg", "-#{t0Vs}");




            //// Create a new bitmap.
            //Bitmap bmp = new Bitmap(rutaOriginal + "C100006036_6_20170307093714-_ID ESP-77738410C001.jpg");
            //// Convert bitmap
            //byte[] imagenOri = imageToByteArray(bmp);
            //// encript bitmap
            //byte[] imagenOriCript = EncryptBytes(imagenOri, "SensitivePhrase", "SodiumChloride");
            //// Re Create bitmap
            //System.Drawing.Image imageOut = byteArrayToImage(imagenOriCript);
            //// delete File Ori
            //File.Delete(rutaOriginal + "C100006036_6_20170307093714-_ID ESP-77738410C001.jpg");
            //// save new file
            //imageOut.Save(rutaOriginal + "C100006036_6_20170307093714-_ID ESP-77738410C001.jpg");

        }

        private void btnCryptoXml_Click(object sender, RoutedEventArgs e)
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void btnDeCryptoImage_Click(object sender, RoutedEventArgs e)
        {
            DecryptSfile(rutaOriginal + "C19806801267_6_20160127181536_NASSER MOHAMMED.jpg", rutaOriginal + "yo.jpg", "-#{t0Vs}");
        }


        #region CRYPTO
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }


        // Example usage: EncryptBytes(someFileBytes, "SensitivePhrase", "SodiumChloride");
        public static byte[] EncryptBytes(byte[] inputBytes, string passPhrase, string saltValue)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            RijndaelCipher.Mode = CipherMode.CBC;
            byte[] salt = Encoding.ASCII.GetBytes(saltValue);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, salt, "SHA1", 2);

            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(password.GetBytes(32), password.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            return CipherBytes;
        }

        // Example usage: DecryptBytes(encryptedBytes, "SensitivePhrase", "SodiumChloride");
        public static byte[] DecryptBytes(byte[] encryptedBytes, string passPhrase, string saltValue)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            RijndaelCipher.Mode = CipherMode.CBC;
            byte[] salt = Encoding.ASCII.GetBytes(saltValue);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, salt, "SHA1", 2);

            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(password.GetBytes(32), password.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream(encryptedBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
            byte[] plainBytes = new byte[encryptedBytes.Length];

            int DecryptedCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            return plainBytes;
        }
        #endregion


        #region SimpleCrypt

        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Encrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        public static void EncryptSfile(string inputFile, string outputFile, string pass)
        {

            try
            {
                string password = @"myKey123"; // Your Key Here
                password = pass; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                //MessageBox.Show("Encryption failed!", "Error");
            }
        }
        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Decrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        public static void DecryptSfile(string inputFile, string outputFile, string pass)
        {

            {
                string password = @"myKey123"; // Your Key Here
                password = pass; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }

        #endregion
    }
}
