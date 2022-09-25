namespace WMS.Ui.Mvc6.Models.Admin
{
    public class AdminViewModel
    {
        public AdminViewModel()
        {
            RolesViewModel = new();
         UsersViewModel = new();
            CategoriesViewModel = new();
            VarietiesViewModel = new();
            YeastsViewModel = new();
            MaloCulturesViewModel = new();
            RecipesViewModel = new();
            JournalsViewModel = new();
        }

        public string TabToShow { get; set; } = string.Empty;
        public RolesViewModel RolesViewModel { get; set; }
        public UsersViewModel UsersViewModel { get; set; }
        public CategoriesViewModel CategoriesViewModel { get; set; }
        public VarietiesViewModel VarietiesViewModel { get; set; }
        public YeastsViewModel YeastsViewModel { get; set; }
        public MaloCulturesViewModel MaloCulturesViewModel { get; set; }
        public RecipesViewModel RecipesViewModel { get; set; }
        public JournalsViewModel JournalsViewModel { get; set; }

    }

}
