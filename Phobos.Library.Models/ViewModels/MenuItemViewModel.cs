namespace Phobos.Library.Models.ViewModels
{
    public class MenuEntriesViewModel
    {
        public MenuEntriesViewModel()
        {
            Childs = new MenuEntriesListViewModel();
        }
        public string Url { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
        public MenuEntriesListViewModel Childs { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}