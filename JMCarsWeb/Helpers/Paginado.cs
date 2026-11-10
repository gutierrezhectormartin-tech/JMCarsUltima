namespace JMCarsWeb.Helpers
{
    public static class Paginado
    {
        public static int CorregirPagina(int pagina, int total, int tamanoPagina)
        {
            int totalPaginas = (int)Math.Ceiling(total / (double)tamanoPagina);
            if(pagina > totalPaginas)
            {
                pagina = totalPaginas;
            }
            if(pagina < 1)
            {
                pagina = 1;
            }
            return pagina;
        }
    }
}
