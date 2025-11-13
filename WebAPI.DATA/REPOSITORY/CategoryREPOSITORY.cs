using System;
using WebAPI.MODEL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WebAPI.DATA.REPOSITORY
{
    public class CategoryREPOSITORY
    {
        //METODO PARA OBTENER UNA LISTA DE TODAS LAS CATEGORIAS
        public List<CATEGORY> ObtenertodaslasCategorias()
        {
            List<CATEGORY> ListCategories = new List<CATEGORY>();

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //LLAMAR Y UTILIZAR EL USP_OBTENERTODASLASCATEGORIAS,USANDO LA CLASE SQLCOMMAND
                    SqlCommand cmd = new SqlCommand("USP_ObtenertodaslasCategorias", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //CAPTURAMOS LOS DATOS DEVUELTOS POR EL USP EN UN DATAREADER
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        ListCategories.Add(new CATEGORY()
                        {
                            Id_Categoria = Convert.ToInt32(dataReader["Id_Categoria"].ToString()),
                            Nombre_Categoria = dataReader["Nombre_Categoria"].ToString(),
                            Descripcion = dataReader["Descripcion"].ToString(),
                            Productos_Id = Convert.ToInt32(dataReader["Productos_Id"].ToString())
                        });
                    }
                    return ListCategories;
                    connection.Close();
                }
                catch
                {
                    ListCategories = null;
                    return ListCategories;
                }
            }
        }
        //Metodo para obtener una categoria por su ID
        public CATEGORY obtenerCategoriaporID(int id)
        {
            // Instancia donde guardar la categoria traida de la BD
            CATEGORY category = new CATEGORY();

            //Acceder a la BD
            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();
                    //Llamar y utilizar el UPS_obtenerCategoriaporID usando la clase sqlcommand
                    SqlCommand cmd = new SqlCommand("UPS_obtenerCategoriaporID", connection);
                    cmd.Parameters.AddWithValue("ID", id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Almacenar los registros obtenidos en n DataReader
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        category.Id_Categoria = Convert.ToInt32(dataReader["Id_Categoria"].ToString());
                        category.Nombre_Categoria = dataReader["Nombre_Categoria"].ToString();
                        category.Descripcion = dataReader["Descripcion"].ToString();
                        category.Productos_Id = Convert.ToInt32(dataReader["Productos_Id"].ToString());
                    }

                    return category;
                    connection.Close();
                } catch
                {
                    category = null;
                    return category;
                }
            }
        }
        //Insertar un nuevo Registro para categoria 
        public bool insertarCategorias(CATEGORY category)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar comando a la BD 
                    SqlCommand cmd = new SqlCommand("USP_insertarCategorias", connection);
                    cmd.Parameters.AddWithValue("Nnombre_categoria", category.Nombre_Categoria);
                    cmd.Parameters.AddWithValue("Descripcion", category.Descripcion);
                    cmd.Parameters.AddWithValue("Productos_Id", category.Productos_Id);
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
        //Metodo para eliminar un registro de Categories
        public bool eliminarCategoria(int id)
        {
            bool result = true;

            using (SqlConnection connection = new SqlConnection(DBConnection.Connect()))
            {
                try
                {
                    connection.Open();

                    //Personalizar nuestro comando a la BD
                    SqlCommand cmd = new SqlCommand("USP_eliminarCategoria", connection);
                    cmd.Parameters.AddWithValue("Id_Categoria", id);
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
