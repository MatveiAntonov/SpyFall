using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
using System.IO;

namespace spyfall_project
{
    class GroupChat
    {
        internal static int questionCount = 0;

        // КНОПКИ ДЛЯ ВХОДА/ВЫХОДА В/ИЗ ИГРЫ(-Ы)
        internal static InlineKeyboardMarkup GetJoinMenu()
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            buttons.Add(new InlineKeyboardButton() { Text = "✅ Присоединиться", CallbackData = "join" });
            buttons.Add(new InlineKeyboardButton() { Text = "⛔️ Покинуть", CallbackData = "leave" });

            var menuColomn = new List<InlineKeyboardButton[]>();
            menuColomn.Add(new[] { buttons[0], buttons[1] });

            var menu = new InlineKeyboardMarkup(menuColomn.ToArray());
            return menu;
        }

        // КНОПКА ДЛЯ ВОПРОСОВ ИГРОКУ
        internal static InlineKeyboardMarkup GetAskMenu(string name)
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            buttons.Add(new InlineKeyboardButton() { Text = $"❓ Задать вопрос игроку {name}", CallbackData = "ask" });

            var menuColomn = new List<InlineKeyboardButton[]>();
            menuColomn.Add(new[] { buttons[0] });

            var menu = new InlineKeyboardMarkup(menuColomn.ToArray());
            return menu;
        }

        // КНОПКА ДЛЯ ОТВЕТА ИГРОКУ
        internal static IReplyMarkup GetAnswerMenu()
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            buttons.Add(new InlineKeyboardButton() { Text = $"❗️ Ответить игроку {secondaryMethods.userNicks.ElementAt(questionCount).Key}", CallbackData = "answer" });
            var menuColomn = new List<InlineKeyboardButton[]>();
            menuColomn.Add(new[] { buttons[0] });

            var menu = new InlineKeyboardMarkup(menuColomn.ToArray());
            return menu;
        }

        // ИЗМЕНИНИЯ ДЛЯ ГОЛОСОВАНИЯ
        internal static InlineKeyboardMarkup VoteForEdit()
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            var menuColomn = new List<InlineKeyboardButton[]>();
            for (int i = 0; i < secondaryMethods.playerCount; i++)
            {
                buttons.Add(new InlineKeyboardButton() { Text = $"📢 {secondaryMethods.userNicks.ElementAt(i).Key}", CallbackData = $"{secondaryMethods.userNicks.ElementAt(i).Key}" });
                menuColomn.Add(new[] { buttons[i] });
            }

            var menu = new InlineKeyboardMarkup(menuColomn.ToArray());
            return menu;
        }

        // ГОЛОСОВАНИЕ
        internal static IReplyMarkup VoteForSend()
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            var menuColomn = new List<InlineKeyboardButton[]>();
            for (int i = 0; i < secondaryMethods.playerCount; i++)
            {
                buttons.Add(new InlineKeyboardButton() { Text = $"📢 {secondaryMethods.userNicks.ElementAt(i).Key}", CallbackData = $"{secondaryMethods.userNicks.ElementAt(i).Key}" });
                menuColomn.Add(new[] { buttons[i] });
            }

            var menu = new InlineKeyboardMarkup(menuColomn.ToArray());
            return menu;
        }
    }
}
