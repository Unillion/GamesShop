using GamesShop.content.db;
using GamesShop.content.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GamesShop.content.GUI.GUI_services
{
    class ProfileService
    {
        private readonly string username;
        private readonly int userId;

        public TextBlock ProfileUsernameText {  get; set; }
        public TextBlock ProfileEmailText { get; set; }
        public TextBlock ProfileBalanceText { get; set; }
        public TextBlock ProfileRegistrationDate { get; set; }
        public TextBlock GamesInLibraryCount { get; set; }
        public TextBlock TotalSpentAmount { get; set; }
        public TextBlock TotalIncomeAmount { get; set; }
        public TextBlock ReviewsWrittenCount { get; set; }
        public TextBlock NameChanges { get; set; }
        public TextBlock PasswordChanges { get; set; }

        public ProfileService(string username, int userId)
        {
            this.username = username;
            this.userId = userId;
        }

        public void LoadProfileInformation()
        {
            var user = UserDatabaseManager.GetUserById(userId);
            var statistics = UserDatabaseManager.GetUserStatistics(userId);
            ProfileBalanceText.Text = user.Username;
            ProfileEmailText.Text = user.Email;
            ProfileRegistrationDate.Text = user.RegistrarionDate.ToString();
            ProfileBalanceText.Text = user.Balance.ToString() + "₽";
            GamesInLibraryCount.Text = statistics.TotalGamesPurchased.ToString();
            TotalSpentAmount.Text = statistics.TotalMoneySpent.ToString() + "₽";
            TotalIncomeAmount.Text = statistics.TotalIncomeAmount.ToString() + "₽";
            ReviewsWrittenCount.Text = statistics.ReviewsWritten.ToString();
            NameChanges.Text = statistics.NameChanges.ToString();
            PasswordChanges.Text = statistics.PasswordChanges.ToString();
        }

    }
}
