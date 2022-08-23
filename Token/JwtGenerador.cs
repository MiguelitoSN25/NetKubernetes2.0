using Microsoft.IdentityModel.Tokens;
using NetKubernetes2._0.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/// Header Contiene el algoritmo para encriptar el token
namespace NetKubernetes2._0.Token
{
    public class JwtGenerador : IJwtGenerador
    {
        public string CrearToken(Usuario usuario)
        {
            /// Payload se agrega data importante del usuario , EJ Email, Username , Password
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName!),
                new Claim("userId",usuario.Id),
                new Claim("email",usuario.Email!)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credenciales
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescripcion);
            return tokenHandler.WriteToken(token);

        }
    }
}
//Signature Crea un key object para proteger al token

//Claim es un objeto de tipo diccionario, tiene 2 partes el key o identificador y el valor 