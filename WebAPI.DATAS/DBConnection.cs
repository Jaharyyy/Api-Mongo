using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.DATAS
{
    public class DBConnection
    {

        public static string Connect()
        {
            //PASAMOS EL ARCHIVO DE CONFIGURACION DONDE ESTA LAS CONNECTION STRING
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            //CONSTRUIMOS LA CADENA DE CONEXION 
            var root = builder.Build();

            //INDICAMOS EL NOMBRE DE LA CADENA DE CONEXION ESPECIFICA
            var Connection = root.GetConnectionString("ConnectionString");

            return Connection;
        }
    }
}
