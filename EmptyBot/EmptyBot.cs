// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.22.0

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmptyBot
{
    public class EmptyBot : ActivityHandler
    {

        // A dictionary to store FAQs and their corresponding answers.
        private readonly Dictionary<string, string> _faqDictionary;

        public EmptyBot()
        {
          
            // Initialize the FAQ dictionary with multiple possible questions and answers.
            _faqDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "hours", "Our hours are 9 AM to 5 PM, Monday to Friday." },
            { "location", "We are located at 123 Main Street, Anytown." },
            { "contact", "You can contact us at contact@ourcompany.com." },
            { "service", "We provide top-notch cloud solutions for your every need." },
            { "price", "Our prices vary depending on the service. Please visit our pricing page for more details." }

            };
        }

		// Send a welcome message when the user joins the chat.
		protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			foreach (var member in membersAdded)
			{
				// Greet only the user, not the bot itself.
				if (member.Id != turnContext.Activity.Recipient.Id)
				{
					var welcomeMessage = "Hello! I am a virtual assistant here to guide you. " +
										 "You can ask me about our hours, location, contact, services, and pricing.";
					await turnContext.SendActivityAsync(MessageFactory.Text(welcomeMessage), cancellationToken);
				}
			}
		}

		// This method handles incoming messages and processes them accordingly.
		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var userQuestion = turnContext.Activity.Text.Trim().ToLower();

            // Log the user interaction (you could replace this with actual logging to a file or database).
            Console.WriteLine($"User asked: {userQuestion}");

            var response = ProcessUserInput(userQuestion);

            await turnContext.SendActivityAsync(MessageFactory.Text(response), cancellationToken);
        }

        // This method processes the user's input and returns the appropriate response.
        private string ProcessUserInput(string input)
        {
            // Check if the input matches any FAQ
            foreach (var faq in _faqDictionary)
            {
                if (input.Contains(faq.Key))
                {
                    return faq.Value;
                }
            }

            // Handle small talk
            if (IsSmallTalk(input))
            {
                return HandleSmallTalk(input);
            }

            // Fallback response if no FAQ matches
            return "I'm sorry, I don't have an answer to that question. Could you rephrase or ask about something else?";
        }

        // This method checks if the input is related to small talk.
        private bool IsSmallTalk(string input)
        {
            // Simple check for small talk phrases
            return input.Contains("hello") || input.Contains("hi") || input.Contains("how are you") || input.Contains("thanks");
        }

        // This method handles small talk responses.
        private string HandleSmallTalk(string input)
        {
            // Respond to small talk
            if (input.Contains("hello") || input.Contains("hi"))
            {
                return "Hello! How can I assist you today?";
            }
            if (input.Contains("how are you"))
            {
                return "I'm just a bot, but I'm here to help you!";
            }
            if (input.Contains("thanks") || input.Contains("thank you"))
            {
                return "You're welcome! If you have any more questions, feel free to ask.";
            }

            return string.Empty;
        }
    }
}