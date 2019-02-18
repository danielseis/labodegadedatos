using PassCube.BusinessLayer;
using PassCube.Entitites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        }
    }
}
