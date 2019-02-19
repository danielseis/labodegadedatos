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
	/// Interaction logic for frmDatos.xaml
	/// </summary>
	public partial class frmDatos : UserControl
	{
		private int _errors = 0;
		private PassBL _datos = new PassBL();

		System.Windows.Forms.NotifyIcon iconoNotificacion;


		public frmDatos()
		{
			InitializeComponent();
			DataContext = _datos;
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
		{

		}
		private void txtPasswordbox_PasswordChanged(object sender, RoutedEventArgs e)
		{

		}
		private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{

		}
		private void btnCharge_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Validation_Error(object sender, ValidationErrorEventArgs e)
		{
			if (e.Action == ValidationErrorEventAction.Added)
				_errors++;
			else
				_errors--;
		}

		
	}
}
