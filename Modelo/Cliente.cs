using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Text;

namespace Modelo
{
    public class Cliente : Usuario
    {
        private string cedula;

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [RegularExpression(@"^[0-9]{7,8}$", ErrorMessage = "La cédula debe tener 7 u 8 dígitos numéricos.")]
        public string Cedula
        {
            get { return cedula; }
            set
            {
                cedula = value;
            }
        }

        public Cliente (int pIdUsuario, string pNombre, string pTelefono, string pEmail, string pPass, bool pEstado, Rol pRolUsu, DateTime? pFechaAceptacion, string pCi) :
            base (pIdUsuario, pNombre, pTelefono, pEmail, pPass, pEstado, pRolUsu, pFechaAceptacion)
        {
            Cedula = pCi;
        }

        public Cliente() { }
    }
}
