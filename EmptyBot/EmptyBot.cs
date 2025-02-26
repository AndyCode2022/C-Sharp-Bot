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
		// A dictionary to store FAQs
		private readonly Dictionary<string, string> _faqDictionary;

		// State Accessor for storing `value`
		private readonly IStatePropertyAccessor<int> _valueAccessor;
		private readonly ConversationState _conversationState;

		public EmptyBot(ConversationState conversationState)
		{
			_conversationState = conversationState;
			_valueAccessor = _conversationState.CreateProperty<int>("Value");

			// Initialize FAQs
			_faqDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				{ "hours", "Our hours are 9 AM to 5 PM, Monday to Friday." },
				{ "location", "We are located at 123 Main Street, Anytown." },
				{ "contact", "You can contact us at contact@ourcompany.com." },
				{ "service", "We provide top-notch cloud solutions for your every need." },
				{ "price", "Our prices vary depending on the service. Please visit our pricing page for more details." },
				{ "options", "We have compute, calculate, and add." }
			};
		}

		// Handle user joining chat
		protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			foreach (var member in membersAdded)
			{
				if (member.Id != turnContext.Activity.Recipient.Id)
				{
					var welcomeMessage = "Hello! I am a virtual assistant here to guide you. " +
										 "You can ask me about our hours, location, contact, services, and pricing. " +
										 "I can do tasks as well! such as compute, calculate, and add.";
					await turnContext.SendActivityAsync(MessageFactory.Text(welcomeMessage), cancellationToken);
				}
			}
		}

		// Handle incoming messages
		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			var userQuestion = turnContext.Activity.Text.Trim().ToLower();
			Console.WriteLine($"User asked: {userQuestion}");

			// Get the persisted value from state
			var value = await _valueAccessor.GetAsync(turnContext, () => 1, cancellationToken);

			var response = await ProcessUserInput(userQuestion, turnContext, value, cancellationToken);
			await turnContext.SendActivityAsync(MessageFactory.Text(response), cancellationToken);
		}

		// Process user input and return appropriate response
		private async Task<string> ProcessUserInput(string input, ITurnContext turnContext, int value, CancellationToken cancellationToken)
		{
			// Check FAQs
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

			// Handle compute tasks
			if (IsComputeTask(input))
			{
				return await HandleComputeTask(input, turnContext, value, cancellationToken);
			}

			return "I'm sorry, I don't have an answer to that question. Could you rephrase or ask about something else?";
		}

		// Check for small talk
		private bool IsSmallTalk(string input)
		{
			return input.Contains("hello") || input.Contains("hi") || input.Contains("how are you") || input.Contains("thanks");
		}

		// Check for computational tasks
		private bool IsComputeTask(string input)
		{
			return input.Contains("compute") || input.Contains("calculate") || input.Contains("add");
		}

		// Handle small talk
		private string HandleSmallTalk(string input)
		{
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

		// Handle compute tasks
		private async Task<string> HandleComputeTask(string input, ITurnContext turnContext, int value, CancellationToken cancellationToken)
		{
			if (input.Contains("compute"))
			{
				return "Your task has been calculated! " + value;
			}
			if (input.Contains("calculate"))
			{
				return "Calculated value: " + value;
			}
			if (input.Contains("add"))
			{
				value++; // Persist the incremented value

				// Save the updated value in state
				await _valueAccessor.SetAsync(turnContext, value, cancellationToken);
				await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);

				return "Combined value: " + value;
			}
			return string.Empty;
		}
	}
}
