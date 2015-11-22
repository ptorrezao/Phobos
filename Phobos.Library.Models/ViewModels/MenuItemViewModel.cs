namespace Phobos.Library.Models.ViewModels
{
    public class MenuEntriesViewModel
    {
        public string Url { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
        public MenuEntriesListViewModel Childs { get; set; }
    }
}