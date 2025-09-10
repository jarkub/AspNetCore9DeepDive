using System.Text;

namespace iMonnit8.OOPatterns.OOP_1_Memento
{
	/*
	 * Memento Pattern
	 * 
	 * The Memento Pattern is used to restore and object to a previous state.
	 * 
	 * A common use case for the Memento Pattern is implementing an undo feature. For example, 
	 * most text editors, such as Microsoft (MS) Word, have an undo feature where you can undo things by pressing Ctrl + Z on Windows, or Cmd + Z on Mac.
	 * 
	 * Here are some things you might do in a text editor:
	 *	1. Add a title to the document: "Test Title".
	 *	2. Write some text: "Hello there, my name is Dan.".
	 *	3. Change the title of the document to "The Life of a Developer: My Memoirs".
	 *
	 * A simple way to implement this text editor in code would be to create a single `Editor` class and 
	 * have a field for `title` and `content`, and also have a field that stores each of the previous 
	 * values for each field in some list:
	 * 
	 * <UML>
	 * Editor
	 * ------
	 * title: string
	 * PrevTitles: List<string>
	 * content: string
	 * PrevContent: List<string>
	 * 
	 * Problem:
	 * Every time we add a new field, e.g. author, date, isPublished, we have to keep storing 
	 * lists of previous states (all the changes) for each field. Also, how would we implement the undo 
	 * feature? If the user changed the title then changed the content, then pressed _undo_, the current 
	 * implementation has no knowledge of what the user last did - did the change the title or the content?
	 * 
	 * How about this?
	 * Instead of having multiple fields in this Editor class, we create a separate class 
	 * to store the state of our editor at a given time:
	 * 
	 * <UML>
	 * Editor
	 * ------
	 * title: string
	 * content: string
	 * prevStates: List<EditorState>
	 * 
	 * `Editor` has a `List<EditorState>`.
	 * `Editor` <solid diamond> <solid line> <solid right arrowhead> `EditorState`
	 * 
	 * EditorState
	 * -----------
	 * title: string
	 * content: string
	 * 
	 * (Note the compositiong relationship: `Editor` is composed of, or has a field of, the `EditorState` class).
	 * 
	 * This is a good solution as we can undo multiple times and we don't pollute the `Editor` class 
	 * with too many fields.
	 * 
	 * However, this solution is violating the *Single Responsibility Principle*, as our `Editor` class 
	 * currently has multiple responsibilites:
	 *	1. State management
	 *	2. Providing the features that we need from an editor
	 *
	 * We should take all the state management stuff out of `Editor` and put it somewhere else:
	 * 
	 * <UML>
	 * Editor
	 * ------
	 * title: string
	 * content: string
	 * ------
	 * CreateState(): EditorState
	 * Restore(EditorState): void
	 * 
	 * `Editor` depends on (implements) `EditorState`
	 * `Editor` <dashed line> <solid arrow> `EditorState`
	 * 
	 * EditorState
	 * -----------
	 * title: string
	 * content: string
	 * 
	 * `History` has a `List<EditorState>`
	 * `History` <solid diamond> <solid line> <solid right arrowhoead> `EditorState`
	 * 
	 * History
	 * -------
	 * states: List<EditorState>
	 * editor: Editor
	 * -------
	 * Backup(): void 
	 *	[
	 *		states.Add(editor.createState())
	 *	]
	 * undo(): void
	 *	[
	 *		EditorState last = states.Last()
	 *		states.Remove(last)
	 *		editor.Restore(last)
	 *	]
	 *
	 * The `createState()` method returns an `EditorState` object, hence the dotted line arrow 
	 * (dependency relationship). History has a field with a `List<EditorState>`, hence the 
	 * diamond arrow (composition relationship).
	 * 
	 * This is the Memento pattern. Here are the abstract names that each class has in 
	 * the abstract Memento Pattern:
	 * 
	 * Originator == Editor
	 * Memento == EditorState
	 * Caretaker == History
	 * 
	 * Originator
	 * ----------
	 * content: string
	 * ----------
	 * CreateState(): Memento
	 * Restore(Memento): void
	 * 
	 * Memento
	 * -------
	 * content: string
	 * 
	 * Caretaker
	 * ---------
	 * states: List<Memento>
	 * ---------
	 * Push(Memento): void
	 * Pop(): Memento
	 * 
	 * These abstract names for the classes in the Memento Pattern come from the original 
	 * Gang of Four (GoF) book. Note that our solution differs slightly from the above pattern, as our 
	 * Caretaker class, `History`, also has a field that stores a reference to the `Editor`, so that 
	 * the `History` class can restore the `Editor`'s state when the user clicks _undo_.
	 */

