using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PassCube.BusinessLayer
{
	public class SegurityBL
	{

		// This constant is used to determine the keysize of the encryption algorithm in bits.
		// We divide this by 8 within the code below to get the equivalent number of bytes.
		private const int Keysize = 256;

		// This constant determines the number of iterations for the password bytes generation function.
		private const int DerivationIterations = 1000;


		public static string SecurityKey { get; set; }

		public static string DestroyerSecurityKey { get; set; }

		public const string AllowedChars =
		"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$?!¿¡^,.%&*()";

		public SegurityBL()
		{
			SecurityKey = "3????[q#";
			DestroyerSecurityKey = "-#{1njust1c1as}[??????]#{6D2}-!";
		}

		public string dataContainer()
		{
			return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}

		public static string MD5Hash(string input)
		{
			StringBuilder hash = new StringBuilder();
			MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
			byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

			for (int i = 0; i < bytes.Length; i++)
			{
				hash.Append(bytes[i].ToString("x2"));
			}
			return hash.ToString();
		}

		#region Encripta/Desencripta Texto
		public static string Encrypt(string plainText, string passPhrase)
		{
			// Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
			// so that the same Salt and IV values can be used when decrypting.  
			var saltStringBytes = Generate256BitsOfRandomEntropy();
			var ivStringBytes = Generate256BitsOfRandomEntropy();
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			try
			{

				using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
				{
					var keyBytes = password.GetBytes(Keysize / 8);
					using (var symmetricKey = new RijndaelManaged())
					{
						symmetricKey.BlockSize = 256;
						symmetricKey.Mode = CipherMode.CBC;
						symmetricKey.Padding = PaddingMode.PKCS7;
						using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
						{
							using (var memoryStream = new MemoryStream())
							{
								using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
								{
									cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
									cryptoStream.FlushFinalBlock();
									// Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
									var cipherTextBytes = saltStringBytes;
									cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
									cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
									memoryStream.Close();
									cryptoStream.Close();
									return Convert.ToBase64String(cipherTextBytes);
								}
							}
						}
					}
				}

			}
			catch (Exception ex) { }
			return null;
		}

		public static string Decrypt(string cipherText, string passPhrase, out bool ok)
		{
			string devolver = "";
			ok = false;
			try
			{
				// Get the complete stream of bytes that represent:
				// [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
				var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
				// Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
				var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
				// Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
				var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
				// Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
				var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

				using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
				{
					var keyBytes = password.GetBytes(Keysize / 8);
					using (var symmetricKey = new RijndaelManaged())
					{
						symmetricKey.BlockSize = 256;
						symmetricKey.Mode = CipherMode.CBC;
						symmetricKey.Padding = PaddingMode.PKCS7;
						using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
						{
							using (var memoryStream = new MemoryStream(cipherTextBytes))
							{
								using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
								{
									var plainTextBytes = new byte[cipherTextBytes.Length];
									var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
									memoryStream.Close();
									cryptoStream.Close();
									devolver = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
									ok = true;
								}
							}
						}
					}
				}

			}
			catch (Exception ex)
			{
				ok = false;
			}

			return devolver;


		}

		private static byte[] Generate256BitsOfRandomEntropy()
		{
			var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
			using (var rngCsp = new RNGCryptoServiceProvider())
			{
				// Fill the array with cryptographically secure random bytes.
				rngCsp.GetBytes(randomBytes);
			}
			return randomBytes;
		}
		#endregion

		#region Encripta/Desencripta Fichero
		//  Call this function to remove the key from memory after use for security
		[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
		public static extern bool ZeroMemory(IntPtr Destination, int Length);

		// Function to Generate a 64 bits Key.
		public static string GenerateKey()
		{
			// Create an instance of Symetric Algorithm. Key and IV is generated automatically.
			DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

			// Use the Automatically generated key for Encryption. 
			return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
		}

		public static void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
		{
			try
			{
				FileStream fsInput = new FileStream(sInputFilename,
			 FileMode.Open,
			 FileAccess.Read);

				FileStream fsEncrypted = new FileStream(sOutputFilename,
				   FileMode.Create,
				   FileAccess.Write);
				DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
				DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
				DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
				ICryptoTransform desencrypt = DES.CreateEncryptor();
				CryptoStream cryptostream = new CryptoStream(fsEncrypted,
				   desencrypt,
				   CryptoStreamMode.Write);

				byte[] bytearrayinput = new byte[fsInput.Length];
				fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
				cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
				cryptostream.Close();
				fsInput.Close();
				fsEncrypted.Close();
			}
			catch (Exception ex) { }

		}

		public static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
		{
			try
			{
				DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
				//A 64 bit key and IV is required for this provider.
				//Set secret key For DES algorithm.
				DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
				//Set initialization vector.
				DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

				//Create a file stream to read the encrypted file back.
				FileStream fsread = new FileStream(sInputFilename,
				   FileMode.Open,
				   FileAccess.Read);
				//Create a DES decryptor from the DES instance.
				ICryptoTransform desdecrypt = DES.CreateDecryptor();
				//Create crypto stream set to read and do a 
				//DES decryption transform on incoming bytes.
				CryptoStream cryptostreamDecr = new CryptoStream(fsread,
				   desdecrypt,
				   CryptoStreamMode.Read);
				//Print the contents of the decrypted file.
				StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
				fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
				fsDecrypted.Flush();
				fsDecrypted.Close();
			}
			catch (Exception ex) { }

		}
		#endregion


		#region Encriptar/Desencriptar Image File
		/// <summary>
		/// Encrypted files
		/// </summary>
		/// <param name="sInputFile">input file</param>
		/// <param name="sOutputFile">Encrypted file</param>
		/// <param name="sKey">Encrypted keys</param>
		/// <param name="sMsg">Encrypted result message</param>
		/// <returns>return true or false.</returns>
		public static bool EncryptFileIMG(string sInputFilename, string sOutputFilename, string sKey, out string sMsg)
		{
			bool IsSuccess = true;
			FileStream fsInput = null;
			FileStream fsEncrypted = null;
			CryptoStream cryptostream = null;
			try
			{
				fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
				fsEncrypted = new FileStream(sOutputFilename,
														FileMode.Create,
														FileAccess.Write);
				DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
				DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
				DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
				ICryptoTransform desencrypt = DES.CreateEncryptor();
				cryptostream = new CryptoStream(fsEncrypted,
															desencrypt,
															CryptoStreamMode.Write);
				byte[] bytearrayinput = new byte[fsInput.Length];
				fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
				cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
				cryptostream.Close();
				fsInput.Close();
				fsEncrypted.Close();
				sMsg = "Encrypted is done";
				return true;
			}
			catch (Exception ex)
			{
				sMsg = ex.Message;
				IsSuccess = true;
			}
			finally
			{
				try
				{
					if (cryptostream != null) cryptostream.Close();
					if (fsInput != null) fsInput.Close();
					if (fsEncrypted != null) fsEncrypted.Close();
				}
				catch
				{
				}
			}
			return IsSuccess;
		}
		/// <summary>
		/// Decrypt files
		/// </summary>
		/// <param name="sInputFile">input file</param>
		/// <param name="sOutputFile">Decrypt file</param>
		/// <param name="sKey">Decrypt keys</param>
		/// <param name="sMsg">Decrypt result message</param>
		/// <returns>return true or false.</returns>
		public static bool DecryptFileIMG(string sInputFilename, string sOutputFilename, string sKey, out string sMsg)
		{
			FileStream fsread = null;
			StreamWriter fsDecrypted = null;
			bool isSuccess = true;
			try
			{
				DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
				//A 64 bit key and IV is required for this provider.
				//Set secret key For DES algorithm.
				DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
				//Set initialization vector.
				DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
				//Create a file stream to read the encrypted file back.
				fsread = new FileStream(sInputFilename,
											   FileMode.Open,
											   FileAccess.Read);
				//Create a DES decryptor from the DES instance.
				ICryptoTransform desdecrypt = DES.CreateDecryptor();
				//Create crypto stream set to read and do a
				//DES decryption transform on incoming bytes.
				CryptoStream cryptostreamDecr = new CryptoStream(fsread,
															 desdecrypt,
															 CryptoStreamMode.Read);
				//Print out the contents of the decrypted file.
				fsDecrypted = new StreamWriter(sOutputFilename);
				fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
				fsDecrypted.Flush();
				fsDecrypted.Close();
				sMsg = "Decrypted is done";
			}
			catch (Exception ex)
			{
				sMsg = ex.Message;
				isSuccess = false;
			}
			finally
			{
				try
				{
					if (fsread != null) fsread.Close();
					if (fsDecrypted != null) fsDecrypted.Close();
				}
				catch
				{
				}
			}
			return isSuccess;
		}
		#endregion


		#region PasswordAleatoria

		public static IEnumerable<string> RandomStrings(
	string allowedChars,
	int minLength,
	int maxLength,
	int count,
	Random rng)
		{
			char[] chars = new char[maxLength];
			int setLength = allowedChars.Length;

			while (count-- > 0)
			{
				int length = rng.Next(minLength, maxLength + 1);

				for (int i = 0; i < length; ++i)
				{
					chars[i] = allowedChars[rng.Next(setLength)];
				}

				yield return new string(chars, 0, length);
			}
		}





		public static string Base62Random()
		{
			Random _random = new Random();

			int random = _random.Next();


			return Base62ToString(random);
		}

		private static string Base62ToString(long value)
		{
			// Divides the number by 64, so how many 64s are in
			// 'value'. This number is stored in Y.
			// e.g #1
			// 1) 1000 / 62 = 16, plus 8 remainder (stored in x).
			// 2) 16 / 62 = 0, remainder 16
			// 3) 16, 8 or G8:
			// 4) 65 is A, add 6 to this = 71 or G.
			//
			// e.g #2:
			// 1) 10000 / 62 = 161, remainder 18
			// 2) 161 / 62 = 2, remainder 37
			// 3) 2 / 62 = 0, remainder 2
			// 4) 2, 37, 18, or 2,b,I:
			// 5) 65 is A, add 27 to this (minus 10 from 37 as these are digits) = 92.
			//    Add 6 to 92, as 91-96 are symbols. 98 is b.
			// 6)
			long x = 0;
			long y = Math.DivRem(value, 62, out x);
			if (y > 0)
				return Base62ToString(y) + ValToChar(x).ToString();
			else
				return ValToChar(x).ToString();
		}

		private static char ValToChar(long value)
		{
			if (value > 9)
			{
				int ascii = (65 + ((int)value - 10));
				if (ascii > 90)
					ascii += 6;
				return (char)ascii;
			}
			else
				return value.ToString()[0];
		}
		#endregion

	}
}
