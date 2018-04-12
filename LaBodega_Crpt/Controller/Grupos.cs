using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;



namespace LaBodega_Crpt.Controller
{
    public class Grupos : IDataErrorInfo, INotifyPropertyChanged
    {

        Dictionary<string, string> validationErrors = new Dictionary<string, string>();

        [XmlElement(ElementName = "id")]
        public static int id { get; set; }
        [XmlElement(ElementName = "grupo")]
        public static string nameGroup { get; set; }
        [XmlElement(ElementName = "lugar")]
        public static string nameUrl { get; set; }
        [XmlElement(ElementName = "user")]
        public static string Usuario { get; set; }
        [XmlElement(ElementName = "pass")]
        public static string Password { get; set; }
        [XmlElement(ElementName = "date")]
        public DateTime dateCreate { get; set; }

        public static bool isEdit { get; set; }

        public static int xmlRow { get; set; }

        public Grupos()
        {
            isEdit = false;
        }

          

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        
        #endregion


        #region IDataErrorInfo
        public string this[string columnName]
        {
            get
            {
                validationErrors.Clear();

                switch (columnName)
                {
                    case "nameGroup":
                        if (String.IsNullOrWhiteSpace(nameGroup))
                        {
                            validationErrors.Add("nameGroup", "*");
                            return "*";
                        }

                        break;
                    case "Usuario":
                        if (String.IsNullOrWhiteSpace(Usuario))
                        {
                            validationErrors.Add("Usuario", "*");
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


       
      

        #endregion

    }
}
