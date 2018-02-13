using System;

namespace Chess.Moves
{

	/// <summary>
	/// Абстрактный класс шахматного хода.
	/// </summary>
	public abstract class CMove
	{
		/// <summary>
		/// Получает значение, указывающее совершён ли ход.
		/// </summary>
		public bool IsDone { get; private set; }

		/// <summary>
		/// Прямое действие — совершение ход.
		/// </summary>
		protected Action DoAction;

		/// <summary>
		/// Обратное действие — отмена хода.
		/// </summary>
		protected Action UndoAction;

		/// <summary>
		/// Совершает ход.
		/// </summary>
		public void Do()
		{
			if (IsDone)
			{
				return;
			}

			DoAction();
			IsDone = true;
		}

		/// <summary>
		/// Отменяет ход.
		/// </summary>
		public void Undo()
		{
			if (!IsDone)
			{
				return;
			}

			UndoAction();
			IsDone = false;
		}
	}
}