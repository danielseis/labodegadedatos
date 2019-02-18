using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassCube.Entitites
{
	public class User : IDataErrorInfo
	{
		public User() { }


		public static string Pass { get; set; }

		public string Error
		{
			get { return null; }
		}

		public string this[string columnName]
		{
			get
			{
				switch (columnName)
				{

					case "Pass":
						if (String.IsNullOrWhiteSpace(Pass))
							return "*";
						break;
				}

				return string.Empty;
			}
		}
	}
}
