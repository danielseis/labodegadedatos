using PassCube.BusinessLayer;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PassCube.TestUserControls
{
	/// <summary>
	/// Interaction logic for GeneratePassRnd.xaml
	/// </summary>
	public partial class GeneratePassRnd : UserControl
	{
		public GeneratePassRnd()
		{
			InitializeComponent();
		}

		private void btnRnd_Click(object sender, RoutedEventArgs e)
		{
			Random rng = new Random();
			var valor = "";
			int rompe = rng.Next(1, 25);
			int i = 0;
			foreach (var randomString in SegurityBL.RandomStrings(SegurityBL.AllowedChars, 8, 10, 25, rng))
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
	}
}
