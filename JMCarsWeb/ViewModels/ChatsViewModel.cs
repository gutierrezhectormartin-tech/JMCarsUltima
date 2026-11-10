using JMCarsWeb.DTOs;
using X.PagedList;
using X.PagedList.Extensions;

namespace JMCarsWeb.ViewModels
{
    public class ChatsViewModel
    {
        public IPagedList<ChatDTO> Chats { get; set; } = new List<ChatDTO>().ToPagedList();
        public string? Buscar { get; set; }
        public int TotalFiltrados { get; set; }
    }
}
