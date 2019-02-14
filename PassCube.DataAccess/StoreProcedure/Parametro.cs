using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassCube.DataAccess.StoreProcedure
{
	public class Parametro
	{
		//Atributos
		public String m_Nombre { get; set; }
		public Object m_Valor { get; set; }
		public SqlDbType m_TipoDato { get; set; }
		public ParameterDirection m_Direccion { get; set; }
		public int m_Tamaño { get; set; }

		/*
        //Constructor para parámetros de Entrada
        public clsParametro(String ObjNombre,Object objValor) {
            m_Nombre = ObjNombre;
            m_Valor = objValor;
            m_Direccion = ParameterDirection.Input;
        }

        //Constructor para parámetros de Salida
        public clsParametro(String objNombre, Object objValor,SqlDbType objTipoDato,ParameterDirection objDireccion,int objTamaño) {
            m_Nombre = objNombre;
            m_Valor = objValor;
            m_TipoDato = objTipoDato;
            m_Direccion = objDireccion;
            m_Tamaño = objTamaño;
        }
        
        */

		//CONSTRUCTORES
		//Parámetros de Entrada
		public Parametro(String objNombre, Object objValor)
		{
			m_Nombre = objNombre;
			m_Valor = objValor;
			m_Direccion = ParameterDirection.Input;
		}

		//Parámetros de Salida
		public Parametro(String objNombre, SqlDbType objTipoDato, Int32 objTamaño)
		{
			m_Nombre = objNombre;
			m_TipoDato = objTipoDato;
			m_Tamaño = objTamaño;
			m_Direccion = ParameterDirection.Output;
		}
	}
}
