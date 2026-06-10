using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class Administrador : Usuario
    {
        public Administrador(int pIdUsuario, string pNombre, string pTelefono, string pEmail, string pPass, bool pEstado) :
            base( pIdUsuario,  pNombre,  pTelefono,  pEmail,  pPass, pEstado){}

    }
}
