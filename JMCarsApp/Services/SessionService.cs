using System;
using System.Collections.Generic;
using System.Text;
using JMCarsApp.DTOs;

namespace JMCarsApp.Services
{
    public class SessionService
    {
        public UsuarioDTO? Usuario { get; private set; }

        public bool EstadoLogueado => Usuario != null;
        public bool EsAdmin => Usuario?.RolUsu == 1;
        public bool EsEscribano => Usuario?.RolUsu == 2;
        public bool EsCliente => Usuario?.RolUsu == 3;

        public event Action? CambioSesion;

        public void Iniciar(UsuarioDTO pUsuario)
        {
            Usuario = pUsuario;
            CambioSesion?.Invoke();
        }

        public void Cerrar()
        {
            Usuario = null;
            CambioSesion?.Invoke();
        }
    }
}
