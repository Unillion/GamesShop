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
                defaultValue: username
            );

            if (dialog.ShowDialog() == true)
            {
                string newUsername = dialog.InputText;
                if (string.IsNullOrWhiteSpace(newUsername))
                {
                    DialogueHelper.ShowMessage("Ошибка", "Поле имени пустое");
                    return username;
                }

                if (UserDatabaseManager.isUserExist(newUsername))
                {
                    DialogueHelper.ShowMessage("Ошибка", "Имя уже занято!");
                    return username;
                }

                if (newUsername.Length <= 3 || newUsername.Length >= 15)
                {
                    DialogueHelper.ShowMessage("Ошибка", "Имя не должно быть меньше 3 и больше 50 символов!");
                    return username;
                }


                 bool success = UserDatabaseManager.UpdateUser(currentUserId, newUsername: newUsername);

                if (success) DialogueHelper.ShowMessage("Успех", "Имя успешно изменено!");
                return newUsername;
            }

            return username;
        }

        public static bool changePassword(int currentUserId)
        {
            var dialog = new InputDialog(
                title: "Смена пароля пользователя",
                prompt: "Введите новый пароль:",
                defaultValue: ""
            );

            if (dialog.ShowDialog() == true)
            {
                string newPassword = dialog.InputText;

                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    DialogueHelper.ShowMessage("Ошибка", "Поле пароля пустое");
                    return false;
                }

                if (newPassword.Length < 6 || newPassword.Length > 15)
                {
                    DialogueHelper.ShowMessage("Ошибка", "Пароль не должен быть меньше 6 и больше 50 символов!");
                    return false;
                }

                bool success = UserDatabaseManager.UpdateUser(currentUserId, newPassword: newPassword);

                if (success)
                {
                    DialogueHelper.ShowMessage("Успех", "Пароль успешно изменён!");
                    return true;
                }
            }

            return false;
        }
    }
}
