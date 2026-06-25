using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public static class Encriptacion
    {
        public static string Hashear(string pContrasenaPlana)
        {
            return BCrypt.Net.BCrypt.HashPassword(pContrasenaPlana);
        }
        public static bool Verificar(string pContrasenaPlana, string pHashGuardado)
        {
            return BCrypt.Net.BCrypt.Verify(pContrasenaPlana, pHashGuardado);
        }
    }
}
