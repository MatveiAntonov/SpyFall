using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Linq;
using Telegram.Bot.Types.Enums;


namespace spyfall_project
{

    class SpyfallProject 
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Сервер работает корректно.");
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            client.OnCallbackQuery += OnCallbackQueryHandler;
            Console.ReadKey();
            client.StopReceiving();
        }

        private static string token { get; set; } = "1824837962:AAH8MKU0j16me3DtBkO5fVHyCzZd4xjnRgs";
        private static bool IsStarted;
        private static Dictionary<string, int> voteNicks = new Dictionary<string, int>();
        private static Dictionary<int, string> alreadyVoted = new Dictionary<int, string>();
        private static string spy;
        private static string location;
        private static bool isLast = false;
        private static string messageText = $"‼️ НОВАЯ МИССИЯ ‼️\nНеобходимо минимум 3 человека для начала:\n\n✅ {secondaryMethods.playerCount} в игре:\n\n";
        private static string voteText = "Круг закончен, но шпиона так и не обнаружили...У нас есть последний шанс это сделать!\n\n"; 
        private static TelegramBotClient client;

        // --ОТВЕТ НА НАЖАТИЕ КНОПОК
        private static async void OnCallbackQueryHandler(object sender, CallbackQueryEventArgs e)
        { 

            switch (e.CallbackQuery.Data)
            {
                case "join":
                    if (!IsStarted)
                    {
                        bool is_activated = true;
                        if (!secondaryMethods.userNicks.ContainsKey(e.CallbackQuery.From.FirstName))
                        {

                            try
                            {

                                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, $"🔎 АГЕНТСТВО SPYFALL 🔍\n\n✅ Вы участвуете в миссии с кодовым названием \"{e.CallbackQuery.Message.Chat.Title}\". Будьте готовы!");

                            }
                            catch
                            {
                                is_activated = false;

                            }

                            if (is_activated)
                            {
                                secondaryMethods.playerCount += 1;
                                secondaryMethods.userNicks.Add(e.CallbackQuery.From.FirstName, e.CallbackQuery.From.Id);
                            }
                            else
                            {
                                await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"@{e.CallbackQuery.From.Username}, для начала активируйте меня в [личных сообщениях](http://telegram.me/spyfall_2_bot)!", ParseMode.Markdown);
                            }


                            messageText = $"‼️ НОВАЯ МИССИЯ ‼️\nНеобходимо минимум 3 человека для начала:\n\n✅ {secondaryMethods.playerCount} в игре:\n\n";
                            foreach (string nick in secondaryMethods.userNicks.Keys)
                            {
                               

                                  messageText += nick + "\n";
                            }
                            try
                            {
                                await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, messageText, replyMarkup: GroupChat.GetJoinMenu());
                            }
                            catch
                            { }

                            if (secondaryMethods.playerCount >= 1)    // КОЛИЧЕСТВО!!
                            {
                                messageText = $"‼️ НОВАЯ МИССИЯ ‼️\nНажмите /startgame чтобы начать\n\n✅ {secondaryMethods.playerCount} в игре:\n\n";
                                foreach (string nick in secondaryMethods.userNicks.Keys)
                                {
                                    messageText += nick + "\n";
                                }
                                await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, messageText, replyMarkup: GroupChat.GetJoinMenu());
                            }
                        }
                    }
                    break;

                case "leave":
                    if(!IsStarted)
                    {
                        if (secondaryMethods.userNicks.ContainsKey(e.CallbackQuery.From.FirstName))
                        {
                            secondaryMethods.userNicks.Remove(e.CallbackQuery.From.FirstName);
                            secondaryMethods.playerCount -= 1;
                            GroupChat.questionCount -= 1;
                            messageText = $"‼️ НОВАЯ МИССИЯ ‼️\nНеобходимо минимум 3 человека для начала:\n\n✅ {secondaryMethods.playerCount} в игре:\n\n";
                            foreach (string nick in secondaryMethods.userNicks.Keys)
                            {
                                messageText += nick + "\n";
                            }
                            await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, messageText, replyMarkup: GroupChat.GetJoinMenu());
                            await client.SendTextMessageAsync(e.CallbackQuery.From.Id, $"🔎 АГЕНТСТВО SPYFALL 🔍\n\n⛔️ Вы покинули миссии с кодовым названием \"{e.CallbackQuery.Message.Chat.Title}\"");
                        }
                    }
                    break;

                case "rules":
                    await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, secondaryMethods.readTextFile(secondaryMethods.rulesPath), replyMarkup: PrivateChat.GetGeneralMenuForEdit(e.CallbackQuery.Data));
                    break;

                case "back":
                    await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, "🔎 АГЕНТСТВО SPYFALL 🔍\n\nПриветствую Вас!\n\nДобавьте меня в группу, чтобы начать в ней игру, если вы этого не сделали!", replyMarkup: PrivateChat.GetGeneralMenuForEdit(e.CallbackQuery.Data));
                    break;

                case "ask":
                    if (e.CallbackQuery.From.FirstName == secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key)
                    {
                        if (!isLast)
                        {
                            await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Вопрос был задан от {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key} к {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount + 1).Key}\n\nИгрок {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount + 1).Key}, после ответа на вопрос от игрока {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key} нажмите кнопку \"❗️ Ответить игроку {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key}\"", replyMarkup: GroupChat.GetAnswerMenu());
                        }
                        else
                        {
                            await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Вопрос был задан от {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key} к {secondaryMethods.userNicks.ElementAt(0).Key}\n\nИгрок {secondaryMethods.userNicks.ElementAt(0).Key}, после ответа на вопрос от игрока {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key} нажмите кнопку \"❗️ Ответить игроку {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key}\"", replyMarkup: GroupChat.GetAnswerMenu());
                        }
                    }
                    break;

                case "answer":
                    if (!isLast)
                    {
                        if (e.CallbackQuery.From.FirstName == secondaryMethods.userNicks.ElementAt(GroupChat.questionCount + 1).Key)
                        {
                            GroupChat.questionCount += 1;
                            if (GroupChat.questionCount == secondaryMethods.playerCount - 1)
                            {
                                isLast = true;
                                await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Отлично! Последний вопрос задаёт {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key}, обращаясь к {secondaryMethods.userNicks.ElementAt(0).Key}.", replyMarkup: GroupChat.GetAskMenu(secondaryMethods.userNicks.ElementAt(0).Key));
                            }
                            else
                            {
                                await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Следующий вопрос задаёт {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key}, обращаясь к {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount + 1).Key}.", replyMarkup: GroupChat.GetAskMenu(secondaryMethods.userNicks.ElementAt(GroupChat.questionCount + 1).Key));
                            }
                        }
                    }
                    else
                    {
                        if (e.CallbackQuery.From.FirstName == secondaryMethods.userNicks.ElementAt(0).Key)
                        {
                            messageText = $"Круг закончен, но шпиона так и не обнаружили...У нас есть последний шанс это сделать!\n\n";
                            await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, messageText, replyMarkup: GroupChat.VoteForSend());
                        }
                            

                    }
                    break;

                case "locations":
                    try
                    {
                        messageText = "🔎 АГЕСТВО SPYFALL 🔍\nБАЗА ДАННЫХ ОПЕРАЦИЙ\n\n";
                        messageText += secondaryMethods.readTextFile(secondaryMethods.locationPath);
                        await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, messageText, replyMarkup: PrivateChat.GetGeneralMenuForEdit(e.CallbackQuery.Data)); 
                    }
                    catch
                    { }
                    break;

                default:

                    voteText = "Круг закончен, время вычислять предателя...У нас только один шанс это сделать" +
                        "" +
                        "!\n\n";
                    if (!alreadyVoted.ContainsKey(e.CallbackQuery.From.Id))
                    {
                        alreadyVoted.Add(e.CallbackQuery.From.Id, e.CallbackQuery.Data);
                        if (!voteNicks.ContainsKey(e.CallbackQuery.Data))
                        {
                            voteNicks.Add(e.CallbackQuery.Data, 1);
                            foreach (KeyValuePair<string, int> vote in voteNicks)
                                voteText += vote.Key + "-----" + vote.Value + "\n";
                            try
                            {
                                await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, voteText, replyMarkup: GroupChat.VoteForEdit());
                            }
                            catch { }
                        }
                        else
                        {
                            voteNicks[e.CallbackQuery.Data]++;
                            foreach (KeyValuePair<string, int> vote in voteNicks)
                                voteText += vote.Key + "-----" + vote.Value + "\n";
                            try
                            {
                                await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, voteText, replyMarkup: GroupChat.VoteForEdit());
                            }
                            catch { }
                        }
                    }
                    else
                    { 
                        if (e.CallbackQuery.Data != alreadyVoted[e.CallbackQuery.From.Id])
                        {
                            voteNicks[alreadyVoted[e.CallbackQuery.From.Id]]--;
                            if (voteNicks[alreadyVoted[e.CallbackQuery.From.Id]] == 0)
                                voteNicks.Remove(alreadyVoted[e.CallbackQuery.From.Id]);
                            alreadyVoted[e.CallbackQuery.From.Id] = e.CallbackQuery.Data;
                            if (!voteNicks.ContainsKey(e.CallbackQuery.Data))
                            {
                                voteNicks.Add(e.CallbackQuery.Data, 1);
                                foreach (KeyValuePair<string, int> vote in voteNicks)
                                    voteText += vote.Key + "-----" + vote.Value + "\n";
                                try
                                {
                                    await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, voteText, replyMarkup: GroupChat.VoteForEdit());
                                }
                                catch { }
                            }
                            else
                            {
                                voteNicks[e.CallbackQuery.Data]++;
                                foreach (KeyValuePair<string, int> vote in voteNicks)
                                    voteText += vote.Key + "-----" + vote.Value + "\n";
                                try
                                {
                                    await client.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, voteText, replyMarkup: GroupChat.VoteForEdit());
                                }
                                catch { }
                            }
                        }
                        
                    }

                    if (alreadyVoted.Count == secondaryMethods.userNicks.Count)
                    {
                        voteNicks = voteNicks.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                        if (voteNicks.ElementAt(0).Value == voteNicks.ElementAt(voteNicks.Count - 1).Value & voteNicks.Count != 1)
                        {
                            await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Мнения разделились! Придётся кого-то переубедить...Дерзайте!");
                        }
                        else
                        {
                            await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"Наиболее подозрительным игроки посчитали...{voteNicks.ElementAt(voteNicks.Count - 1).Key}! Так ли это?");
                            for (int i = 3; i >= 1; i--)
                            {
                                await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"{i}");
                                System.Threading.Thread.Sleep(500);

                            }
                            if (voteNicks.ElementAt(voteNicks.Count - 1).Key == spy)
                                await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"ДА! {spy}, сегодня не твой день. Проваливай!\n\nВведите команду \"/start@spyfall_2_bot\", чтобы начать новую игру!");
                            else
                                await client.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, $"НЕТ! {spy} обыграл каждого. Жаль, что следующего раза уже не будет.\n\nВведите команду \"/start@spyfall_2_bot\", чтобы начать новую игру!");
                        }

                    }

                    break;
            }
        }

        // ОТВЕТ НА СООБЩЕНИЯ
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg.Text != null)
            {
                switch (msg.Text)
                {
                    case "/start@spyfall_2_bot":
                        Console.WriteLine($"{DateTime.Now} --- Пользователь: { msg.From.FirstName}; Сообщение: {msg.Text}");
                        IsStarted = false;
                        isLast = false;
                        voteNicks.Clear();
                        alreadyVoted.Clear();
                        GroupChat.questionCount = 0;
                        secondaryMethods.playerCount = 0;
                        secondaryMethods.userNicks.Clear();
                        await client.SendTextMessageAsync(msg.Chat.Id, $"‼️ НОВАЯ МИССИЯ ‼️\nНеобходимо минимум 3 человека для начала:\n\n✅  {secondaryMethods.playerCount} в игре:\n\n", replyMarkup: GroupChat.GetJoinMenu());
                        break;
      
                    case "/start":
                        Console.WriteLine($"{DateTime.Now} --- Пользователь: { msg.From.FirstName}; Сообщение: {msg.Text}");
                        if (e.Message.Chat.Id < 0 & e.Message.Chat.Title != null)
                            await client.SendTextMessageAsync(msg.Chat.Id, "Команда не распознана!");
                        else
                            await client.SendTextMessageAsync(msg.Chat.Id, "🔎 АГЕНТСТВО SPYFALL 🔍\n\nПриветствую Вас!\n\nДобавьте меня в группу, чтобы начать новую игру, если вы этого еще не сделали!", replyMarkup: PrivateChat.GetGeneralMenuForSend());
                        break;

                    case "/startgame@spyfall_2_bot":
                        Console.WriteLine($"{DateTime.Now} --- Пользователь: { msg.From.FirstName}; Сообщение: {msg.Text}");
                        if (!IsStarted)
                        {
                            IsStarted = true;
                            messageText = $"✅ МИССИЯ НАЧАЛАСЬ  ✅\n\n";

                            foreach (string nick in secondaryMethods.userNicks.Keys)
                            {
                                messageText += nick + ", ";
                            }
                            messageText = messageText.Remove(messageText.Length - 2) + " проверьте личные сообщения.";
                            messageText += $"\n{secondaryMethods.userNicks.ElementAt(GroupChat.questionCount).Key}, задайте вопрос для {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount + 1).Key} в чат, затем нажмите кнопку \"❓ Задать вопрос игроку {secondaryMethods.userNicks.ElementAt(GroupChat.questionCount + 1).Key}\".";
                            await client.SendTextMessageAsync(msg.Chat.Id, messageText, replyMarkup: GroupChat.GetAskMenu(secondaryMethods.userNicks.ElementAt(GroupChat.questionCount + 1).Key));
                            messageText = $"🔎 АГЕНСТВО SPYFALL 🔍\n\n📄 Миссия  \"{e.Message.Chat.Title}\"";
                            location = secondaryMethods.randomLocation();
                            spy = secondaryMethods.randomSpy();
                            foreach (KeyValuePair<string, int> val in secondaryMethods.userNicks)
                            {
                                messageText = $"🔎 АГЕНСТВО SPYFALL 🔍\n\n📄 Миссия: \"{e.Message.Chat.Title}\"\n";
                                if (val.Key == spy)
                                {
                                    messageText += $"🕶 Шпион.\n📍 Локация: Засекречено";
                                }
                                else
                                {
                                    messageText += $"🕴 Агент\n📍 Локация: {location}";
                                }
                                await client.SendTextMessageAsync(val.Value, messageText);

                            }
                        }
                        break;

                    default:
                        Console.WriteLine($"{DateTime.Now} --- Пользователь: { msg.From.FirstName}; Сообщение: {msg.Text}");
                        await client.SendTextMessageAsync(msg.Chat.Id, "Такая команда мне неизвестна!");
                        break;
                }                    
            }
        }  
    }
}
