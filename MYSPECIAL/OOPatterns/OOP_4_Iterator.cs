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
		public class ShoppingList
		{
			private readonly List<string> _list = new List<string>();



			public void Push(string itemName)
			{
				_list.Add(itemName);
			}

			public string Pop()
			{
				var last = _list.Last();
				_list.Remove(last);
				return last;
			}

			public List<string> GetList()
			{
				return _list;
			}
		}
	}

	namespace GoodExample
	{
		public interface IIterator<T>
		{
			void Next();
			bool HasNext();
			// String Current(); // PROBLEM: what if don't always wanna iterate over strings? E.g. Customers or integers.
			T Current(); // SOLUTION: Use a Generic type
		}

		public class ShoppingList
		{
			private readonly List<string> _list = new List<string>();

			public void Push(string itemName)
			{
				_list.Add(itemName);
			}

			public string Pop()
			{
				var last = _list.Last();
				_list.Remove(last);
				return last;
			}

			public IIterator<String> CreateIterator()
			{
				return new ListIterator(this);
			}

			private class ListIterator : IIterator<String>
			{
				private readonly ShoppingList _shoppingList;
				private int _index;

				public ListIterator(ShoppingList shoppingList)
				{
					_shoppingList = shoppingList;
				}

				public string Current()
				{
					return _shoppingList._list[_index];
				}

				public void Next()
				{
					_index++;
				}

				public bool HasNext()
				{
					return _index < _shoppingList._list.Count;
				}

			}
		}
	}

	namespace CounterExample
	{
	}

	namespace GoF
	{
	}
}
