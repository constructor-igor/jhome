// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Microsoft.BotBuilderSamples.Bots
{
    /*
        file attachments sample: https://habr.com/ru/company/microsoft/blog/498224/
        bots
            @zmanim_bot
    */
    public class EchoBot : ActivityHandler
    {
        private ConversationState m_conversationState;
        private UserState m_userState;
        public EchoBot(ConversationState conversationState, UserState userState)
        {
            m_conversationState = conversationState;
            m_userState = userState;
        }
        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occurred during the turn.
            await m_conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await m_userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string message = turnContext.Activity.Text;
            bool isCancel = message.ToLower() == "cancel";

            var conversationStateAccessors = m_conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

            var userStateAccessors = m_userState.CreateProperty<UserProfile>(nameof(UserProfile));
            var userProfile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());  

            switch (userProfile.DialogState){
                case "gematria":
                    if (isCancel){
                        userProfile.DialogState = "";
                        await SendSuggestedActionsAsync("Main menu", turnContext, cancellationToken);
                    } else{
                        GematriaCalculator calculator = new GematriaCalculator();
                        int gematriaValue = calculator.Calculate(message);
                        var replyText = $"Standard Gematria of '{message}' = gematriaValue";
                        replyText = replyText.Replace("gematriaValue", "\u200e"+gematriaValue.ToString());
                        await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
                        await SendCancelActionsAsync("Input text for gematria", turnContext, cancellationToken);
                    }
                    break;
                default:
                    switch (message.ToLower()){
                        case "gematria":
                            userProfile.DialogState = "gematria";
                            string replyText = "Input text for gematria";
                            // await turnContext.SendActivityAsync(MessageFactory.Text($"Input text for gematria", $"Input text for gematria"), cancellationToken);
                            await SendCancelActionsAsync(replyText, turnContext, cancellationToken);
                        break;
                        case "today":
                            //https://stackoverflow.com/questions/6245546/how-can-i-get-todays-jewish-date-in-c
                            DateTime today = DateTime.Now;
                            HebrewCalenderManager manager = new HebrewCalenderManager();
                            string hebrewDate = manager.GetHebrewDateAsString(today);
                            await turnContext.SendActivityAsync(MessageFactory.Text(hebrewDate), cancellationToken);
                            await SendSuggestedActionsAsync("Main menu", turnContext, cancellationToken);
                        break;
                        case "/help":
                        case "/commands":
                        case "/start":
                            await SendSuggestedActionsAsync("Commands", turnContext, cancellationToken);
                            break;
                        default:
                            var localTime =  turnContext.Activity.LocalTimestamp;
                            string localTimeStr = localTime.ToString();
                            var defaultReplyText = $"Echo: {message} ({localTimeStr})";
                            await turnContext.SendActivityAsync(MessageFactory.Text(defaultReplyText), cancellationToken);
                            // await SendSuggestedActionsAsync(turnContext, cancellationToken);
                            break;
                    }
                    break;
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
        // Creates and sends an activity with suggested actions to the user. When the user
        /// clicks one of the buttons the text value from the "CardAction" will be
        /// displayed in the channel just as if the user entered the text. There are multiple
        /// "ActionTypes" that may be used for different situations.
        private static async Task SendSuggestedActionsAsync(string text, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text(text);
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction() { Title = "Gematria", Type = ActionTypes.ImBack, Value = "Gematria" },
                    new CardAction() { Title = "Today", Type = ActionTypes.ImBack, Value = "Today" },
                    // new CardAction() { Title = "Gematria", Type = ActionTypes.ImBack, Value = "Gematria", Image = "https://via.placeholder.com/20/FF0000?text=R", ImageAltText = "R" },
                    // new CardAction() { Title = "Today", Type = ActionTypes.ImBack, Value = "Today", Image = "https://via.placeholder.com/20/FFFF00?text=Y", ImageAltText = "Y" },
                },
            };
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
        private static async Task SendCancelActionsAsync(string text, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text(text);
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction() { Title = "Cancel", Type = ActionTypes.ImBack, Value = "Cancel" },
                },
            };
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
    }
}
