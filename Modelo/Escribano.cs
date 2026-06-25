using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo
{
    public class Escribano : Usuario
    {
        private string numeroCaja;
        private string direccion;

        [Required(ErrorMessage = "El número de caja profesional es obligatorio.")]
        [StringLength(50, ErrorMessage = "El número de caja no puede superar los 50 caracteres.")]
        public string NumeroCaja
        {
            get { return numeroCaja; }
            set
            {
                numeroCaja = value;
            }
        }

        [Required(ErrorMessage = "La dirección del estudio es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres.")]
        public string Direccion
        {
            get { return direccion; }
            set
            {
                direccion = value;
            }
        }

        public Escribano(int pIdUsuario, string pNombre, string pTelefono, string pEmail, string pPass, bool pEstado, Rol pRolUsu, DateTime? pFechaAceptacion, string pNumeroCaja, string pDireccion) :
            base(pIdUsuario, pNombre, pTelefono, pEmail, pPass, pEstado, pRolUsu, pFechaAceptacion)
        {
            NumeroCaja = pNumeroCaja;
            Direccion = pDireccion;
        }

        public Escribano()
        {
            NumeroCaja = string.Empty;
            Direccion = string.Empty;
        }
    }
}
