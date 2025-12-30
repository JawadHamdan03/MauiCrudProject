using CRUD_APP.Data;

namespace CRUD_APP
{
    public partial class MainPage : ContentPage
    {
        private ApplicationDbContext _context;
        public MainPage(ApplicationDbContext context)
        {
            this._context = context;
            InitializeComponent();
        }

    }
}
