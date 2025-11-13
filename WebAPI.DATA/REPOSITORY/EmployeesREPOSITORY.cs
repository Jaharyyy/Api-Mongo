using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.MODEL;

namespace WebAPI.DATA.REPOSITORY
{
    public class EmployeesREPOSITORY
    {
        //METODO PARA OBTENER UNA LISTA DE TODOS LOS EMPLEADOS
        public List<EMPLOYEES> obtenertodosEmpleados()
        {
            List<EMPLOYEES> ListEmployees = new List<EMPLOYEES>();

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //LLAMAR Y UTILIZAR EL USP_OBTENERTODOS LOS EMPLEADOS,USANDO LA CLASE SQLCOMMAND
                    SqlCommand cmd = new SqlCommand("USP_ObtenertodosEmpleados",connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //CAPTURAMOS LOS DATOS DEVUELTOS POR EL USP EN UN DATAREADER
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        ListEmployees.Add(new EMPLOYEES()
                        {
                            Id_Empleados = Convert.ToInt32(dataReader["Id_Empleados"].ToString()),
                            Nombre = dataReader["Nombre"].ToString(),
                            Apellido = dataReader["Apellido"].ToString(),
                            Telefono = dataReader["Telefono"].ToString(),
                            Numero_Cedula = dataReader["Numero_Cedula"].ToString(),
                            Estado = Convert.ToBoolean(dataReader["Estado"].ToString())
                        });
                    }
                    return ListEmployees;
                    connection.Close();
                }
                catch
                {
                    ListEmployees = null;
                    return ListEmployees;
                }
            }
        }
        //Metodo para obtener un Empleado por su ID
        public EMPLOYEES obtenerEmpleado(int id)
        {
            // Instancia donde guardar el Empleado traido de la BD
            EMPLOYEES eMPLOYEES = new EMPLOYEES();

            //Acceder a la BD
            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();
                    //Llamar y utilizar el UPS_obtenerEmpleado usando la clase sqlcommand
                    SqlCommand cmd = new SqlCommand("UPS_obtenerEmpleado", connection);
                    cmd.Parameters.AddWithValue("ID", id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Almacenar los registros obtenidos en n DataReader
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        eMPLOYEES.Id_Empleados = Convert.ToInt32(dataReader["Id_Cliente"].ToString());
                        eMPLOYEES.Nombre = dataReader["Nombre"].ToString();
                        eMPLOYEES.Apellido = dataReader["Apellido"].ToString();
                        eMPLOYEES.Telefono = dataReader["Telefono"].ToString();
                        eMPLOYEES.Numero_Cedula = dataReader["Numero_Cedula"].ToString();
                        eMPLOYEES.Estado = Convert.ToBoolean(dataReader["Estado"].ToString());
                    }

                    return eMPLOYEES;
                    connection.Close();
                }
                catch
                {
                    eMPLOYEES = null;
                    return eMPLOYEES;
                }
            }
        }
        //Insertar un nuevo Registro para Cliente 
        public bool insertarEmpleados(EMPLOYEES eMPLOYEES)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar comando a la BD 
                    SqlCommand cmd = new SqlCommand("USP_insertarCategorias", connection);
                    cmd.Parameters.AddWithValue("Id_Empleados", eMPLOYEES.Id_Empleados);
                    cmd.Parameters.AddWithValue("Nombre", eMPLOYEES.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", eMPLOYEES.Apellido);
                    cmd.Parameters.AddWithValue("Telefono", eMPLOYEES.Telefono);
                    cmd.Parameters.AddWithValue("Numero_Cedula", eMPLOYEES.Numero_Cedula);
                    cmd.Parameters.AddWithValue("Estado", eMPLOYEES.Estado);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Tipo de ejecucion que se hara en la BD
                    cmd.ExecuteNonQuery();

                    return result;
                    connection.Close();
                }
                catch
                {
                    result = false;
                    return result;
                }
            }
        }
        //Metodo para eliminar un registro de Empleados
        public bool eliminarEmpleados(int id)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar nuestro comando a la BD
                    SqlCommand cmd = new SqlCommand("USP_eliminarEmpleados", connection);
                    cmd.Parameters.AddWithValue(" Id_Empleados", id);
                    cmd.Parameters.Add("Result", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Tipo de ejecucion que se hara en la BD
                    cmd.ExecuteNonQuery();

                    //Capturar el valor/resultado que envía el Proced. Almacenado
                    result = Convert.ToBoolean(cmd.Parameters["Result"].Value);

                    return result;
                    connection.Close();

                }
                catch
                {
                    result = false;
                    return result;
                }
            }
        }
    }
}
