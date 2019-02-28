using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassCube.Entitites
{
	public class Settings
	{

		public static string MiLogin { get; set; }
		public static string MiLoginDe { get; set; }
		public static string MiLoginDebk { get; set; }
		public static string myData { get; set; }
		
		public static string myDataDeBK { get; set; }

		//public static string version { get; set; }
		//public static string innstall { get; set; }
		//public static string lastacces { get; set; }
		//public static string myDataDe { get; set; }


		public static Byte[] md5Login { get; set; }
		

		public Settings()
		{
			MiLogin = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\labodega.xml";
			MiLoginDe = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\labodegade.xml";
			MiLoginDebk = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\labodegade(send).xml";

			myData = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\myBodega.xml";
			myDataDeBK = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\myBodega(send).xml";

			//myDataDe = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\myBodegaDe.xml";

		}
	}
}
