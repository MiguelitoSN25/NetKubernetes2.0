using Microsoft.AspNetCore.Identity;
using NetKubernetes2._0.Models;

namespace NetKubernetes2._0.Data
{
    public class LoadDatabase
    {
        public static async Task InsertarData(AppDbContext context, UserManager <Usuario> usuarioManager) 
        {


            if (usuarioManager.Users.Any())
            {
                var usuario = new Usuario
                {
                    Name = "Miguel",
                    LastName = "Soriano",
                    Email ="rensomiguel1@gmail.com",
                    UserName ="Miguelito123",
                    Phone = "8097079027"
                };

                await usuarioManager.CreateAsync(usuario, "PasswordVxidrez123$");
            }

            if (!context.Inmuebles!.Any())
            {
                context.Inmuebles!.AddRange(
                    new Inmueble
                    {
                        Nombre = "Casa de Playa",
                        Direccion = "Calle 27 de Febrero",
                        Precio = 25756M,
                        FechaCreacion = DateTime.Now
                    },
                    new Inmueble
                    {
                        Nombre = "Casa de Invierno",
                        Direccion = "Calle 27 de Marzo",
                        Precio = 35756M,
                        FechaCreacion = DateTime.Now
                    });
            }

            context.SaveChanges();

        }
    }
}
