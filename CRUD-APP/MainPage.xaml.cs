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



        private async void OnEditUserCilcked(Object sender , EventArgs e)
        {
            if(sender is Button button && button.CommandParameter is User user)
            {
                string newName = await DisplayPromptAsync("Edit User","Enter new Name :",initialValue:user.Name);

                if (string.IsNullOrWhiteSpace(newName))
                {
                    await DisplayAlertAsync("Edit User","Name can not be empty","Ok");
                    return;
                }

                try
                {
                    user.Name = newName.Trim();
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    var index = Users.IndexOf(user);
                    if (index!=-1)
                    {
                        Users[index] = user;

                    }
                    await DisplayAlertAsync("Success","User updated successfully","Ok");
                }
                catch (Exception ex)
                {
                    await DisplayAlertAsync("Error",$"Failed to Update user :{ex.Message}","Ok");
                }
            }
        }
    }
}
