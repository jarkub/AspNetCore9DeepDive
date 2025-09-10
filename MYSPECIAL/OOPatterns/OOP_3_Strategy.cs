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
		public enum Compressors
		{
			MOV,
			MP4,
			WEBM
		}

		public enum Overlays
		{
			NONE,
			BLACK_AND_WHITE,
			BLUR
		}

		public class VideoStorage
		{
			private Compressors _compressor;
			private Overlays _overlay;

			public VideoStorage(Compressors compressor, Overlays overlay = Overlays.NONE)
			{
				_compressor = compressor;
				_overlay = overlay;
			}

			public void SetCompressor(Compressors compressor)
			{
				_compressor = compressor;
			}

			public void SetOverlay(Overlays overlay)
			{
				_overlay = overlay;
			}

			public void Store(string fileName)
			{
				if (_compressor == Compressors.MOV)
				{
					System.Console.WriteLine("Compressing using MOV");
				}
				else if (_compressor == Compressors.MP4)
				{
					System.Console.WriteLine("Compressing using MP4");
				}
				else if (_compressor == Compressors.WEBM)
				{
					System.Console.WriteLine("Compressing using WEBM");
				}

				if (_overlay == Overlays.BLACK_AND_WHITE)
				{
					System.Console.WriteLine("Applying black and white overlay");
				}
				else if (_overlay == Overlays.BLUR)
				{
					System.Console.WriteLine("Applying blur overlay");
				}
				else if (_overlay == Overlays.NONE)
				{
					System.Console.WriteLine("Not applying an overlay");
				}

				System.Console.WriteLine("Storing video to " + fileName);
			}
		}
	}

	namespace GoodExample
	{
		public class CompressorMOV : ICompressor
		{
			public void Compress()
			{
				System.Console.WriteLine("Compressing video using MOV");
			}
		}

		public class CompressorMP4 : ICompressor
		{
			public void Compress()
			{
				System.Console.WriteLine("Compressing video using MP4");
			}
		}

		public class CompressorWebM : ICompressor
		{
			public void Compress()
			{
				System.Console.WriteLine("Compressing video using WebM");
			}
		}

		public interface ICompressor
		{
			void Compress();
		}

		public interface IOverlay
		{
			void Apply();
		}

		public class OverlayBlackAndWhite : IOverlay
		{
			public void Apply()
			{
				System.Console.WriteLine("Applying black and white overlay");
			}
		}

		public class OverlayBlur : IOverlay
		{
			public void Apply()
			{
				System.Console.WriteLine("Applying blur overlay");
			}
		}

		public class OverlayNone : IOverlay
		{
			public void Apply()
			{
				System.Console.WriteLine("Not applying an overlay");
			}
		}

		public class VideoStorage
		{
			// Store references to the strategies, coding to interfaces for polymorphism/flexibility
			private ICompressor _compressor;
			private IOverlay _overlay;

			// It's common to pass the strategies via constructor
			public VideoStorage(ICompressor compressor, IOverlay overlay)
			{
				_compressor = compressor;
				_overlay = overlay;
			}

			// Provide setters so strategies can be changed at runtime
			public void SetCompressor(ICompressor compressor)
			{
				_compressor = compressor;
			}

			public void SetOverlay(IOverlay overlay)
			{
				_overlay = overlay;
			}

			public void Store(string fileName)
			{
				// Work is now delegated to the concrete compressor and overlay objects. VideoStorage now has no knowledge of the implementation details of each compression and overlay algorithm.
				_compressor.Compress();
				_overlay.Apply();

				System.Console.WriteLine("Storing video to " + fileName);
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
