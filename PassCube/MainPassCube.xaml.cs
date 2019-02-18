using PassCube.BusinessLayer;
using PassCube.Entitites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace PassCube
{
    /// <summary>
    /// Interaction logic for MainPassCube.xaml
    /// </summary>
    public partial class MainPassCube : Window
    {
        
		private int _errors = 0;
		private PassBL _datos = new PassBL();

		System.Windows.Forms.NotifyIcon iconoNotificacion;

		//string myData = @"c:\myBodega.xml";
		//string myDataDe = @"c:\myBodegaDe.xml";

		bool correctDecript;

		bool firstOrder;

		private SegurityBL _help = new SegurityBL();
		private Settings _settings = new Settings();


		public MainPassCube()
		{
			InitializeComponent();
			DataContext = _datos;


			iconoNotificacion = new System.Windows.Forms.NotifyIcon();
			iconoNotificacion.BalloonTipText = "La Aplicación se encuentra ejecutando";
			iconoNotificacion.BalloonTipTitle = "LaBodega Notificación";
			iconoNotificacion.Text = "Presione Click para Mostrar";
			iconoNotificacion.Icon = new System.Drawing.Icon("kei.ico");
			iconoNotificacion.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			//iconoNotificacion.Click += new EventHandler(iconoNotificacion_Click);
			System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
			System.Windows.Forms.MenuItem item1 = new System.Windows.Forms.MenuItem("Mostrar");
			item1.Click += new EventHandler(item1_Click);
			System.Windows.Forms.MenuItem item2 = new System.Windows.Forms.MenuItem("Info");
			item2.Click += new EventHandler(item2_Click);
			System.Windows.Forms.MenuItem item3 = new System.Windows.Forms.MenuItem("Cerrar");
			item3.Click += new EventHandler(item3_Click);
			menu.MenuItems.Add(item1);
			menu.MenuItems.Add(item2);
			menu.MenuItems.Add(item3);
			iconoNotificacion.ContextMenu = menu;


			firstOrder = false;



			if (!File.Exists(Settings.myData))
			{
				// string pass = Helper.MD5Hash(txtPasswordbox.Password);

				string encryptedstring = SegurityBL.Encrypt("Ipsum", User.Pass);

				string encryptedPastore = SegurityBL.Encrypt("Ipsum", SegurityBL.DestroyerSecurityKey);

				string data = string.Format("<labodega><item><id>1</id><grupo>Lorem Ipsum</grupo><lugar>Lorem Ipsum</lugar><user>Lorem</user><pass>{0}</pass><date></date><passtore>{1}</passtore></item></labodega>", encryptedstring, encryptedPastore);

				string decryptedstring = SegurityBL.Decrypt(encryptedstring, User.Pass, out correctDecript);

				XmlDocument doc = new XmlDocument();
				doc.LoadXml(data);


				doc.PreserveWhitespace = true;
				doc.Save(Settings.myData);

				BindGrid();

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
				//    using (var stream = File.OpenRead(myDataDe))
				//    {
				//        if (Controller.Settings.md5Login == md5.ComputeHash(stream))
				//            Ok = true;
				//    }
				//}


				// todo: if Ok...

				// Decrypt the file.
				//Helper.DecryptFile(myDataDe,
				//   myData,
				//   Helper.SecurityKey);


				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(Settings.myData);


				BindGrid();

				//Helper.EncryptFile(myData, myDataDe, Helper.SecurityKey);
				//File.Delete(myData);

			}

		}

		private void Validation_Error(object sender, ValidationErrorEventArgs e)
		{
			if (e.Action == ValidationErrorEventAction.Added)
				_errors++;
			else
				_errors--;
		}


		#region Minimizar aplicacion

		void item3_Click(object sender, EventArgs e)
		{
			Close();
		}

		void item2_Click(object sender, EventArgs e)
		{
			MessageBox.Show("LaBodega version 1.0 Seguridad para tus datos.");
		}

		void item1_Click(object sender, EventArgs e)
		{
			Show();
			WindowState = System.Windows.WindowState.Normal;
		}

		void iconoNotificacion_Click(object sender, EventArgs e)
		{
			Show();
			WindowState = System.Windows.WindowState.Normal;
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
			{
				Hide();
				if (iconoNotificacion != null)
					iconoNotificacion.ShowBalloonTip(2000);
			}
		}

		private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			VerificarIcono();
		}

		void VerificarIcono()
		{
			MostrarIcono(!IsVisible);
		}

		void MostrarIcono(bool mostrar)
		{
			if (iconoNotificacion != null)
				iconoNotificacion.Visible = mostrar;
		}
		#endregion

		private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			// doble click Edit (recoger datos y volcarlos en los txt)
		}

		private void btnCharge_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//XmlReader xmlFile;
				//xmlFile = XmlReader.Create(myData, new XmlReaderSettings());
				//DataSet ds = new DataSet();
				//ds.ReadXml(xmlFile);
				//dataGrid.DataContext = ds.Tables[0];

				//XElement mymusic = XElement.Load(myData);
				//dataGrid.DataContext = mymusic.Elements("grupos");

				string sampleXmlFile = Settings.myData;
				DataSet dataSet = new DataSet();
				dataSet.ReadXml(sampleXmlFile);
				DataView dataView = new DataView(dataSet.Tables[0]);
				dataGrid1.ItemsSource = dataView;
				// dataGrid1.ItemsSource = dataView;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}



		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			if (_errors == 0)
			{
				if (Controller.Grupos.isEdit == false)
				{

					//Helper.DecryptFile(myDataDe,
					//   myData,
					//   Helper.SecurityKey);

					XmlDocument xmlEmloyeeDoc = new XmlDocument();
					xmlEmloyeeDoc.Load(Settings.myData);

					XmlElement ParentElement = xmlEmloyeeDoc.CreateElement("item");
					XDocument xDoc = XDocument.Load(Settings.myData);
					int maxNr = 0;
					try
					{
						maxNr = xDoc.Root.Elements().Max(x => (int)x.Element("id"));
					}
					catch (Exception ex)
					{

					}



					XmlElement ID = xmlEmloyeeDoc.CreateElement("id");
					ID.InnerText = (maxNr + 1).ToString();
					XmlElement Grupo = xmlEmloyeeDoc.CreateElement("grupo");
					Grupo.InnerText = txtGrupo.Text;
					XmlElement Lugar = xmlEmloyeeDoc.CreateElement("lugar");
					Lugar.InnerText = txtUrl.Text;
					XmlElement User = xmlEmloyeeDoc.CreateElement("user");
					User.InnerText = txt_Usuario.Text;

					string encryptedstring = Helper.Encrypt(txtPassword.Password, Controller.Usuario.Pass);
					XmlElement Pass = xmlEmloyeeDoc.CreateElement("pass");
					Pass.InnerText = encryptedstring;

					string encryptedPastore = Helper.Encrypt(txtPassword.Password, Controller.Helper.DestroyerSecurityKey);
					XmlElement PassTore = xmlEmloyeeDoc.CreateElement("passtore");
					PassTore.InnerText = encryptedPastore;

					XmlElement Date = xmlEmloyeeDoc.CreateElement("date");
					Date.InnerText = DateTime.Now.ToString();



					// el correcto orden de los campos es necesario.
					ParentElement.AppendChild(ID);
					ParentElement.AppendChild(Grupo);
					ParentElement.AppendChild(Lugar);
					ParentElement.AppendChild(User);
					ParentElement.AppendChild(Pass);
					ParentElement.AppendChild(Date);
					ParentElement.AppendChild(PassTore);

					xmlEmloyeeDoc.DocumentElement.AppendChild(ParentElement);
					xmlEmloyeeDoc.Save(Settings.myData);
					BindGrid();

					LimpiaForm();

					//Helper.EncryptFile(myData, myDataDe, Helper.SecurityKey);
					//File.Delete(myData);
				}
				else
				{
					//Helper.DecryptFile(myDataDe,
					//    myData,
					//    Helper.SecurityKey);

					Controller.Grupos.isEdit = false;

					DataSet ds = new DataSet();
					ds.ReadXml(Settings.myData);

					int xmlRow = Controller.Grupos.xmlRow;
					int idval = Controller.Grupos.id;

					ds.Tables[0].Rows[xmlRow]["id"] = idval;
					ds.Tables[0].Rows[xmlRow]["grupo"] = txtGrupo.Text;
					ds.Tables[0].Rows[xmlRow]["lugar"] = txtUrl.Text;
					ds.Tables[0].Rows[xmlRow]["user"] = txt_Usuario.Text;

					string encryptedstring = Helper.Encrypt(txtPassword.Password, Controller.Usuario.Pass);
					ds.Tables[0].Rows[xmlRow]["pass"] = encryptedstring;


					string encryptedPastore = Helper.Encrypt(txtPassword.Password, Controller.Helper.DestroyerSecurityKey);
					ds.Tables[0].Rows[xmlRow]["passtore"] = encryptedPastore;


					ds.WriteXml(Settings.myData);
					BindGrid();

					LimpiaForm();
					//Helper.EncryptFile(myData, myDataDe, Helper.SecurityKey);
					//File.Delete(myData);
				}
			}
		}

		private void LimpiaForm()
		{
			txtGrupo.Text = "";
			txtPassword.Password = "";
			txtUrl.Text = "";
			txt_Usuario.Text = "";
			txtVisiblePasswordbox.Text = "";
		}

		private void BindGrid()
		{
			#region Metodologia
			/*
             -Si no existe el fichero lo creamos. y encriptamos
             -si existe desencriptamos y encriptamos
             -toda pass va encriptada.

             */
			#endregion
			try
			{
				string sampleXmlFile = Settings.myData;
				DataSet dataSet = new DataSet();
				dataSet.ReadXml(sampleXmlFile);
				DataView dataView = new DataView(dataSet.Tables[0]);
				dataGrid1.ItemsSource = dataView;
			}
			catch (Exception ex)
			{
				// vacio.

				dataGrid1.ItemsSource = "";
			}

		}

		private void Edit_click(object sender, RoutedEventArgs e)
		{
			//Helper.DecryptFile(myDataDe,
			//        myData,
			//        Helper.SecurityKey);

			Controller.Grupos.isEdit = true;

			DataSet ds = new DataSet();
			ds.ReadXml(Settings.myData);


			if (dataGrid1.SelectedItem != null)
			{
				DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

				int id = dataGrid1.Items.IndexOf(dataGrid1.CurrentItem);
				Controller.Grupos.xmlRow = id;

				Controller.Grupos.id = Convert.ToInt32(row["id"]);
				txtGrupo.Text = row["grupo"].ToString();
				txtUrl.Text = row["lugar"].ToString();
				txt_Usuario.Text = row["user"].ToString();


				string decryptedstring = Helper.Decrypt(row["pass"].ToString(), Controller.Usuario.Pass, out correctDecript);


				txtPassword.Password = decryptedstring;
			}

			//File.Delete(myData);

		}

		private void delete_click(object sender, RoutedEventArgs e)
		{



			if (dataGrid1.SelectedItem != null)
			{



				DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

				int id = dataGrid1.Items.IndexOf(dataGrid1.CurrentItem);
				//int id = Convert.ToInt32(row["id"]);


				if (MessageBox.Show("Delete?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					//Helper.DecryptFile(myDataDe,
					//myData,
					//Helper.SecurityKey);


					DataSet ds = new DataSet();
					ds.ReadXml(Settings.myData);
					ds.Tables[0].Rows.RemoveAt(id);
					ds.WriteXml(Settings.myData);
					BindGrid();

					//Helper.EncryptFile(myData, myDataDe, Helper.SecurityKey);

					//File.Delete(myData);

					MessageBox.Show("Eliminacion realizada correctamente.", "LaBodega Informa!", MessageBoxButton.OK, MessageBoxImage.Information);

				}
				else
				{
					MessageBox.Show("Cancelado.", "LaBodega Informa!", MessageBoxButton.OK, MessageBoxImage.Information);
				}



			}



		}

		private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			var currentRowIndex = dataGrid1.Items.IndexOf(dataGrid1.CurrentItem);

		}

		private void btnShow_Click(object sender, RoutedEventArgs e)
		{


		}

		#region Show/Hide Password
		public DataGridCell GetCell(int row, int column)
		{
			DataGridRow rowContainer = GetRow(row);
			if (rowContainer != null)
			{
				DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
				if (presenter == null)
				{
					dataGrid1.ScrollIntoView(rowContainer, dataGrid1.Columns[column]);
					presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
				}
				DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
				return cell;
			}
			return null;
		}

		public DataGridRow GetRow(int index)
		{
			DataGridRow row = (DataGridRow)dataGrid1.ItemContainerGenerator.ContainerFromIndex(index);
			if (row == null)
			{
				dataGrid1.UpdateLayout();
				dataGrid1.ScrollIntoView(dataGrid1.Items[index]);
				row = (DataGridRow)dataGrid1.ItemContainerGenerator.ContainerFromIndex(index);
			}
			return row;
		}

		public static T GetVisualChild<T>(Visual parent) where T : Visual
		{
			T child = default(T);
			int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < numVisuals; i++)
			{
				Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
				child = v as T;
				if (child == null)
				{
					child = GetVisualChild<T>
					(v);
				}
				if (child != null)
				{
					break;
				}
			}
			return child;
		}


		/*
          <!--<DataGridTemplateColumn Header="Pass" Width="120" >
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBox x:Name="pass" Text="{Binding pass, Mode=OneWay}" Visibility="Hidden" Width="90" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>-->
             */
		private void btnShow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			#region MouseDown DataTemplate
			//if (dataGrid1.SelectedItem != null)
			//{
			//    // DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

			//    int id = dataGrid1.Items.IndexOf(dataGrid1.CurrentItem);
			//    //string pas = row["pass"].ToString();

			//    //Find the DataRowView for DataGrid.
			//    DataRowView Grdrow = ((FrameworkElement)sender).DataContext as DataRowView;

			//    //Fidn the DataGrid row index
			//    int rowIndex = dataGrid1.Items.IndexOf(Grdrow);

			//    //Find the DataGridCell
			//    DataGridCell cell = GetCell(rowIndex, 4); //Pass the row and column

			//    //Find the "lblVehicleName" lable.
			//    TextBox txtpasas = GetVisualChild<TextBox>(cell); // pass the DataGridCell as a parameter to GetVisualChild

			//    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
			//    int idd = dataGrid1.Items.IndexOf(dataGrid1.CurrentItem);
			//    Controller.Grupos.xmlRow = idd;
			//    Controller.Grupos.Password = row["pass"].ToString();

			//    string decryptedstring="";

			//    if (Controller.Grupos.Password== txtpasas.Text)
			//        decryptedstring = Helper.Decrypt(  txtpasas.Text, Controller.Usuario.Pass, out correctDecript);

			//    txtpasas.Text = decryptedstring;
			//    txtpasas.Visibility = Visibility.Visible;

			//    firstOrder = true;
			//}
			#endregion

			if (dataGrid1.SelectedItem != null)
			{
				DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
				string decryptedstring = Helper.Decrypt(row["pass"].ToString(), Controller.Usuario.Pass, out correctDecript);
				row["pass"] = decryptedstring;
			}


		}

		private void btnShow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			#region MouseDown DataTemplate
			//if (dataGrid1.SelectedItem != null && firstOrder)
			//{
			//    // DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

			//    int id = dataGrid1.Items.IndexOf(dataGrid1.CurrentItem);
			//    //string pas = row["pass"].ToString();

			//    //Find the DataRowView for DataGrid.
			//    DataRowView Grdrow = ((FrameworkElement)sender).DataContext as DataRowView;

			//    //Fidn the DataGrid row index
			//    int rowIndex = dataGrid1.Items.IndexOf(Grdrow);

			//    //Find the DataGridCell
			//    DataGridCell cell = GetCell(rowIndex, 4); //Pass the row and column

			//    //Find the "lblVehicleName" lable.
			//    TextBox txtpasas = GetVisualChild<TextBox>(cell); // pass the DataGridCell as a parameter to GetVisualChild


			//    txtpasas.Visibility = Visibility.Hidden;

			//    firstOrder = false;

			//    BindGrid();
			//}
			#endregion


			//DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
			//string decryptedstring = Helper.Encrypt(row["pass"].ToString(), Controller.Usuario.Pass);
			//row["pass"] = decryptedstring;

			BindGrid();

		}



		#endregion

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			LimpiaForm();
			Controller.Grupos.isEdit = false;
		}

		private void btnCopy(object sender, MouseButtonEventArgs e)
		{
			try
			{
				CopiarClipboardPassDesencipt();
			}
			catch (Exception)
			{
				var mensaje = "elemento clipboard ocupado.";
				throw;
			}



		}

		private void CopiarClipboardPassDesencipt()
		{

			if (dataGrid1.SelectedItem != null)
			{
				DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

				int id = dataGrid1.Items.IndexOf(dataGrid1.CurrentItem);
				Controller.Grupos.xmlRow = id;


				string decryptedstring = Helper.Decrypt(row["pass"].ToString(), Controller.Usuario.Pass, out correctDecript);
				try
				{
					Clipboard.Clear();
					Clipboard.SetText(decryptedstring);
				}
				catch (Exception ex)
				{
					Clipboard.SetDataObject(decryptedstring);
				}

			}
		}

		private void btnRnd_Click(object sender, RoutedEventArgs e)
		{

			Random rng = new Random();
			var valor = "";
			int rompe = rng.Next(1, 25);
			int i = 0;
			foreach (var randomString in Helper.RandomStrings(Helper.AllowedChars, 8, 10, 25, rng))
			{
				if (rompe == i)
				{
					valor = randomString;
					// break;
				}
				//Console.WriteLine(randomString);
				i++;
			}
			txtRndKey.Text = valor;
			//Console.ReadLine();

			//txtRndKey.Text = Helper.Base62Random();
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{

			//viewbtn1.Visibility = Visibility.Hidden;
			//viewbtn2.Visibility = Visibility.Visible;

			ShowLastSearch();


			//dataGrid1.ItemsSource = mydata;



		}

		public void ShowLastSearch()
		{

			if (!string.IsNullOrWhiteSpace(txtSearch.Text))
			{
				XDocument xmlFile = XDocument.Load(Settings.myData);

				//var mydata = from item in xmlFile.Root.Elements("item")
				//             where item.Element("grupo").Value.Contains(txtSearch.Text) || item.Element("lugar").Value.Contains(txtSearch.Text) || item.Element("user").Value.Contains(txtSearch.Text)
				//             select new
				//             {
				//                 id = item.Element("id").Value,
				//                 grupo = item.Element("grupo").Value,
				//                 lugar = item.Element("lugar").Value,
				//                 user = item.Element("user").Value,
				//                 pass = item.Element("pass").Value,
				//                 date = item.Element("date").Value
				//             };

				XElement xml = new XElement("labodega",
				from dynamic item in xmlFile.Root.Elements("item")
				where item.Element("grupo").Value.Contains(txtSearch.Text) || item.Element("lugar").Value.Contains(txtSearch.Text) || item.Element("user").Value.Contains(txtSearch.Text)
				select new XElement("item",
					new XElement("id", item.Element("id").Value),
					 new XElement("grupo", item.Element("grupo").Value),
					  new XElement("lugar", item.Element("lugar").Value),
					   new XElement("user", item.Element("user").Value),
						new XElement("pass", item.Element("pass").Value),
						 new XElement("date", item.Element("date").Value)
				));

				try
				{
					StringReader theReader = new StringReader(xml.ToString().Trim().Replace("\r\n", string.Empty));
					DataSet theDataSet = new DataSet();
					theDataSet.ReadXml(theReader);
					DataView dataView = new DataView(theDataSet.Tables[0]);
					dataGrid1.ItemsSource = dataView;
				}
				catch (Exception ex) // error si no encuentra nada.
				{
					dataGrid1.ItemsSource = "";
				}

			}
			else
			{
				BindGrid();
			}

		}
		private void btnclean_Click(object sender, RoutedEventArgs e)
		{
			//viewbtn1.Visibility = Visibility.Visible;
			//viewbtn2.Visibility = Visibility.Hidden;

			txtSearch.Text = "";

			BindGrid();
		}

		private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void btn_decrpt_Click(object sender, RoutedEventArgs e)
		{

			if (dataGrid1.SelectedItem != null)
			{
				DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

				int id = dataGrid1.Items.IndexOf(dataGrid1.CurrentItem);
				Controller.Grupos.xmlRow = id;


				string decryptedstring = Helper.Decrypt(row["pass"].ToString(), Controller.Usuario.Pass, out correctDecript);


				txtPassword.Password = decryptedstring;
			}
		}

		private void btnShow2_Click(object sender, RoutedEventArgs e)
		{
			// show dialog box to inform de pass copied to clipboard
			// or show into show box

			//if (dataGrid1.SelectedItem != null)
			//{
			//    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

			//    string pass = "";
			//    pass = row["pass"].ToString();

			//    string decryptedstring = Helper.Decrypt(pass, Controller.Usuario.Pass, out correctDecript);
			//    row["pass"] = decryptedstring;
			//    //MessageBox.Show(decryptedstring);

			//}

			if (dataGrid1.SelectedItem != null)
			{
				DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
				string decryptedstring = Helper.Decrypt(row["pass"].ToString(), Controller.Usuario.Pass, out correctDecript);
				row["pass"] = decryptedstring;
			}

		}

		private void btnShow2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (dataGrid1.SelectedItem != null)
			{
				DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
				string decryptedstring = Helper.Decrypt(row["pass"].ToString(), Controller.Usuario.Pass, out correctDecript);
				row["pass"] = decryptedstring;
			}
		}

		private void btnShow2_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			ShowLastSearch();
		}

		private void btnGo_Click(object sender, RoutedEventArgs e)
		{
			string programFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
			string programFilesX86 = Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%");

			if (dataGrid1.SelectedItem != null)
			{
				DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

				//todo: determinar si se le puede pasar http://
				string InternetVar = row["lugar"].ToString();
				if (!row["lugar"].ToString().Contains("http"))
				{
					InternetVar = "http://" + row["lugar"].ToString();
				}


				Uri uriResult;
				bool result = Uri.TryCreate(InternetVar, UriKind.Absolute, out uriResult)
					&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

				if (result)
				{
					if (Directory.Exists(programFiles + @"\Google\Chrome\Application\"))
					{
						Process Chrome = new Process(); //Create the process
						Chrome.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";  // Needs to be full path
						Chrome.StartInfo.Arguments = row["lugar"].ToString(); // If you have any arguments
						Chrome.Start();
					}
					else if (Directory.Exists(programFilesX86 + @"\Google\Chrome\Application\"))
					{
						Process Chrome = new Process(); //Create the process
						Chrome.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";  // Needs to be full path
						Chrome.StartInfo.Arguments = row["lugar"].ToString(); // If you have any arguments
						Chrome.Start();
					}
					else
					{
						MessageBox.Show("No encuentro el Chrome.", "LaBodega Informa!", MessageBoxButton.OK, MessageBoxImage.Information);
					}


				}
				else
				{
					MessageBox.Show("No es una url valida.", "LaBodega Informa!", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}


		}


		#region VisiblePassEditing
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
			if (txtPassword.Password.Length > 0)
				ImgShowHide.Visibility = Visibility.Visible;
			else
				ImgShowHide.Visibility = Visibility.Hidden;
		}

		void ShowPassword()
		{
			// ImgShowHide.Source = new BitmapImage(new Uri(AppPath + "\\img\\cus.jpg"));
			txtVisiblePasswordbox.Visibility = Visibility.Visible;
			txtPassword.Visibility = Visibility.Hidden;
			txtVisiblePasswordbox.Text = txtPassword.Password;
		}
		void HidePassword()
		{
			// ImgShowHide.Source = new BitmapImage(new Uri(AppPath + "\\img\\clip.jpg"));
			txtVisiblePasswordbox.Visibility = Visibility.Hidden;
			txtPassword.Visibility = Visibility.Visible;
			txtPassword.Focus();
		}

		#endregion

		private void btnShow1_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnCopiar_Click(object sender, RoutedEventArgs e)
		{
			CopiarClipboardPassDesencipt();

		}

		private void txtSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				//viewbtn1.Visibility = Visibility.Hidden;
				//viewbtn2.Visibility = Visibility.Visible;

				ShowLastSearch();
			}
		}

		private void Duplicar_click(object sender, RoutedEventArgs e)
		{


			if (dataGrid1.SelectedItem != null)
			{
				// read *****************************************************
				DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

				int id = dataGrid1.Items.IndexOf(dataGrid1.CurrentItem);
				Controller.Grupos.xmlRow = id;

				Controller.Grupos.id = Convert.ToInt32(row["id"]);
				Controller.Grupos.nameGroup = row["grupo"].ToString();
				Controller.Grupos.nameUrl = row["lugar"].ToString();
				Controller.Grupos.Usuario = row["user"].ToString();
				Controller.Grupos.Password = "";

				//Write *****************************************************
				XmlDocument xmlEmloyeeDoc = new XmlDocument();
				xmlEmloyeeDoc.Load(Settings.myData);

				XmlElement ParentElement = xmlEmloyeeDoc.CreateElement("item");
				XDocument xDoc = XDocument.Load(Settings.myData);
				int maxNr = 0;
				try
				{
					maxNr = xDoc.Root.Elements().Max(x => (int)x.Element("id"));
				}
				catch (Exception ex)
				{

				}

				XmlElement ID = xmlEmloyeeDoc.CreateElement("id");
				ID.InnerText = (maxNr + 1).ToString();
				XmlElement Grupo = xmlEmloyeeDoc.CreateElement("grupo");
				Grupo.InnerText = Controller.Grupos.nameGroup;
				XmlElement Lugar = xmlEmloyeeDoc.CreateElement("lugar");
				Lugar.InnerText = Controller.Grupos.nameUrl;
				XmlElement User = xmlEmloyeeDoc.CreateElement("user");
				User.InnerText = Controller.Grupos.Usuario;

				//string encryptedstring = Helper.Encrypt(txtPassword.Password, Controller.Usuario.Pass);
				XmlElement Pass = xmlEmloyeeDoc.CreateElement("pass");
				Pass.InnerText = "";

				//string encryptedPastore = Helper.Encrypt(txtPassword.Password, Controller.Helper.DestroyerSecurityKey);
				XmlElement PassTore = xmlEmloyeeDoc.CreateElement("passtore");
				PassTore.InnerText = "";

				XmlElement Date = xmlEmloyeeDoc.CreateElement("date");
				Date.InnerText = DateTime.Now.ToString();

				// el correcto orden de los campos es necesario.
				ParentElement.AppendChild(ID);
				ParentElement.AppendChild(Grupo);
				ParentElement.AppendChild(Lugar);
				ParentElement.AppendChild(User);
				ParentElement.AppendChild(Pass);
				ParentElement.AppendChild(Date);
				ParentElement.AppendChild(PassTore);

				xmlEmloyeeDoc.DocumentElement.AppendChild(ParentElement);
				xmlEmloyeeDoc.Save(Settings.myData);
				BindGrid();


			}






		}

		private void btnDonacion_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://paypal.me/BodegaDeDatos");
		}
	}


}

/*  Todo: Ampliar campos con - descripcion texto normal -nº de serie encriptado.
   
    Todo: Modificar fichero de datos y en la primera parte incluir datos de configuracion y clave maestra.

    Elflujo:
    yabe maestra usuario guardada en fichero 2 datos
    yabe de enctiptacion de aplicacion para encriptar configuracion en fichero datos.

    datos configuracion:    - clave del usuario encriptada
                            - ruta de los archivos

    Utilizar la llave de usuario para encriptar todo el contenido de fichero 2 datos.

    Todo: generar boton donacion pasta en la aplicacion para mejorar y desarrollar.

    Todo: Formulario sugerencias y peticiones de la aplicacion, send email comprobando conexion internet

    Todo: modificacion ruta de ficheros en configuracion
     
     */
