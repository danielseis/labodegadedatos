using PassCube.Entitites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PassCube.BusinessLayer
{
	public class FrmDatosBL : IDataErrorInfo, INotifyPropertyChanged
	{

		Dictionary<string, string> validationErrors = new Dictionary<string, string>();

		Acceso _ac;

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string name)
		{
			if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		//ICommand to bind to button
		public ICommand ValidateInputCommand
		{
			get { return new RelayCommand(); }
			set { }
		}
		#endregion

		public string this[string columnName]
		{
			get
			{
				validationErrors.Clear();

				switch (columnName)
				{
					case "Grupo":
						if (String.IsNullOrWhiteSpace(_ac.Grupo))
						{
							validationErrors.Add("Grupo", "*");
							return "*";
						}

						break;
					case "Usuario":
						if (String.IsNullOrWhiteSpace(_ac.Usuario))
						{
							validationErrors.Add("Usuario", "*");
							return "*";
						}
						break;
					case "Pass":
						if (String.IsNullOrWhiteSpace(_ac.Pass))
						{
							validationErrors.Add("Pass", "*");
							return "*";
						}
						break;
				}

				return string.Empty;
			}
		}
		public string Error
		{
			//get { throw new NotImplementedException(); }

			get
			{
				if (validationErrors.Count > 0)
				{
					return "Errors found.";
				}
				return null;
			}
		}
	}
}
