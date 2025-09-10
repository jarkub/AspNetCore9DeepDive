namespace iMonnit8.OOPatterns
{
	/*
	 * Command Pattern
	 * 
	 * The Command Pattern is a behavioral design pattern that encapsulates a request as an object, allowing
	 * you to parameterize clients with queues, requests, or operations. It enables you to decouple the sender from the receiver, providing flexibility in the execution of commands and supporting undoable operations.
	 */

	namespace BadExample
	{
		public class Document
		{
			public DocumentStates State { get; set; }
			public UserRoles CurrentUserRole { get; set; }

			public void Publish()
			{
				if (State == DocumentStates.DRAFT)
				{
					State = DocumentStates.MODERATION;
				}
				else if (State == DocumentStates.MODERATION)
				{
					if (CurrentUserRole == UserRoles.ADMIN)
					{
						State = DocumentStates.PUBLISHED;
					}
				}
				else if (State == DocumentStates.PUBLISHED)
				{
					// do nothing
				}
			}
		}

		public enum DocumentStates
		{
			DRAFT,
			MODERATION,
			PUBLISHED
		}

		public enum UserRoles
		{
			READER,
			EDITOR,
			ADMIN
		}
	}

	namespace GoodExample
	{
		public class Document
		{
			public State State { get; set; }
			public UserRoles CurrentUserRole { get; set; }

			public Document(UserRoles currentUserRole)
			{
				State = new DraftState(this); // New documents have draft state by default
				CurrentUserRole = currentUserRole;
			}

			public void Publish()
			{
				State.Publish();
			}
		}

		public class DraftState : State
		{
			private readonly Document _document;

			public DraftState(Document document)
			{
				_document = document;
			}

			public void Publish()
			{
				_document.State = new ModerationState(_document);
			}
		}

		public class ModerationState : State
		{
			private readonly Document _document;

			public ModerationState(Document document)
			{
				_document = document;
			}

			public void Publish()
			{
				if (_document.CurrentUserRole == UserRoles.ADMIN)
				{
					_document.State = new PublishedState(_document);
				}
			}
		}

		public class PublishedState : State
		{
			private readonly Document _document;

			public PublishedState(Document document)
			{
				_document = document;
			}

			public void Publish()
			{
				// do nothing
			}
		}

		public interface State
		{
			void Publish();

			// A real Document would have more state-dependent methods, such as Render() -- but we'll keep it simple with one method for this example.
		}

		public enum UserRoles
		{
			READER,
			EDITOR,
			ADMIN
		}
	}

	namespace CounterExample
	{
		public class Stopwatch
		{
			private bool IsRunning { get; set; } = false;

			public void click()
			{
				if (IsRunning)
				{
					IsRunning = false;
					System.Console.WriteLine("Stopped");
				}
				else
				{
					IsRunning = true;
					System.Console.WriteLine("Running");
				}
			}
		}
	}

	namespace GoF
	{
	}
}
