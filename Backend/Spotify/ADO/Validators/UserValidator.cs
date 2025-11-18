using Spotify.ADO;

namespace Spotify.Validators
{
    public class UserValidator
    {
        private readonly DatabaseConnection _dbConn;

        public UserValidator(DatabaseConnection dbConn)
        {
            _dbConn = dbConn;
        }

        
        //  true = Usuari existent
        //  false = Usuari disponible.
       
        public bool UserExists(string username)
        {
            var user = UserADO.GetByName(_dbConn, username);
            return user != null;
        }

        
        /// Lanza una excepción si el usuario ya existe.
        
        public void ValidateUserDoesNotExist(string username)
        {
            if (UserExists(username))
                throw new Exception($"El usuario '{username}' ya existe.");
        }
    }
}
