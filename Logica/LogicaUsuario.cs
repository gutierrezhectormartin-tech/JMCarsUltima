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
        private readonly IPersistenciaUsuario persistenciaUsuario;
        private readonly IPersistenciaTokenRecuperacion persistenciaToken;
        private readonly IPersistenciaRegistroActividad persistenciaRegistro;
        public LogicaUsuario()
        {
            persistenciaUsuario = FabricaPersistencia.GetInstancia().GetPersistenciaUsuario();
            persistenciaToken = FabricaPersistencia.GetInstancia().GetPersistenciaTokenRecuperacion();
            persistenciaRegistro = FabricaPersistencia.GetInstancia().GetPersistenciaRegistroActividad();
        }

        public Usuario Login(string pEmail, string pPass)
        {
            Usuario usuario = persistenciaUsuario.Login(pEmail);

            if (usuario == null)
            {
                return null!;
            }

            bool valida = Encriptacion.Verificar(pPass, usuario.Contrasena!);

            if (!valida)
            {
                return null!;
            }

            if (!usuario.EstadoUsu)
            {
                throw new Exception("Tu cuenta de escribano no está activa. Si te registraste recientemente, un administrador tiene que aprobarla; si fue inactivada, un administrador puede reactivarla.");
            }

            try
            {
                persistenciaRegistro.RegistrarActividad(usuario.IdUsuario, "Login", null);
            }
            catch (Exception) // no va a trabar nada si no registra tiene que permitir que siga todo funcando
            {
            }
            return usuario;
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

        private void ValidarContrasenaNueva(string pContrasena)
        {
            if(string.IsNullOrWhiteSpace(pContrasena) || pContrasena.Length < 3)
            {
                throw new Exception("La contraseña debe tener al menos 3 caracteres");
            }
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
            ValidarContrasenaNueva(pNuevaContrasena);

            TokenRecuperacion tokenValido = persistenciaToken.ObtenerValido(pToken);

            if (tokenValido == null)
            {
                return false;
            }

            string hashNuevo = Encriptacion.Hashear(pNuevaContrasena);
            persistenciaUsuario.ActualizarContrasena(tokenValido.IdUsuario, hashNuevo);
            persistenciaToken.MarcarUsado(tokenValido.IdToken);

            return true;
        }

        public List<Usuario> ListarAdministradoresActivos()
        {
            return persistenciaUsuario.ListarAdministradoresActivos();
        }

        void CambiarContrasena(string pEmail, string pContrasenaActual, string pContrasenaNueva)
        {
            Usuario usuario = persistenciaUsuario.Login(pEmail);

            if(usuario == null || string.IsNullOrEmpty(pContrasenaActual) || Encriptacion.Verificar(pContrasenaActual, pContrasenaNueva!))
            {
                throw new Exception("La contraseña actual es incorrecta");
            }
            ValidarContrasenaNueva(pContrasenaNueva);
            if(pContrasenaActual == pContrasenaNueva)
            {
                throw new Exception("La nueva contraseña no puede ser igual a la actual");
            }
            string hashNuevo = Encriptacion.Hashear(pContrasenaNueva);
            persistenciaUsuario.ActualizarContrasena(usuario.IdUsuario, hashNuevo);

            try
            {
                persistenciaRegistro.RegistrarActividad(usuario.IdUsuario, "Cambio de Contraseña", null)
            }
            catch (Exception)
            {
            }
        }

    }
}
