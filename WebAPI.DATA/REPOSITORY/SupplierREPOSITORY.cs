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
    public class SupplierREPOSITORY
    {
        //METODO PARA OBTENER UNA LISTA DE TODOS LOS PROVEEDORES
        public List<SUPPLIER> obtenertodoslosProveedores()
        {
            List<SUPPLIER> ListSuppliers = new List<SUPPLIER>();

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //LLAMAR Y UTILIZAR EL USP_OBTENERTODOS LOS PROVEEDORES,USANDO LA CLASE SQLCOMMAND
                    SqlCommand cmd = new SqlCommand("USP_obtenertodoslosProveedores",connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //CAPTURAMOS LOS DATOS DEVUELTOS POR EL USP EN UN DATAREADER
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        ListSuppliers.Add(new SUPPLIER()
                        {
                            Id_Proveedor = Convert.ToInt32(dataReader["Id_Proveedor"].ToString()),
                            Productos_Id = Convert.ToInt32(dataReader["Productos_Id"].ToString()),
                            Nombre_Negocio = dataReader["Nombre_Negocio"].ToString(),
                            Nombre = dataReader["Nombre"].ToString(),
                            Apellido = dataReader["Apellido"].ToString(),
                            Telefono = dataReader["Telefono"].ToString(),
                            Dirección = dataReader["Dirección"].ToString(),
                            Estado = Convert.ToBoolean(dataReader["Estado"].ToString())
                        });
                    }
                    return ListSuppliers;
                    connection.Close();
                }
                catch
                {
                    ListSuppliers = null;
                    return ListSuppliers;
                }
            }
        }
        //Metodo para obtener un Proveedor por su ID
        public SUPPLIER obtenerProveedorporID(int id)
        {
            // Instancia donde guardar la categoria traida de la BD
            SUPPLIER sUPPLIER = new SUPPLIER();

            //Acceder a la BD
            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();
                    //Llamar y utilizar el UPS_obtenerCategoriaporID usando la clase sqlcommand
                    SqlCommand cmd = new SqlCommand("UPS_obtenerProveedorporID", connection);
                    cmd.Parameters.AddWithValue("ID", id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Almacenar los registros obtenidos en n DataReader
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        sUPPLIER.Id_Proveedor = Convert.ToInt32(dataReader["Id_Proveedor"].ToString());
                       
                    }

                    return sUPPLIER;
                    connection.Close();
                }
                catch
                {
                    sUPPLIER = null;
                    return sUPPLIER;
                }
            }
        }
        //Insertar un nuevo Registro para Proveedor 
        public bool insertarProveedor(SUPPLIER sUPPLIER)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar comando a la BD 
                    SqlCommand cmd = new SqlCommand("USP_insertarCategorias", connection);
                    cmd.Parameters.AddWithValue("Productos_Id", sUPPLIER.Productos_Id);
                    cmd.Parameters.AddWithValue("Nombre_Negocio", sUPPLIER.Nombre_Negocio);
                    cmd.Parameters.AddWithValue("Nombre", sUPPLIER.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", sUPPLIER.Apellido);
                    cmd.Parameters.AddWithValue("Telefono", sUPPLIER.Telefono);
                    cmd.Parameters.AddWithValue("Direccion", sUPPLIER.Dirección);
                    cmd.Parameters.AddWithValue("Estado", sUPPLIER.Estado);
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
        //Metodo para eliminar un registro de Proveedor
        public bool eliminarProveedor(int id)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar nuestro comando a la BD
                    SqlCommand cmd = new SqlCommand("USP_eliminarProveedor", connection);
                    cmd.Parameters.AddWithValue(" Id:Proveedor", id);
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
