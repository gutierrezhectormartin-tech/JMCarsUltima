using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Administrador : Usuario
    {
        public Administrador(int pIdUsuario, string pNombre, string pTelefono, string pEmail, string pPass, bool pEstado, Rol pRolUsu, DateTime? pFechaAceptacion) :
            base( pIdUsuario,  pNombre,  pTelefono,  pEmail,  pPass, pEstado, pRolUsu, pFechaAceptacion){}

    }
}
