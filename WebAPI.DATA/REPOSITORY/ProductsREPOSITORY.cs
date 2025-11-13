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
    public class ProductsREPOSITORY
    {
        //METODO PARA OBTENER UNA LISTA DE TODOS LOS PRODUCTOS
        public List<PRODUCTS> obtenerProductos()
        {
            List<PRODUCTS> ListProducts = new List<PRODUCTS>();

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //LLAMAR Y UTILIZAR EL USP_OBTENERTODOS LOS PRODUCTOS,USANDO LA CLASE SQLCOMMAND
                    SqlCommand cmd = new SqlCommand("USP_ObtenerProductos",connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //CAPTURAMOS LOS DATOS DEVUELTOS POR EL USP EN UN DATAREADER
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        ListProducts.Add(new PRODUCTS()
                        {
                            Id_Productos = Convert.ToInt32(dataReader["Id_Productos"].ToString()),
                            Nombre_Producto = dataReader["Nombre_Producto"].ToString(),
                            Cantidad_Existente = dataReader["Cantidad_Existente"].ToString(),
                            Precio = Convert.ToInt32(dataReader["Precio"].ToString()),
                            categoria_Id = Convert.ToInt32(dataReader["Categoria_Id"].ToString()),
                            Estado = Convert.ToBoolean(dataReader["Estado"].ToString())
                        });
                    }
                    return ListProducts;
                    connection.Close();
                }
                catch
                {
                    ListProducts = null;
                    return ListProducts;
                }
            }
        }
        
        //Insertar un nuevo Registro para Productos 
        public bool insertarProductos(PRODUCTS pRODUCTS)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar comando a la BD 
                    SqlCommand cmd = new SqlCommand("USP_insertarCategorias", connection);
                    cmd.Parameters.AddWithValue("Nombre_Producto", pRODUCTS.Nombre_Producto);
                    cmd.Parameters.AddWithValue("Cantidad:Existente", pRODUCTS.Cantidad_Existente);
                    cmd.Parameters.AddWithValue("Precio", pRODUCTS.Precio);
                    cmd.Parameters.AddWithValue("categoria_Id", pRODUCTS.categoria_Id);
                    cmd.Parameters.AddWithValue("Estado", pRODUCTS.Estado);
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
        //Metodo para eliminar un registro de Productos
        public bool eliminarProductos(int id)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar nuestro comando a la BD
                    SqlCommand cmd = new SqlCommand("USP_eliminarProductos", connection);
                    cmd.Parameters.AddWithValue(" Id_Productos", id);
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
