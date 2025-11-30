using GamesShop.content.db;
using GamesShop.dialogueUserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GamesShop.content.utilities
{
    public class UserManager
    {
        public static string changeUsername(int currentUserId, string username)
        {
            var dialog = new InputDialog(
                title: "Смена имени пользователя",
                prompt: "Введите новое имя пользователя:",
                defaultValue: username,
                validationFunc: newUsername =>
                    !string.IsNullOrWhiteSpace(newUsername) &&
                    newUsername.Length >= 3 &&
                    newUsername.Length <= 50 &&
                    !UserDatabaseManager.isUserExist(newUsername) 
            );

            if (dialog.ShowDialog() == true)
            {
                string newUsername = dialog.InputText;
                bool success = UserDatabaseManager.UpdateUser(currentUserId, newUsername: newUsername);

                if (success) DialogueHelper.ShowMessage("Успех", "Имя успешно изменено!");
                return newUsername;
            }

            return username;
        }
    }
}