	namespace BadExample
	{
		public class Editor
		{
			private string _content = "";
			private readonly List<string> _prevContents = new List<string>();

			private string _title = "";
			private readonly List<string> _prevTitles = new List<string>();


			public string GetContent()
			{
				return _content;
			}

			public void SetContent(string content)
			{
				_prevContents.Add(_content);
				_content = content;
			}

			public string GetTitle()
			{
				return _title;
			}

			public void SetTitle(string title)
			{
				_prevTitles.Add(_title);
				_title = title;
			}

			public void Undo()
			{
				UndoContent();
				UndoTitle();
			}

			public void UndoContent()
			{
				var lastContent = _prevContents.Last();
				_prevContents.Remove(lastContent);
				_content = lastContent;
			}

			public void UndoTitle()
			{
				var lastTitle = _prevTitles.Last();
				_prevTitles.Remove(lastTitle);
				_title = lastTitle;
			}

		}
	}

	namespace GoodExample
	{
		// Memento
		public class EditorState
		{
			// Editor state data:
			// `readonly` so once created we cannot change it, adding robustness to our code.
			private readonly string _title;
			private readonly string _content;

			// State meta data:
			private readonly DateTime _stateCreatedAt;


			public EditorState(string title, string content)
			{
				_title = title;
				_content = content;
				_stateCreatedAt = DateTime.Now;
			}

			public string GetTitle()
			{
				return _title;
			}

			public string GetContent()
			{
				return _content;
			}

			// The rest of the methods are used by the CareTaker (History) to display meta data:
			public DateTime GetDate()
			{
				return _stateCreatedAt;
			}

			public string GetName()
			{
				// return $"{_stateCreatedAt} / ({_title.Substring(0, 9)})...";
				return $"{_stateCreatedAt} / ({_title})";
			}
		}

		// Originator
		public class Editor
		{
			public string Title { get; set; }
			public string Content { get; set; }

			public EditorState CreateState()
			{
				return new EditorState(Title, Content);
			}

			public void Restore(EditorState state)
			{
				Title = state.GetTitle();
				Content = state.GetContent();
			}
		}

		// Caretaker
		public class History
		{
			private readonly List<EditorState> _states = new List<EditorState>();
			private readonly Editor _editor;

			public History(Editor editor)
			{
				_editor = editor;
			}

			public void Backup()
			{
				_states.Add(_editor.CreateState());
			}

			public void Undo()
			{
				if (_states.Count == 0)
				{
					return;
				}

				EditorState prevState = _states.Last();
				_states.Remove(prevState);

				_editor.Restore(prevState);
			}

			public string ShowHistory()
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\nShowHistory()");
				sb.AppendLine("History: Here's the list of mementos:");

				foreach (var state in _states)
				{
					sb.AppendLine(state.GetName());
				}

