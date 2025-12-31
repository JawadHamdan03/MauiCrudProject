using CRUD_APP.Data;
using System.Collections.ObjectModel;
using CRUD_APP.Models;
using Microsoft.EntityFrameworkCore;
namespace CRUD_APP
{
    public partial class MainPage : ContentPage
    {
        private readonly ApplicationDbContext _context;
        public ObservableCollection<User> Users { get; set; }
        public  MainPage(ApplicationDbContext context)
        {
            InitializeComponent();
            this._context = context;
            Users = new ObservableCollection<User>();
            BindingContext = this;
            LoadDataAsync();
        }

        public async void LoadDataAsync()
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();
                var users = await _context.Users.ToListAsync();
                foreach(var user in users)
                    Users.Add(user);
            }
            catch(Exception ex)
            {
                DisplayAlert("Error",$"failed to load users {ex.Message}","OK");
            }
        }


        private async void OnAddUserClicked(Object sender , EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                await DisplayAlertAsync("Error","name can't be Empty","Ok");
                return;
            }

            try
            {
                var user = new User { Name = NameEntry.Text.Trim() };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                Users.Add(user);
                NameEntry.Text = "";
                await DisplayAlertAsync("Success", $"User :{user.Name} added successfully","OK");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"{ex.Message}", "Ok");
                return;
            }
        }


        private async void OnDeleteUserClicked(Object sender , EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is User user )
            {

                bool conf = await DisplayAlertAsync("Confirm Delete","Are you sure you want to delete this user ?","Yes","No");

                if (!conf) return;

                try
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    Users.Remove(user);
                    await DisplayAlertAsync("Success", $"User {user.Name} is Deleted", "Ok");

                }
                catch (Exception ex)
                {
                    await DisplayAlertAsync("Error",$"{ex.Message}","Ok");
                    return;
                }
            }

            
        }
    }
}
