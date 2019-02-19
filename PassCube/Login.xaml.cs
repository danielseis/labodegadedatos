using PassCube.BusinessLayer;
using PassCube.Entitites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Xml;

namespace PassCube
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

		//configuracion de fichero Login
		//string MiLogin = @"c:\data.xml";
		//string MiLoginDe = @"c:\datade.xml";

		string AppPath = Directory.GetCurrentDirectory();

		private SegurityBL _help = new SegurityBL();
		private Settings _settings = new Settings();

		private User _usr = new User();

		// Must be 64 bits, 8 bytes.
		// Distribute this key to the user who will decrypt this file.
		string sSecretKey;

		bool correctDecript;


		public Login()
        {
			InitializeComponent();
			// ImgShowHide.Source = new BitmapImage(new Uri(AppPath + "\\img\\clip.jpg"));
			// ImgShowHide.Kind = "clippy";

			//MiLogin = _help.dataContainer() + "labodegalg.bdg";
			//Settings.directory = _help.dataContainer() + "labodegalg.bdg";
		}

		private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			HidePassword();
		}

		private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			ShowPassword();
		}
		private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
		{
			HidePassword();
		}
		private void txtPasswordbox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (txtPasswordbox.Password.Length > 0)
				ImgShowHide.Visibility = Visibility.Visible;
			else
				ImgShowHide.Visibility = Visibility.Hidden;
		}

		void ShowPassword()
		{
			// ImgShowHide.Source = new BitmapImage(new Uri(AppPath + "\\img\\cus.jpg"));
			txtVisiblePasswordbox.Visibility = Visibility.Visible;
			txtPasswordbox.Visibility = Visibility.Hidden;
			txtVisiblePasswordbox.Text = txtPasswordbox.Password;
		}
		void HidePassword()
		{
			// ImgShowHide.Source = new BitmapImage(new Uri(AppPath + "\\img\\clip.jpg"));
			txtVisiblePasswordbox.Visibility = Visibility.Hidden;
			txtPasswordbox.Visibility = Visibility.Visible;
			txtPasswordbox.Focus();
		}

		private void txtPasswordbox_KeyDown(object sender, KeyEventArgs e)
		{
			#region Metodologia
			// SI es la primera vez 
			// si no existe el fichero.

			// Encriptar
			// creamos el fichero con el password creado.


			// Si no es la primera vez
			// accedemos al fichero y consultamos
			// Desencriptar
			#endregion





			if (e.Key == Key.Return)
			{
				bool isLgIn = false;
				bool isOk = false;

				bool isBlocked = false;

				//string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				//string filePath = Path.Combine(directory, "SomeFile.exe");

				// Helper.SecurityKey = txtPasswordbox.Password;

				// Get the Key for the file to Encrypt.
				//sSecretKey = Helper.GenerateKey();
				sSecretKey = SegurityBL.SecurityKey;





				if (comprobar())
				{

					if (!File.Exists(Settings.MiLoginDebk))
					{

						if (!File.Exists(Settings.MiLoginDe))
						{
							// string pass = Helper.MD5Hash(txtPasswordbox.Password);
							User.Pass = txtPasswordbox.Password;
							string encryptedstring = SegurityBL.Encrypt(txtPasswordbox.Password, txtPasswordbox.Password);

							// For additional security Pin the key.
							GCHandle gch = GCHandle.Alloc(txtPasswordbox.Password, GCHandleType.Pinned);

							string data = string.Format("<item><pass>{0}</pass></item>", encryptedstring);


							XmlDocument doc = new XmlDocument();
							doc.LoadXml(data);


							doc.PreserveWhitespace = true;
							doc.Save(Settings.MiLogin);


							//file encript
							// Encrypt the file.        
							SegurityBL.EncryptFile(Settings.MiLogin,
							   Settings.MiLoginDe,
							   sSecretKey);

							using (var md5 = MD5.Create())
							{
								using (var stream = File.OpenRead(Settings.MiLoginDe))
								{
									Settings.md5Login = md5.ComputeHash(stream);
								}
							}/*
                    Controller.Settings.directory
                    Controller.Settings.version
                    Controller.Settings.innstall
                    Controller.Settings.lastacces
                        */
							 // todo: generate file data default. And Encript


							File.Delete(Settings.MiLogin);


							isLgIn = true;
						}
						else
						{
							#region Metodologia
							/* Vamos a recoger el fichero de datos 
                               Decryptaremos el fichero de datos
                               Leeremos el md5
                               Compararemos el md5 con el del login*/
							#endregion
							//bool Ok = false;
							//using (var md5 = MD5.Create())
							//{
							//    using (var stream = File.OpenRead(Settings.MiLoginDe))
							//    {
							//        if (Controller.Settings.md5Login == md5.ComputeHash(stream))
							//            Ok = true;
							//    }
							//}


							// todo: if Ok...


							// For additional security Pin the key.
							GCHandle gch = GCHandle.Alloc(SegurityBL.SecurityKey, GCHandleType.Pinned);

							// Decrypt the file.
							SegurityBL.DecryptFile(Settings.MiLoginDe,
							   Settings.MiLogin,
							   sSecretKey);


							XmlDocument xmlDoc = new XmlDocument();
							xmlDoc.Load(Settings.MiLogin);


							XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/item");
							string proPass = "";
							foreach (XmlNode node in nodeList)
							{
								proPass = node.SelectSingleNode("pass").InnerText;
							}

							string decryptedstring = SegurityBL.Decrypt(proPass, txtPasswordbox.Password, out correctDecript);

							if (correctDecript)
							{
								if (decryptedstring == txtPasswordbox.Password)
								{
									User.Pass = txtPasswordbox.Password;
									isLgIn = true;
								}
							}
						}



						//// Remove the Key from memory. 
						//Helper.ZeroMemory(gch.AddrOfPinnedObject(), sSecretKey.Length * 2);
						//gch.Free();
						if (File.Exists(Settings.MiLogin))
						{
							File.Delete(Settings.MiLogin);
						}

						if (isLgIn)
						{
							//Door win2 = new Door();
							//win2.Show();

							MainPassCube  win3 = new MainPassCube();
							win3.Show();
							this.Close();

							//ShowCards win4 = new ShowCards();
							//win4.Show();
							//this.Close();
						}
						else
						{
							if (string.IsNullOrEmpty(lblCursorPosition.Text))
							{
								lblCursorPosition.Text = (0).ToString();
							}
							lblCursorPosition.Text = (Convert.ToInt32(lblCursorPosition.Text) + 1).ToString();

							if (Convert.ToInt32(lblCursorPosition.Text) > 5)
							{
								// Encriptamos para romper.
								SegurityBL.EncryptFile(Settings.MiLoginDe, Settings.MiLoginDebk, SegurityBL.DestroyerSecurityKey);
								SegurityBL.EncryptFile(Settings.myData, Settings.myDataDeBK, SegurityBL.DestroyerSecurityKey);
								//todo: mensaje contacte con el creador para recuperar el fichero. Demasiados intentos erroneos.
								// por seguridad de fichero robado, se sobre encriptado y a quedado inaccesible.

								MessageBox.Show("Fichero Bloqueado contacta con el creador.", "LaBodega Informa!", MessageBoxButton.OK, MessageBoxImage.Information);

							}
						}

					}
					else
					{
						MessageBox.Show("Fichero Bloqueado contacta con el creador.", "LaBodega Informa!", MessageBoxButton.OK, MessageBoxImage.Information);


					}
				}
				else
				{
					//todo: show errror Comprobar.

				}



			}
		}

		private bool comprobar()
		{
			bool isOk = false;

			if (!string.IsNullOrWhiteSpace(txtPasswordbox.Password))
			{
				if (txtPasswordbox.Password.Length >= 4)
				{
					isOk = true;
				}
			}


			return isOk;
		}
	}
}