				return OOPUtils.PrintThenReturnResultString(sb);
			}
		}
	}

	namespace CounterExample
	{
	}

	namespace GoF
	{
		public class CareTaker
		{
			private readonly List<IMemento> _mementos = new List<IMemento>();

			private readonly Originator _originator;

			public string ConstructorMessage { get; private set; } = string.Empty;

			public CareTaker(Originator originator)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\nCareTaker created");
				sb.AppendLine($"Originator: My Originator is: [{originator}]");

				_originator = originator;

				ConstructorMessage = OOPUtils.PrintThenReturnResultString(sb);
			}

			public string Backup()
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\nCaretaker: Saving Originator's state...");

				_mementos.Add(_originator.Save());

				return OOPUtils.PrintThenReturnResultString(sb);
			}

			public string Undo()
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\nUndo()");
				if (_mementos.Count == 0)
				{
					sb.AppendLine("No History to restore");
					return OOPUtils.PrintThenReturnResultString(sb);
				}

				var memento = _mementos.Last();
				_mementos.Remove(memento);

				sb.AppendLine("Caretaker: Restoring state to: " + memento.GetName());

				try
				{
					_originator.Restore(memento);
					sb.AppendLine("Restore succeeded");
				}
				catch (Exception)
				{
					sb.AppendLine("Restore failed. Undoing...");
					Undo();
					sb.AppendLine("Undo succeeded");
				}

				return OOPUtils.PrintThenReturnResultString(sb);
			}


			public string ShowHistory()
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\nShowHistory()");
				sb.AppendLine("Caretaker: Here's the list of mementos:");

				foreach (var memento in _mementos)
				{
					sb.AppendLine(memento.GetName());
				}

				return OOPUtils.PrintThenReturnResultString(sb);
			}
		}

		public class ConcreteMemento : IMemento
		{
			private readonly string _state;

			private readonly DateTime _date;

			public ConcreteMemento(string state)
			{
				_state = state;
				_date = DateTime.Now;
			}

			// The Originator uses this method when restoring its state.
			public string GetState()
			{
				return _state;
			}

			// The rest of the methods are used by the Caretaker to display
			// metadata.
			public string GetName()
			{
				return $"{_date} / ({_state.Substring(0, 9)})...";
			}

			public DateTime GetDate()
			{
				return _date;
			}
		}

		// The Memento interface provides a way to retrieve the memento's metadata,
		// such as creation date or name. However, it doesn't expose the
		// Originator's state.
		public interface IMemento
		{
			string GetName();

			string GetState();

			DateTime GetDate();
		}

		// The Originator holds some important state that may change over time. It
		// also defines a method for saving the state inside a memento and another
		// method for restoring the state from it.
		public class Originator
		{
			// For the sake of simplicity, the originator's state is stored inside a
			// single variable.
			private string _state;

			public string ConstructorMessage { get; private set; } = string.Empty;

			public Originator(string state)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\nOriginator created");
				sb.AppendLine($"Originator: My initial state is: [{state}]");
				_state = state;
				ConstructorMessage = OOPUtils.PrintThenReturnResultString(sb);
			}

			// The Originator's business logic may affect its internal state.
			// Therefore, the client should backup the state before launching
			// methods of the business logic via the save() method.
			public string DoSomething()
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\nDoSomething()");
				sb.AppendLine("Originator: I'm doing something important.");

				_state = GenerateRandomString(30);

				sb.AppendLine($"Originator: and my state has changed to: {_state}");

				return OOPUtils.PrintThenReturnResultString(sb);
			}

			private string GenerateRandomString(int length = 10)
			{
				string allowedSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
				string result = string.Empty;

				while (length > 0)
				{
					result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];

					Thread.Sleep(12);

					length--;
				}

				return result;
			}

			// Saves the current state inside a memento.
			public IMemento Save()
			{
				return new ConcreteMemento(_state);
			}

			// Restores the Originator's state from a memento object.
			public string Restore(IMemento memento)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("\nRestore()");

				if (!(memento is ConcreteMemento))
				{
					sb.AppendLine("Unknown memento class " + memento.ToString());
					OOPUtils.PrintThenReturnResultString(sb);
					throw new Exception("Unknown memento class " + memento.ToString());
				}

				_state = memento.GetState();
				sb.AppendLine($"Originator: My state has changed to: {_state}");
				return OOPUtils.PrintThenReturnResultString(sb);
			}
		}
	}
}
