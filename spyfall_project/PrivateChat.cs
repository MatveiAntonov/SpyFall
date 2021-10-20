using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
using System.IO;

namespace spyfall_project
{
    class PrivateChat
    {
        // ОБЩЕЕ МЕНЮ ДЛЯ ОТПРАВКИ
        internal static IReplyMarkup GetGeneralMenuForSend()
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            buttons.Add(new InlineKeyboardButton() { Text = "❓ Правила игры", CallbackData = "rules" });
            buttons.Add(new InlineKeyboardButton() { Text = "🌆 Локации", CallbackData = "locations" });
            var menuColomn = new List<InlineKeyboardButton[]>();
            menuColomn.Add(new[] { buttons[0], buttons[1] });

            var menu = new InlineKeyboardMarkup(menuColomn.ToArray());
            return menu;
        }

        // ОБЩЕЕ МЕНЮ ДЛЯ ИЗМЕНЕНИЯ
        internal static InlineKeyboardMarkup GetGeneralMenuForEdit(string data)
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            var menuColomn = new List<InlineKeyboardButton[]>();

            switch (data)
            {
                case "locations":

                    buttons.Add(new InlineKeyboardButton() { Text = "❓ Правила игры", CallbackData = "rules" });
                    buttons.Add(new InlineKeyboardButton() { Text = "⬇️Спрятать⬇️", CallbackData = "back" });
                    menuColomn.Add(new[] { buttons[0], buttons[1] });
                    break;

                case "rules":
                    buttons.Add(new InlineKeyboardButton() { Text = "⬇️Спрятать⬇️", CallbackData = "back" });
                    buttons.Add(new InlineKeyboardButton() { Text = "🌆 Локации", CallbackData = "locations" });
                    menuColomn.Add(new[] { buttons[0], buttons[1] });
                    break;

                default:
                    buttons.Add(new InlineKeyboardButton() { Text = "❓ Правила игры", CallbackData = "rules" });
                    buttons.Add(new InlineKeyboardButton() { Text = "🌆 Локации", CallbackData = "locations" });
                    menuColomn.Add(new[] { buttons[0], buttons[1] });
                    break;
            }
            var menu = new InlineKeyboardMarkup(menuColomn.ToArray());
            return menu;
        }
    }
}
