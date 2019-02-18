using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassCube.Entitites
{
	public class Acceso
	{

		private string grupo { get; set; }
		private string usuario { get; set; }
		private string pass { get; set; }

		public string Grupo
		{
			get { return grupo; }
			set { grupo = value; }
		}

		public string Usuario
		{
			get { return usuario; }
			set { usuario = value; }
		}

		public string Pass
		{
			get { return pass; }
			set { pass = value; }
		}
	}
}
