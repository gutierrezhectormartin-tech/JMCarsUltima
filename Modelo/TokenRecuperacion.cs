using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class TokenRecuperacion
    {
        private int idToken;
        private int idUsuario;
        private string token;
        private DateTime fechaCreacion;
        private DateTime fechaExpiracion;
        private bool usado;

        public int IdToken
        {
            get { return idToken; }
            set
            {
                idToken = value;
            }
        }

        public int IdUsuario
        {
            get { return idUsuario; }
            set
            {
                idUsuario = value;
            }
        }

        public string Token
        {
            get { return token; }
            set
            {
                token = value;
            }
        }

        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set
            {
                fechaCreacion = value;
            }
        }

        public DateTime FechaExpiracion
        {
            get { return fechaExpiracion; }
            set
            {
                fechaExpiracion = value;
            }
        }

        public bool Usado
        {
            get { return usado; }
            set
            {
                usado = value;
            }
        }

        public TokenRecuperacion() { }

        public TokenRecuperacion(int pIdToken, int pIdUsuario, string pToken, DateTime pFechaCreacion, DateTime pFechaExpiracion, bool pUsado)
        {
            IdToken = pIdToken;
            IdUsuario = pIdUsuario;
            Token = pToken;
            FechaCreacion = pFechaCreacion;
            FechaExpiracion = pFechaExpiracion;
            Usado = pUsado;
        }
    }
}
