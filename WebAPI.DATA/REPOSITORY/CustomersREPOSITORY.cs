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
    public class CustomersREPOSITORY
    {
        //METODO PARA OBTENER UNA LISTA DE TODOS LOS CLIENTES
        public List<CUSTOMERS> obtenernumerototalClientes()
        {
            List<CUSTOMERS> ListCustomers = new List<CUSTOMERS>();

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //LLAMAR Y UTILIZAR EL USP_OBTENERTODOSLOSCLIENTES,USANDO LA CLASE SQLCOMMAND
                    SqlCommand cmd = new SqlCommand("USP_obtenernumerototalClientes",connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //CAPTURAMOS LOS DATOS DEVUELTOS POR EL USP EN UN DATAREADER
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        ListCustomers.Add(new CUSTOMERS()
                        {
                            Id_Cliente = Convert.ToInt32(dataReader["Id_Cliente"].ToString()),
                            Empleados_Id = Convert.ToInt32(dataReader["Empleados_Id"].ToString()),
                            Nombre = dataReader["Nombre"].ToString(),
                            Apellido = dataReader["Apellido"].ToString(),
                            Telefono = dataReader["Telefono"].ToString(),
                            Dirección = dataReader["Dirección"].ToString(),
                            Estado = Convert.ToBoolean(dataReader["Estado"].ToString())
                        });
                    }
                    return ListCustomers;
                    connection.Close();
                }
                catch
                {
                    ListCustomers = null;
                    return ListCustomers;
                }
            }
        }
        //Metodo para obtener un Cliente por su ID
        public CUSTOMERS seleccionarClienteporID(int id)
        {
            // Instancia donde guardar el Cliente traido de la BD
            CUSTOMERS cUSTOMERS = new CUSTOMERS();

            //Acceder a la BD
            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();
                    //Llamar y utilizar el UPS_seleccionarClienteporID usando la clase sqlcommand
                    SqlCommand cmd = new SqlCommand("UPS_seleccionarClienteporID", connection);
                    cmd.Parameters.AddWithValue("ID", id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Almacenar los registros obtenidos en n DataReader
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        cUSTOMERS.Id_Cliente = Convert.ToInt32(dataReader["Id_Cliente"].ToString());
                        cUSTOMERS.Empleados_Id = Convert.ToInt32(dataReader["Empleados_Id"].ToString());
                        cUSTOMERS.Nombre = dataReader["Nombre"].ToString();
                        cUSTOMERS.Apellido = dataReader["Apellido"].ToString();
                        cUSTOMERS.Dirección = dataReader["Direccion"].ToString();
                        cUSTOMERS.Telefono = dataReader["Telefono"].ToString();
                        cUSTOMERS.Estado = Convert.ToBoolean(dataReader["Estado"].ToString());
                    }

                    return cUSTOMERS;
                    connection.Close();
                }
                catch
                {
                    cUSTOMERS = null;
                    return cUSTOMERS;
                }
            }
        }
        //Insertar un nuevo Registro para Cliente 
        public bool insertarClientes(CUSTOMERS cUSTOMERS)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar comando a la BD 
                    SqlCommand cmd = new SqlCommand("USP_insertarCategorias", connection);
                    cmd.Parameters.AddWithValue("Empleado_Id", cUSTOMERS.Empleados_Id);
                    cmd.Parameters.AddWithValue("Nombre", cUSTOMERS.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", cUSTOMERS.Apellido);
                    cmd.Parameters.AddWithValue("Direccion", cUSTOMERS.Dirección);
                    cmd.Parameters.AddWithValue("Telefono", cUSTOMERS.Telefono);
                    cmd.Parameters.AddWithValue("Estado", cUSTOMERS.Estado);
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
        //Metodo para eliminar un registro de Clientes
        public bool eliminarClientes(int id)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar nuestro comando a la BD
                    SqlCommand cmd = new SqlCommand("USP_eliminarClientes", connection);
                    cmd.Parameters.AddWithValue(" Id_Cliente", id);
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
