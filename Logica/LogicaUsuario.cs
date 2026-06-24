using Logica.Interfaces;
using Modelo;
using Persistencia;
using Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class LogicaUsuario : ILogicaUsuario
    {
        private IPersistenciaUsuario persistenciaUsuario;
        private IPersistenciaTokenRecuperacion persistenciaToken;
        public LogicaUsuario()
        {
            persistenciaUsuario = FabricaPersistencia.GetInstancia().GetPersistenciaUsuario();
            persistenciaToken = FabricaPersistencia.GetInstancia().GetPersistenciaTokenRecuperacion();
        }

        public Usuario Login(string pEmail, string pPass)
        {
            return persistenciaUsuario.Login(pEmail, pPass);
        }

        public bool ExisteEmail(string pEmail)
        {
            return persistenciaUsuario.ExisteEmail(pEmail);
        }

        private string GenerarToken()
        {
            byte[] bytes = new byte[32];
            using (var aleatorio = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                aleatorio.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        public TokenRecuperacion RecuperarContrasena(string pEmail)
        {
            Usuario usuario = persistenciaUsuario.ObtenerPorEmail(pEmail);

            if (usuario == null)
            {
                return null;
            }

            TokenRecuperacion nuevo = new TokenRecuperacion
            {
                IdUsuario = usuario.IdUsuario,
                Token = GenerarToken(),
                FechaCreacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddMinutes(30),
                Usado = false
            };

            persistenciaToken.Crear(nuevo);
            return nuevo;
        }

        public bool ResetearContrasena(string pToken, string pNuevaContrasena)
        {
            TokenRecuperacion tokenValido = persistenciaToken.ObtenerValido(pToken);

            if(tokenValido == null)
            {
                return false;
            }

            persistenciaUsuario.ActualizarContrasena(tokenValido.IdUsuario, pNuevaContrasena);
            persistenciaToken.MarcarUsado(tokenValido.IdToken);

            return true;
        }

    }
}
