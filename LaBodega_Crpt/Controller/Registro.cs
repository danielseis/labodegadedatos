using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LaBodega_Crpt.Controller
{
    class Registro : IDataErrorInfo, INotifyPropertyChanged
    {

        Dictionary<string, string> validationErrors = new Dictionary<string, string>();


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
                        if (String.IsNullOrWhiteSpace(grupo))
                        {
                            validationErrors.Add("Grupo", "*");
                            return "*";
                        }

                        break;
                    case "Usuario":
                        if (String.IsNullOrWhiteSpace(usuario))
                        {
                            validationErrors.Add("Usuario", "*");
                            return "*";
                        }
                        break;
                    case "Pass":
                        if (String.IsNullOrWhiteSpace(pass))
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
