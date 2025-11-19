using Spotify.Repository;
using Spotify.DTO;
using System.Security.Cryptography;

namespace Spotify.Services
{
    public class LoginService
    {
        private readonly DatabaseConnection _dbConn;

        public LoginService(DatabaseConnection dbConn)
        {
            _dbConn = dbConn;
        }

        public LoginResponse? Login(string name, string password)
        {
             var roles = new List<string>
            {
                "ADMIN",
                "USER"
            };

            
            var permissions = new List<string>
            {
                "CAN_PLAY",
                "CAN_EDIT",
                "CAN_DELETE"
            };
            // Buscar usuari
            var user = UserADO.GetByName(_dbConn, name);
            if (user == null)
                return null;

            //  Validar contrasenya
            byte[] saltBytes = Convert.FromBase64String(user.Salt);

            using var hash = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            byte[] enteredHash = hash.GetBytes(16);
            string enteredHashBase64 = Convert.ToBase64String(enteredHash);

            if (enteredHashBase64 != user.Password)
                return null;

            // Obtenir Rols
            // var roles = UserRolesADO.GetRolesByUserId(_dbConn, user.Id); 
            // devuelve List<string>

            // Obtenir els permisos
            // List<string> permissions = new();

            // foreach (var roleCode in roles)
            // {
            //     var perms = RolePermissionsADO.GetPermissionsByRoleCode(_dbConn, roleCode);
            //     permissions.AddRange(perms);
            // }

            // Resposta
            return new LoginResponse
            {
                UserId = user.Id,
                Roles = roles,
                Permissions = permissions,
                Token = "" // vacío por ahora
            };
        }
    }
}
