using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo
{
    public class Usuario
    {
        private int idUsuario;
        private string nombreCompleto;
        private string telefono;
        private string email;
        private string? contrasena;
        private bool estadoUsu;
        private Rol rolUsu;

        public int IdUsuario
        {
            get { return idUsuario; }
            set
            {
                idUsuario = value;
            }
        }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string NombreCompleto
        {
            get { return nombreCompleto; }
            set
            {
                nombreCompleto = value;
            }
        }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string Telefono
        {
            get { return telefono; }
            set
            {
                telefono = value;
            }
        }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        [StringLength(255, ErrorMessage = "El email no puede superar los 255 caracteres.")]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
            }
        }

        //Martin la contraseña no es obligatoria en el cliente, eso solo es obligatorio en el registro se saca, sino
        //cada vez que vas a usar el modelo, te pide si o si que mandes contraseña.
        [StringLength(255, MinimumLength = 3, ErrorMessage = "La contraseña debe tener entre 3 y 255 caracteres.")]
        public string? Contrasena
        {
            get { return contrasena; }
            set
            {
                contrasena = value;
            }
        }

        public bool EstadoUsu
        {
            get { return estadoUsu; }
            set
            {
                estadoUsu = value;
            }
        }

        public Rol RolUsu
        {
            get { return rolUsu; }
            set
            {
                rolUsu = value;
            }
        }

        public Usuario(int pIdUsuario, string pNombre, string pTelefono, string pEmail, string pPass, bool pEstado, Rol pRolUsu)
        {
            IdUsuario = pIdUsuario;
            NombreCompleto = pNombre;
            Telefono = pTelefono;
            Email = pEmail;
            Contrasena = pPass;
            EstadoUsu = pEstado;
            RolUsu = pRolUsu;
        }

        public Usuario() { }

    }
}
